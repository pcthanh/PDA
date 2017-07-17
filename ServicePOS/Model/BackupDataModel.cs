using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicePOS.Model
{
    public class BackupDataModel
    {
        public int ID { get; set; }

        public string FileName { get; set; }
        public string CreateDate { get; set; }
        public string FullName { get; set; }

        public string Description { get; set; }

        public string Status { get; set; }
    }
}
