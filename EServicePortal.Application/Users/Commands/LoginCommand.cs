using System.Security.Claims;
using EServicePortal.Application.Common.Interfaces;
using EServicePortal.Application.Common.Wrappers;
using EServicePortal.Application.Users.DTOs;
using FluentValidation;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;

namespace EServicePortal.Application.Users.Commands;

public record LoginCommand(string Username, string Password) : ICommand<LoginResponse>;

internal sealed class LoginValidator : AbstractValidator<LoginCommand>
{
    public LoginValidator()
    {
        RuleFor(x => x.Username).NotNull().NotEmpty();
        RuleFor(x => x.Password).NotNull().NotEmpty();
    }
}

internal sealed class LoginCommandHandler : ICommandHandler<LoginCommand, LoginResponse>
{
    private readonly IAppDbContext _dbContext;
    private readonly IDataProtectionProvider _protectionProvider;
    private readonly ITokenService _tokenService;


    public LoginCommandHandler(IAppDbContext dbContext, ITokenService tokenService, IDataProtectionProvider protectionProvider)
    {
        _dbContext = dbContext;
        _tokenService = tokenService;
        _protectionProvider = protectionProvider;
    }

    public async Task<Response<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users
            .Include(x => x.Roles).ThenInclude(x => x.Role)
            .Where(x => x.Username.Equals(request.Username)).FirstOrDefaultAsync(cancellationToken);

        if (user == null)
        {
            //TODO: Throw explicit exception
            throw new Exception("Signin failed");
        }

        var dataProtector = _protectionProvider.CreateProtector(user.SecurityStamp);
        var unprotectedPassword = dataProtector.Unprotect(user.PasswordHash);
        var passwordCheck = $"{user.SecurityStamp}+{request.Password}";
        if (!unprotectedPassword.Equals(passwordCheck))
        {
            //TODO: Throw explicit exception
            throw new Exception("Signin failed");
        }

        var expires = DateTime.UtcNow.AddDays(3);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Sid, user.Id.Value.ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Username),
            new Claim(ClaimTypes.GivenName, user.DisplayName ?? ""),
            new Claim(ClaimTypes.Email, user.EmailAddress ?? ""),
            new Claim(ClaimTypes.MobilePhone, user.Mobile ?? "")
        };

        foreach (var role in user.Roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role.Role.Name));
        }

        var token = _tokenService.GenerateToken(claims, expires);
        var result = new LoginResponse(token, expires);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return Result.Success(result);
    }
}
