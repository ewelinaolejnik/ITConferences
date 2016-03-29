using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ITConferences.Domain.Entities;
using ITConferences.Domain.Abstract;
using ITConferences.WebUI.Controllers;
using System.Web.Mvc;

namespace ITConferences.UnitTests.Controllers
{
    [TestClass]
    public class BaseControllerTests
    {
        #region | Sut |
        BaseController sut;
        private Mock<IGenericRepository> _imageRepositoryMock;

        [TestInitialize]
        public void StartUp()
        {
            _imageRepositoryMock = new Mock<IGenericRepository>();
            sut = new BaseController(_imageRepositoryMock.Object);
           
        }

        [TestCleanup]
        public void Cleanup()
        {
            _imageRepositoryMock = null;
            sut = null;
        }
        #endregion

        #region | Ctor |

        [TestMethod]
        [TestCategory("BaseController")]
        [Owner("Ewelina Olejnik")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BaseController_Ctor_throws_exception_if_image_repository_is_null()
        {
            sut = new BaseController(null);
        }
        #endregion

        #region | GetImage |

        [TestMethod]
        [TestCategory("BaseController")]
        [Owner("Ewelina Olejnik")]
        public void BaseController_GetImage_returns_type_of_file_and_calls_get_image()
        {
            //Arrange
            _imageRepositoryMock.Setup(e => e.GetById<Image>(1, null)).Returns(new Image() { ImageMimeType = "jpg", ImageData = new byte[0] });
            var result = sut.GetImage(1);

            //Assert
            _imageRepositoryMock.Verify(e => e.GetById<Image>(1, null), Times.Once);
            Assert.IsInstanceOfType(result, typeof(FileContentResult));

        }
        #endregion
    }
}
