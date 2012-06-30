using System;
using System.Web;

namespace Zac.RepositorySiteMapProvider
{
    internal class InternalSiteMapNode : Node<SiteMapNode>
    {
        public new InternalSiteMapNode Parent
        {
            get
            {
                return (InternalSiteMapNode) base.Parent;
            }
        }

        public SiteMapNode SiteMapNode
        {
            get
            {
                return Value;
            }
        }

        public InternalSiteMapNode( SiteMapNode siteMapNode )
            : this( siteMapNode, null )
        {
        }

        public InternalSiteMapNode( SiteMapNode siteMapNode, InternalSiteMapNode parent )
            : base( siteMapNode, parent )
        {
            ValidateSiteMapNode( siteMapNode );
        }

        private void ValidateSiteMapNode( SiteMapNode siteMapNode )
        {
            var root = (InternalSiteMapNode) GetRoot();
            int count = GetNodeCountByKey( siteMapNode.Key, 0, root );

            if ( count < 1 )
            {
                string message = string.Format( "No SiteMapNode with key '{0}'", siteMapNode.Key );
                throw new Exception( message );
            }

            if ( count > 1 )
            {
                string message = string.Format( "Multiple SiteMapNode with key '{0}'. SiteMapNode key must be unique.",
                                                siteMapNode.Key );
                throw new Exception( message );
            }
        }

        private static int GetNodeCountByKey( string key, int count, InternalSiteMapNode node )
        {
            if ( node.SiteMapNode.Key == key )
            {
                count++;
            }

            foreach ( InternalSiteMapNode child in node.Children )
            {
                count += GetNodeCountByKey( key, count, child );
            }

            return count;
        }

        public InternalSiteMapNode GetNodeByKey( string key )
        {
            if ( key == SiteMapNode.Key )
            {
                return this;
            }

            foreach ( InternalSiteMapNode child in Children )
            {
                InternalSiteMapNode node = child.GetNodeByKey( key );

                if ( node != null )
                {
                    return node;
                }
            }

            return null;
        }

        public InternalSiteMapNode GetNodeByUrl( string url )
        {
            if ( url == SiteMapNode.Url )
            {
                return this;
            }

            foreach ( InternalSiteMapNode child in Children )
            {
                InternalSiteMapNode node = child.GetNodeByUrl( url );

                if ( node != null )
                {
                    return node;
                }
            }

            return null;
        }

    }
}