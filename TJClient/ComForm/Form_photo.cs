using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing.Imaging;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using System.IO;
using System.Windows.Media.Imaging;
using AForge;
using AForge.Controls;
using AForge.Video;
using AForge.Video.DirectShow;
using Size = System.Drawing.Size;
using TJClient.sys.Bll;
using FBYClient;
using Microsoft.Office.Interop.Word;
using TJClient.Common;


namespace TJClient.ComForm
{
    public partial class Form_photo : Form
    {
        private FilterInfoCollection videoDevices;
        private VideoCaptureDevice videoSource;
        public string jkdah = "";
        public string sfzh = "";
        public Form_photo()
        {
            InitializeComponent();
        }

        private void Init()
        {
            InitUserInfo();
            InitUserPhoto();
            InitUserSignname();
        }

        private void InitUserInfo()
        {
            txtName.Text = Signname.Model.HealthExaminedUserInfo.Name;
            txtDGrdabh.Text = Signname.Model.HealthExaminedUserInfo.DGrdabh;
            txtSFZH.Text = Signname.Model.HealthExaminedUserInfo.CardNo;
            //初始化签字日期  默认是登记或者当天
            if(string.IsNullOrEmpty ( Signname.Model.HealthExaminedUserInfo.CreateDate)==false){
                dateTimePicker_benren.Value = Convert.ToDateTime(Signname.Model.HealthExaminedUserInfo.CreateDate);
                dateTimePicker_jiashu.Value = Convert.ToDateTime(Signname.Model.HealthExaminedUserInfo.CreateDate);
            }
        }

        private void InitUserPhoto()
        {
            string photoPath=GetUserPhoto();
            if (!string.IsNullOrEmpty(photoPath))
            {
                picPhoto.Visible = true;
                picPhoto.ImageLocation = photoPath;
            }
            else
            {
                picPhoto.Visible=false;
            }
        }

        /// <summary>
        /// 初始化签名信息
        /// </summary>
        private void InitUserSignname()
        {
            Signname.Model.JktjSignname jktjSignname = new Signname.Model.JktjSignname()
            {
                Czy = UserInfo.userId,
                Yljgbm = UserInfo.Yybm,
                Tjsj = DateTime.Now.Year.ToString(),
                D_Grdabh = txtDGrdabh.Text,
            };
            DataTable dt_Signname = null;
            Dictionary<string, string> signnames = Signname.Operation.FkSignnamePicPath(jktjSignname, out dt_Signname);
            //foreach (var signname in signnames)
            //{
            //    if (signname.Key == "FKQZBR")
            //    {
            //        Signname.ControlOperation.SignnamePicInit(picSignnameBySelf,signname.Value,"",null);
            //    }
            //    if (signname.Key == "FKQZJS")
            //    {
            //        Signname.ControlOperation.SignnamePicInit(piciSignnameByFamilyMembers, signname.Value,"",null);
            //    }
            //}

            label_benren.Text = "";
            label_jiashu.Text = "";

            foreach (DataRow dtrow in dt_Signname.Rows)
            {
                string signnamegroup = dtrow["signnamegroup"].ToString();

                string signnamepicpath = dtrow["signnamepicpath"].ToString();

                string realname = dtrow["realname"].ToString();

                string tjsj_date = dtrow["tjsj"].ToString();
                if (signnamegroup.Equals("FKQZBR"))
                {
                    Signname.ControlOperation.SignnamePicInit(picSignnameBySelf, signnamepicpath, realname, textBox_benren);

                    if (string.IsNullOrEmpty(tjsj_date) == false)
                    {
                        Signname.Model.HealthExaminedUserInfo.oldtjsj_br = tjsj_date;
                        dateTimePicker_benren.Value = Convert.ToDateTime(tjsj_date);
                    }
                    if (string.IsNullOrEmpty(realname) == false)
                    {
                        label_benren.Text = "已签名";
                    }
                }
                if (signnamegroup.Equals("FKQZJS"))
                {
                    Signname.ControlOperation.SignnamePicInit(piciSignnameByFamilyMembers, signnamepicpath, realname, textBox_jiashu);
                    if (string.IsNullOrEmpty(tjsj_date) == false)
                    {
                        Signname.Model.HealthExaminedUserInfo.oldtjsj_js = tjsj_date;
                        dateTimePicker_jiashu.Value = Convert.ToDateTime(tjsj_date);
                    }
                    if (string.IsNullOrEmpty(realname) == false)
                    {
                        label_jiashu.Text = "已签名";
                    }
                }
            }

            //没有进行签名前，本人签名内容默认本人的姓名
            if (string.IsNullOrEmpty(label_benren.Text) == true && string.IsNullOrEmpty(textBox_jiashu.Text)==true)
            {
                textBox_benren.Text = Signname.Model.HealthExaminedUserInfo.Name;
                label_benren.Text = " 未保存签名";
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {            
            try
            {
                // 枚举所有视频输入设备
                videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);

                if (videoDevices.Count > 0)
                {
                    foreach (FilterInfo device in videoDevices)
                    {
                        tscbxCameras.Items.Add(device.Name);
                    }
                    tscbxCameras.SelectedIndex = 0;
                }
                    //throw new ApplicationException();
                Init();

            }
            catch (ApplicationException)
            {
                tscbxCameras.Items.Add("No local capture devices");
                videoDevices = null;
            }
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (tscbxCameras.Items.Count <= 0)
            {
                MessageBox.Show("请先选择拍照设备");
                return;
            }
            if (picPhoto.Visible)
            {
                picPhoto.Visible=false;
            }
            CameraConn();
        }
        //连接摄像头
        private void CameraConn()
        {
            
            VideoCaptureDevice videoSource = new VideoCaptureDevice(videoDevices[tscbxCameras.SelectedIndex].MonikerString);
            videoSource.DesiredFrameSize = new System.Drawing.Size(320, 240);
            videoSource.DesiredFrameRate = 1;

            videoSourcePlayer.VideoSource = videoSource;
            videoSourcePlayer.Start();
        }

        //关闭摄像头
        private void btnClose_Click(object sender, EventArgs e)
        {
            videoSourcePlayer.SignalToStop();
            videoSourcePlayer.WaitForStop();
        }

        //主窗体关闭
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            btnClose_Click(null, null);
        }

        //拍照
        private void Photograph_Click(object sender, EventArgs e)
        {
            try
            {
                if (videoSourcePlayer.IsRunning)
                {
                    BitmapSource bitmapSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                                    videoSourcePlayer.GetCurrentVideoFrame().GetHbitmap(),
                                    IntPtr.Zero,
                                     Int32Rect.Empty,
                                    BitmapSizeOptions.FromEmptyOptions());
                    PngBitmapEncoder pE = new PngBitmapEncoder();
                    pE.Frames.Add(BitmapFrame.Create(bitmapSource));
                    string picName = GetImagePath() + "\\" + "DJ_" + jkdah + "_" + DateTime.Now.ToString("yyyy-MM-dd") + ".jpg";
                    if (File.Exists(picName))
                    {
                        File.Delete(picName);
                    }
                    using (Stream stream = File.Create(picName))
                    {
                        pE.Save(stream);
                    }
                    //拍照完成后关摄像头并刷新同时关窗体
                    if (videoSourcePlayer != null && videoSourcePlayer.IsRunning)
                    {
                        videoSourcePlayer.SignalToStop();
                        videoSourcePlayer.WaitForStop();
                    }
                    addphoto(picName);
                    InitUserPhoto();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("摄像头异常：" + ex.Message);
            }
        }

        private string GetImagePath()
        {
            string personImgPath = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory)
                         + Path.DirectorySeparatorChar + "photo";
            if (!Directory.Exists(personImgPath))
            {
                Directory.CreateDirectory(personImgPath);
            }
            return personImgPath;
        }
        private void addphoto(string path)
        {
            try
            {
                string year = DateTime.Now.Year.ToString();
                DBAccess db = new DBAccess();
                db.ExecuteNonQueryBySql("delete from t_jk_photo where JKDAH='" + jkdah + "' and nd='" + year + "'");
                db.ExecuteNonQueryBySql("insert into t_jk_photo (JKDAH,YLJGBM,SFZH,CREATTIME,photourl,nd)values('" + jkdah + "','" + TJClient.Common.UserInfo.Yybm + "','" + sfzh + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + path + "','" + year + "')");

                //MessageBox.Show("拍照成功，保存路径：" + path);
                //this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("拍照保存失败，原因：" + ex.Message);
            }
        }

        private string GetUserPhoto()
        {
            string sql = string.Format("select photourl from t_jk_photo where jkdah='{0}' and nd='{1}'", 
                Signname.Model.HealthExaminedUserInfo.DGrdabh,DateTime.Now.Year.ToString());
            DBAccess db=new DBAccess();
            DataTable dt=db.ExecuteQueryBySql(sql);
            if (dt == null || dt.Rows.Count <= 0)
            {
                return "";
            }
            return dt.Rows[0]["photourl"].ToString();
        }

        private void SaveSignnameToDbAfterSignnameSuccess(string saveSignnamePicPath, string signnameGroup,string realname)
        {
            Signname.Model.JktjSignname jktjSignname = new Signname.Model.JktjSignname()
            {
                SignnamePicPath = saveSignnamePicPath,
                Czy = UserInfo.userId,
                Yljgbm = UserInfo.Yybm,
                Tjsj = Signname.Model.HealthExaminedUserInfo.CreateDate,
                D_Grdabh = txtDGrdabh.Text,
                SignnameGroup = signnameGroup,
                Realname = realname,
                oldTjsj=Signname.Model.HealthExaminedUserInfo.oldtjsj

            };
            Signname.Operation.SaveJktjSignname(jktjSignname);
        }

        private void HealthExaminedUserSignnameOperation(System.Windows.Forms.PictureBox picControl, string signnameGroup,string realname)
        {
            string way = string.Format("姓名：{0}  身份证号：{1}", txtName.Text, txtSFZH.Text);
            string saveSignnamePicPath = Signname.Operation.GetTabletSignnamePicPath();
            string msg = Signname.TabletHelper.TabletSignname(way, picControl, saveSignnamePicPath);
            if (msg.Length > 0)
            {
                MessageBox.Show(msg);
                return;
            }
            //如果签名成功，将签名保存到数据库
            SaveSignnameToDbAfterSignnameSuccess(saveSignnamePicPath, signnameGroup,realname);

        }

        private void btnTabletSignnameBySelf_Click(object sender, EventArgs e)
        {
            HealthExaminedUserSignnameOperation(picSignnameBySelf, "FKQZBR",textBox_benren.Text );
        }

        private void btnSignnameByFamilyMembers_Click(object sender, EventArgs e)
        {
            HealthExaminedUserSignnameOperation(piciSignnameByFamilyMembers, "FKQZJS",textBox_jiashu.Text);
        }

        /// <summary>
        /// 保存本人文字签名
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_benren_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(textBox_benren.Text) == true)
            {
                MessageBox.Show("请录入[居民姓名]，再保存签字！");
                label_benren.Text = "请录入[居民姓名]，再保存签字！";
                textBox_benren.Focus();

                return;
            }

            string strpath = picSignnameBySelf.ImageLocation;
            Signname.Model.HealthExaminedUserInfo.CreateDate = dateTimePicker_benren.Value.ToString("yyyy-MM-dd");
            Signname.Model.HealthExaminedUserInfo.oldtjsj = Signname.Model.HealthExaminedUserInfo.oldtjsj_br;
            SaveSignnameToDbAfterSignnameSuccess(strpath, "FKQZBR", textBox_benren.Text);

            //保存签名后刷新页面
            InitUserSignname();
        }

        /// <summary>
        /// 保存家属文字签名
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_jiashu_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox_jiashu.Text) == true)
            {
                MessageBox.Show("请录入[家属姓名]，再保存签字！");
                label_jiashu.Text = "请录入[家属姓名]，再保存签字！";
                textBox_jiashu.Focus();
                return;
            }
            Signname.Model.HealthExaminedUserInfo.CreateDate = dateTimePicker_jiashu.Value.ToString("yyyy-MM-dd");
            string strpath = piciSignnameByFamilyMembers.ImageLocation;
            Signname.Model.HealthExaminedUserInfo.oldtjsj = Signname.Model.HealthExaminedUserInfo.oldtjsj_js;
            SaveSignnameToDbAfterSignnameSuccess(strpath, "FKQZJS", textBox_jiashu.Text);

            //保存签名后刷新页面
            InitUserSignname();
        }
    }
}
