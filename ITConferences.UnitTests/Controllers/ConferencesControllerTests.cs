using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using ITConferences.Domain.Abstract;
using ITConferences.Domain.Concrete;
using ITConferences.Domain.Entities;
using ITConferences.WebUI.Abstract.Helpers;
using ITConferences.WebUI.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ITConferences.WebUI.Helpers;
using System.Web;
using System.Web.Routing;

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
        private Mock<IGenericRepository<Image>> _imageRepositoryMock;
        private Mock<IFilterConferenceHelper> _filterHelperMock;
        private Mock<IControllerHelper> _controllerHelperMock;
        private Country country1;
        private Conference[] conferences;
        private Mock<HttpRequestBase> requestMock;

        [TestInitialize]
        public void StartUp()
        {
            _conferenceRepositoryMock = new Mock<IGenericRepository<Conference>>();
            _countryRepositoryMock = new Mock<IGenericRepository<Country>>();
            _cityRepositoryMock = new Mock<IGenericRepository<City>>();
            _tagRepositoryMock = new Mock<IGenericRepository<Tag>>();
            _imageRepositoryMock = new Mock<IGenericRepository<Image>>();
            _filterHelperMock = new Mock<IFilterConferenceHelper>();
            _controllerHelperMock = new Mock<IControllerHelper>();
            requestMock = new Mock<HttpRequestBase>();


            var city1 = new City() { Name = "Asd", CityID = 1, Country = new Country() { Name = "Asd" } };
            country1 = new Country() { Name = "Asd", CountryID = 1, Cities = new City[] { city1 } };
            var city2 = new City() { Name = "Test", CityID = 2, Country = new Country() { Name = "Test" } };
            var country2 = new Country() { Name = "Test", CountryID = 2, Cities = new City[] { city2 } };
            var tag1 = new Tag() { TagID = 1, Name = "testTag1" };
            var tag2 = new Tag() { TagID = 2, Name = "testTag2" };
            var tag3 = new Tag() { TagID = 3, Name = "testTag3" };
            conferences = new[]
            {
                new Conference()
                {
                    Name = "asd",
                    TargetCity = city2,
                    TargetCountry = country2,
                    Tags = new List<Tag>() {tag1, tag2}
                },
                new Conference()
                {
                    Name = "test",
                    TargetCity = city1,
                    TargetCountry = country1,
                    Tags = new List<Tag>() {tag3}
                }
            };

            _conferenceRepositoryMock.Setup(e => e.GetAll())
                .Returns(conferences);

            _filterHelperMock.Setup(e => e.Conferences)
                .Returns(conferences);

            _countryRepositoryMock.Setup(e => e.GetAll())
                .Returns(new[]
                {country1, country2});

            _cityRepositoryMock.Setup(e => e.GetAll())
                .Returns(new[]
                {city1, city2});

            _tagRepositoryMock.Setup(e => e.GetAll()).Returns(new[] { tag1, tag2, tag3 });

            _controllerHelperMock.Setup(e => e.GetPageSize(0, 15, 2)).Returns(2);

            sut = new ConferencesController(_conferenceRepositoryMock.Object, _countryRepositoryMock.Object,
                _tagRepositoryMock.Object, _cityRepositoryMock.Object, _filterHelperMock.Object,
                _imageRepositoryMock.Object, _controllerHelperMock.Object);

            requestMock.SetupGet(x => x.Headers).Returns(
               new System.Net.WebHeaderCollection {
                {"X-Requested-With", "XMLHttpRequest"}
               });
            var context = new Mock<HttpContextBase>();
            context.SetupGet(x => x.Request).Returns(requestMock.Object);
            sut.ControllerContext = new ControllerContext(context.Object, new RouteData(), sut);
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
            sut = new ConferencesController(null, _countryRepositoryMock.Object, _tagRepositoryMock.Object, _cityRepositoryMock.Object, _filterHelperMock.Object, _imageRepositoryMock.Object, _controllerHelperMock.Object);
        }


        [TestMethod]
        [TestCategory("ConferencesController")]
        [Owner("Ewelina Olejnik")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConferencesController_Ctor_throws_exception_if_country_repository_is_null()
        {
            sut = new ConferencesController(_conferenceRepositoryMock.Object, null, _tagRepositoryMock.Object, _cityRepositoryMock.Object, _filterHelperMock.Object, _imageRepositoryMock.Object, _controllerHelperMock.Object);
        }

        [TestMethod]
        [TestCategory("ConferencesController")]
        [Owner("Ewelina Olejnik")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConferencesController_Ctor_throws_exception_if_tag_repository_is_null()
        {
            sut = new ConferencesController(_conferenceRepositoryMock.Object, _countryRepositoryMock.Object, null, _cityRepositoryMock.Object, _filterHelperMock.Object, _imageRepositoryMock.Object, _controllerHelperMock.Object);
        }

        [TestMethod]
        [TestCategory("ConferencesController")]
        [Owner("Ewelina Olejnik")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConferencesController_Ctor_throws_exception_if_city_repository_is_null()
        {
            sut = new ConferencesController(_conferenceRepositoryMock.Object, _countryRepositoryMock.Object, _tagRepositoryMock.Object, null, _filterHelperMock.Object, _imageRepositoryMock.Object, _controllerHelperMock.Object);
        }

        [TestMethod]
        [TestCategory("ConferencesController")]
        [Owner("Ewelina Olejnik")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConferencesController_Ctor_throws_exception_if_filter_helper_is_null()
        {
            sut = new ConferencesController(_conferenceRepositoryMock.Object, _countryRepositoryMock.Object, _tagRepositoryMock.Object, _cityRepositoryMock.Object, null, _imageRepositoryMock.Object, _controllerHelperMock.Object);
        }

        [TestMethod]
        [TestCategory("ConferencesController")]
        [Owner("Ewelina Olejnik")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConferencesController_Ctor_throws_exception_if_controller_helper_is_null()
        {
            sut = new ConferencesController(_conferenceRepositoryMock.Object, _countryRepositoryMock.Object, _tagRepositoryMock.Object, _cityRepositoryMock.Object, _filterHelperMock.Object, _imageRepositoryMock.Object, null);
        }
        #endregion

        #region | Index |
        [TestMethod]
        [TestCategory("ConferencesController")]
        [Owner("Ewelina Olejnik")]
        public void ConferencesController_Index_assign_tags_filter_view_data()
        {
            //Arrange
            sut.Index(null, null);

            //Assign
            var viewData = ((MultiSelectList)sut.ViewData["TagsFilter"]).ToList();
            var expectedData = new MultiSelectList(_tagRepositoryMock.Object.GetAll(), "TagID", "Name").ToList();

            //Assert
            Assert.AreEqual(expectedData.Count, viewData.Count);
        }

        [TestMethod]
        [TestCategory("ConferencesController")]
        [Owner("Ewelina Olejnik")]
        public void ConferencesController_Index_assign_results_count_view_data()
        {
            //Arrange
            _controllerHelperMock.Setup(e => e.GetResultsCount(2, false)).Returns("2 results");
            sut.Index(null, null);

            //Assign
            var viewData = ((string)sut.ViewData["ResultsCount"]);
            var expectedData = "2 results";

            //Assert
            Assert.AreEqual(expectedData, viewData);
        }

        [TestMethod]
        [TestCategory("ConferencesController")]
        [Owner("Ewelina Olejnik")]
        public void ConferencesController_Index_call_filters_methods()
        {
            //Arrange
            sut.Index("test", null);

            //Assign


            //Assert
            _filterHelperMock.Verify(e => e.FilterByName(sut.ViewData, "test"), Times.Once);
            _filterHelperMock.Verify(e => e.FilterByTags(sut.ViewData, It.IsAny<int[]>(), sut.Tags), Times.Once);
        }

        [TestMethod]
        [TestCategory("ConferencesController")]
        [Owner("Ewelina Olejnik")]
        public void ConferencesController_Index_call_get_page_size()
        {
            //Arrange
            sut.Index(null, null);

            //Assign


            //Assert
            _controllerHelperMock.Verify(e => e.GetPageSize(0, 15, 2), Times.Once);
        }

        [TestMethod]
        [TestCategory("ConferencesController")]
        [Owner("Ewelina Olejnik")]
        public void ConferencesController_Index_page_conferences()
        {
            //Arrange
            sut.Index(null, null);

            //Assign
            var actual = sut.PagedConferences.Count();
            var expected = 2;

            //Assert
            Assert.AreEqual(expected, actual);
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
            var expectedList = new List<string>() { "Asd, Asd" };

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
            _countryRepositoryMock.Setup(e => e.GetById(1, null))
             .Returns(country1);

            //Assign
            IEnumerable<SelectListItem> jsonSelectedCities = (IEnumerable<SelectListItem>)sut.GetSelectedCities(1).Data;
            var expectedList = new List<SelectListItem>() { new SelectListItem() { Text = "Asd", Value = 1.ToString() } };

            //Assert
            Assert.AreEqual(jsonSelectedCities.ToList()[0].Text, expectedList[0].Text);
            Assert.AreEqual(jsonSelectedCities.ToList()[0].Value, expectedList[0].Value);
        }
        #endregion

        #region | GetConferences |
        [TestMethod]
        [TestCategory("ConferencesController")]
        [Owner("Ewelina Olejnik")]
        public void ConferencesController_GetConferences_call_filters_methods()
        {
            //Arrange
            sut.GetConferences("test", "test", new int[] { 1, 4 }, DateFilter.All, null);

            //Assign


            //Assert
            _filterHelperMock.Verify(e => e.FilterByName(sut.ViewData, "test"), Times.Once);
            _filterHelperMock.Verify(e => e.FilterByLocation(sut.ViewData, "test"), Times.Once);
            _filterHelperMock.Verify(e => e.FilterByTags(sut.ViewData, new int[] { 1, 4 }, sut.Tags), Times.Once);
            _filterHelperMock.Verify(e => e.FilterByTime(sut.ViewData, DateFilter.All, sut.Conferences), Times.Once);
        }


        [TestMethod]
        [TestCategory("ConferencesController")]
        [Owner("Ewelina Olejnik")]
        public void ConferencesController_GetConferences_returns_null_if_there_is_count_conferences_less_then_multiplication_of_pagesize_and_page_id()
        {
            //Arrange
            var result = sut.GetConferences("test", "test", new int[] { 1, 4 }, DateFilter.All, 5);

            //Assign


            //Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        [TestCategory("ConferencesController")]
        [Owner("Ewelina Olejnik")]
        public void ConferencesController_GetConferences_assign_results_count_view_data()
        {
            //Arrange
            _controllerHelperMock.Setup(e => e.GetResultsCount(2, true)).Returns(string.Empty);
            sut.GetConferences("test", "test", new int[] { 1, 4 }, DateFilter.All, null);

            //Assign
            var viewData = ((string)sut.ViewData["ResultsCount"]);
            var expectedData = string.Empty;

            //Assert
            Assert.AreEqual(expectedData, viewData);
        }

        [TestMethod]
        [TestCategory("ConferencesController")]
        [Owner("Ewelina Olejnik")]
        public void ConferencesController_GetConferences_page_conferences()
        {
            //Arrange
            sut.GetConferences("test", "test", new int[] { 1, 4 }, DateFilter.All, null);

            //Assign
            var actual = sut.PagedConferences.Count();
            var expected = 2;

            //Assert
            Assert.AreEqual(expected, actual);
        }
        #endregion

        #region | Details |
        [TestMethod]
        [TestCategory("ConferencesController")]
        [Owner("Ewelina Olejnik")]
        public void ConferencesController_Details_returns_bad_request_if_id_is_null()
        {
            //Arrange
            var result = sut.Details(null);

            //Assign


            //Assert
            Assert.IsInstanceOfType(result, typeof(HttpStatusCodeResult));
        }

        [TestMethod]
        [TestCategory("ConferencesController")]
        [Owner("Ewelina Olejnik")]
        public void ConferencesController_Details_returns_http_not_found_if_conference_hasnt_been_found()
        {
            //Arrange
            _conferenceRepositoryMock.Setup(e => e.GetById(1, null)).Returns((Conference)null);
            var result = sut.Details(1);

            //Assign


            //Assert
            Assert.IsInstanceOfType(result, typeof(HttpNotFoundResult));
        }
        #endregion

        #region | AddEvaluation |
        [TestMethod]
        [TestCategory("ConferencesController")]
        [Owner("Ewelina Olejnik")]
        public void ConferencesController_AddEvaluation_returns_http_not_found_if_conference_hasnt_been_found()
        {
            //Arrange
            _conferenceRepositoryMock.Setup(e => e.GetById(1, null)).Returns((Conference)null);
            var result = sut.AddEvaluation(1, 0, null, null);

            //Assign


            //Assert
            Assert.IsInstanceOfType(result, typeof(HttpNotFoundResult));
        }

        [TestMethod]
        [TestCategory("ConferencesController")]
        [Owner("Ewelina Olejnik")]
        public void ConferencesController_AddEvaluation_call_get_evaluation()
        {
            //Arrange
            _conferenceRepositoryMock.Setup(e => e.GetById(1, null)).Returns(new Conference() { Evaluation = new List<Evaluation>() });
            _controllerHelperMock.Setup(e => e.GetEvaluation(It.IsAny<string>(), "asd", 1)).Returns(new Evaluation());
            sut.AddEvaluation(1, 1, "asd", "1234");

            //Assign


            //Assert
            _controllerHelperMock.Verify(e => e.GetEvaluation(It.IsAny<string>(), "asd", 1), Times.Once);
        }
        #endregion

        #region | Create GET |
        //[TestMethod]
        //[TestCategory("ConferencesController")]
        //[Owner("Ewelina Olejnik")]
        //public void ConferencesController_Create_assign_view_data()
        //{
        //    //Arrange
        //    sut.Create();

        //    //Assign
        //    var viewData = ((MultiSelectList)sut.ViewData["TagsFilter"]).ToList();
        //    var expectedData = new MultiSelectList(_tagRepositoryMock.Object.GetAll(), "TagID", "Name").ToList();

        //    //Assert
        //    Assert.AreEqual(expectedData, viewData);
        //}

        #endregion
    }
}
