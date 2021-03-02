using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Xml;
using System.Collections.Generic;
using System.Text;
using System.IO;
using TJClient.Common;
namespace TJClient.Common
{
    public class XmlRW
    {
        //WriteXml 完成对User的添加操作 
        //FileName 当前xml文件的存放位置
        //UserCode 欲添加用户的编码
        //UserName 欲添加用户的姓名
        //UserPassword 欲添加用户的密码

        public static void WriteXML(string FileName, string code, string value, string element)
        {
            ////按照文件创建数据库
            //createXmlFile(FileName);

            //初始化XML文档操作类
            XmlDocument myDoc = new XmlDocument();
            //加载XML文件
            myDoc.Load(FileName);

            XmlNode xmlnode = null;
            if (myDoc.InnerXml.IndexOf(element) > -1)
            {
                xmlnode = myDoc.GetElementsByTagName(element)[0].CloneNode(true);
                myDoc.GetElementsByTagName(element)[0].ParentNode.RemoveChild(myDoc.GetElementsByTagName(element)[0]);
                //id
                xmlnode.ChildNodes[0].InnerText = code;
                //value
                xmlnode.ChildNodes[1].InnerText = value;
                myDoc.LastChild.AppendChild(xmlnode);
            }
            else
            {

                //添加元素--UserCode
                XmlElement ele = myDoc.CreateElement("text");
                XmlText text = myDoc.CreateTextNode(code);

                //添加元素--UserName
                XmlElement ele1 = myDoc.CreateElement("value");
                XmlText text1 = myDoc.CreateTextNode(value);

                //添加节点 User要对应我们xml文件中的节点名字
                XmlNode newElem = myDoc.CreateNode("element", element, "");

                //在节点中添加元素
                newElem.AppendChild(ele);
                newElem.LastChild.AppendChild(text);
                newElem.AppendChild(ele1);
                newElem.LastChild.AppendChild(text1);

                //将节点添加到文档中
                XmlElement root = myDoc.DocumentElement;
                root.AppendChild(newElem);
            }
            //保存
            myDoc.Save(FileName);

        }


        //WriteXml 完成对User的修改密码操作
        //FileName 当前xml文件的存放位置
        //UserCode 欲操作用户的编码
        //UserPassword 欲修改用户的密码

        public void UpdateXML(string FileName, string UserCode, string time)
        {

            //初始化XML文档操作类
            XmlDocument myDoc = new XmlDocument();
            //加载XML文件
            myDoc.Load(FileName);

            //搜索指定的节点
            System.Xml.XmlNodeList nodes = myDoc.SelectNodes("//UserSqlConfig.xml");

            if (nodes != null)
            {
                foreach (System.Xml.XmlNode xn in nodes)
                {
                    if (xn.SelectSingleNode("userId").InnerText == UserCode)
                    {
                        xn.SelectSingleNode("userFreshTime").InnerText = time;
                    }

                }
            }

            myDoc.Save(FileName);

        }
        //CheckExistNode 查找userid是否存在 
        //FileName 当前xml文件的存放位置
        //UserCode 欲添加用户的编码

        public bool CheckExistNode(string FileName, string UserCode, string element)
        {
            //初始化XML文档操作类
            XmlDocument myDoc = new XmlDocument();
            //加载XML文件
            myDoc.Load(FileName);

            //搜索指定某列，一般是主键列
            XmlNodeList myNode = myDoc.SelectNodes("//" + element);

            //判断是否有这个节点

            if (!(myNode == null))
            {
                //遍历节点，找到符合条件的元素

                foreach (XmlNode xn in myNode)
                {
                    if (xn.InnerXml == UserCode)
                        return true;
                }
            }
            return false;
        }

        //GetTimeFormXML 读取xml文件更新时间信息
        //FileName 当前xml文件的存放位置
        //UserCode 欲操作用户的编码

        public string GetValueFormXML(string FileName, string element)
        {
            try
            {
                //按照文件创建数据库
                //createXmlFile(FileName);

                //初始化XML文档操作类
                XmlDocument myDoc = new XmlDocument();
                //加载XML文件
                myDoc.Load(FileName);

                //搜索指定的节点
                XmlNode xmlnode = null;
                if (myDoc.InnerXml.IndexOf(element) > -1)
                {
                    xmlnode = myDoc.GetElementsByTagName(element)[0].CloneNode(true);
                    //value
                    return xmlnode.ChildNodes[1].InnerText;

                }
                else
                {
                    return "";
                }
            }
            catch (Exception ex)
            {
                throw new Exception (("GetValueFormXML错误!"+ex.Message)) ;
                //return "";
            }

        }

        /// <summary>
        /// 取得sql
        /// </summary>
        /// <param name="FileName"></param>
        /// <param name="element"></param>
        /// <returns></returns>
        public static string GetSqlFormXML(string FileName, string element)
        {
            try
            {
                ////按照文件创建数据库
                //createXmlFile(FileName);

                //初始化XML文档操作类
                XmlDocument myDoc = new XmlDocument();
                //加载XML文件
                myDoc.Load(FileName);

                //搜索指定的节点
                XmlNode xmlnode = null;
                if (myDoc.InnerXml.IndexOf(element) > -1)
                {
                    xmlnode = myDoc.GetElementsByTagName(element)[0].CloneNode(true);
                    //sql
                    for (int i = 0; i < xmlnode.ChildNodes.Count; i++)
                    {
                        if (xmlnode.ChildNodes[i].Name.ToLower().Equals("sql") == true)
                        {
                            return xmlnode.ChildNodes[i].InnerText;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(("GetValueFormXML错误!" + ex.Message));
            }
            return "";

        }

        /// <summary>
        /// 取得数据值
        /// </summary>
        /// <param name="FileName"></param>
        /// <param name="element"></param>
        /// <returns></returns>
        public static string GetValueFormXML(string FileName, string element, string code)
        {
            try
            {
                ////按照文件创建数据库
                //createXmlFile(FileName);

                //初始化XML文档操作类
                XmlDocument myDoc = new XmlDocument();
                //加载XML文件
                myDoc.Load(FileName);

                //搜索指定的节点
                XmlNode xmlnode = null;
                if (myDoc.InnerXml.IndexOf(element) > -1)
                {
                    xmlnode = myDoc.GetElementsByTagName(element)[0].CloneNode(true);
                    //sql
                    for (int i = 0; i < xmlnode.ChildNodes.Count; i++)
                    {
                        if (xmlnode.ChildNodes[i].Name.ToLower().Equals(code.ToLower()) == true)
                        {
                            return xmlnode.ChildNodes[i].InnerText;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(("GetValueFormXML错误!" + ex.Message));
            }
            return "";

        }

        public static void WriteXML_set(string FileName, string text, string value, string element)
        {
            ////按照文件创建数据库
            //createXmlFile(FileName);

            //初始化XML文档操作类
            XmlDocument myDoc = new XmlDocument();
            //加载XML文件
            myDoc.Load(FileName);

            XmlNode xmlnode = null;
            if (myDoc.InnerXml.IndexOf(element) > -1)
            {
                xmlnode = myDoc.GetElementsByTagName(element)[0].CloneNode(true);
                myDoc.GetElementsByTagName(element)[0].ParentNode.RemoveChild(myDoc.GetElementsByTagName(element)[0]);
                //text
                xmlnode.ChildNodes[0].InnerText = text;
                //value
                xmlnode.ChildNodes[1].InnerText = value;
                myDoc.LastChild.AppendChild(xmlnode);
            }
            else
            {

                //添加元素--UserCode
                XmlElement ele = myDoc.CreateElement("text");
                XmlText text2 = myDoc.CreateTextNode(text);

                //添加元素--UserName
                XmlElement ele1 = myDoc.CreateElement("value");
                XmlText text1 = myDoc.CreateTextNode(value);

                //添加节点 User要对应我们xml文件中的节点名字
                XmlNode newElem = myDoc.CreateNode("element", element, "");

                //在节点中添加元素
                newElem.AppendChild(ele);
                newElem.LastChild.AppendChild(text2);
                newElem.AppendChild(ele1);
                newElem.LastChild.AppendChild(text1);

                //将节点添加到文档中
                XmlElement root = myDoc.DocumentElement;
                root.AppendChild(newElem);
            }
            //保存
            myDoc.Save(FileName);

        }



        /// <summary>
        /// 取得数据值
        /// </summary>
        /// <param name="FileName"></param>
        /// <param name="element"></param>
        /// <returns></returns>
        public static string GetValueFormXMLstr(string xmlString, string element, string code)
        {
            try
            {
                //初始化XML文档操作类
                XmlDocument myDoc = new XmlDocument();
                //加载XML文件
                //myDoc.Load(FileName);
                myDoc.LoadXml(xmlString);

                //搜索指定的节点
                XmlNode xmlnode = null;
                if (myDoc.InnerXml.IndexOf(element) > -1)
                {
                    xmlnode = myDoc.GetElementsByTagName(element)[0].CloneNode(true);
                    //sql
                    for (int i = 0; i < xmlnode.ChildNodes.Count; i++)
                    {
                        if (xmlnode.ChildNodes[i].Name.ToLower().Equals(code.ToLower()) == true)
                        {
                            return xmlnode.ChildNodes[i].InnerText;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(("GetValueFormXML错误!" + ex.Message + string.Format("【{0}】", xmlString)));
            }
            return "";

        }
    }
}
