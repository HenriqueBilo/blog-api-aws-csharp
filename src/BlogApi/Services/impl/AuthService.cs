using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;

namespace BlogApi.Services.impl;

public class AuthService : IAuthService
{
    private readonly IAmazonCognitoIdentityProvider _cognitoClient;
    private readonly string _clientId;

    public AuthService(IAmazonCognitoIdentityProvider cognito, IConfiguration configuration)
    {
        _cognitoClient = cognito;
        _clientId = configuration["Aws:Cognito:ClientId"]!;
    }

    public async Task SignUpAsync(string email, string password)
    {
        var request = new SignUpRequest()
        {
            ClientId = _clientId,
            Username = email,
            Password = password,
            UserAttributes = new List<AttributeType>
            {
                new()
                {
                    Name = "email",
                    Value = email
                }
            }
        };

        await _cognitoClient.SignUpAsync(request);
    }

    public async Task ConfirmSignUpAsync(string email, string code)
    {
        var request = new ConfirmSignUpRequest()
        {
            ClientId = _clientId,
            Username = email,
            ConfirmationCode = code
        };

        await _cognitoClient.ConfirmSignUpAsync(request);
    }

    public async Task<(string IdToken, string AccessToken, string RefreshToken)> LoginAsync(string email, string password)
    {
        var request = new InitiateAuthRequest()
        {
            ClientId = _clientId,
            AuthFlow = AuthFlowType.USER_PASSWORD_AUTH,
            AuthParameters = new Dictionary<string, string>
            {
                { "USERNAME", email },
                { "PASSWORD", password }
            }
        };

        var response = await _cognitoClient.InitiateAuthAsync(request);

        return (response.AuthenticationResult.IdToken, response.AuthenticationResult.AccessToken, response.AuthenticationResult.RefreshToken);
    }

   
}
