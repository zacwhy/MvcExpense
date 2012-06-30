using System.Collections.Generic;
using Zac.DesignPattern.Models;
using Zac.Tree;

namespace Zac.StandardCore.Models
{
    public class SiteMapNode : StandardPersistentObject, ITreeNodeData
    {
        public SiteMapNode()
        {
            Children = new HashSet<SiteMapNode>();
        }

        public long? ParentId { get; set; }
        public long Lft { get; set; }
        public long Rgt { get; set; }
        public string Url { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }

        public virtual ICollection<SiteMapNode> Children { get; private set; }
        public virtual SiteMapNode Parent { get; set; }
    }
}
