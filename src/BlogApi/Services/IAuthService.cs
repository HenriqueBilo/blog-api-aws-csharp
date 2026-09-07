namespace BlogApi.Services;

public interface IAuthService
{
    Task SignUpAsync(string email, string password);
    Task ConfirmSignUpAsync(string email, string code);
    Task<(string IdToken, string AccessToken, string RefreshToken)> LoginAsync(string email, string password);
}
