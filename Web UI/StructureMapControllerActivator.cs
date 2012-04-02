using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StructureMap;
using System.Web.Routing;

namespace Web_UI
{
    public class StructureMapControllerActivator : IControllerActivator
    {
        public StructureMapControllerActivator(IContainer container)
        {
            _container = container;
        }

        private IContainer _container;

        public IController Create(RequestContext requestContext, Type controllerType)
        {
            return _container.GetInstance(controllerType) as IController;
        }
    }
}