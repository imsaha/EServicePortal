using EServicePortal.Application.Common.Interfaces;
using EServicePortal.Application.Common.Wrappers;
using EServicePortal.Domain.Entites;
using FluentValidation;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;

namespace EServicePortal.Application.Users.Commands;

public record SignupCommand(
    string Username,
    string Password,
    string ConfirmPassword,
    string Name,
    string Email,
    string Mobile,
    string Role) : ICommand;

internal sealed class SignupCommandValidator : AbstractValidator<SignupCommand>
{
    public SignupCommandValidator()
    {
        RuleFor(x => x.Email).NotEmpty().NotNull().EmailAddress();
        RuleFor(x => x.Username).NotEmpty().NotNull();
        RuleFor(x => x.Mobile).NotEmpty().NotNull();
        RuleFor(x => x.Name).NotEmpty().NotNull();
        RuleFor(x => x.Password).NotEmpty().NotNull();
        RuleFor(x => x.ConfirmPassword).NotEmpty()
            .Must((command, confirmPassword) => confirmPassword == command.Password).WithMessage("Confirm password must be equal to password.");
    }
}

internal sealed class SignupCommandHandler : ICommandHandler<SignupCommand>
{
    private readonly IAppDbContext _dbContext;
    private readonly IDataProtectionProvider _protectionProvider;

    public SignupCommandHandler(IDataProtectionProvider protectionProvider, IAppDbContext dbContext)
    {
        _protectionProvider = protectionProvider;
        _dbContext = dbContext;
    }
    public async Task<Response> Handle(SignupCommand request, CancellationToken cancellationToken)
    {
        var userExists = await _dbContext.Users.Where(x => x.Username == request.Username).AnyAsync(cancellationToken);
        if (userExists)
        {
            //TODO: Throw explicit exception
            throw new Exception("Username taken");
        }

        var user = new User
        {
            Username = request.Username,
            DisplayName = request.Name,
            EmailAddress = request.Email,
            Mobile = request.Mobile
        };

        var dataProtector = _protectionProvider.CreateProtector(user.SecurityStamp);
        var hashedPassword = dataProtector.Protect($"{user.SecurityStamp}+{request.ConfirmPassword}");
        user.PasswordHash = hashedPassword;
        _dbContext.Users.Add(user);

        if (!string.IsNullOrEmpty(request.Role))
        {
            var role = await _dbContext.Roles.FirstOrDefaultAsync(x => x.Name.ToLower() == request.Role.ToLower(), cancellationToken);
            if (role == null)
            {
                //TODO: Throw explicit exception
                throw new Exception("Unsupported role");
            }

            user.Roles.Add(new UserRole
            {
                RoleId = role.Id,
                Role = role
            });
        }
        else
        {
            var customer = await _dbContext.Roles.FirstOrDefaultAsync(x => x.Name == "Customer", cancellationToken);
            if (customer == null)
            {
                //TODO: Throw explicit exception
                throw new Exception("Role not fund!");
            }

            user.Roles.Add(new UserRole
            {
                RoleId = customer.Id,
                Role = customer
            });
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
