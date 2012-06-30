using System.Linq;

namespace MvcExpense.UI.Models.Input
{
    public class SiteMapNodeChangeChildrenOrderInput
    {
        public string OrderedSiteMapNodeIdsString { get; set; }

        public long[] OrderedSiteMapNodeIds
        {
            get
            {
                return OrderedSiteMapNodeIdsString.Split( ',' ).Select( long.Parse ).ToArray();
            }
        }

    }
}