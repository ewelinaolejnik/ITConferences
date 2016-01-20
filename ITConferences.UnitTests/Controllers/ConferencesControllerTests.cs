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
        private Mock<IGenericRepository<City>> _cityRepositoryMock;
        private Mock<IGenericRepository<Tag>> _tagRepositoryMock;
        private Country country1;

        [TestInitialize]
        public void StartUp()
        {
            _conferenceRepositoryMock = new Mock<IGenericRepository<Conference>>();
            _countryRepositoryMock = new Mock<IGenericRepository<Country>>();
            _cityRepositoryMock = new Mock<IGenericRepository<City>>();
            _tagRepositoryMock = new Mock<IGenericRepository<Tag>>();
            var city1 = new City() {Name = "Asd", CityID = 1, Country = new Country() {Name = "Asd"}};
            country1 = new Country() {Name = "Asd", CountryID = 1, Cities = new City[] {city1}};
            var city2 = new City() {Name = "Test", CityID = 2, Country = new Country() {Name = "Test"}};
            var country2 = new Country() { Name = "Test", CountryID = 2, Cities = new City[] { city2 } };

            _conferenceRepositoryMock.Setup(e => e.GetAll())
                .Returns(new[]
                {
                    new Conference() {Name = "asd", TargetCity = city2, TargetCountry = country2 },
                    new Conference() {Name = "test", TargetCity = city1, TargetCountry = country1 }
                });

            _countryRepositoryMock.Setup(e => e.GetAll())
                .Returns(new[]
                {country1, country2});

            _cityRepositoryMock.Setup(e => e.GetAll())
                .Returns(new[]
                {city1, city2});

            sut = new ConferencesController(_conferenceRepositoryMock.Object, _countryRepositoryMock.Object, _tagRepositoryMock.Object, _cityRepositoryMock.Object);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _conferenceRepositoryMock = null;
            _countryRepositoryMock = null;
            _cityRepositoryMock = null;
            sut = null;
        }
        #endregion

        #region | Ctor |

        [TestMethod]
        [TestCategory("ConferencesController")]
        [Owner("Ewelina Olejnik")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConferencesController_Ctor_throws_exception_if_conference_repository_is_null()
        {
            sut = new ConferencesController(null, _countryRepositoryMock.Object, _tagRepositoryMock.Object, _cityRepositoryMock.Object);
        }


        [TestMethod]
        [TestCategory("ConferencesController")]
        [Owner("Ewelina Olejnik")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConferencesController_Ctor_throws_exception_if_country_repository_is_null()
        {
            sut = new ConferencesController(_conferenceRepositoryMock.Object, null, _tagRepositoryMock.Object, _cityRepositoryMock.Object);
        }

        [TestMethod]
        [TestCategory("ConferencesController")]
        [Owner("Ewelina Olejnik")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConferencesController_Ctor_throws_exception_if_tag_repository_is_null()
        {
            sut = new ConferencesController(_conferenceRepositoryMock.Object, _countryRepositoryMock.Object, null, _cityRepositoryMock.Object);
        }

        [TestMethod]
        [TestCategory("ConferencesController")]
        [Owner("Ewelina Olejnik")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConferencesController_Ctor_throws_exception_if_city_repository_is_null()
        {
            sut = new ConferencesController(_conferenceRepositoryMock.Object, _countryRepositoryMock.Object, _tagRepositoryMock.Object, null);
        }
        #endregion

        #region | Index |

        [TestMethod]
        [TestCategory("ConferencesController")]
        [Owner("Ewelina Olejnik")]
        public void ConferencesController_Index_Initialize_ViewData_with_countries_when_passed_string_is_null_or_empty()
        {
            //Arrange
            sut.Index(null,null);

            //Assign
            var countriesSelected = sut.ViewData["Countries"] as SelectList;
            var countriesFromDb = _countryRepositoryMock.Object.GetAll();

            //Assert
            Assert.IsNotNull(countriesSelected);
            Assert.AreEqual(countriesFromDb.Count(), countriesSelected.Count());
        }

        [TestMethod]
        [TestCategory("ConferencesController")]
        [Owner("Ewelina Olejnik")]
        public void ConferencesController_Index_assign_name_filter_view_data_when_passed_string_is_neither_null_nor_empty()
        {
            //Arrange
            sut.Index("asd",null);

            //Assign
            var viewData = sut.ViewData["NameFilter"];

            //Assert
            Assert.AreEqual("asd", viewData);
        }

        [TestMethod]
        [TestCategory("ConferencesController")]
        [Owner("Ewelina Olejnik")]
        public void ConferencesController_Index_assing_conferences_as_filtered_items_when_passed_name_filter_is_neither_null_nor_empty()
        {
            //Arrange
            sut.Index("asd",null);

            //Assign
            var expectedConferences = new[] {new Conference() {Name = "asd"}};
            var actualConferences = sut.Conferences.ToArray();

            //Assert
            Assert.AreEqual(expectedConferences[0].Name,actualConferences[0].Name);
        }


        [TestMethod]
        [TestCategory("ConferencesController")]
        [Owner("Ewelina Olejnik")]
        public void ConferencesController_Index_assign_location_filter_view_data_when_passed_string_is_neither_null_nor_empty()
        {
            //Arrange
            sut.Index(null, "asd");

            //Assign
            var viewData = sut.ViewData["LocationFilter"];

            //Assert
            Assert.AreEqual("asd", viewData);
        }


        [TestMethod]
        [TestCategory("ConferencesController")]
        [Owner("Ewelina Olejnik")]
        public void ConferencesController_Index_assing_conferences_as_filtered_items_when_passed_location_filter_is_neither_null_nor_empty()
        {
            //Arrange
            sut.Index(null, "asd");

            //Assign
            var expectedConferencesByCity = new[] { new Conference() { TargetCity = new City() { Name = "Asd" } } };
            var expectedConferencesByCountry = new[] { new Conference() { TargetCountry = new Country() { Name = "Asd" } } };
            var actualConferences = sut.Conferences.ToArray();

            //Assert
            Assert.AreEqual(expectedConferencesByCity[0].TargetCity.Name, actualConferences[0].TargetCity.Name);
            Assert.AreEqual(expectedConferencesByCountry[0].TargetCountry.Name, actualConferences[0].TargetCountry.Name);
        }

        [TestMethod]
        [TestCategory("ConferencesController")]
        [Owner("Ewelina Olejnik")]
        public void ConferencesController_Index_assing_conferences_as_filtered_items_when_passed_location_filter_is_neither_null_nor_empty_and_if_contains_comma()
        {
            //Arrange
            sut.Index(null, "asd, asd");

            //Assign
            var expectedConferencesByCity = new[] { new Conference() { TargetCity = new City() { Name = "Asd" } } };
            var expectedConferencesByCountry = new[] { new Conference() { TargetCountry = new Country() { Name = "Asd" } } };
            var actualConferences = sut.Conferences.ToArray();

            //Assert
            Assert.AreEqual(expectedConferencesByCity[0].TargetCity.Name, actualConferences[0].TargetCity.Name);
            Assert.AreEqual(expectedConferencesByCountry[0].TargetCountry.Name, actualConferences[0].TargetCountry.Name);
            Assert.AreEqual(actualConferences.Length, expectedConferencesByCity.Length);
            Assert.AreEqual(actualConferences.Length, expectedConferencesByCountry.Length);
        }
        #endregion

        #region | GetLocations |
        [TestMethod]
        [TestCategory("ConferencesController")]
        [Owner("Ewelina Olejnik")]
        public void ConferencesController_GetLocations_returns_json_data_with_specified_formatting()
        {
            //Arrange


            //Assign
            List<string> jsonLocation = (List<string>)sut.GetLocations("asd").Data;
            var expectedList = new List<string>() {"Asd, Asd"};

            //Assert
            CollectionAssert.AreEqual(expectedList, jsonLocation);
        }
        #endregion

        #region | GetSelectedCities |
        [TestMethod]
        [TestCategory("ConferencesController")]
        [Owner("Ewelina Olejnik")]
        public void ConferencesController_GetSelectedCities_returns_json_data_with_specified_formatting()
        {
            //Arrange
            _countryRepositoryMock.Setup(e => e.GetById(1))
                .Returns(country1);

            //Assign
            IEnumerable<SelectListItem> jsonSelectedCities = (IEnumerable<SelectListItem>)sut.GetSelectedCities(1).Data;
            var expectedList = new List<SelectListItem>() { new SelectListItem() {Text = "Asd", Value = 1.ToString()} };

            //Assert
            Assert.AreEqual(jsonSelectedCities.ToList()[0].Text, expectedList[0].Text);
            Assert.AreEqual(jsonSelectedCities.ToList()[0].Value, expectedList[0].Value);
        }
        #endregion
    }
}
