using AwesomeCatApi.Controllers;
using AwesomeCatApi.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace AwesomeCatApiTests
{
    public class CatsControllerTests
    {
        private readonly Mock<ICatService> _mockCatService;
        private readonly CatsController _controller;

        public CatsControllerTests()
        {
            _mockCatService = new Mock<ICatService>();
            _controller = new CatsController(_mockCatService.Object);
        }

        [Fact]
        public async Task GetCat_ReturnsOkResult_WhenCatExists()
        {
            // Arrange
            var testCat = new CatEntity
            {
                Id = 12,
                CatId = "test123",
                Width = 100,
                Height = 100,
                ImageUrl = "http://example.com/cat.jpg",
                Tags = new List<TagEntity>()
            };

            _mockCatService.Setup(service => service.GetCatByIdAsync(12))
                .ReturnsAsync(testCat);

            // Act
            var result = await _controller.GetCat(12);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedCat = Assert.IsType<CatEntity>(okResult.Value);
            Assert.Equal(testCat.Id, returnedCat.Id);
            Assert.Equal(testCat.CatId, returnedCat.CatId);
            Assert.Equal(testCat.Width, returnedCat.Width);
            Assert.Equal(testCat.Height, returnedCat.Height);
            Assert.Equal(testCat.ImageUrl, returnedCat.ImageUrl);
        }
    }
}