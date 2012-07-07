using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Microsoft.Web.Mvc;
using MvcExpense.Core;
using MvcExpense.UI.Models.Display;
using MvcExpense.UI.Models.Input;
using Zac.MvcFlashMessage;
using Zac.Tree;
using SiteMapNode = Zac.StandardCore.Models.SiteMapNode;

namespace MvcExpense.UI.Controllers
{
    public partial class SiteMapNodeController : AbstractMvcExpenseController
    {
        private Zac.StandardCore.Repositories.ISiteMapNodeRepository Repository
        {
            get { return UnitOfWork.SiteMapNodeRepository; }
        }

        public SiteMapNodeController( IMvcExpenseUnitOfWork unitOfWork )
            : base( unitOfWork )
        {
        }

        public virtual ActionResult Refresh()
        {
            var siteMapProvider = SiteMap.Provider as Zac.RepositorySiteMapProvider.RepositorySiteMapProvider;

            string flashMessage;

            if ( siteMapProvider != null )
            {
                siteMapProvider.Refresh();
                flashMessage = string.Format( "Refreshed at {0}.", DateTime.Now );
            }
            else
            {
                // todo log SiteMap.Provider is not Zac.RepositorySiteMapProvider.RepositorySiteMapProvider
                flashMessage = "Could not refresh site map.";
            }

            return this.RedirectToAction( x => x.Index() ).WithFlash( new { notice = flashMessage } );
        }

        public virtual ActionResult Tree()
        {
            TreeNode<SiteMapNode> tree = GetTree();
            return View( tree );
        }

        public virtual ActionResult Index()
        {
            return this.RedirectToAction( x => x.Tree() );
        }

        public virtual ActionResult List()
        {
            var list = Repository.GetAll();
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


                    // move this away
                    const long subtreeSize = 2; // left - right + 1
                    if ( input.PreviousSibling.HasValue )
                    {
                        SiteMapNode previousSibiling = Repository.GetById( input.PreviousSibling.Value ); // rgt
                        model.Lft = previousSibiling.Rgt + 1;
                    }
                    else
                    {
                        SiteMapNode parent = Repository.GetById( input.ParentId ); // lft
                        model.Lft = parent.Lft + 1;
                    }
                    model.Rgt = model.Lft + subtreeSize - 1;


                    Repository.Insert( model );
                    UnitOfWork.Save();
                    return this.RedirectToAction( x => x.Index() );
                }
                catch ( Exception ex )
                {
                    // todo log and display error and remove throw
                    throw;
                }
            }

            SiteMapNodeCreateDisplay display = ToCreateDisplay( input );
            PopulateCreateDisplay( display );
            return View( display );
        }

        public virtual ActionResult Edit( long id )
        {
            SiteMapNode model = Repository.GetById( id );
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
                    Repository.Update( model );
                    UnitOfWork.Save();
                    return this.RedirectToAction( x => x.Index() );
                }
                catch ( Exception ex )
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
            SiteMapNode model = Repository.GetById( id );
            return View( model );
        }

        [HttpPost, ActionName( "Delete" )]
        public virtual ActionResult DeleteConfirmed( long id )
        {
            Repository.Delete( id );
            UnitOfWork.Save();
            string flashMessage = string.Format( "Deleted Id {0}.", id ); // todo improve message
            return this.RedirectToAction( x => x.Index() ).WithFlash( new { notice = flashMessage } );
        }

        private TreeNode<SiteMapNode> GetTree()
        {
            IEnumerable<SiteMapNode> siteMapNodes = Repository.GetAll();
            return TreeNodeHelper.CreateTree( siteMapNodes );
        }

        private void PopulateCreateDisplay( SiteMapNodeCreateDisplay display )
        {
            display.Tree = GetTree();
        }

        private void PopulateEditDisplay( SiteMapNodeEditDisplay display )
        {
            TreeNode<SiteMapNode> tree = GetTree();
            TreeNode<SiteMapNode> currentNode = TreeNodeHelper.FindNodeById( tree, display.Id );

            if ( currentNode.PreviousSibling != null )
            {
                display.PreviousSibling = currentNode.PreviousSibling.Data.Id;
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
