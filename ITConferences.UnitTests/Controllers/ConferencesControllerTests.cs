using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using ITConferences.Domain.Abstract;
using ITConferences.Domain.Entities;
using ITConferences.WebUI.Abstract.Helpers;
using ITConferences.WebUI.Controllers;
using ITConferences.WebUI.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace ITConferences.UnitTests.Controllers
{
    [TestClass]
    public class ConferencesControllerTests
    {
        #region | GetLocations |

        [TestMethod]
        [TestCategory("ConferencesController")]
        [Owner("Ewelina Olejnik")]
        public void ConferencesController_GetLocations_returns_json_data_with_specified_formatting()
        {
            //Arrange


            //Assign
            var jsonLocation = (List<string>) sut.GetLocations("asd").Data;
            var expectedList = new List<string> {"Asd, Asd"};

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
            _repositoryMock.Setup(e => e.GetById<Country>(1, null))
                .Returns(country1);

            //Assign
            var jsonSelectedCities = (IEnumerable<SelectListItem>) sut.GetSelectedCities(1).Data;
            var expectedList = new List<SelectListItem> {new SelectListItem {Text = "Asd", Value = 1.ToString()}};

            //Assert
            Assert.AreEqual(jsonSelectedCities.ToList()[0].Text, expectedList[0].Text);
            Assert.AreEqual(jsonSelectedCities.ToList()[0].Value, expectedList[0].Value);
        }

        #endregion

        #region | Create GET |

        [TestMethod]
        [TestCategory("ConferencesController")]
        [Owner("Ewelina Olejnik")]
        public void ConferencesController_Create_assign_view_data()
        {
            //Arrange
            sut.Create();

            //Assign
            var viewDataTag = ((MultiSelectList) sut.ViewData["TagsSelector"]).ToList();
            var expectedDataTag = new MultiSelectList(_repositoryMock.Object.GetAll<Tag>(), "TagID", "Name").ToList();

            var viewDataCountry = ((SelectList) sut.ViewData["TargetCountryId"]).ToList();
            var expectedDataCountry =
                new SelectList(_repositoryMock.Object.GetAll<Country>(), "CountryID", "Name").ToList();

            //Assert
            Assert.AreEqual(expectedDataTag.Count, viewDataTag.Count);
            Assert.AreEqual(expectedDataCountry.Count, viewDataCountry.Count);
        }

        #endregion

        #region | Create POST |

        [TestMethod]
        [TestCategory("ConferencesController")]
        [Owner("Ewelina Olejnik")]
        public void ConferencesController_Create_calls_controller_helper_methods_if_passed_parameters_arent_null()
        {
            //Arrange
            sut.Create("1,3", conferences[0], null, "123");

            //Assign


            //Assert
            _controllerHelperMock.Verify(e => e.AssignOrganizer("123", conferences[0]), Times.Once);
            _repositoryMock.Verify(e => e.InsertAndSubmit(conferences[0]), Times.Once);
        }

        #endregion

        #region | Manage POST |

        [TestMethod]
        [TestCategory("ConferencesController")]
        [Owner("Ewelina Olejnik")]
        public void ConferencesController_Manage_calls_controller_helper_methods_if_passed_parameters_arent_null()
        {
            //Arrange
            //sut.Manage("1,3", conferences[0], null);

            //Assign


            //Assert
            _repositoryMock.Verify(e => e.UpdateAndSubmit(conferences[0]), Times.Once);
        }

        #endregion

        #region | Sut |

        private ConferencesController sut;
        private Mock<IGenericRepository> _repositoryMock;
        private Mock<IFilterConferenceHelper> _filterHelperMock;
        private Mock<IControllerHelper> _controllerHelperMock;
        private Country country1;
        private Conference[] conferences;
        private Mock<HttpRequestBase> requestMock;

        [TestInitialize]
        public void StartUp()
        {
            _repositoryMock = new Mock<IGenericRepository>();
            _filterHelperMock = new Mock<IFilterConferenceHelper>();
            _controllerHelperMock = new Mock<IControllerHelper>();
            requestMock = new Mock<HttpRequestBase>();


            var city1 = new City {Name = "Asd", CityID = 1, Country = new Country {Name = "Asd"}};
            country1 = new Country {Name = "Asd", CountryID = 1, Cities = new[] {city1}};
            var city2 = new City {Name = "Test", CityID = 2, Country = new Country {Name = "Test"}};
            var country2 = new Country {Name = "Test", CountryID = 2, Cities = new[] {city2}};
            var tag1 = new Tag {TagID = 1, Name = "testTag1"};
            var tag2 = new Tag {TagID = 2, Name = "testTag2"};
            var tag3 = new Tag {TagID = 3, Name = "testTag3"};
            conferences = new[]
            {
                new Conference
                {
                    Name = "asd",
                    TargetCity = city2,
                    TargetCountry = country2,
                    Tags = new List<Tag> {tag1, tag2}
                },
                new Conference
                {
                    Name = "test",
                    TargetCity = city1,
                    TargetCountry = country1,
                    Tags = new List<Tag> {tag3}
                }
            };

            _repositoryMock.Setup(e => e.GetAll<Conference>())
                .Returns(conferences);

            _filterHelperMock.Setup(e => e.Conferences)
                .Returns(conferences);

            _repositoryMock.Setup(e => e.GetAll<Country>())
                .Returns(new[]
                {country1, country2});

            _repositoryMock.Setup(e => e.GetAll<City>())
                .Returns(new[]
                {city1, city2});

            _repositoryMock.Setup(e => e.GetAll<Tag>()).Returns(new[] {tag1, tag2, tag3});

            sut = new ConferencesController(_filterHelperMock.Object,
                _repositoryMock.Object, _controllerHelperMock.Object);


            _controllerHelperMock.Setup(e => e.GetPageSize(0, It.IsAny<int>(), 2)).Returns(2);

            requestMock.SetupGet(x => x.Headers).Returns(
                new WebHeaderCollection
                {
                    {"X-Requested-With", "XMLHttpRequest"}
                });
            requestMock.Setup(e => e.IsAuthenticated).Returns(true);
            var context = new Mock<HttpContextBase>();
            context.SetupGet(x => x.Request).Returns(requestMock.Object);
            sut.ControllerContext = new ControllerContext(context.Object, new RouteData(), sut);

            _repositoryMock.Setup(e => e.GetById<Conference>(1, null)).Returns((Conference) null);
            _repositoryMock.Setup(e => e.GetById<Conference>(2, null)).Returns(new Conference());
        }

        [TestCleanup]
        public void Cleanup()
        {
            _repositoryMock = null;
            sut = null;
        }

        #endregion

        #region | Ctor |

        [TestMethod]
        [TestCategory("ConferencesController")]
        [Owner("Ewelina Olejnik")]
        [ExpectedException(typeof (ArgumentNullException))]
        public void ConferencesController_Ctor_throws_exception_if_repository_is_null()
        {
            sut = new ConferencesController(_filterHelperMock.Object, null, _controllerHelperMock.Object);
        }


        [TestMethod]
        [TestCategory("ConferencesController")]
        [Owner("Ewelina Olejnik")]
        [ExpectedException(typeof (ArgumentNullException))]
        public void ConferencesController_Ctor_throws_exception_if_filter_helper_is_null()
        {
            sut = new ConferencesController(null, _repositoryMock.Object, _controllerHelperMock.Object);
        }

        [TestMethod]
        [TestCategory("ConferencesController")]
        [Owner("Ewelina Olejnik")]
        [ExpectedException(typeof (ArgumentNullException))]
        public void ConferencesController_Ctor_throws_exception_if_controller_helper_is_null()
        {
            sut = new ConferencesController(_filterHelperMock.Object, _repositoryMock.Object, null);
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
            var viewData = ((MultiSelectList) sut.ViewData["TagsFilter"]).ToList();
            var expectedData = new MultiSelectList(_repositoryMock.Object.GetAll<Tag>(), "TagID", "Name").ToList();

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
            var viewData = (string) sut.ViewData["ResultsCount"];
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
            _controllerHelperMock.Verify(e => e.GetPageSize(0, It.IsAny<int>(), 2), Times.Once);
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

        #region | GetConferences |

        [TestMethod]
        [TestCategory("ConferencesController")]
        [Owner("Ewelina Olejnik")]
        public void ConferencesController_GetConferences_call_filters_methods()
        {
            //Arrange
            sut.GetConferences("test", "test", new[] {1, 4}, DateFilter.All, null);

            //Assign


            //Assert
            _filterHelperMock.Verify(e => e.FilterByName(sut.ViewData, "test"), Times.Once);
            _filterHelperMock.Verify(e => e.FilterByLocation(sut.ViewData, "test"), Times.Once);
            _filterHelperMock.Verify(e => e.FilterByTags(sut.ViewData, new[] {1, 4}, sut.Tags), Times.Once);
            _filterHelperMock.Verify(e => e.FilterByTime(sut.ViewData, DateFilter.All, sut.Conferences), Times.Once);
        }


        [TestMethod]
        [TestCategory("ConferencesController")]
        [Owner("Ewelina Olejnik")]
        public void
            ConferencesController_GetConferences_returns_null_if_there_is_count_conferences_less_then_multiplication_of_pagesize_and_page_id
            ()
        {
            //Arrange
            var result = sut.GetConferences("test", "test", new[] {1, 4}, DateFilter.All, 5);

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
            sut.GetConferences("test", "test", new[] {1, 4}, DateFilter.All, null);

            //Assign
            var viewData = (string) sut.ViewData["ResultsCount"];
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
            sut.GetConferences("test", "test", new[] {1, 4}, DateFilter.All, null);

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
            Assert.IsInstanceOfType(result, typeof (HttpStatusCodeResult));
        }

        [TestMethod]
        [TestCategory("ConferencesController")]
        [Owner("Ewelina Olejnik")]
        public void ConferencesController_Details_returns_http_not_found_if_conference_hasnt_been_found()
        {
            //Arrange
            var result = sut.Details(1);

            //Assign


            //Assert
            Assert.IsInstanceOfType(result, typeof (HttpNotFoundResult));
        }

        #endregion

        #region | AddEvaluation |

        [TestMethod]
        [TestCategory("ConferencesController")]
        [Owner("Ewelina Olejnik")]
        public void ConferencesController_AddEvaluation_returns_http_not_found_if_conference_hasnt_been_found()
        {
            //Arrange
            _repositoryMock.Setup(e => e.GetById<Conference>(1, null)).Returns((Conference) null);
            var result = sut.AddEvaluation(1, 0, null, null);

            //Assign


            //Assert
            Assert.IsInstanceOfType(result, typeof (HttpNotFoundResult));
        }

        [TestMethod]
        [TestCategory("ConferencesController")]
        [Owner("Ewelina Olejnik")]
        public void ConferencesController_AddEvaluation_call_get_evaluation()
        {
            //Arrange
            _repositoryMock.Setup(e => e.GetById<Conference>(1, null))
                .Returns(new Conference {Evaluation = new List<Evaluation>()});
            _controllerHelperMock.Setup(e => e.GetEvaluation(It.IsAny<string>(), "asd", 1)).Returns(new Evaluation());
            sut.AddEvaluation(1, 1, "asd", "1234");

            //Assign


            //Assert
            _controllerHelperMock.Verify(e => e.GetEvaluation(It.IsAny<string>(), "asd", 1), Times.Once);
        }

        #endregion

        #region | Manage GET |

        [TestMethod]
        [TestCategory("ConferencesController")]
        [Owner("Ewelina Olejnik")]
        public void ConferencesController_Manage_assign_view_data()
        {
            //Arrange
            _controllerHelperMock.Setup(e => e.AllUsers).Returns(new List<Attendee>());
            sut.Manage(2);

            //Assign
            var viewDataTag = ((MultiSelectList) sut.ViewData["TagsSelector"]).ToList();
            var expectedDataTag = new MultiSelectList(_repositoryMock.Object.GetAll<Tag>(), "TagID", "Name").ToList();

            var viewDataSpeaker = ((MultiSelectList) sut.ViewData["SpeakersSelector"]).ToList();
            var expectedDataSpeaker =
                new MultiSelectList(_controllerHelperMock.Object.AllUsers, "TagID", "Name").ToList();

            var viewDataCountry = ((SelectList) sut.ViewData["TargetCountryId"]).ToList();
            var expectedDataCountry =
                new SelectList(_repositoryMock.Object.GetAll<Country>(), "CountryID", "Name").ToList();

            //Assert
            Assert.AreEqual(expectedDataTag.Count, viewDataTag.Count);
            Assert.AreEqual(expectedDataCountry.Count, viewDataCountry.Count);
        }

        [TestMethod]
        [TestCategory("ConferencesController")]
        [Owner("Ewelina Olejnik")]
        public void ConferencesController_Manage_returns_bad_request_if_id_is_null()
        {
            //Arrange
            var result = sut.Manage(null);

            //Assign


            //Assert
            Assert.IsInstanceOfType(result, typeof (HttpStatusCodeResult));
        }

        [TestMethod]
        [TestCategory("ConferencesController")]
        [Owner("Ewelina Olejnik")]
        public void ConferencesController_Manage_returns_http_not_found_if_conference_hasnt_been_found()
        {
            //Arrange
            var result = sut.Manage(1);

            //Assign


            //Assert
            Assert.IsInstanceOfType(result, typeof (HttpNotFoundResult));
        }

        #endregion
    }
}