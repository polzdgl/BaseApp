using BaseApp.Data.User.Dtos;

namespace BaseApp.ServiceProvider.Interfaces.Auth
{
    public interface IAuthApiClient 
    {
        Task<HttpResponseMessage> RegisterAsync(UserRegisterDto userRegisterDto, CancellationToken cancellationToken = default);
        Task<HttpResponseMessage> LoginAsync(UserLoginDto userLoginDto, CancellationToken cancellationToken = default);
    }
}
