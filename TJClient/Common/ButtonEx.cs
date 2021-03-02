using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FBYClient
{
    public class ButtonEx : Button
    {
        public ButtonEx()
        {

        }

        protected override bool ShowFocusCues
        {
            get
            {
                return false;
            }
        }
    }
}
