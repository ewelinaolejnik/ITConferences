using System;
using System.Collections.Generic;
using System.Web.Mvc;
using ITConferences.Domain.Abstract;
using ITConferences.Domain.Concrete;
using ITConferences.WebUI.Abstract.Helpers;
using ITConferences.WebUI.Helpers;
using Ninject;
using Ninject.Web.Common;

namespace ITConferences.WebUI.Infrastructure
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private readonly IKernel _kernel;

        public NinjectDependencyResolver(IKernel kernel)
        {
            _kernel = kernel;
            AddBindings();
        }

        public object GetService(Type serviceType)
        {
            return _kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _kernel.GetAll(serviceType);
        }

        private void AddBindings()
        {
            _kernel.Bind<IDataContext>().To<DataContext>().InRequestScope();
            _kernel.Bind<IGenericRepository>().To<GenericRepository>().InRequestScope();
            _kernel.Bind<IFilterConferenceHelper>().To<FilterHelper>();
            _kernel.Bind<IFilterSpeakerHelper>().To<FilterHelper>();
            _kernel.Bind<IControllerHelper>().To<ControllerHelper>();
        }
    }
}