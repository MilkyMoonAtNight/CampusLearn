using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace campusLearn.Test.Controller
{
    public class TopicTestController
    {
        [Fact]
        public void Create_ValidInput_ReturnsRedirectAndAddsTopic()
        {
            var controller = new TopicsController();

            var result = controller.Create("AI Ethics", "Discussion on bias", "Iwan");

            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);

            var topics = GetPrivateList<TopicsController.Topic>(controller, "Topics");
            Assert.Contains(topics, t => t.Title == "AI Ethics" && t.Content == "Discussion on bias");
        }

        [Fact]
        public void Create_MissingTitleOrContent_ReturnsRedirectWithError()
        {
            var controller = new TopicsController();

            var result = controller.Create("", "", "Iwan");

            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);
            Assert.Equal("Title and Content are required.", controller.TempData["Error"]);
        }
        [Fact]
        public void Create_DuplicateTitle_ReturnsRedirectWithError()
        {
            var controller = new TopicsController();
            controller.Create("AI Ethics", "First post", "Iwan");

            var result = controller.Create("AI Ethics", "Second post", "Iwan");

            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);
            Assert.Equal("There is a topic that has the same Topic Title", controller.TempData["Error"]);
        }
    }
}
