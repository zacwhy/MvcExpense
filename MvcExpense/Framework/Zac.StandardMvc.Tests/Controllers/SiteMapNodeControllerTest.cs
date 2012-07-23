using System.Collections.Generic;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using Zac.StandardCore;
using Zac.StandardCore.Models;
using Zac.StandardCore.Repositories;
using Zac.StandardCore.Services;
using Zac.StandardMvc.Controllers;
using Zac.StandardMvc.Models.Display;
using Zac.StandardMvc.Models.Input;
using Zac.StandardMvc.Tests.Repositories;
using Zac.Tree;

namespace Zac.StandardMvc.Tests.Controllers
{
    [TestClass]
    public class SiteMapNodeControllerTest
    {
        [TestMethod]
        public void Index()
        {
            // Arrange
            var standardServices = GenerateStub<IStandardServices>();
            var controller = new SiteMapNodeController( standardServices );

            // Act
            var result = (RedirectToRouteResult) controller.Index();

            // Assert
            Assert.AreEqual( "SiteMapNode", result.RouteValues["controller"] );
            Assert.AreEqual( "Tree", result.RouteValues["action"] );
        }

        [TestMethod]
        public void List()
        {
        }

        [TestMethod]
        public void Tree()
        {
            // Arrange
            var siteMapNodeService = GenerateStub<ISiteMapNodeService>();
            siteMapNodeService.Stub( x => x.GetTree() ).Return( GetTree() );

            var standardServices = GenerateStub<IStandardServices>();
            standardServices.Stub( x => x.SiteMapNodeService ).Return( siteMapNodeService );

            var controller = new SiteMapNodeController( standardServices );

            // Act
            var result = (ViewResult) controller.Tree();

            // Assert
            Assert.AreEqual( "", result.ViewName );

            Assert.IsTrue( result.Model is TreeNode<SiteMapNode> );
            var resultTree = (TreeNode<SiteMapNode>) result.Model;
        }

        [TestMethod]
        public void CreateHttpGet()
        {
            // Arrange
            var siteMapNodeService = GenerateStub<ISiteMapNodeService>();
            siteMapNodeService.Stub( x => x.GetTree() ).Return( GetTree() );

            var standardServices = GenerateStub<IStandardServices>();
            standardServices.Stub( x => x.SiteMapNodeService ).Return( siteMapNodeService );

            var controller = new SiteMapNodeController( standardServices );

            // Act
            var result = (ViewResult) controller.Create();

            // Assert
            Assert.IsTrue( result.Model is SiteMapNodeCreateDisplay );
            var display = (SiteMapNodeCreateDisplay) result.Model;
            var selectListItems = (IEnumerable<SelectListItem>) display.ParentSelectList.Items;
        }

        [TestMethod]
        public void CreateHttpPost()
        {
            // Arrange
            SiteMapNodeController.CreateMaps();

            ISiteMapNodeRepository siteMapNodeRepository = new MockSiteMapNodeRepository();
            IStandardUnitOfWork standardUnitOfWork = NewStandardUnitOfWork( siteMapNodeRepository );
            IStandardServices standardServices = new StandardServices( standardUnitOfWork );
            var controller = new SiteMapNodeController( standardServices );

            var input = new SiteMapNodeCreateInput { ParentId = 1, Url = "/Test", Title = "Test" };

            // Act
            var result = (RedirectToRouteResult) controller.Create( input );

            // Assert
            Assert.AreEqual( "SiteMapNode", result.RouteValues["controller"] );
            Assert.AreEqual( "Index", result.RouteValues["action"] );
        }


        private static T GenerateStub<T>( params object[] argumentsForConstructor ) where T : class
        {
            return MockRepository.GenerateStub<T>( argumentsForConstructor );
        }

        private static List<SiteMapNode> GetSiteMapNodes()
        {
            var list = new List<SiteMapNode>();
            list.Add( new SiteMapNode { Id = 1, Lft = 1, Rgt = 2 } );
            return list;
        }

        private static TreeNode<SiteMapNode> GetTree()
        {
            return TreeNodeHelper.CreateTree( GetSiteMapNodes() );
        }

        private static IStandardUnitOfWork NewStandardUnitOfWork( ISiteMapNodeRepository siteMapNodeRepository )
        {
            var standardUnitOfWork = GenerateStub<IStandardUnitOfWork>();
            standardUnitOfWork.Stub( x => x.SiteMapNodeRepository ).Return( siteMapNodeRepository );
            return standardUnitOfWork;
        }

    }
}
