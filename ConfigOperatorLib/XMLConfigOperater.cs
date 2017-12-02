using System;
using System.Text;
using System.Xml;

namespace ConfigOperatorLib
{
    //class XMLPropertyName
    //{
    //    public const string CompanyWebPage = "CompanyWebPage"; 
    //    public const string SoftWareInfo = "SoftWareInfo";
    //    public const string CompanyName = "CompanyName";

    //    public const string MsgWebSite = "msgwebsite";
    //    public const string InitWebSite = "initwebsite";
    //    public const string UserCount = "UserCount";
    //    public const string DefaultBuyerIDd = "88";
    //    //其他的property
        

    //}

    public class XMLConfigOperater:IConfigOperator
    {
     private XmlDocument xDoc;

     private string strAppNodeName = "AppSettings//";
     public string strCfgFile = "app.config";
     private  string strConfigFilePath;
        public XMLConfigOperater()
     {
         strConfigFilePath = System.IO.Path.Combine(System.Environment.CurrentDirectory, strCfgFile);
            
           xDoc = new XmlDocument();
             xDoc.Load(strConfigFilePath);

     }
      ~XMLConfigOperater()
     {
         if (System.IO.File.Exists(strConfigFilePath))
         {
             xDoc.Save(strConfigFilePath);
         }
     }
        
        public  void SetProperty(string pName,string pValue)
        {
            XmlNode  xn= xDoc.SelectSingleNode("//"+strAppNodeName+pName);
            if (xn == null)
            {
                xn = xDoc.CreateElement(pName);
                xn.InnerText = pValue;
                XmlNode xndd = xDoc.LastChild.LastChild;//appsettings 节点
                xndd.InsertAfter(xn, xndd.LastChild);

            }
            else
            {
                xn.InnerText = pValue;
            }
            //xDoc.InsertAfter(xn, xDoc.LastChild.LastChild);
            //不确定是否会关闭文件句柄
          //  xDoc.Save(strConfigFilePath);
        }
         public  string getProperty(string pName)
        {
            XmlNode xn = xDoc.SelectSingleNode("//"+strAppNodeName+pName);
            return xn.InnerText;// = pValue;
        }

         public string WriteProperty(string pName)
         {
             XmlNode xn = xDoc.SelectSingleNode("//" + pName);
             return xn.InnerText;// = pValue;
         }

    }
}
