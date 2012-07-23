using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Zac.StandardCore.Models;
using Zac.StandardCore.Repositories;

namespace Zac.StandardMvc.Tests.Repositories
{
    class MockSiteMapNodeRepository : ISiteMapNodeRepository
    {
        ICollection<SiteMapNode> _siteMapNodes = new List<SiteMapNode>();

        #region ISiteMapNodeRepository Members

        public void InsertNode( SiteMapNode entity, long parentId, long? previousSiblingId )
        {
            _siteMapNodes.Add( entity );
        }

        public void UpdateNode( SiteMapNode entity, long parentId, long? previousSiblingId )
        {
            throw new NotImplementedException();
        }

        public void DeleteNode( long id )
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IRepository<SiteMapNode,long> Members

        public IQueryable<SiteMapNode> GetQueryable()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<SiteMapNode> GetAll()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<SiteMapNode> Get( Expression<Func<SiteMapNode, bool>> filter = null, Func<IQueryable<SiteMapNode>, IOrderedQueryable<SiteMapNode>> orderBy = null, string includeProperties = "" )
        {
            throw new NotImplementedException();
        }

        public SiteMapNode GetById( long id )
        {
            throw new NotImplementedException();
        }

        public void Insert( SiteMapNode entity )
        {
            //throw new NotImplementedException();
        }

        public void Delete( long id )
        {
            throw new NotImplementedException();
        }

        public void Update( SiteMapNode entity )
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
