using DevFlow.Identity.Application.Authentication.Register;
using DevFlow.Identity.Domain.Authentication.Users;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DevFlow.Identity.Infrastructure.Seed;

internal sealed class IdentitySeeder
{
    private readonly IUserRepository _userRepository;
    private readonly ISender _sender;
    private readonly ILogger<IdentitySeeder> _logger;

    public IdentitySeeder(
        IUserRepository userRepository,
        ISender sender,
        ILogger<IdentitySeeder> logger)
    {
        _userRepository = userRepository;
        _sender = sender;
        _logger = logger;
    }

    public async Task SeedAsync(
        CancellationToken cancellationToken = default)
    {
        foreach (var user in DemoUsers.All)
        {
            await SeedUserAsync(
                user,
                cancellationToken);
        }
    }

    private async Task SeedUserAsync(
        DemoUser demoUser,
        CancellationToken cancellationToken)
    {
        var exists =
            await _userRepository.ExistsByEmailAsync(
                demoUser.Email,
                cancellationToken);

        if (exists)
        {
            //_logger.LogInformation(
            //    "Demo user '{Email}' already exists.",
            //    demoUser.Email);

            return;
        }

        var result =
            await _sender.Send(
                new RegisterCommand(
                    demoUser.Email,
                    demoUser.Password,
                    demoUser.FirstName,
                    demoUser.LastName),
                cancellationToken);

        if (result.IsFailure)
        {
            //_logger.LogWarning(
            //    "Failed to create demo user '{Email}'. Reason: {Reason}",
            //    demoUser.Email,
            //    result.Error?.Description);

            return;
        }

        //_logger.LogInformation(
        //    "Created demo user '{Email}'.",
        //    demoUser.Email);
    }
}
