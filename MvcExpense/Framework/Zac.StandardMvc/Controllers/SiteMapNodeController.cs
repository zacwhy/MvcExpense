using System;
using System.Web.Mvc;
using AutoMapper;
using Microsoft.Web.Mvc;
using Zac.MvcFlashMessage;
using Zac.StandardCore.Services;
using Zac.StandardMvc.Models.Display;
using Zac.StandardMvc.Models.Input;
using Zac.Tree;
using SiteMapNode = Zac.StandardCore.Models.SiteMapNode;

namespace Zac.StandardMvc.Controllers
{
    public partial class SiteMapNodeController : AbstractStandardController
    {
        private ISiteMapNodeService SiteMapNodeService
        {
            get { return StandardServices.SiteMapNodeService; }
        }

        public SiteMapNodeController( IStandardServices standardServices )
            : base( standardServices )
        {
        }

        public virtual ActionResult Refresh()
        {
            string flashMessage;

            if ( SiteMapNodeService.TryRefresh() )
            {
                flashMessage = string.Format( "Refreshed at {0}.", DateTime.Now );
            }
            else
            {
                flashMessage = "Could not refresh site map.";
            }

            return this.RedirectToAction( x => x.Index() ).WithFlash( new { notice = flashMessage } );
        }

        public virtual ActionResult Tree()
        {
            TreeNode<SiteMapNode> tree = SiteMapNodeService.GetTree();
            return View( tree );
        }

        public virtual ActionResult Index()
        {
            return this.RedirectToAction( x => x.Tree() );
        }

        public virtual ActionResult List()
        {
            var list = SiteMapNodeService.FindAll();
            return View( list );
        }

        public virtual ActionResult Create()
        {
            var display = new SiteMapNodeCreateDisplay();
            PopulateCreateDisplay( display );
            return View( display );
        }

        [HttpPost]
        public virtual ActionResult Create( SiteMapNodeCreateInput input )
        {
            if ( ModelState.IsValid )
            {
                try
                {
                    SiteMapNode model = ToModel( input );

                    if ( input.PreviousSiblingId.HasValue )
                    {
                        SiteMapNodeService.InsertAfterNode( model, input.PreviousSiblingId.Value );
                    }
                    else
                    {
                        SiteMapNodeService.InsertUnderNode( model, input.ParentId );
                    }

                    return this.RedirectToAction( x => x.Index() );
                }
                catch ( Exception ex )
                {
                    // todo display error
                    ErrorLogHelper.Log( ex );
                    throw; // todo remove
                }
            }

            SiteMapNodeCreateDisplay display = ToCreateDisplay( input );
            PopulateCreateDisplay( display );
            return View( display );
        }

        public virtual ActionResult Edit( long id )
        {
            SiteMapNode model = SiteMapNodeService.FindById( id );
            SiteMapNodeEditDisplay display = ToEditDisplay( model );
            PopulateEditDisplay( display );
            return View( display );
        }

        [HttpPost]
        public virtual ActionResult Edit( SiteMapNodeEditInput input )
        {
            if ( ModelState.IsValid )
            {
                try
                {
                    SiteMapNode model = ToModel( input );

                    if ( input.PreviousSiblingId.HasValue )
                    {
                        SiteMapNodeService.UpdateAndPlaceAfterNode( model, input.PreviousSiblingId.Value );
                    }
                    else
                    {
                        SiteMapNodeService.UpdateAndPlaceUnderNode( model, input.ParentId );
                    }

                    return this.RedirectToAction( x => x.Index() );
                }
                catch /*( Exception ex )*/
                {
                    // todo log and display error and remove throw
                    throw;
                }
            }

            SiteMapNodeEditDisplay display = ToEditDisplay( input );
            PopulateEditDisplay( display );
            return View( display );
        }

        public virtual ActionResult Delete( long id )
        {
            SiteMapNode model = SiteMapNodeService.FindById( id );
            return View( model );
        }

        [HttpPost, ActionName( "Delete" )]
        public virtual ActionResult DeleteConfirmed( long id )
        {
            try
            {
                SiteMapNodeService.DeleteNode( id );
                string flashMessage = string.Format( "Deleted Id {0}.", id ); // todo improve message
                return this.RedirectToAction( x => x.Index() ).WithFlash( new { notice = flashMessage } );
            }
            catch
            {
                // todo log and display error and remove throw
                throw;
            }
        }

        private void PopulateCreateDisplay( SiteMapNodeCreateDisplay display )
        {
            display.Tree = SiteMapNodeService.GetTree();
        }

        private void PopulateEditDisplay( SiteMapNodeEditDisplay display )
        {
            TreeNode<SiteMapNode> tree = SiteMapNodeService.GetTree();
            TreeNode<SiteMapNode> currentNode = TreeNodeHelper.FindNodeById( tree, display.Id );

            if ( currentNode.PreviousSibling != null )
            {
                display.PreviousSiblingId = currentNode.PreviousSibling.Data.Id;
            }

            tree.RemoveNode( currentNode );
            display.Tree = tree;
        }

        private static SiteMapNode ToModel( SiteMapNodeCreateInput input )
        {
            return Mapper.Map<SiteMapNodeCreateInput, SiteMapNode>( input );
        }

        private static SiteMapNode ToModel( SiteMapNodeEditInput input )
        {
            return Mapper.Map<SiteMapNodeEditInput, SiteMapNode>( input );
        }

        private SiteMapNodeCreateDisplay ToCreateDisplay( SiteMapNodeCreateInput input )
        {
            return Mapper.Map<SiteMapNodeCreateInput, SiteMapNodeCreateDisplay>( input );
        }

        private static SiteMapNodeEditDisplay ToEditDisplay( SiteMapNode model )
        {
            return Mapper.Map<SiteMapNode, SiteMapNodeEditDisplay>( model );
        }

        private SiteMapNodeEditDisplay ToEditDisplay( SiteMapNodeEditInput input )
        {
            return Mapper.Map<SiteMapNodeEditInput, SiteMapNodeEditDisplay>( input );
        }

        public static void CreateMaps()
        {
            Mapper.CreateMap<SiteMapNodeCreateInput, SiteMapNode>();
            Mapper.CreateMap<SiteMapNodeEditInput, SiteMapNode>();
            Mapper.CreateMap<SiteMapNodeCreateInput, SiteMapNodeCreateDisplay>();
            Mapper.CreateMap<SiteMapNode, SiteMapNodeEditDisplay>();
            Mapper.CreateMap<SiteMapNodeEditInput, SiteMapNodeEditDisplay>();
        }

    }
}
