using BaseApp.Data.User.Dtos;

namespace BaseApp.ServiceProvider.Auth.Interfaces
{
    public interface IAuthProvider
    {
        Task<HttpResponseMessage> RegisterAsync(UserRegisterDto userRegisterDto, CancellationToken cancellationToken = default);
        Task<HttpResponseMessage> LoginAsync(UserLoginDto userLoginDto, CancellationToken cancellationToken = default);
    }
}
