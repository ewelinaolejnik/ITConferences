using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ITConferences.Domain.Abstract;
using ITConferences.WebUI.Controllers;
using Moq;
using ITConferences.Domain.Entities;
using ITConferences.WebUI.Abstract.Helpers;
using System.Web;
using System.Web.Routing;
using System.Web.Mvc;
using System.Linq;
using System.Collections.Generic;

namespace ITConferences.UnitTests.Controllers
{
    [TestClass]
    public class SpeakersControllerTests
    {
        #region | Sut |
        SpeakersController sut;
        private Mock<IGenericRepository> _repositoryMock;
        private Mock<IFilterSpeakerHelper> _filterHelperMock;
        private Mock<IControllerHelper> _controllerHelperMock;
        private Speaker[] speakers;
        private Mock<HttpRequestBase> requestMock;

        [TestInitialize]
        public void StartUp()
        {
            _repositoryMock = new Mock<IGenericRepository>();
            _filterHelperMock = new Mock<IFilterSpeakerHelper>();
            _controllerHelperMock = new Mock<IControllerHelper>();
            requestMock = new Mock<HttpRequestBase>();

            speakers = new[]
            {
                new Speaker()
                {
                    User = new Attendee()
                },
                new Speaker()
                {
                    User = new Attendee()
                }
            };

            _repositoryMock.Setup(e => e.GetAll<Speaker>())
                .Returns(speakers);

            _filterHelperMock.Setup(e => e.Speakers)
                .Returns(speakers);

           
            sut = new SpeakersController(_repositoryMock.Object, _filterHelperMock.Object,
                _controllerHelperMock.Object);


            _controllerHelperMock.Setup(e => e.GetPageSize(0, It.IsAny<int>(), 2)).Returns(2);

            requestMock.SetupGet(x => x.Headers).Returns(
               new System.Net.WebHeaderCollection {
                {"X-Requested-With", "XMLHttpRequest"}
               });
            requestMock.Setup(e => e.IsAuthenticated).Returns(true);
            var context = new Mock<HttpContextBase>();
            context.SetupGet(x => x.Request).Returns(requestMock.Object);
            sut.ControllerContext = new ControllerContext(context.Object, new RouteData(), sut);

            _repositoryMock.Setup(e => e.GetById<Speaker>(1, null)).Returns((Speaker)null);
            _repositoryMock.Setup(e => e.GetById<Speaker>(2, null)).Returns(new Speaker());
        }

        [TestCleanup]
        public void Cleanup()
        {
            _repositoryMock = null;
            _filterHelperMock = null;
            _controllerHelperMock = null;
            sut = null;
        }
        #endregion

        #region | Ctor |

        [TestMethod]
        [TestCategory("SpeakersController")]
        [Owner("Ewelina Olejnik")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SpeakersController_Ctor_throws_exception_if_repository_is_null()
        {
            sut = new SpeakersController(null, _filterHelperMock.Object,
                _controllerHelperMock.Object);
        }


        [TestMethod]
        [TestCategory("SpeakersController")]
        [Owner("Ewelina Olejnik")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SpeakersController_Ctor_throws_exception_if_speakers_filter_is_null()
        {
            sut = new SpeakersController(_repositoryMock.Object, null, _controllerHelperMock.Object);
        }

        [TestMethod]
        [TestCategory("SpeakersController")]
        [Owner("Ewelina Olejnik")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SpeakersController_Ctor_throws_exception_if_controller_helper_is_null()
        {
            sut = new SpeakersController(_repositoryMock.Object, _filterHelperMock.Object, null);
        }
        #endregion

        #region | Index |

        [TestMethod]
        [TestCategory("SpeakersController")]
        [Owner("Ewelina Olejnik")]
        public void SpeakersController_Index_assign_results_count_view_data()
        {
            //Arrange
            _controllerHelperMock.Setup(e => e.GetResultsCount(2, false)).Returns("2 results");
            sut.Index(null);

            //Assign
            var viewData = ((string)sut.ViewData["ResultsCount"]);
            var expectedData = "2 results";

            //Assert
            Assert.AreEqual(expectedData, viewData);
        }

        [TestMethod]
        [TestCategory("SpeakersController")]
        [Owner("Ewelina Olejnik")]
        public void SpeakersController_Index_call_filters_methods()
        {
            //Arrange
            sut.Index("test");

            //Assign


            //Assert
            _filterHelperMock.Verify(e => e.FilterBySpeakerName(sut.ViewData, "test"), Times.Once);
        }

        [TestMethod]
        [TestCategory("SpeakersController")]
        [Owner("Ewelina Olejnik")]
        public void SpeakersController_Index_call_get_page_size()
        {
            //Arrange
            sut.Index(null);

            //Assign


            //Assert
            _controllerHelperMock.Verify(e => e.GetPageSize(0, It.IsAny<int>(), 2), Times.Once);
        }

        [TestMethod]
        [TestCategory("SpeakersController")]
        [Owner("Ewelina Olejnik")]
        public void SpeakersController_Index_page_conferences()
        {
            //Arrange
            sut.Index(null);

            //Assign
            var actual = sut.PagedSpeakers.Count();
            var expected = 2;

            //Assert
            Assert.AreEqual(expected, actual);
        }
        #endregion

        #region | Details |
        [TestMethod]
        [TestCategory("SpeakersController")]
        [Owner("Ewelina Olejnik")]
        public void SpeakersController_Details_returns_bad_request_if_id_is_null()
        {
            //Arrange
            var result = sut.Details(null);

            //Assign


            //Assert
            Assert.IsInstanceOfType(result, typeof(HttpStatusCodeResult));
        }

        [TestMethod]
        [TestCategory("SpeakersController")]
        [Owner("Ewelina Olejnik")]
        public void SpeakersController_Details_returns_http_not_found_if_speaker_hasnt_been_found()
        {
            //Arrange
            var result = sut.Details(1);

            //Assign


            //Assert
            Assert.IsInstanceOfType(result, typeof(HttpNotFoundResult));
        }
        #endregion

        #region | GetSpeakers |
        [TestMethod]
        [TestCategory("SpeakersController")]
        [Owner("Ewelina Olejnik")]
        public void SpeakersController_GetSpeakers_call_filters_methods()
        {
            //Arrange
            sut.GetSpeakers("test", 1);

            //Assign


            //Assert
            _filterHelperMock.Verify(e => e.FilterBySpeakerName(sut.ViewData, "test"), Times.Once);
        }


        [TestMethod]
        [TestCategory("SpeakersController")]
        [Owner("Ewelina Olejnik")]
        public void SpeakersController_GetSpeakers_returns_null_if_there_is_count_conferences_less_then_multiplication_of_pagesize_and_page_id()
        {
            //Arrange
            var result = sut.GetSpeakers("test", 1);

            //Assign


            //Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        [TestCategory("SpeakersController")]
        [Owner("Ewelina Olejnik")]
        public void SpeakersController_GetSpeakers_assign_results_count_view_data()
        {
            //Arrange
            _controllerHelperMock.Setup(e => e.GetResultsCount(2, true)).Returns(string.Empty);
            sut.GetSpeakers("test", 0);

            //Assign
            var viewData = ((string)sut.ViewData["ResultsCount"]);
            var expectedData = string.Empty;

            //Assert
            Assert.AreEqual(expectedData, viewData);
        }

        [TestMethod]
        [TestCategory("SpeakersController")]
        [Owner("Ewelina Olejnik")]
        public void SpeakersController_GetSpeakers_page_conferences()
        {
            //Arrange
            sut.GetSpeakers("test", 0);

            //Assign
            var actual = sut.PagedSpeakers.Count();
            var expected = 2;

            //Assert
            Assert.AreEqual(expected, actual);
        }
        #endregion

        #region | AddEvaluation |
        [TestMethod]
        [TestCategory("SpeakersController")]
        [Owner("Ewelina Olejnik")]
        public void SpeakersController_AddEvaluation_returns_http_not_found_if_speaker_hasnt_been_found()
        {
            //Arrange
            _repositoryMock.Setup(e => e.GetById<Speaker>(4, null)).Returns((Speaker)null);
            var result = sut.AddEvaluation(4, 0, null, null);

            //Assign


            //Assert
            Assert.IsInstanceOfType(result, typeof(HttpNotFoundResult));
        }

        [TestMethod]
        [TestCategory("SpeakersController")]
        [Owner("Ewelina Olejnik")]
        public void SpeakersController_AddEvaluation_call_get_evaluation()
        {
            //Arrange
            _repositoryMock.Setup(e => e.GetById<Speaker>(1, null)).Returns(new Speaker() { Evaluations = new List<Evaluation>() });
            _controllerHelperMock.Setup(e => e.GetEvaluation(It.IsAny<string>(), "asd", 1)).Returns(new Evaluation());
            sut.AddEvaluation(1, 1, "asd", "1234");

            //Assign


            //Assert
            _controllerHelperMock.Verify(e => e.GetEvaluation(It.IsAny<string>(), "asd", 1), Times.Once);
        }
        #endregion
    }
}
