using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Web.Routing;

namespace System.Web.Mvc
{
    public static class GroupSelectExtensions
    {
        public static MvcHtmlString GroupDropDownListFor<TModel, TProperty>( this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression,
            IEnumerable<SelectListGroupItem> selectList,
            string optionLabel )
        {
            string name = ExpressionHelper.GetExpressionText( expression );
            object selectedValue = htmlHelper.ViewData.Eval( name );
            return GetMvcHtmlString( htmlHelper, name, selectList, optionLabel, selectedValue, null );
        }

        private static MvcHtmlString GetMvcHtmlString( this HtmlHelper htmlHelper,
            string name,
            IEnumerable<SelectListGroupItem> selectList,
            string optionLabel,
            object selectedValue,
            object htmlAttributes )
        {
            string htmlString = GetHtmlString( htmlHelper, name, selectList, optionLabel, selectedValue, htmlAttributes );
            return MvcHtmlString.Create( htmlString );
        }

        private static string GetHtmlString( this HtmlHelper htmlHelper,
            string name,
            IEnumerable<SelectListGroupItem> selectList,
            string optionLabel,
            object selectedValue,
            object htmlAttributes )
        {
            if ( selectList == null && htmlHelper.ViewData != null )
            {
                selectList = htmlHelper.ViewData.Eval( name ) as IEnumerable<SelectListGroupItem>;
            }

            if ( selectList == null )
            {
                return string.Empty;
            }

            var selectTag = new TagBuilder( "select" );
            selectTag.GenerateId( name );
            selectTag.Attributes.Add( "name", htmlHelper.Encode( name ) );

            if ( htmlAttributes != null )
            {
                selectTag.MergeAttributes( new RouteValueDictionary( htmlAttributes ) );
            }

            var selectTagInnerHtml = new StringBuilder();
            //List<SelectListGroupItem> groups = selectList.ToList();

            if ( optionLabel != null )
            {
                var optionTag = new TagBuilder( "option" );
                optionTag.InnerHtml = htmlHelper.Encode( optionLabel );
                selectTagInnerHtml.AppendLine( optionTag.ToString( TagRenderMode.Normal ) );
            }

            foreach ( SelectListGroupItem group in selectList )
            {
                var optgroupTag = new TagBuilder( "optgroup" );
                optgroupTag.Attributes.Add( "label", htmlHelper.Encode( group.Name ) );
                var optgroupTagInnerHtml = new StringBuilder();

                foreach ( var item in group.Items )
                {
                    var optionTag = new TagBuilder( "option" );
                    optionTag.Attributes.Add( "value", htmlHelper.Encode( item.Value ) );

                    if ( selectedValue != null && item.Value == selectedValue.ToString() )
                    {
                        optionTag.Attributes.Add( "selected", "selected" );
                    }

                    optionTag.InnerHtml = htmlHelper.Encode( item.Text );
                    optgroupTagInnerHtml.AppendLine( optionTag.ToString( TagRenderMode.Normal ) );
                }

                optgroupTag.InnerHtml = optgroupTagInnerHtml.ToString();
                selectTagInnerHtml.AppendLine( optgroupTag.ToString( TagRenderMode.Normal ) );
            }

            selectTag.InnerHtml = selectTagInnerHtml.ToString();
            return selectTag.ToString( TagRenderMode.Normal );
        }
    }

}
