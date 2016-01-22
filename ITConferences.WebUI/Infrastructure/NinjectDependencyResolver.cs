using System;
using System.Collections.Generic;
using System.Web.Mvc;
using ITConferences.Domain.Abstract;
using ITConferences.Domain.Concrete;
using ITConferences.Domain.Entities;
using ITConferences.WebUI.Abstract.Helpers;
using ITConferences.WebUI.Helpers;
using Ninject;
using Ninject.Web.Common;

namespace ITConferences.WebUI.Infrastructure
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private IKernel _kernel;

        public NinjectDependencyResolver(IKernel kernel)
        {
            _kernel = kernel;
            AddBindings();
        }

        private void AddBindings()
        {
            _kernel.Bind<IDataContext>().To<DataContext>();
            _kernel.Bind<IGenericRepository<Attendee>>().To<GenericRepository<Attendee>>();
            _kernel.Bind<IGenericRepository<City>>().To<GenericRepository<City>>();
            _kernel.Bind<IGenericRepository<Conference>>().To<GenericRepository<Conference>>();
            _kernel.Bind<IGenericRepository<Country>>().To<GenericRepository<Country>>();
            _kernel.Bind<IGenericRepository<Evaluation>>().To<GenericRepository<Evaluation>>();
            _kernel.Bind<IGenericRepository<Inspiration>>().To<GenericRepository<Inspiration>>();
            _kernel.Bind<IGenericRepository<Organizer>>().To<GenericRepository<Organizer>>();
            _kernel.Bind<IGenericRepository<Speaker>>().To<GenericRepository<Speaker>>();
            _kernel.Bind<IGenericRepository<Tag>>().To<GenericRepository<Tag>>();
            _kernel.Bind<IFilterConferenceHelper>().To<FilterHelper>();
        }

        public object GetService(Type serviceType)
        {
            return _kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _kernel.GetAll(serviceType);
        }
    }
}
