using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Web.Mvc;
using Zac.StandardCore.Models;
using Zac.Tree;

namespace Zac.Mvc.Extensions
{
    public static class SiteMapNodeExtensions
    {
        public static string ToJson( this TreeNode<SiteMapNode> node )
        {
            var json = new StringBuilder();
            json.Append( "{ " );
            json.AppendFormat( "'id': {0}", node.Data.Id );
            json.AppendFormat( ", 'data': {{ 'title': '{0}' }}", node.Data.Title );

            if ( node.HasChildren )
            {
                var children = new StringBuilder();

                int i = 0;
                foreach ( TreeNode<SiteMapNode> child in node.Children )
                {
                    if (i > 0)
                    {
                        children.Append( ", " );
                    }
                    string childJson = ToJson( child );
                    children.AppendFormat( "{0}", childJson );
                    i++;
                }

                json.AppendFormat( ", 'children': [ {0} ]", children );
            }

            json.Append( " }" );
            return json.ToString();
        }

        public static SelectList ToSelectList( this TreeNode<SiteMapNode> node )
        {
            List<SelectListItem> items = BuildSelectListItems( node );
            return new SelectList( items, "Value", "Text" );
        }

        private static List<SelectListItem> BuildSelectListItems( TreeNode<SiteMapNode> node, List<SelectListItem> items = null, SelectListItem parentSelectListItem = null )
        {
            var selectListItem = new SelectListItem();

            string text = string.Empty;

            if ( parentSelectListItem != null )
            {
                text = parentSelectListItem.Text + " > ";
            }

            text += node.Data.Title;

            selectListItem.Text = text;
            selectListItem.Value = node.Data.Id.ToString( CultureInfo.InvariantCulture );

            if ( items == null )
            {
                items = new List<SelectListItem>();
            }

            items.Add( selectListItem );

            foreach ( TreeNode<SiteMapNode> childSiteMapNode in node.Children )
            {
                BuildSelectListItems( childSiteMapNode, items, selectListItem );
            }

            return items;
        }

    }
}