using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using ITConferences.Domain.Abstract;
using ITConferences.Domain.Concrete;
using ITConferences.Domain.Entities;
using ITConferences.WebUI.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace ITConferences.UnitTests.Controllers
{
    [TestClass]
    public class ConferencesControllerTests
    {
        #region | Sut |
        ConferencesController sut;
        private Mock<IGenericRepository<Conference>> _conferenceRepositoryMock;
        private Mock<IGenericRepository<Country>> _countryRepositoryMock;

        [TestInitialize]
        public void StartUp()
        {
            _conferenceRepositoryMock = new Mock<IGenericRepository<Conference>>();
            _countryRepositoryMock = new Mock<IGenericRepository<Country>>();
            _conferenceRepositoryMock.Setup(e => e.GetAll()).Returns(new[] {new Conference(), new Conference()});
            _countryRepositoryMock.Setup(e => e.GetAll()).Returns(new[] {new Country(), new Country()});
            sut = new ConferencesController(_conferenceRepositoryMock.Object, _countryRepositoryMock.Object);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _conferenceRepositoryMock = null;
            _countryRepositoryMock = null;
            sut = null;
        }
        #endregion

        #region | Ctor |

        [TestMethod]
        [TestCategory("ConferencesControllerTests")]
        [Owner("Ewelina Olejnik")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConferencesControllerTests_Ctor_throws_exception_if_conference_repository_is_null()
        {
            sut = new ConferencesController(null, _countryRepositoryMock.Object);
        }


        [TestMethod]
        [TestCategory("ConferencesControllerTests")]
        [Owner("Ewelina Olejnik")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConferencesControllerTests_Ctor_throws_exception_if_country_repository_is_null()
        {
            sut = new ConferencesController(_conferenceRepositoryMock.Object, null);
        }
        #endregion

        #region | Index |

        [TestMethod]
        [TestCategory("ConferencesControllerTests")]
        [Owner("Ewelina Olejnik")]
        public void ConferencesControllerTests_Index_Initialize_ViewData_with_countries()
        {
            //Arrange
            sut.Index();

            //Assign
            var countriesSelected = sut.ViewData["Countries"] as SelectList;
            var countriesFromDb = _countryRepositoryMock.Object.GetAll();

            //Assert
            Assert.IsNotNull(countriesSelected);
            Assert.AreEqual(countriesFromDb.Count(), countriesSelected.Count());
        }
        #endregion
    }
}
