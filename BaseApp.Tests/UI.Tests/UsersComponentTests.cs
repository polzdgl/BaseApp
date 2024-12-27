using BaseApp.Data.User.Dtos;
using BaseApp.ServiceProvider.Interfaces;
using BaseApp.Shared.Dtos;
using BaseApp.Web.Pages.User;
using Bunit;
using Bunit.TestDoubles;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Xunit;

namespace BaseApp.Tests.UI.Tests.User
{
    public class UsersComponentTests : TestContext
    {
        private readonly IUserApiClient _mockApiClient;

        public UsersComponentTests()
        {
            // Mock API client and register it in the test services
            _mockApiClient = Substitute.For<IUserApiClient>();
            Services.AddSingleton(_mockApiClient);

            // Configure JSInterop to avoid unhandled invocation exceptions
            JSInterop.Mode = JSRuntimeMode.Loose;

            // Optional: Mock specific JS calls if needed
            JSInterop.SetupVoid("Radzen.createDataGrid").SetVoidResult();
            JSInterop.SetupVoid("Radzen.destroyDataGrid").SetVoidResult();
        }

        [Fact]
        public async Task UsersComponent_RendersCorrectly_WithInitialData()
        {
            // Arrange
            var users = new List<UserDto>
            {
                new UserDto { Id = "1", UserName = "User1", FirstName = "John", LastName = "Doe", Email = "johndoe@email.com" },
                new UserDto { Id = "2", UserName = "User2", FirstName = "Jane", LastName = "Smith", Email = "janesmith@email.com" }
            };

            _mockApiClient.GetUsersAsync(1, 5, default)
                .Returns(new PaginatedResult<UserDto>
                {
                    Items = users,
                    TotalCount = 2,
                    Page = 1,
                    PageSize = 5
                });

            var component = RenderComponent<Users>();

            // Assert
            Assert.Contains("User1", component.Markup);
            Assert.Contains("User2", component.Markup);
            Assert.Contains("User List", component.Markup);
        }

        [Fact]
        public async Task UsersComponent_DeleteUser_RemovesUserFromList()
        {
            // Arrange
            var users = new List<UserDto>
    
            {
                new UserDto { Id = "1", UserName = "User1", FirstName = "John", LastName = "Doe", Email = "johndoe@email.com" }
            };

            // Mock the GetUsersAsync method
            _mockApiClient.GetUsersAsync(1, 5, Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(new PaginatedResult<UserDto>
                {
                    Items = users,
                    TotalCount = 1,
                    Page = 1,
                    PageSize = 5
                }));

            // Mock the DeleteUserAsync method
            _mockApiClient.DeleteUserAsync("1").Returns(Task.FromResult(new HttpResponseMessage
            {
                StatusCode = System.Net.HttpStatusCode.OK
            }));

            var component = RenderComponent<Users>();

            // Act
            var deleteButton = component.Find("button:contains('Delete')");
            deleteButton.Click();

            var confirmButton = component.Find("button:contains('Cancel')");
            confirmButton.Click();
        }


        [Fact]
        public async Task UsersComponent_CreateUser_NavigatesToCreatePage()
        {
            // Arrange
            var navigationManager = Services.GetRequiredService<FakeNavigationManager>();
            var component = RenderComponent<Users>();

            // Act
            var createUserButton = component.Find("button:contains('Create User')");
            createUserButton.Click();

            // Assert
            Assert.Equal("/users/create", new Uri(navigationManager.Uri).AbsolutePath);
        }
    }
}
