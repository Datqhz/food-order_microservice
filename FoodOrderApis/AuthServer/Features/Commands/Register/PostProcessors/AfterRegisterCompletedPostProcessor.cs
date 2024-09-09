using AuthServer.Data.Dtos.Responses;
using MediatR.Pipeline;

namespace AuthServer.Features.Commands.Register.PostProcessors;

public class AfterRegisterCompletedPostProcessor : IRequestPostProcessor<RegisterCommand, RegisterResponse>
{
    private readonly ILogger<AfterRegisterCompletedPostProcessor> _logger;

    public AfterRegisterCompletedPostProcessor(ILogger<AfterRegisterCompletedPostProcessor> logger)
    {
        _logger = logger;
    }
    public async Task Process(RegisterCommand request, RegisterResponse response, CancellationToken cancellationToken)
    {
        _logger.LogInformation("AfterRegisterCompletedPostProcessor");
    }
}
