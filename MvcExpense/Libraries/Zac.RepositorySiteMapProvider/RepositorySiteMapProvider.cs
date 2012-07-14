using System;
using System.Collections.Specialized;
using System.Web;

namespace Zac.RepositorySiteMapProvider
{
    public class RepositorySiteMapProvider : SiteMapProvider
    {
        private const string SiteMapNodeRepositoryAttributeName = "siteMapNodeRepository";

        private ISiteMapNodeRepository _siteMapNodeRepository;
        private InternalSiteMapNode _rootRepositorySiteMapNode;

        public override void Initialize( string name, NameValueCollection attributes )
        {
            lock ( this )
            {
                base.Initialize( name, attributes );

                //Debug.Assert( attributes != null, "attributes != null" );

                string siteMapNodeRepositoryValue = attributes[SiteMapNodeRepositoryAttributeName];
                _siteMapNodeRepository = GetSiteMapNodeRepository( siteMapNodeRepositoryValue );

                LoadSiteMapFromStore();
            }
        }

        private static ISiteMapNodeRepository GetSiteMapNodeRepository( string siteMapNodeRepositoryValue )
        {
            if ( string.IsNullOrEmpty( siteMapNodeRepositoryValue ) )
            {
                string message = string.Format( "Required attribute '{0}' not found.", SiteMapNodeRepositoryAttributeName );
                throw new Exception( message );
            }

            Type type = Type.GetType( siteMapNodeRepositoryValue );

            if ( type == null )
            {
                string message = string.Format( "Could not load type '{0}'.", siteMapNodeRepositoryValue );
                throw new Exception( message );
            }

            var siteMapNodeRepository = Activator.CreateInstance( type ) as ISiteMapNodeRepository;

            if ( siteMapNodeRepository == null )
            {
                string interfaceFullName = typeof (ISiteMapNodeRepository).FullName;
                string message = string.Format( "SiteMapNodeRepository must implement the interface '{0}'.", interfaceFullName );
                throw new Exception( message );
            }

            return siteMapNodeRepository;
        }

        private void LoadSiteMapFromStore()
        {
            // If a root node exists, LoadSiteMapFromStore has already
            // been called, and the method can return.
            if ( _rootRepositorySiteMapNode != null )
            {
                return;
            }

            ExternalSiteMapNode root = _siteMapNodeRepository.GetTree();
            _rootRepositorySiteMapNode = CreateSiteMap( null, root );
        }

        private InternalSiteMapNode CreateSiteMap( InternalSiteMapNode parentInternalSiteMapNode, ExternalSiteMapNode externalSiteMapNode )
        {
            InternalSiteMapNode internalSiteMapNode;
            var siteMapNode = new SiteMapNode( this, externalSiteMapNode.Key, externalSiteMapNode.Url, externalSiteMapNode.Title, externalSiteMapNode.Description );

            if ( parentInternalSiteMapNode == null )
            {
                internalSiteMapNode = new InternalSiteMapNode( siteMapNode );
            }
            else
            {
                internalSiteMapNode = new InternalSiteMapNode( siteMapNode, parentInternalSiteMapNode );
            }

            foreach ( ExternalSiteMapNode child in externalSiteMapNode.Children )
            {
                CreateSiteMap( internalSiteMapNode, child );
            }

            return internalSiteMapNode;
        }

        public override SiteMapNode FindSiteMapNode( string rawUrl )
        {
            lock ( this )
            {
                InternalSiteMapNode repositorySiteMapNode = _rootRepositorySiteMapNode.GetNodeByUrl( rawUrl );

                if ( repositorySiteMapNode == null )
                {
                    return _rootRepositorySiteMapNode.SiteMapNode;
                    //return null;
                }

                return repositorySiteMapNode.SiteMapNode;
            }
        }

        public override SiteMapNodeCollection GetChildNodes( SiteMapNode node )
        {
            lock ( this )
            {
                var children = new SiteMapNodeCollection();

                InternalSiteMapNode repositorySiteMapNode = _rootRepositorySiteMapNode.GetNodeByKey( node.Key );

                foreach ( InternalSiteMapNode child in repositorySiteMapNode.Children )
                {
                    children.Add( child.SiteMapNode );
                }

                return children;
            }
        }

        public override SiteMapNode GetParentNode( SiteMapNode node )
        {
            lock ( this )
            {
                InternalSiteMapNode repositorySiteMapNode = _rootRepositorySiteMapNode.GetNodeByKey( node.Key );
                InternalSiteMapNode parent = repositorySiteMapNode.Parent;

                if ( parent == null )
                {
                    return null;
                }

                return parent.SiteMapNode;
            }
        }

        protected override SiteMapNode GetRootNodeCore()
        {
            lock ( this )
            {
                return _rootRepositorySiteMapNode.SiteMapNode;
            }
        }

        public void Refresh()
        {
            _rootRepositorySiteMapNode = null;
            LoadSiteMapFromStore();
        }

    }
}