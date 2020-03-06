using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RestSharp;
using OfficeOpenXml;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using uspto.Models;
using System.Threading;

namespace uspto.Controls
{
    public partial class UcStatusSearch : UserControl
    {
        public UcStatusSearch()
        {
            InitializeComponent();

        }

        private void btn_clear_Click(object sender, EventArgs e)
        {
            tbx_nums.Clear();
        }

        CancellationTokenSource CancellationTokenSourceSearch;
        private void btn_search_Click(object sender, EventArgs e)
        {
            if (btn_search.Text == "取消" && CancellationTokenSourceSearch != null)
            {
                CancellationTokenSourceSearch.Cancel();
                return;
            }
            CancellationTokenSourceSearch = new CancellationTokenSource();
            CancellationTokenSourceSearch.Token.Register(() =>
            {
                btn_search.Text = "查询";
            });

            var nums = tbx_nums.Lines.Distinct().Where(m => m.Length > 0 && Regex.IsMatch(m, @"\d+")).ToArray();
            DoSearch(nums);
        }

        private List<Models.Patent> patents = new List<Models.Patent>();

        private void DoSearch(string[] nums)
        {
            if (nums != null && nums.Length > 0)
            {
                SaveData(nums);
                lb_staus.Text = nums.Length.ToString();
                pb_nums.Value = 0;
                pb_nums.Maximum = nums.Length;
                btn_search.Text = "取消";

                var client = new RestClient();
                client.BaseUrl = new Uri("http://tsdr.uspto.gov/");
                client.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/80.0.3987.122 Safari/537.36 Edg/80.0.361.62";

                var htmldoc = new HtmlAgilityPack.HtmlDocument();

                var list = new List<Models.Patent>();
                Task.Run(async () =>
                {
                    foreach (var item in nums)
                    {
                        if (CancellationTokenSourceSearch.IsCancellationRequested)
                            break;

                        var model = new Models.Patent();
                        model.Num = item.Trim();

                        var request = new RestRequest();
                        request.AddHeader("Referer", "http://tsdr.uspto.gov/");
                        request.Resource = $"statusview/sn{model.Num}";
                        var resp = client.Get(request);
                        var step = $"{item} {resp.StatusCode} \r\n";

                        tbx_rst.Invoke(new MethodInvoker(() =>
                        {
                            pb_nums.PerformStep();
                            tbx_rst.AppendText(step);
                            lb_staus.Text = pb_nums.Value + "/" + pb_nums.Maximum;
                        }));

                        if (resp.IsSuccessful)
                        {

                            htmldoc.LoadHtml(resp.Content);
                            var name = htmldoc.DocumentNode.SelectSingleNode("//*[@id='summary']//div/div[@class='key'][contains(text(),'Mark:')]/../div[2]");
                            var status = htmldoc.DocumentNode.SelectSingleNode("//*[@id='summary']//div/div[@class='key'][contains(text(),'Status:')]/../div[2]");
                            var statusdate = htmldoc.DocumentNode.SelectSingleNode("//*[@id='summary']//div/div[@class='key'][contains(text(),'Status Date:')]/../div[2]");
                            var pubDate = htmldoc.DocumentNode.SelectSingleNode("//*[@id='summary']//div/div[@class='key'][contains(text(),'Publication Date:')]/../div[2]");

                            if (name != null)
                            {
                                model.Name = name.InnerText.Trim();
                            }
                            if (status != null)
                            {
                                model.Status = status.InnerText.Trim();
                            }
                            if (statusdate != null)
                            {
                                model.StatusDate = statusdate.InnerText.Trim();
                            }
                            if (pubDate != null)
                            {
                                model.PublicationDate = pubDate.InnerText.Trim();
                            }

                            await Task.Delay(new Random().Next(500, 2000));
                            request.Resource = $"documentviewer?caseId=sn{model.Num}&";
                            var resp2 = client.Get(request);
                            if (resp2.IsSuccessful)
                            {
                                htmldoc.LoadHtml(resp2.Content);
                                var nodejson = htmldoc.DocumentNode.SelectSingleNode("/html/head/script[contains(text(),'DocsList')]");
                                if (nodejson != null)
                                {
                                    var json = nodejson.InnerText.Replace("var DocsList =", "");
                                    var docslist = Newtonsoft.Json.JsonConvert.DeserializeObject<DocsList>(json);
                                    model.DocDates = docslist.caseDocs.Take(5).Select(m => m.displayDate + " " + m.description).ToArray();
                                    var cert = docslist.caseDocs.Where(m => m.description.Contains("Registration Certificate")).FirstOrDefault();
                                    if (cert != null && cert.urlPathList.Length > 0)
                                    {
                                        model.PdfFile = cert.urlPathList[0];
                                    }
                                }
                            }

                        }
                        list.Add(model);

                        await Task.Delay(new Random().Next(2000, 3000));
                    }

                    patents.Clear();
                    patents = list;
                }, CancellationTokenSourceSearch.Token);

            }
            else
            {
                tbx_nums.Focus();
                CancellationTokenSourceSearch.Cancel();
            }
        }

        private void btn_export_Click(object sender, EventArgs e)
        {
            var saveDialog = new SaveFileDialog();
            saveDialog.Filter = "xls|*.xls";
            saveDialog.FileName = DateTime.Now.ToString("yyyyMMdd-HHmmssfff");
            saveDialog.DefaultExt = ".xls";
            saveDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                using (ExcelPackage excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Worksheets.Add(DateTime.Now.ToString("yyyyMMdd"));


                    for (int i = 0; i < patents.Count; i++)
                    {
                        var item = patents[i];
                        if (i == 0)
                        {
                            excelPackage.Workbook.Worksheets[1].Cells[1, 1].Value = "商标名称";
                            excelPackage.Workbook.Worksheets[1].Cells[1, 2].Value = "申请号";
                            excelPackage.Workbook.Worksheets[1].Cells[1, 3].Value = "状态时间";
                            excelPackage.Workbook.Worksheets[1].Cells[1, 4].Value = "申请状态";
                            excelPackage.Workbook.Worksheets[1].Cells[1, 5].Value = "发布时间";
                            excelPackage.Workbook.Worksheets[1].Cells[1, 6].Value = "文档时间";

                        }
                        excelPackage.Workbook.Worksheets[1].Cells[i + 2, 1].Value = item.Name;
                        excelPackage.Workbook.Worksheets[1].Cells[i + 2, 2].Value = item.Num;
                        excelPackage.Workbook.Worksheets[1].Cells[i + 2, 3].Value = item.StatusDate;
                        excelPackage.Workbook.Worksheets[1].Cells[i + 2, 4].Value = item.Status;
                        excelPackage.Workbook.Worksheets[1].Cells[i + 2, 5].Value = item.PublicationDate;
                        var docdata = string.Empty;
                        item.DocDates.ToList().ForEach(m => docdata = docdata + m + "\r\n");
                        excelPackage.Workbook.Worksheets[1].Cells[i + 2, 6].Value = item.DocDates == null ? "" : docdata;// string.Join("\t",item.DocDates);

                    }

                    excelPackage.SaveAs(new FileInfo(saveDialog.FileName));

                }
                MessageBox.Show("导出成功", "提示");


            }
        }

        private void SaveData(string[] datas, bool overwirte = false)
        {

            var filename = Directory.GetCurrentDirectory() + "/file.db";
            if (overwirte)
            {
                File.Delete(filename);
            }
            if (!File.Exists(filename))
            {
                var file = File.Create(filename);
                file.Close();
            }
            File.AppendAllLines(filename, datas, Encoding.UTF8);
        }


        CancellationTokenSource CancellationTokenSourceDown = new CancellationTokenSource();
        private void btn_down_Click(object sender, EventArgs e)
        {
            if (btn_down.Text == "取消" && CancellationTokenSourceDown != null)
            {
                CancellationTokenSourceDown.Cancel();
                return;
            }
            CancellationTokenSourceDown.Token.Register(() =>
            {
                btn_down.Text = "下载PDF";
            });

            var downlist = patents.Where(m => !string.IsNullOrEmpty(m.PdfFile) && m.PdfFile.StartsWith("http") && m.PdfFile.Contains(".pdf")).ToArray();

            if (downlist == null || downlist.Length < 1)
            {
                MessageBox.Show("没有可供下载的文件");
                return;
            }
            DownloadPdf(downlist);
        }

        private void DownloadPdf(Patent[] patents)
        {
            if (patents != null)
            {
                pb_down.Maximum = patents.Length;
                pb_down.Value = 0;
                Task.Run(() =>
                {

                    var dir = $"{Directory.GetCurrentDirectory()}/download/{DateTime.Now.ToString("yyyyMMdd")}";
                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }
                    var client = new WebClient();
                    foreach (var item in patents)
                    {
                        if (CancellationTokenSourceDown.IsCancellationRequested)
                            break;
                        client.DownloadFile(item.PdfFile, $"{dir}/{item.Name}-{item.Num}.pdf");

                        tbx_rst.Invoke(new MethodInvoker(() =>
                        {
                            pb_down.PerformStep();
                            lb_down.Text = $"{pb_down.Value}/{pb_down.Maximum}";
                            tbx_rst.AppendText($"{item.Name}-{item.Num}.PDF DOWNLOAD OK\r\n");
                        }));
                    }

                }, CancellationTokenSourceDown.Token);
            }

        }

        private void btn_load_Click(object sender, EventArgs e)
        {
            var file = Directory.GetCurrentDirectory() + "/file.db";
            if (File.Exists(file))
            {
                var lines = File.ReadAllLines(file, Encoding.UTF8).Distinct().Where(m => Regex.IsMatch(m, @"\d+")).ToArray();
                tbx_nums.Lines = lines;
                SaveData(lines, true);
            }
        }
    }
}
