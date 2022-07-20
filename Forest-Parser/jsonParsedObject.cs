using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Forest_Parser
{
    public class PageJSON
    {
        public Data data { get; set; }
    }

    public class Data
    {
        public Searchreportwooddeal searchReportWoodDeal { get; set; }
    }

    public class Searchreportwooddeal
    {
        public Content[] content { get; set; }
        public string __typename { get; set; }
    }

    public class Content
    {
        public string? sellerName { get; set; }
        public string? sellerInn { get; set; }
        public string? buyerName { get; set; }
        public string? buyerInn { get; set; }
        public float? woodVolumeBuyer { get; set; }
        public float? woodVolumeSeller { get; set; }
        public string? dealDate { get; set; }
        public string? dealNumber { get; set; }
    }

}
