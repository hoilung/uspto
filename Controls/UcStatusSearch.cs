﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RestSharp;

using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using uspto.Models;
using System.Threading;

using MiniExcelLibs;

namespace uspto.Controls
{
    public partial class UcStatusSearch : UcBase
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
                if (selectWait)
                {
                    if (MessageBox.Show("继续查询？", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        selectWait = false;
                        return;
                    }
                }
                else
                {
                    if (MessageBox.Show("暂停查询？，", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        selectWait = true;
                        return;
                    }
                }

                CancellationTokenSourceSearch.Cancel();
                return;
            }
            CancellationTokenSourceSearch = new CancellationTokenSource();
            CancellationTokenSourceSearch.Token.Register(() =>
            {
                btn_search.Text = "查询";
                selectWait = false;
            });


            var nums = tbx_nums.Lines.Distinct().Where(m => m.Length > 0 && Regex.IsMatch(m, @"\d+")).ToArray();
            DoSearch(nums);
        }

        private List<Models.Patent> patents = new List<Models.Patent>();

        /// <summary>
        /// 查询是否暂停
        /// </summary>
        private bool selectWait = false;

        private void DoSearch(string[] nums)
        {
            if (nums != null && nums.Length > 0)
            {
                SaveData(nums);
                lb_staus.Text = nums.Length.ToString();
                pb_nums.Value = 0;
                pb_nums.Maximum = nums.Length;
                btn_search.Text = "取消";

                var spiderAll = cbx_spiderAll.Checked;
                if (spiderAll)
                {
                    tbx_rst.AppendText("\r\n当前查询全部信息内容较多，请耐心等候\r\n");
                }


                Task.Run(async () =>
                {

                    var client = new RestClient("https://tsdr.uspto.gov/");
                    var htmldoc = new HtmlAgilityPack.HtmlDocument();
                    var list = new List<Models.Patent>();

                    foreach (var item in nums)
                    {
                        while (selectWait)
                        {
                            await Task.Delay(5000);
                            tbx_rst.Invoke(new MethodInvoker(() =>
                            {
                                tbx_rst.AppendText($"查询信息暂停，等待恢复\r\n");
                            }));
                            continue;
                        }
                        if (CancellationTokenSourceSearch.IsCancellationRequested)
                            break;

                        var model = new Models.Patent();
                        model.Num = item.Trim();

                        var request = new RestRequest();

                        request.AddHeader("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/80.0.3987.149 Safari/537.36");
                        request.AddHeader("Referer", "https://tsdr.uspto.gov/");
                        request.Resource = $"statusview/sn{model.Num}";
                        var resp = client.Get(request);
                        if (!resp.IsSuccessful)
                        {
                            resp = client.Get(request);
                        }

                        var consoleInfo = $"{item} status {resp.StatusCode}";

                        if (resp.IsSuccessful)
                        {

                            htmldoc.LoadHtml(resp.Content);
                            var name = htmldoc.DocumentNode.SelectSingleNode("//*[@id='summary']//div/div[@class='key'][contains(text(),'Mark:')]/following-sibling::div[1]");
                            var status = htmldoc.DocumentNode.SelectSingleNode("//*[@id='summary']//div/div[@class='key'][contains(text(),'Status:')]/following-sibling::div[1]");
                            var statusdate = htmldoc.DocumentNode.SelectSingleNode("//*[@id='summary']//div/div[@class='key'][contains(text(),'Status Date:')]/following-sibling::div[1]");
                            var pubDate = htmldoc.DocumentNode.SelectSingleNode("//*[@id='summary' or @id='sumary-section']//div/div[@class='key'][contains(text(),'Publication Date:')]/following-sibling::div[1]");

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

                            var InternationalClass = htmldoc.DocumentNode.SelectSingleNode("//*[@id='data_container']//div/div[@class='key'][contains(text(),'International Class')]/following-sibling::div[1]");
                            var filingDate = htmldoc.DocumentNode.SelectSingleNode("//*[@id='summary']//div/div[@class='key'][contains(text(),'Filing Date')]/following-sibling::div[1]");
                            if (InternationalClass != null)
                            {
                                model.TEASPlusNewApplication["INTERNATIONAL CLASS"] = Regex.Match(InternationalClass.InnerText, "\\d+").Value;
                            }
                            if (filingDate != null)
                            {
                                model.TEASPlusNewApplication["Filing Date"] = filingDate.InnerText.Trim();
                            }


#if !DEBUG
                            await Task.Delay(new Random().Next(200, 500));
#endif
                            //以上为基础信息
                            //以下为需要二次查询的信息
                            if (spiderAll)//查询增值信息
                            {

                                request.Resource = $"documentviewer?caseId=sn{model.Num}&";
                                var resp2 = client.Get(request);

                                consoleInfo += " document " + resp2.StatusCode;
                                if (!resp2.IsSuccessful)
                                {
                                    resp2 = client.Get(request);
                                }

                                if (resp2.IsSuccessful)
                                {
                                    htmldoc.LoadHtml(resp2.Content);
                                    var nodejson = htmldoc.DocumentNode.SelectSingleNode("/html/head/script[contains(text(),'DocsList')]");
                                    if (nodejson != null)
                                    {
                                        var json = nodejson.InnerText.Replace("var DocsList =", "");
                                        var docslist = Newtonsoft.Json.JsonConvert.DeserializeObject<DocsList>(json);
                                        model.DocDates = docslist.caseDocs.Select(m => m.displayDate + " " + m.description).ToArray();
                                        var cert = docslist.caseDocs.Where(m => m.description.Contains("Registration Certificate")).FirstOrDefault();
                                        if (cert != null && cert.urlPathList.Length > 0)
                                        {
                                            model.PdfFile = cert.urlPathList[0];//注册文件下载地址                                       
                                        }
                                        var oafile = docslist.caseDocs.Where(m => m.description.Contains("Offc Action Outgoing")).FirstOrDefault();
                                        if (oafile != null && oafile.urlPathList.Length > 0)
                                        {
                                            model.OffcActionFile = oafile.urlPathList[0];//复生文件html地址
                                        }
                                        var newapplication = docslist.caseDocs.Where(m => m.description.Contains("New Application")).FirstOrDefault();
                                        if (newapplication != null && newapplication.urlPathList.Length > 0)
                                        {
                                            var newUrl = newapplication.urlPathList[0];
                                            if (!string.IsNullOrEmpty(newUrl) && newUrl.StartsWith("http"))
                                            {
                                                request.Resource = newUrl;
                                                var resp3 = client.Get(request);
                                                consoleInfo += " new application " + resp2.StatusCode;
                                                if (resp3.IsSuccessful)
                                                {
                                                    htmldoc.LoadHtml(resp3.Content);
                                                    try
                                                    {
                                                        var classcdNode = htmldoc.DocumentNode.SelectSingleNode("/html//table/tr/td[@headers='entered goods-new classcd-goods']");
                                                        if (classcdNode != null)
                                                            model.TEASPlusNewApplication["INTERNATIONAL CLASS"] = System.Web.HttpUtility.HtmlDecode(classcdNode.InnerText).Trim();
                                                        var descNode = htmldoc.DocumentNode.SelectSingleNode("/html//table/tr/td[@headers='entered goods-new desc-goods']");
                                                        if (descNode != null)
                                                            model.TEASPlusNewApplication["IDENTIFICATION"] = descNode.InnerText.Trim();
                                                        var nameNode = htmldoc.DocumentNode.SelectSingleNode("/html//table/tr/td[@headers='entered corr name-corr']");
                                                        if (nameNode != null)
                                                            model.TEASPlusNewApplication["NAME"] = nameNode.InnerText.Trim();
                                                        var streetNode = htmldoc.DocumentNode.SelectSingleNode("/html//table/tr/td[@headers='entered corr street-corr' or @headers='entered att street-att']");
                                                        if (streetNode != null)
                                                            model.TEASPlusNewApplication["STREET"] = streetNode.InnerText.Trim();
                                                        var cityNode = htmldoc.DocumentNode.SelectSingleNode("/html//table/tr/td[@headers='entered corr city-corr' or @headers='entered att city-att']");
                                                        if (cityNode != null)
                                                            model.TEASPlusNewApplication["CITY"] = cityNode.InnerText.Trim();
                                                        var stateNode = htmldoc.DocumentNode.SelectSingleNode("/html//table/tr/td[@headers='entered corr state-corr' or @headers='entered att state-att']");
                                                        if (stateNode != null)
                                                            model.TEASPlusNewApplication["STATE"] = stateNode.InnerText.Trim();
                                                        var countryNode = htmldoc.DocumentNode.SelectSingleNode("/html//table/tr/td[@headers='entered corr country-corr' or @headers='entered att country-att']");
                                                        if (countryNode != null)
                                                            model.TEASPlusNewApplication["COUNTRY"] = countryNode.InnerText.Trim();
                                                        var postalcdNode = htmldoc.DocumentNode.SelectSingleNode("/html//table/tr/td[@headers='entered corr postalcd-corr' or @headers='entered att postalcd-att']");
                                                        if (postalcdNode != null)
                                                            model.TEASPlusNewApplication["ZIP"] = postalcdNode.InnerText.Trim();
                                                        var phoneNode = htmldoc.DocumentNode.SelectSingleNode("/html//table/tr/td[@headers='entered corr phone-corr' or @headers='entered att zipcd-att']");
                                                        if (phoneNode != null)
                                                            model.TEASPlusNewApplication["PHONE"] = phoneNode.InnerText.Trim();
                                                        var emailNode = htmldoc.DocumentNode.SelectSingleNode("/html//table/tr/td[@headers='entered corr emailAddr-corr' or @headers='entered att emailAddr-att']");
                                                        if (emailNode != null)
                                                            model.TEASPlusNewApplication["EMAIL ADDRESS"] = emailNode.InnerText.Trim();
                                                        var auemailNode = htmldoc.DocumentNode.SelectSingleNode("/html//table/tr/td[@headers='entered corr auEmail-corr' or @headers='entered att auEmail-att']");
                                                        if (auemailNode != null)
                                                            model.TEASPlusNewApplication["AUTHORIZED TO COMMUNICATE VIA EMAIL"] = auemailNode.InnerText.Trim();
                                                        var numclassesHeadingNode = htmldoc.DocumentNode.SelectSingleNode("/html//table/tr/td[@headers='entered fees numclasses-heading']");
                                                        if (numclassesHeadingNode != null)
                                                            model.TEASPlusNewApplication["APPLICATION FILING OPTION"] = numclassesHeadingNode.InnerText.Trim();
                                                        var numclassesFeesNode = htmldoc.DocumentNode.SelectSingleNode("/html//table/tr/td[@headers='entered fees numclasses-fees']");
                                                        if (numclassesFeesNode != null)
                                                            model.TEASPlusNewApplication["NUMBER OF CLASSES"] = numclassesFeesNode.InnerText.Trim();
                                                        var feeNode = htmldoc.DocumentNode.SelectSingleNode("/html//table/tr/td[@headers='entered fees fee-per-class-fees']");
                                                        if (feeNode != null)
                                                            model.TEASPlusNewApplication["FEE PER CLASS"] = feeNode.InnerText.Trim();
                                                        var totalfeefeesNode = htmldoc.DocumentNode.SelectSingleNode("/html//table/tr/td[@headers='entered fees totalfee-fees']");
                                                        if (totalfeefeesNode != null)
                                                            model.TEASPlusNewApplication["TOTAL FEE PAID"] = totalfeefeesNode.InnerText.Trim();
                                                        var signamesignNode = htmldoc.DocumentNode.SelectSingleNode("/html//table/tr/td[@headers='entered signatures signame-sign']");
                                                        if (signamesignNode != null)
                                                            model.TEASPlusNewApplication["SIGNATURE"] = signamesignNode.InnerText.Trim();
                                                        var signatorynameNode = htmldoc.DocumentNode.SelectSingleNode("/html//table/tr//td[@headers='entered signatures signatoryname-sign']");
                                                        if (signatorynameNode != null)
                                                            model.TEASPlusNewApplication["SIGNATORY'S NAME"] = signatorynameNode.InnerText.Trim();
                                                        var signpositionNode = htmldoc.DocumentNode.SelectSingleNode("/html//table/tr//td[@headers='entered signatures sign-position-sign']");
                                                        if (signpositionNode != null)
                                                            model.TEASPlusNewApplication["SIGNATORY'S POSITION"] = signpositionNode.InnerText.Trim();
                                                        var signdtNode = htmldoc.DocumentNode.SelectSingleNode("/html//table/tr/td[@headers='entered signatures sign-dt-sign']");
                                                        if (signdtNode != null)
                                                            model.TEASPlusNewApplication["DATE SIGNED"] = signdtNode.InnerText.Trim();

                                                        var filingDataNode = htmldoc.DocumentNode.SelectSingleNode("//table//tr//td/b[contains(text(),'Filing')]/../b[2]");
                                                        if (filingDataNode != null)
                                                            model.TEASPlusNewApplication["Filing Date"] = filingDataNode.InnerText.Trim();
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        continue;
                                                        //throw;
                                                    }

                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        tbx_rst.Invoke(new MethodInvoker(() =>
                        {
                            pb_nums.PerformStep();
                            tbx_rst.AppendText(consoleInfo + "\r\n");
                            lb_staus.Text = pb_nums.Value + "/" + pb_nums.Maximum;
                        }));
                        list.Add(model);
#if !DEBUG
                        await Task.Delay(new Random().Next(1000, 2000));
#endif
                    }

                    patents.Clear();
                    patents.AddRange(list);
                    btn_search.Invoke(new MethodInvoker(() =>
                    {
                        btn_search.Text = "查询";
                        tbx_rst.AppendText("查询完成\r\n");
                    }));
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
            if (patents != null && patents.Count > 0)
            {

                var saveDialog = new SaveFileDialog();
                saveDialog.Filter = "xlsx|*.xlsx";
                saveDialog.FileName = DateTime.Now.ToString("yyyyMMdd-HHmmssfff");
                saveDialog.DefaultExt = ".xlsx";
                saveDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                if (saveDialog.ShowDialog() == DialogResult.OK)
                {

                    var table = new DataTable();
                    {
                        table.Columns.Add("商标名称", typeof(string));
                        table.Columns.Add("申请号", typeof(string));
                        table.Columns.Add("状态时间", typeof(string));
                        table.Columns.Add("申请状态", typeof(string));
                        table.Columns.Add("发布时间", typeof(string));
                        table.Columns.Add("文档时间", typeof(string));
                        table.Columns.Add("PDF文件", typeof(string));


                        foreach (var info in patents.FirstOrDefault().TEASPlusNewApplication)
                        {
                            table.Columns.Add(info.Key, typeof(string));

                        }


                        for (int i = 0; i < patents.Count; i++)
                        {
                            try
                            {
                                var item = patents[i];

                                //var docdata = new StringBuilder();
                                //if (item.DocDates != null && item.DocDates.Length > 0)
                                //{                                    
                                //    //item.DocDates.Take(5).ToList().ForEach(m => docdata.Append(m + "\r\n"));
                                //}
                                table.Rows.Add(item.Name, item.Num, item.StatusDate, item.Status, item.PublicationDate, (item.DocDates != null ? string.Join("\r\n", item.DocDates.Take(5)) : ""), item.PdfFile);

                                int j = 7;
                                foreach (var info in item.TEASPlusNewApplication)
                                {
                                    table.Rows[i][j] = info.Value;
                                    j++;
                                }


                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                                throw;
                            }
                        }
                        MiniExcel.SaveAs(saveDialog.FileName, table, sheetName: DateTime.Now.ToString("yyyyMMdd"));
                    }
                    MessageBox.Show("导出成功\r\n" + saveDialog.FileName, "提示");
                }
            }
            else
            {
                MessageBox.Show("请等待查询结果完成", "提示");
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
        /// <summary>
        /// 是否暂停下载
        /// </summary>
        private bool downloadWait = false;
        CancellationTokenSource CancellationTokenSourceDown = new CancellationTokenSource();
        private void btn_down_Click(object sender, EventArgs e)
        {
            if (btn_down.Text == "取消" && CancellationTokenSourceDown != null)
            {

                if (downloadWait)
                {
                    if (MessageBox.Show("继续下载？", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        downloadWait = false;
                        return;
                    }
                }
                else
                {
                    if (MessageBox.Show("暂停下载？", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        downloadWait = true;
                        return;
                    }
                }

                if (MessageBox.Show("取消下载？", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    CancellationTokenSourceDown.Cancel();
                    return;
                }
                return;

            }
            CancellationTokenSourceDown.Token.Register(() =>
            {
                btn_down.Text = "注册证书下载";
                downloadWait = false;
            });

            var downlist = patents.Where(m => !string.IsNullOrEmpty(m.PdfFile) && m.PdfFile.StartsWith("http") && m.PdfFile.Contains(".pdf")).ToArray();
            if (downlist == null || downlist.Length < 1)
            {
                MessageBox.Show("没有可供下载的 Registration Certificate", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            DownloadPdf(downlist, DownloadType.RegistrationCertificate);
        }

        private void btn_down2_Click(object sender, EventArgs e)
        {
            if (btn_down2.Text == "取消" && CancellationTokenSourceDown != null)
            {

                if (downloadWait)
                {
                    if (MessageBox.Show("继续下载？", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        downloadWait = false;
                        return;
                    }
                }
                else
                {
                    if (MessageBox.Show("暂停下载？", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        downloadWait = true;
                        return;
                    }
                }

                if (MessageBox.Show("取消下载？", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    CancellationTokenSourceDown.Cancel();
                    return;
                }
                return;

            }
            CancellationTokenSourceDown.Token.Register(() =>
            {
                btn_down2.Text = "复审文件下载";
                downloadWait = false;
            });

            var downlist = patents.Where(m => !string.IsNullOrEmpty(m.OffcActionFile) && m.OffcActionFile.StartsWith("http")).ToArray();
            if (downlist == null || downlist.Length < 1)
            {
                MessageBox.Show("没有可供下载的 Offc Action Outgoing", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            DownloadPdf(downlist, DownloadType.OffcActionOutgoing);
        }

        private void DownloadPdf(Patent[] patents, DownloadType downloadType)
        {
            if (patents != null)
            {

                var queuePatents = new Queue<Patent>();


                pb_down.Maximum = patents.Length;
                pb_down.Value = 0;
                if (downloadType == DownloadType.RegistrationCertificate)
                    btn_down.Text = "取消";
                if (downloadType == DownloadType.OffcActionOutgoing)
                    btn_down2.Text = "取消";


                Task.Run(async () =>
                {

                    var dir = $"{Directory.GetCurrentDirectory()}/download/{DateTime.Now.ToString("yyyyMMdd")}/{downloadType}";

                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }

                    foreach (var item in patents)
                    {
                        queuePatents.Enqueue(item);
                    }
                    var client = new WebClient();
                    while (queuePatents.Any())
                    {
                        try
                        {
                            if (downloadWait)
                            {
                                await Task.Delay(5000);
                                tbx_rst.Invoke(new MethodInvoker(() =>
                                {
                                    tbx_rst.AppendText($"下载文件暂停，等待恢复\r\n");
                                }));
                                continue;
                            }

                            if (CancellationTokenSourceDown.IsCancellationRequested)
                            {
                                queuePatents.Clear();
                                break;
                            }

                            var item = queuePatents.Dequeue();
                            var localfile = $"{dir}/{item.Name}-{item.Num}.pdf";
                            if (downloadType == DownloadType.OffcActionOutgoing)
                            {
                                localfile = $"{dir}/{item.Name}-{item.Num}.html";
                            }

                            var cosoleinfo = $"{item.Name}-{item.Num} ";
                            if (!File.Exists(localfile))
                            {
                                if (downloadType == DownloadType.RegistrationCertificate)
                                    client.DownloadFile(item.PdfFile, localfile);
                                if (downloadType == DownloadType.OffcActionOutgoing)
                                    client.DownloadFile(item.OffcActionFile, localfile);

                                cosoleinfo += $" {downloadType} Download OK\r\n";
                            }
                            else
                            {
                                cosoleinfo += " File Exists\r\n";
                            }

                            tbx_rst.Invoke(new MethodInvoker(() =>
                            {
                                pb_down.PerformStep();
                                lb_down.Text = $"{pb_down.Value}/{pb_down.Maximum}";
                                tbx_rst.AppendText(cosoleinfo);
                            }));
                            await Task.Delay(new Random().Next(1000, 2000));
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "错误");
                            break;
                        }
                    }



                    btn_down.Invoke(new MethodInvoker(() =>
                    {
                        if (downloadType == DownloadType.RegistrationCertificate)
                            btn_down.Text = "注册证书下载";
                        else
                            btn_down2.Text = "复审文件下载";

                        tbx_rst.AppendText(downloadType + " 下载完成\r\n");
                        downloadWait = false;

                    }));
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

        private void cbx_spiderAll_CheckedChanged(object sender, EventArgs e)
        {
            btn_down.Enabled = btn_down2.Enabled = cbx_spiderAll.Checked;
        }
    }
}
