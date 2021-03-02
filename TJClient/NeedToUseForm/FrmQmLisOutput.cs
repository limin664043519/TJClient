using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TJClient.Bll;
using TJClient.Common;
using TJClient.Common.Config;
using Spire.Xls;

namespace TJClient.NeedToUseForm
{
    public partial class FrmQmLisOutput : Form
    {
        public FrmQmLisOutput()
        {
            InitializeComponent();
        }

        private void WriteToResult(string message)
        {
            txtResult.Invoke(new Action(() =>
            {
                txtResult.AppendText(message+Environment.NewLine);
            }));
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private string GetSexName(string value)
        {
            return value == "1" ? "男" : "女";
        }

        private Dictionary<string, string> GetQianmaiCorrespondingDic()
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            result.Add("中心条码", "qmlistm");
            result.Add("医院条码", "listm");
            result.Add("病人类别", "");
            result.Add("病员号", "");
            result.Add("病人姓名", "d_xm");
            result.Add("病人性别", "d_xb");
            result.Add("年龄", "nl");
            result.Add("年龄类型", "1");  //单独处理
            result.Add("床号", "");
            result.Add("标本种类", "多标本类型"); //单独处理
            result.Add("采样日期", "createdate"); //单独处理
            result.Add("送检医生", "doctor"); //单独处理
            result.Add("检验项目描述", "");
            result.Add("病人科室", "");  //单独处理
            result.Add("备注", "includeblood");
            result.Add("申请单项目说明", "三分类;血型;生化"); //单独处理
            result.Add("临床诊断", "");
            result.Add("接收员说明", "");
            result.Add("检验目的", "");
            return result;
        }

        private bool OutPutToCsv(DataTable dt,OutputInfo info)
        {
            
            if (dt != null && dt.Rows.Count > 0)
            {
                WriteToResult(string.Format("共需要导出{0}条数据",dt.Rows.Count));
                Dictionary<string, string> dics = GetQianmaiCorrespondingDic();
                using (Workbook workbook = new Workbook())
                {
                    workbook.LoadFromFile(ConfigHelper.GetQianmaiExcelTemplate());
                    Worksheet sheet = workbook.Worksheets[0];
                    int rowCount = dt.Rows.Count;
                    
                    for (int i = 0; i < rowCount; i++)
                    {
                        int j = 0;
                        foreach (var dic in dics)
                        {
                            if (dic.Key == "采样日期")
                            {
                                sheet.Range[i + 2, j + 1].Text = DateHelper.ConvertToQmLisDate(dt.Rows[i][dic.Value].ToString());
                            }
                            else if (dic.Key == "病人性别")
                            {
                                sheet.Range[i + 2, j + 1].Text = GetSexName(dt.Rows[i][dic.Value].ToString());
                            }
                            else if (dic.Key == "年龄类型" || dic.Key== "标本种类" || dic.Key== "申请单项目说明")
                            {
                                sheet.Range[i + 2, j + 1].Text = dic.Value;
                            }
                            else if (dic.Key == "病人科室")
                            {
                                sheet.Range[i + 2, j + 1].Text = info.Department;
                            }
                            else if (dic.Key == "送检医生")
                            {
                                sheet.Range[i + 2, j + 1].Text = info.Doctor;
                            }
                            else
                            {
                                if (dic.Value == "")
                                {
                                    sheet.Range[i + 2, j + 1].Text = "";
                                }
                                else
                                {
                                    sheet.Range[i + 2, j + 1].Text = dt.Rows[i][dic.Value].ToString();
                                }
                            }
                            sheet.Range[i + 2, j + 1].NumberFormat = "@";
                            j++;
                        }
                    }
                    workbook.SaveToFile(info.Path);
                }
                return true;
            }
            WriteToResult("没有数据需要导出");
            return false;
        }

        private Dictionary<string, string> GetJinyuCorrespondingDic()
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            result.Add("条码", "qmlistm");
            result.Add("医院条码", "listm");
            result.Add("医院标识", UserInfo.Yybm);
            result.Add("采样日期", "createdate"); //单独处理
            result.Add("姓名", "d_xm");
            result.Add("出生日期", "");
            result.Add("性别", "d_xb");   //单独处理
            result.Add("年龄", "nl");
            result.Add("年龄单位", "岁"); //单独处理
            result.Add("婚否(是/否)", "");
            result.Add("病人电话", "");
            result.Add("科室（县、区）", ""); //单独处理
            result.Add("门诊/住院号（医院编号）", "");
            result.Add("床号", "");
            result.Add("送检医生", "doctor"); //单独处理
            result.Add("身份证号码", "D_SFZH");
            result.Add("诊断", "");
            result.Add("孕周（周）", "");
            result.Add("孕周（天）", "");
            result.Add("送检标本", "");
            result.Add("医生电话", "");
            result.Add("体重", "");
            result.Add("体位", "");
            result.Add("尿量（ml）", "");
            result.Add("尿量（h）", "");
            result.Add("末次月经", "");
            return result;
        }

        private bool JinyuOutputToCsv(DataTable dt,OutputInfo info)
        {
            if (dt != null && dt.Rows.Count > 0)
            {
                WriteToResult(string.Format("共需要导出{0}条数据", dt.Rows.Count));
                Dictionary<string, string> dics = GetJinyuCorrespondingDic();
                using (Workbook workbook = new Workbook())
                {
                    workbook.LoadFromFile(ConfigHelper.GetJinyuExcelTemplate());
                    Worksheet sheet = workbook.Worksheets[0];
                    int rowCount = dt.Rows.Count;
                    
                    for (int i = 0; i < rowCount; i++)
                    {
                        int j = 0;
                        foreach (var dic in dics)
                        {
                            if (dic.Key == "采样日期")
                            {
                                sheet.Range[i + 2, j + 1].Text = DateHelper.ConvertToQmLisDate(dt.Rows[i][dic.Value].ToString());
                            }
                            else if (dic.Key == "性别")
                            {
                                sheet.Range[i + 2, j + 1].Text = GetSexName(dt.Rows[i][dic.Value].ToString());
                            }
                            else if (dic.Key == "年龄单位" || dic.Key== "医院标识")
                            {
                                sheet.Range[i + 2, j + 1].Text = dic.Value;
                            }
                            else if (dic.Key == "科室（县、区）")
                            {
                                sheet.Range[i + 2, j + 1].Text = info.Department;
                            }
                            else if (dic.Key == "送检医生")
                            {
                                sheet.Range[i + 2, j + 1].Text = info.Doctor;
                            }
                            else
                            {
                                if (dic.Value == "")
                                {
                                    sheet.Range[i + 2, j + 1].Text = "";
                                }
                                else
                                {
                                    sheet.Range[i + 2, j + 1].Text = dt.Rows[i][dic.Value].ToString();
                                }
                            }
                            sheet.Range[i + 2, j+1].NumberFormat = "@";
                            j++;
                        }
                    }
                    workbook.SaveToFile(info.Path);
                }
                return true;
            }
            WriteToResult("没有数据需要导出");
            return false;
        }

        private void Operation(OutputInfo info)
        {
            try
            {
                int status = info.DontInludeOutput ? 0 : -1;
                DataTable dt = LisTmCorrespondingBll.GetLisTmCorrespondingByStatus(status, info.BeginDate, info.EndDate);
                bool isSucc = false;
                if (ConfigHelper.IsQianmai())
                {
                    isSucc = OutPutToCsv(dt, info);
                }
                else
                {
                    isSucc = JinyuOutputToCsv(dt,info);
                }
                if (isSucc)
                {
                    WriteToResult("导出成功");
                    LisTmCorrespondingBll.UpdateStatus(dt);
                    WriteToResult("更改状态成功");
                }
                WriteToResult("导出完毕");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }
        private void btnOutput_Click(object sender, EventArgs e)
        {
            sfd.Filter = "Excel文件(*.xls,*.xlsx)|*.xls,*.xlsx";
            sfd.DefaultExt = "xls";
            sfd.AddExtension = true;
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                string path = sfd.FileName;
                bool chk = chkDontIncludeOutput.Checked;
                string beginDate = DateHelper.ConvertToCurrDate(dtpBegin.Text);
                string endDate = DateHelper.ConvertToCurrDate(dtpEnd.Text);
                string doctor = cboDoctor.Text;
                string dept = txtDep.Text;
                Task t = new Task(() =>
                {
                    try
                    {
                        Operation(new OutputInfo(chk, beginDate, endDate, path,doctor,dept));
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                });
                t.Start();
            }
            
        }

        private void GetDoctor()
        {
            List<string> realNameList = Signname.Operation.GetAllRealname(UserInfo.Yybm);
            cboDoctor.Items.Clear();
            realNameList.ForEach(x=>cboDoctor.Items.Add(x));
            //前医生，后科室
            string[] doctor = DoctorConfigHelper.GetDoctor().Split('|');
            cboDoctor.Text = doctor[0];
            if (doctor.Length > 1)
            {
                txtDep.Text = doctor[1];
            }
        }

        private void FrmQmLisOutput_Load(object sender, EventArgs e)
        {
            dtpBegin.Text = DateHelper.CurrDate();
            dtpEnd.Text = DateHelper.CurrDate();
            GetDoctor();
        }

        private void cboDoctor_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void btnSetting_Click(object sender, EventArgs e)
        {
            DoctorConfigHelper.SetDoctor(string.Format("{0}|{1}",cboDoctor.Text,txtDep.Text));
            txtResult.AppendText("设置成功"+Environment.NewLine);
        }
    }

    public class OutputInfo
    {
        public bool DontInludeOutput { set; get; }
        public string BeginDate { set; get; }
        public string EndDate { set; get; }
        public string Path { set; get; }
        public string Doctor { set; get; }
        public string Department { set; get; }

        public OutputInfo(bool chk, string beginDate, string endDate, string path, string doctor, string department)
        {
            this.DontInludeOutput = chk;
            this.BeginDate = beginDate;
            this.EndDate = endDate;
            this.Path = path;
            this.Doctor = doctor;
            this.Department = department;
        }
    }
}
