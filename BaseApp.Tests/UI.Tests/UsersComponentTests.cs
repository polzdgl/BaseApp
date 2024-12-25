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
        private readonly IUserApiClient _mockApiClient;

        public UsersComponentTests()
        {
            _mockApiClient = Substitute.For<IUserApiClient>();
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
            // Arrange: Mock data for 15-20 items
            var allUsers = Enumerable.Range(1, 20).Select(i => new UserDto
            {
                Id = i.ToString(),
                UserName = $"User{i}",
                FirstName = $"FirstName{i}",
                LastName = $"LastName{i}",
                Email = $"User{i}@email.com"
            }).ToList();

            var usersPage1 = allUsers.Take(10).ToList(); // First 10 items for page 1
            var usersPage2 = allUsers.Skip(10).Take(10).ToList(); // Next 10 items for page 2

            // Mock API response for page 1
            _mockApiClient.GetUsersAsync(1, 10, Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(new PaginatedResult<UserDto>
                {
                    Items = usersPage1,
                    TotalCount = 20,
                    Page = 1,
                    PageSize = 10
                }));

            // Mock API response for page 2
            _mockApiClient.GetUsersAsync(2, 10, Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(new PaginatedResult<UserDto>
                {
                    Items = usersPage2,
                    TotalCount = 20,
                    Page = 2,
                    PageSize = 10
                }));

            var component = RenderComponent<Users>();

            // Act
            var nextPageButton = component.FindAll("button.page-link").Last(); // Find "Next" button
            nextPageButton.Click(); // Simulate clicking "Next"

            // Assert
            Assert.Contains("User11", component.Markup);
            Assert.Contains("User12", component.Markup);
            Assert.Contains("User20", component.Markup);
        }

        [Fact]
        public async Task UsersComponent_PageSizeChange_ReloadsData()
        {
            // Arrange: Mock data for 15-20 items
            var allUsers = Enumerable.Range(1, 20).Select(i => new UserDto
            {
                Id = i.ToString(),
                UserName = $"User{i}",
                FirstName = $"FirstName{i}",
                LastName = $"LastName{i}",
                Email = $"User{i}@email.com"
            }).ToList();

            var usersPage1 = allUsers.Take(10).ToList(); // First 10 items for page 1
            var usersPage2 = allUsers.Skip(10).Take(10).ToList(); // Next 10 items for page 2

            // Mock API response for page 1
            _mockApiClient.GetUsersAsync(1, 10, Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(new PaginatedResult<UserDto>
                {
                    Items = usersPage1,
                    TotalCount = 20,
                    Page = 1,
                    PageSize = 10
                }));

            // Mock API response for page size 50
            _mockApiClient.GetUsersAsync(1, 50, Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(new PaginatedResult<UserDto>
                {
                    Items = allUsers, // All users fit in one page with size 50
                    TotalCount = 20,
                    Page = 1,
                    PageSize = 50
                }));

            var component = RenderComponent<Users>();

            // Act
            var pageSizeDropdown = component.WaitForElement("select#pageSize"); // Wait for dropdown to render
            pageSizeDropdown.Change("50"); // Simulate changing page size to 50

            // Assert
            _mockApiClient.Received(1).GetUsersAsync(1, 50, Arg.Any<CancellationToken>());
            Assert.Contains("User1", component.Markup); // First user from the updated list
            Assert.Contains("User20", component.Markup); // Last user from the updated list
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
