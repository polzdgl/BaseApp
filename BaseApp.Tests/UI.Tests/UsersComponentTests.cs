using BaseApp.Data.User.Dtos;
using BaseApp.ServiceProvider.Interfaces;
using BaseApp.Shared.Dtos;
using BaseApp.Web.Pages.User;
using Bunit;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace BaseApp.Tests.UI.Tests.User
{
    public class UsersComponentTests : TestContext
    {
        private readonly IUserClient _mockApiClient;

        public UsersComponentTests()
        {
            _mockApiClient = Substitute.For<IUserClient>();
            Services.AddSingleton(_mockApiClient);
        }

        [Fact]
        public async Task UsersComponent_RendersCorrectly_WithInitialData()
        {
            // Arrange
            var users = new List<UserDto>
            {
                new UserDto { Id = "1", UserName = "User1", FirstName = "John", LastName = "Doe", Email= "johndoe@email.com" },
                new UserDto { Id = "2", UserName = "User2", FirstName = "Jane", LastName = "Smith", Email= "janesmith@email.com"}
            };

            _mockApiClient.GetUsersAsync(1, 10, default)
                .Returns(Task.FromResult(new PaginatedResult<UserDto>
                {
                    Items = users,
                    TotalCount = 20,
                    Page = 1,
                    PageSize = 10
                }));

            var component = RenderComponent<Users>();

            // Assert
            Assert.Contains("User1", component.Markup);
            Assert.Contains("User2", component.Markup);
            Assert.Contains("Page Size:", component.Markup);
        }

        [Fact]
        public async Task UsersComponent_Pagination_NavigatesToCorrectPage()
        {
            // Arrange
            var usersPage2 = new List<UserDto>
            {
                new UserDto { Id = "3", UserName = "User3", FirstName = "Alice", LastName = "Brown", Email= "AliceBrown@email.com"  },
                new UserDto { Id = "4", UserName = "User4", FirstName = "Bob", LastName = "White" , Email= "BobWhite@email.com" }
            };

            _mockApiClient.GetUsersAsync(2, 10, default)
                .Returns(Task.FromResult(new PaginatedResult<UserDto>
                {
                    Items = usersPage2,
                    TotalCount = 20,
                    Page = 2,
                    PageSize = 10
                }));

            var component = RenderComponent<Users>();

            // Act
            var nextPageButton = component.FindAll("button.page-link").Last();
            nextPageButton.Click(); // Simulate clicking "Next"

            // Assert
            Assert.Contains("User3", component.Markup);
            Assert.Contains("User4", component.Markup);
        }

        [Fact]
        public async Task UsersComponent_PageSizeChange_ReloadsData()
        {
            // Arrange
            var users = new List<UserDto>
            {
                new UserDto { Id = "1", UserName = "User1", FirstName = "John", LastName = "Doe" , Email= "johndoe@email.com" },
                new UserDto { Id = "2", UserName = "User2", FirstName = "Jane", LastName = "Smith" , Email = "johndoe@email.com"}
            };

            _mockApiClient.GetUsersAsync(1, 50, default)
                .Returns(Task.FromResult(new PaginatedResult<UserDto>
                {
                    Items = users,
                    TotalCount = 50,
                    Page = 1,
                    PageSize = 50
                }));

            var component = RenderComponent<Users>();

            // Act
            var pageSizeDropdown = component.Find("select#pageSize");
            pageSizeDropdown.Change("50"); // Simulate changing page size to 50

            // Assert
            _mockApiClient.Received(1).GetUsersAsync(1, 50, default);
            Assert.Contains("Page Size: 50", component.Markup);
        }

        [Fact]
        public async Task UsersComponent_NoData_ShowsNoUsersFoundMessage()
        {
            // Arrange
            _mockApiClient.GetUsersAsync(1, 10, default)
                .Returns(Task.FromResult(new PaginatedResult<UserDto>
                {
                    Items = Enumerable.Empty<UserDto>().ToList(),
                    TotalCount = 0,
                    Page = 1,
                    PageSize = 10
                }));

            var component = RenderComponent<Users>();

            // Assert
            Assert.Contains("No users found.", component.Markup);
        }

        [Fact]
        public async Task UsersComponent_ShowsErrorMessage_OnApiFailure()
        {
            // Arrange
            _mockApiClient.GetUsersAsync(1, 10, default)
                .Throws(new System.Exception("API failure"));

            var component = RenderComponent<Users>();

            // Assert
            Assert.Contains("Failed to load user list. Please try again later.", component.Markup);
        }
    }
}
