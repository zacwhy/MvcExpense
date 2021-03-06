﻿using System;
using System.Collections.Generic;
using System.Transactions;
using Zac.StandardCore;
using Zac.StandardCore.Models;
using Zac.StandardCore.Repositories;
using Zac.StandardCore.Services;
using Zac.Tree;

namespace Zac.StandardInfrastructure.EntityFramework.Services
{
    class SiteMapNodeService : ISiteMapNodeService
    {
        private IStandardUnitOfWork StandardUnitOfWork;

        private ISiteMapNodeRepository SiteMapNodeRepository
        {
            get { return StandardUnitOfWork.SiteMapNodeRepository; }
        }

        public SiteMapNodeService( IStandardUnitOfWork standardUnitOfWork )
        {
            StandardUnitOfWork = standardUnitOfWork;
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
                StandardUnitOfWork.Save();
                scope.Complete();
            }
        }

        public void UpdateAndPlaceUnderNode( SiteMapNode entity, long id )
        {
            UpdateNode( entity, id, null );
        }

        public void UpdateAndPlaceAfterNode( SiteMapNode entity, long id )
        {
            UpdateNode( entity, 0, id );
        }

        private void UpdateNode( SiteMapNode entity, long parentId, long? previousSiblingId )
        {
            using ( var scope = new TransactionScope() )
            {
                SiteMapNodeRepository.UpdateNode( entity, parentId, previousSiblingId );
                SiteMapNodeRepository.Update( entity );
                StandardUnitOfWork.Save();
                scope.Complete();
            }
        }

        public void DeleteNode( long id )
        {
            SiteMapNodeRepository.DeleteNode( id );
            StandardUnitOfWork.Save();
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
