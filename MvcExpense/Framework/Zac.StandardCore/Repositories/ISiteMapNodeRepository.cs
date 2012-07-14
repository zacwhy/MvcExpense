using System.Collections.Generic;
using Zac.DesignPattern.Repositories;
using Zac.StandardCore.Models;

namespace Zac.StandardCore.Repositories
{
    public interface ISiteMapNodeRepository : IStandardRepository<SiteMapNode>
    {
        IEnumerable<SiteMapNode> GetChildrenByParentId( long id );
        void InsertNode( SiteMapNode entity, long parentId, long? previousSiblingId );
        void UpdateNode( SiteMapNode entity, long parentId, long? previousSiblingId );
        void DeleteNode( long id );
    }
}
