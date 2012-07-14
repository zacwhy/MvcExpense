using System;
using System.Collections.Generic;
using System.Linq;
using Zac.StandardCore.Models;

namespace Zac.StandardCore.Helpers
{
    public static class SiteMapNodeHelper
    {
        //public static SiteMapNode GetRoot( IEnumerable<SiteMapNode> siteMapNodes )
        //{
        //    List<SiteMapNode> list = siteMapNodes.ToList();
        //    Func<SiteMapNode, bool> predicate = x => x.ParentId.HasValue == false;
        //    int count = list.Count( predicate );

        //    if ( count < 1 )
        //    {
        //        throw new Exception( "There is no root." );
        //    }

        //    if ( count > 1 )
        //    {
        //        throw new Exception( "There should only be one root." );
        //    }

        //    return list.Single( predicate );
        //}

        //public static SiteMapNode FindNodeById( SiteMapNode tree, long id )
        //{
        //    if ( tree.Id == id )
        //    {
        //        return tree;
        //    }

        //    foreach ( SiteMapNode child in tree.Children )
        //    {
        //        SiteMapNode siteMapNode = FindNodeById( child, id );
        //        if ( siteMapNode != null )
        //        {
        //            return siteMapNode;
        //        }
        //    }

        //    return null;
        //}

    }
}
