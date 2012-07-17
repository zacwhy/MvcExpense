using System.Collections.Generic;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using Zac.StandardCore;
using Zac.StandardCore.Models;
using Zac.StandardCore.Repositories;
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
            IStandardUnitOfWork standardUnitOfWork = Stub<IStandardUnitOfWork>();
            var controller = new SiteMapNodeController( standardUnitOfWork );

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

            var siteMapNodeRepository = Stub<ISiteMapNodeRepository>();
            siteMapNodeRepository.Stub( x => x.GetAll() ).Return( list );

            var unitOfWork = Stub<IStandardUnitOfWork>();
            unitOfWork.Expect( x => x.SiteMapNodeRepository ).Return( siteMapNodeRepository );

            var controller = new SiteMapNodeController( unitOfWork );

            // Act
            var result = (ViewResult) controller.Tree();
            var tree = (TreeNode<SiteMapNode>) result.Model;

            // Assert
            var a = tree.Siblings;
        }

        private T Stub<T>( params object[] argumentsForConstructor ) where T : class
        {
            return MockRepository.GenerateStub<T>( argumentsForConstructor );
        }

    }
}
