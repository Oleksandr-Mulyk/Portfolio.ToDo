using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Identity.Web;
using Portfolio.ToDo.Auth;

namespace Portfolio.ToDo.Auth.Services
{
    public class AuthApiService(ITokenAcquisition tokenAcquisition, IConfiguration configuration)
        : AuthService.AuthServiceBase
    {
        private readonly ITokenAcquisition _tokenAcquisition = tokenAcquisition;
        private readonly IConfiguration _configuration = configuration;

        [Authorize]
        public override async Task<LoginResponse> Login(LoginRequest request, ServerCallContext context)
        {
            string? scope = _configuration["AzureAdB2C:Scope"] ??
                throw new InvalidOperationException("Scope is not set in configuration");

            string token = await _tokenAcquisition.GetAccessTokenForUserAsync([scope]);
            return new LoginResponse { Token = token };
        }
    }
}
