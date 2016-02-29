using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ITConferences.Domain.Abstract;
using ITConferences.Domain.Entities;
using ITConferences.WebUI.Controllers;
using ITConferences.WebUI.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace ITConferences.UnitTests.Helpers
{
    [TestClass]
    public class FilterHelperTests
    {
        #region | Sut |
        FilterHelper sut;
        private Mock<IGenericRepository<Conference>> _conferenceRepositoryMock;
        private Mock<IGenericRepository<Tag>> _tagRepositoryMock;
        private ViewDataDictionary viewData;
        private Tag tag1;
        private Tag tag2;
        private Tag tag3;

        [TestInitialize]
        public void StartUp()
        {
            viewData = new ViewDataDictionary();
            _conferenceRepositoryMock = new Mock<IGenericRepository<Conference>>();
            _tagRepositoryMock = new Mock<IGenericRepository<Tag>>();
            var city1 = new City() { Name = "Asd", CityID = 1, Country = new Country() { Name = "Asd" } };
            var country1 = new Country() { Name = "Asd", CountryID = 1, Cities = new City[] { city1 } };
            var city2 = new City() { Name = "Test", CityID = 2, Country = new Country() { Name = "Test" } };
            var country2 = new Country() { Name = "Test", CountryID = 2, Cities = new City[] { city2 } };
            tag1 = new Tag() { TagID = 1, Name = "testTag1" };
            tag2 = new Tag() { TagID = 2, Name = "testTag2" };
            tag3 = new Tag() { TagID = 3, Name = "testTag3" };
            var conferences = new[]
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

            _tagRepositoryMock.Setup(e => e.GetAll()).Returns(new[] { tag1, tag2, tag3, new Tag() { TagID = 4 } });

            sut = new FilterHelper { Conferences = _conferenceRepositoryMock.Object.GetAll() };
            sut.Speakers = new List<Speaker>() { new Speaker() { User = new Attendee() } };
        }

        [TestCleanup]
        public void Cleanup()
        {
            _conferenceRepositoryMock = null;
            _tagRepositoryMock = null;
            sut = null;
            viewData = null;
        }
        #endregion

        #region | FilterByName |
        [TestMethod]
        [TestCategory("FilterHelper")]
        [Owner("Ewelina Olejnik")]
        public void FilterHelper_FilterByName_assign_name_filter()
        {
            //Arrange
            sut.FilterByName(viewData, "asd");

            //Assign
            var nameFilter = "asd";
            var expectedConferences = new[]
            {
                new Conference()
                {
                    Name = "asd"
                }
            };

            //Assert
            Assert.AreEqual(expectedConferences.Count(), sut.Conferences.Count());
            Assert.AreEqual(expectedConferences[0].Name, sut.Conferences.ToList()[0].Name);
            Assert.AreEqual(nameFilter, viewData["NameFilter"]);
        }
        #endregion

        #region | FilterByLocation |
        [TestMethod]
        [TestCategory("FilterByLocation")]
        [Owner("Ewelina Olejnik")]
        public void FilterByLocation_FilterByLocation_assign_location_filter_when_passed_name_filter_is_neither_null_nor_empty()
        {
            //Arrange
            sut.FilterByLocation(viewData, "asd");

            //Assign
            var locationFilter = "asd";

            //Assert
            Assert.AreEqual(locationFilter, viewData["LocationFilter"]);
        }

        [TestMethod]
        [TestCategory("FilterByLocation")]
        [Owner("Ewelina Olejnik")]
        public void FilterByLocation_FilterByLocation_when_param_contains_comma_then_assign_to_conferences_these_items_which_contains_city_and_country_names_equals_passed_param()
        {
            //Arrange
            sut.FilterByLocation(viewData, "asd, asd");

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

        [TestMethod]
        [TestCategory("FilterByLocation")]
        [Owner("Ewelina Olejnik")]
        public void FilterByLocation_FilterByLocation_when_param_doesnt_contains_comma_then_assign_to_conferences_these_items_which_contains_city_or_country_names_equals_passed_param()
        {
            //Arrange
            sut.FilterByLocation(viewData, "asd");

            //Assign
            var expectedConferencesByCity = new[] { new Conference() { TargetCity = new City() { Name = "Asd" } } };
            var expectedConferencesByCountry = new[] { new Conference() { TargetCountry = new Country() { Name = "Asd" } } };
            var actualConferences = sut.Conferences.ToArray();

            //Assert
            Assert.AreEqual(expectedConferencesByCity[0].TargetCity.Name, actualConferences[0].TargetCity.Name);
            Assert.AreEqual(expectedConferencesByCountry[0].TargetCountry.Name, actualConferences[0].TargetCountry.Name);
        }
        #endregion

        #region | FilterByTags |
        [TestMethod]
        [TestCategory("FilterByTags")]
        [Owner("Ewelina Olejnik")]
        public void FilterHelper_FilterByTags_assign_view_data_with_selected_tags()
        {
            //Arrange
            sut.FilterByTags(viewData, new int[] { 1, 4 }, _tagRepositoryMock.Object.GetAll());

            //Assign
            var actualViewData = ((MultiSelectList)viewData["TagsFilter"]);
            var selectedTags = new[] { tag1, new Tag() { TagID = 4 } };
            var filteredTags = new[] { tag1 };
            var expectedData = new MultiSelectList(_tagRepositoryMock.Object.GetAll(), "TagID", "Name", selectedTags);

            //Assert
            Assert.AreEqual(expectedData.ToList().Count, actualViewData.ToList().Count);
            Assert.AreEqual(selectedTags.Length, actualViewData.SelectedValues.Cast<Tag>().Count());
            Assert.AreEqual(filteredTags.Length, sut.Conferences.Count());
        }
        #endregion
    }
}
