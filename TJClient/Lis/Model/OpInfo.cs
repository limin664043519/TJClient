using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TJClient.Lis.Model
{
    public class OpInfo
    {
        public string Begin { set; get; }
        public string End { set; get; }

        public string OpType { set; get; }

        public OpInfo(string begin, string end, string opType)
        {
            this.Begin = begin;
            this.End = end;
            this.OpType = opType;
        }
    }
}
