using BaseApp.Client.Pages.User;
using BaseApp.Data.User.Dtos;
using BaseApp.ServiceProvider.User.Interfaces;
using BaseApp.Shared.Dtos;
using Bunit;
using Bunit.TestDoubles;
using Microsoft.AspNetCore.Components;
using NSubstitute;

namespace BaseApp.Tests.UI.Tests.User
{
    public class UsersComponentTests : TestContext
    {
        private readonly IUserClient _mockApiClient;

        public UsersComponentTests()
        {
            // Mock API client and register it in the test services
            _mockApiClient = Substitute.For<IUserClient>();
            Services.AddSingleton(_mockApiClient);

            // Configure JSInterop to avoid unhandled invocation exceptions
            JSInterop.Mode = JSRuntimeMode.Loose;

            // Optional: Mock specific JS calls if needed
            JSInterop.SetupVoid("Radzen.createDataGrid").SetVoidResult();
            JSInterop.SetupVoid("Radzen.destroyDataGrid").SetVoidResult();

            // Arrange: Add Radzen services
            Services.AddSingleton<Radzen.NotificationService>();
            Services.AddSingleton<FakeNavigationManager>();
        }

        [Fact]
        public async Task UsersComponent_RendersCorrectly_WithInitialData()
        {
            // Arrange
            var users = new List<UserDto>
            {
                new UserDto { Id = "1", FirstName = "John", LastName = "Doe", Email = "johndoe@email.com" },
                new UserDto { Id = "2", FirstName = "Jane", LastName = "Smith", Email = "janesmith@email.com" }
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
            Assert.Contains("johndoe@email.com", component.Markup);
            Assert.Contains("janesmith@email.com", component.Markup);
            Assert.Contains("Users", component.Markup);
        }

        [Fact]
        public async Task UsersComponent_DeleteUser_RemovesUserFromList()
        {
            // Arrange
            var users = new List<UserDto>
            {
                new UserDto { Id = "1", FirstName = "John", LastName = "Doe", Email = "johndoe@email.com" }
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
            var navigationManager = Services.GetRequiredService<FakeNavigationManager>();

            // Act
            var deleteButton = component.Find("button:contains('Delete')");
            deleteButton.Click();

            Assert.Equal("/", new Uri(navigationManager.Uri).AbsolutePath);
        }

        [Fact]
        public async Task UsersComponent_DeleteUser_CancelDelete()
        {
            // Arrange
            var users = new List<UserDto>
            {
                new UserDto { Id = "1", FirstName = "John", LastName = "Doe", Email = "johndoe@email.com" }
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
            var navigationManager = Services.GetRequiredService<FakeNavigationManager>();

            // Act
            var confirmButton = component.Find("button:contains('Cancel')");
            confirmButton.Click();

            Assert.Equal("/", new Uri(navigationManager.Uri).AbsolutePath);
        }
    }
}
