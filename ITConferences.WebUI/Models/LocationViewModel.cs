using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ITConferences.Domain.Abstract;
using ITConferences.Domain.Concrete;
using ITConferences.Domain.Entities;

namespace ITConferences.WebUI.Models
{
    public class LocationViewModel
    {
        private IGenericRepository<Country> _countriesRepository;
        private IGenericRepository<City> _citiesRepository;

        public LocationViewModel(IGenericRepository<Country> countriesRepository, IGenericRepository<City> citiesRepository)
        {
            if (countriesRepository == null || citiesRepository == null)
            {
                throw new ArgumentNullException("Some repository does not exist!");
            }
            _countriesRepository = countriesRepository;
            _citiesRepository = citiesRepository;
        }

        public IList<Country> Countries
        {
            get { return _countriesRepository.GetAll().ToList(); }
        }

        public IList<City> Cities
        {
            get { return _citiesRepository.GetAll().ToList(); }
        }
    }
}