using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using Zac.StandardCore;
using Zac.StandardCore.Models;
using Zac.StandardCore.Repositories;
using Zac.StandardCore.Services;
using Zac.Tree;

namespace Zac.StandardMvc.Services
{
    class SiteMapNodeService : ISiteMapNodeService
    {
        private IStandardUnitOfWork _standardUnitOfWork;

        private ISiteMapNodeRepository SiteMapNodeRepository
        {
            get { return _standardUnitOfWork.SiteMapNodeRepository; }
        }

        public SiteMapNodeService( IStandardUnitOfWork standardUnitOfWork )
        {
            _standardUnitOfWork = standardUnitOfWork;
        }

        public IEnumerable<SiteMapNode> FindAll()
        {
            return SiteMapNodeRepository.GetAll();
        }

        public SiteMapNode FindById( long id )
        {
            return SiteMapNodeRepository.GetById( id );
        }

        public void Save( SiteMapNode entity )
        {
            throw new NotImplementedException();
        }

        public SiteMapNode GetParent( SiteMapNode entity )
        {
            var query =
                from x in SiteMapNodeRepository.GetQueryable()
                where x.Lft < entity.Lft && x.Rgt > entity.Lft
                orderby x.Lft descending
                select x;

            SiteMapNode parent = query.FirstOrDefault();
            return parent;
        }

        public TreeNode<SiteMapNode> GetTree()
        {
            IEnumerable<SiteMapNode> entities = SiteMapNodeRepository.GetAll();
            return TreeNodeHelper.CreateTree( entities );
        }

        public void InsertUnderNode( SiteMapNode entity, long id )
        {
            InsertNode( entity, id, null );
        }

        public void InsertAfterNode( SiteMapNode entity, long id )
        {
            InsertNode( entity, 0, id );
        }

        private void InsertNode( SiteMapNode entity, long parentId, long? previousSiblingId )
        {
            using ( var scope = new TransactionScope() )
            {
                SiteMapNodeRepository.InsertNode( entity, parentId, previousSiblingId );
                SiteMapNodeRepository.Insert( entity );
                _standardUnitOfWork.Save();
                scope.Complete();
            }
        }

        public void UpdateAndPositionUnderNode( SiteMapNode entity, long id )
        {
            UpdateNode( entity, id, null );
        }

        public void UpdateAndPositionAfterNode( SiteMapNode entity, long id )
        {
            UpdateNode( entity, 0, id );
        }

        private void UpdateNode( SiteMapNode entity, long parentId, long? previousSiblingId )
        {
            using ( var scope = new TransactionScope() )
            {
                SiteMapNodeRepository.UpdateNode( entity, parentId, previousSiblingId );
                SiteMapNodeRepository.Update( entity );
                _standardUnitOfWork.Save();
                scope.Complete();
            }
        }

        public void DeleteNode( long id )
        {
            SiteMapNodeRepository.DeleteNode( id );
            _standardUnitOfWork.Save();
        }

        public bool TryRefresh()
        {
            var siteMapProvider = System.Web.SiteMap.Provider as Zac.RepositorySiteMapProvider.RepositorySiteMapProvider;

            if ( siteMapProvider == null )
            {
                return false;
            }

            siteMapProvider.Refresh();
            return true;
        }

    }
}
