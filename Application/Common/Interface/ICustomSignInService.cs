using Application.Features.BlogUser;
using Microsoft.AspNetCore.Identity;

namespace Application.Common.Interface;

public interface ICustomSignInService
{
    Task<SignInResult> LoginWithEmailOrUsername(LoginCommand command);
}