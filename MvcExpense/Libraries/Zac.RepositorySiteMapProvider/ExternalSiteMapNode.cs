using System.Collections.Generic;

namespace Zac.RepositorySiteMapProvider
{
    public class ExternalSiteMapNode
    {
        public ExternalSiteMapNode()
        {
            Children = new HashSet<ExternalSiteMapNode>();
        }
    
        public string Key { get; set; }
        public string Url { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        //public ExternalSiteMapNode Parent { get; set; }
        public ICollection<ExternalSiteMapNode> Children { get; private set; }
    }
}