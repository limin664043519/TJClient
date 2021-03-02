using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TJClient.Signname.Model
{
    public class Tablet
    {

        private string _tabletType = "";
        public string Who { set; get; }
        public string Why { set; get; }
        public string What { set; get; }
        public string SaveSignnamePicPath { set; get; }

        public string TabletType
        {
            set { _tabletType = value; }
            get
            {
                if (_tabletType == "")
                {
                    if (System.Configuration.ConfigurationManager.AppSettings["TabletType"] != null)
                    {
                        _tabletType = System.Configuration.ConfigurationManager.AppSettings["TabletType"];
                    }
                }
                return _tabletType;
            }
        }

    }
}
