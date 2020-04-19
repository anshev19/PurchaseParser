using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PurchaseParser
{
    public class PurchaseData
    {
        public string Title { get; set; } 
        public string Customer { get; set; }
        public string Price { get; set; }
        public string AllocationDate { get; set; }
        public string UpdatedDate { get; set; }
        public string PartitionFz { get; set; }
        public string PurchaseNumber { get; set; }
        public string PurchaseType { get; set; }
        public string PurchaseStatus { get; set; }

    }
}
