using System.Collections.Generic;
using System.Globalization;
using Zac.RepositorySiteMapProvider;
using Zac.StandardCore.Models;
using Zac.StandardInfrastructure.EntityFramework;
using Zac.Tree;

namespace MvcExpense.UI.RepositorySiteMapProvider
{
    public class SiteMapNodeRepository : ISiteMapNodeRepository
    {
        public ExternalSiteMapNode GetTree()
        {
            const string nameOrConnectionString = "zExpenseConnectionString"; // todo remove hardcode
            var dbContext = new StandardDbContext( nameOrConnectionString );
            Zac.StandardCore.Repositories.ISiteMapNodeRepository repository = 
                new Zac.StandardInfrastructure.EntityFramework.Repositories.SiteMapNodeRepository( dbContext );

            IEnumerable<SiteMapNode> siteMapNodes = repository.GetAll();
            TreeNode<SiteMapNode> root = TreeNodeHelper.CreateTree( siteMapNodes );

            return GetExternalSiteMap( root, null );
        }

        private ExternalSiteMapNode GetExternalSiteMap( TreeNode<SiteMapNode> siteMapNode, ExternalSiteMapNode parent )
        {
            var externalSiteMapNode = new ExternalSiteMapNode
            {
                Key = siteMapNode.Data.Id.ToString( CultureInfo.InvariantCulture ),
                Url = siteMapNode.Data.Url,
                Title = siteMapNode.Data.Title,
                Description = siteMapNode.Data.Description
            };

            foreach ( TreeNode<SiteMapNode> childSiteMapNode in siteMapNode.Children )
            {
                ExternalSiteMapNode childExternalSiteMapNode = GetExternalSiteMap( childSiteMapNode, externalSiteMapNode );
                externalSiteMapNode.Children.Add( childExternalSiteMapNode );
            }

            return externalSiteMapNode;
        }

    }
}