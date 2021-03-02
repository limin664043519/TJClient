using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TJClient.model
{
    public class LisTmCorresponding
    {
        public int Id { set; get; }
        public string QmLisTm { set; get; }
        public string LisTm { set; get; }

        public int IncludeBlood { set; get; }
        public string CreateDate { set; get; }
        public int Status { set; get; }
    }
}
