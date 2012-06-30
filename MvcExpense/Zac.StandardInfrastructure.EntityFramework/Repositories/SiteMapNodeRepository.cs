using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Zac.DesignPattern.EntityFramework.Repositories;
using Zac.StandardCore.Models;
using Zac.StandardCore.Repositories;

namespace Zac.StandardInfrastructure.EntityFramework.Repositories
{
    public class SiteMapNodeRepository : StandardRepository<SiteMapNode>, ISiteMapNodeRepository
    {
        public SiteMapNodeRepository( DbContext context )
            : base( context )
        {
        }

        public IEnumerable<SiteMapNode> GetChildrenByParentId( long id )
        {
            return GetQueryable().Where( x => x.ParentId == id ).AsEnumerable(); // todo don't use GetQueryable()
        }

    }
}
