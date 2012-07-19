using System.Collections.Generic;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using Zac.StandardCore.Models;
using Zac.StandardCore.Services;
using Zac.StandardMvc.Controllers;
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
            Assert.AreEqual( result.RouteValues["controller"], "SiteMapNode" );
            Assert.AreEqual( result.RouteValues["action"], "Tree" );
        }

        [TestMethod]
        public void Tree()
        {
            // Arrange

            var list = new List<SiteMapNode>();
            list.Add( new SiteMapNode { Id = 1, Lft = 1, Rgt = 2 } );

            TreeNode<SiteMapNode> tree = TreeNodeHelper.CreateTree( list );

            var siteMapNodeService = GenerateStub<ISiteMapNodeService>();
            siteMapNodeService.Stub( x => x.GetTree() ).Return( tree );

            var standardServices = GenerateStub<IStandardServices>();
            standardServices.Stub( x => x.SiteMapNodeService ).Return( siteMapNodeService );

            var controller = new SiteMapNodeController( standardServices );

            // Act
            var result = (ViewResult) controller.Tree();
            var resultTree = (TreeNode<SiteMapNode>) result.Model;

            // Assert
            Assert.AreEqual( "", result.ViewName );
        }

        private T GenerateStub<T>( params object[] argumentsForConstructor ) where T : class
        {
            return MockRepository.GenerateStub<T>( argumentsForConstructor );
        }

    }
}
