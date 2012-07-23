using System.Collections.Generic;
using Zac.DesignPattern.Services;
using Zac.StandardCore.Models;
using Zac.Tree;

namespace Zac.StandardCore.Services
{
    public interface ISiteMapNodeService : IStandardService<SiteMapNode>
    {
        IEnumerable<SiteMapNode> FindAll();
        SiteMapNode FindById( long id );
        void Save( SiteMapNode entity );

        SiteMapNode GetParent( SiteMapNode entity );
        TreeNode<SiteMapNode> GetTree();
        void InsertUnderNode( SiteMapNode entity, long id );
        void InsertAfterNode( SiteMapNode entity, long id );
        void UpdateAndPositionUnderNode( SiteMapNode entity, long id );
        void UpdateAndPositionAfterNode( SiteMapNode entity, long id );
        void DeleteNode( long id );

        bool TryRefresh();
    }
}
