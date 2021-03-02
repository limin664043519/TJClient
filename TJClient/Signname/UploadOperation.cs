using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace TJClient.Signname
{
    public class UploadOperation
    {
        private static DataTable _uploadSignnameDataTable = null;

        public static void SetUploadSignnameDataTable(DataTable dt)
        {
            _uploadSignnameDataTable = dt;
        }

        public static DataTable GetUploadSignnameDataTable()
        {
            return _uploadSignnameDataTable;
        }
    }
}
