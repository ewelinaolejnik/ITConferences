using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ITConferences.WebUI.Controllers;
using ITConferences.Domain.Entities;
using ITConferences.Domain.Abstract;
using Moq;
using System.Web;
using System.Web.Mvc;

namespace ITConferences.UnitTests.Controllers
{
    [TestClass]
    public class OrganizersControllerTests
    {
        #region | Sut |
        OrganizersController sut;
        private Mock<IGenericRepository> _repositoryMock;
        private Organizer[] organizers;

        [TestInitialize]
        public void StartUp()
        {
            _repositoryMock = new Mock<IGenericRepository>();

            organizers = new[]
            {
                new Organizer()
                {
                    User = new Attendee()
                },
                new Organizer()
                {
                    User = new Attendee()
                }
            };

            _repositoryMock.Setup(e => e.GetAll<Organizer>())
                .Returns(organizers);

            sut = new OrganizersController(_repositoryMock.Object);

            _repositoryMock.Setup(e => e.GetById<Organizer>(1, null)).Returns((Organizer)null);
            _repositoryMock.Setup(e => e.GetById<Organizer>(2, null)).Returns(new Organizer());
        }

        [TestCleanup]
        public void Cleanup()
        {
            _repositoryMock = null;
            _repositoryMock = null;
            sut = null;
        }
        #endregion

        #region | Ctor |

        [TestMethod]
        [TestCategory("OrganizersController")]
        [Owner("Ewelina Olejnik")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void OrganizersController_Ctor_throws_exception_if_repository_is_null()
        {
            sut = new OrganizersController(null);
        }
        #endregion

        #region | Details |
        [TestMethod]
        [TestCategory("OrganizersController")]
        [Owner("Ewelina Olejnik")]
        public void OrganizersController_Details_returns_bad_request_if_id_is_null()
        {
            //Arrange
            var result = sut.Details(null);

            //Assign


            //Assert
            Assert.IsInstanceOfType(result, typeof(HttpStatusCodeResult));
        }

        [TestMethod]
        [TestCategory("OrganizersController")]
        [Owner("Ewelina Olejnik")]
        public void OrganizersController_Details_returns_http_not_found_if_speaker_hasnt_been_found()
        {
            //Arrange
            var result = sut.Details(1);

            //Assign


            //Assert
            Assert.IsInstanceOfType(result, typeof(HttpNotFoundResult));
        }
        #endregion
    }
}
