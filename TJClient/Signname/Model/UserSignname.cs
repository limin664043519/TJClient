using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TJClient.Signname.Model
{
    public class UserSignname
    {
        public string SignnameTitle { set; get; }
        public string SignnamePicPath { set; get; }
        public string Realname { set; get; }

        public UserSignname()
        {
            
        }

        public UserSignname(string signnameTitle, string signnamePicPath, string realname)
        {
            this.SignnameTitle = signnameTitle;
            this.SignnamePicPath = signnamePicPath;
            this.Realname = realname;
        }

        public override string ToString()
        {
            return this.SignnameTitle;
        }


        public string Key
        {

            get
            {

                return this.SignnameTitle;

            }

            set
            {

                this.SignnameTitle = value;

            }

        }

        public string Value
        {

            get
            {

                return this.SignnamePicPath!=""? this.SignnamePicPath:this.Realname;

            }

            set
            {

                if (value.Length > 5)
                {
                    this.SignnamePicPath = value;
                }
                else
                {
                    this.Realname = value;
                }


            }

        }
    }
}
