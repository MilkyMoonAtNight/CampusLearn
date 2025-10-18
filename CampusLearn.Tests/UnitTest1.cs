using Xunit;
using CampusLearn.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;

namespace CampusLearn.Tests
{
    public class UnitTest1
    {
        public TopicsController CreateControllerWithTempData()
        {
            var controller = new TopicsController();

            var httpContext = new DefaultHttpContext();
            var tempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(httpContext, tempDataProvider.Object);
            controller.TempData = tempData;
            return controller;
        }

        [Fact]
        public void DummyTest()
        {
            Assert.True(true); 
        }

        [Fact]
        public void AddReply_EmptyMessage_SetsErrorAndRedirects()
        {
            var controller = CreateControllerWithTempData();
            controller.Create("Empty Message Test", "Testing empty reply", "Iwan");
            var topicId = GetPrivateList<TopicsController.Topic>(controller, "Topics").First().TopicId;
            var result = controller.AddReply(topicId, "", false);
            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Detail", redirect.ActionName);
            Assert.Equal("Message is required.", controller.TempData["Error"]);
        }

        [Fact]
        public void Create_MissingTitleOrContent_ReturnsRedirectWithError()
        {
            var controller = CreateControllerWithTempData();
            var result = controller.Create("", "", "Iwan");
            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);
            Assert.Equal("Title and Content are required.", controller.TempData["Error"]);
        }

        [Fact]
        public void Create_DuplicateTitle_ReturnsRedirectWithError()
        {
            var controller = CreateControllerWithTempData();
            controller.Create("AI Ethics", "First post", "Iwan");
            var result = controller.Create("AI Ethics", "Second post", "Iwan");
            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);
            Assert.Equal("There is a topic that has the same Topic Title", controller.TempData["Error"]);
        }
        private List<T> GetPrivateList<T>(object controller, string fieldName)
        {
            var field = controller.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Static);
            return field?.GetValue(null) as List<T>;
        }

    }
}
