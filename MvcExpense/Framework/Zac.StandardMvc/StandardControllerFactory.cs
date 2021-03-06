﻿using System;
using System.Diagnostics;
using System.Web.Mvc;
using System.Web.Routing;
using StructureMap;

namespace Zac.StandardMvc
{
    public class StandardControllerFactory : DefaultControllerFactory
    {
        private IContainer _container;

        public StandardControllerFactory( IContainer container )
        {
            _container = container;
        }

        protected override IController GetControllerInstance( RequestContext requestContext, Type controllerType )
        {
            try
            {
                return _container.GetInstance( controllerType ) as Controller;
            }
            catch ( StructureMapException )
            {
                Debug.WriteLine( _container.WhatDoIHave() );
                throw;
            }
        }

    }
}
