using MediatR;
using MediatRSample.Terminal.Notifications;
using Microsoft.Extensions.Logging;

namespace MediatRSample.Terminal.NotificationHandlers;

public class TickNotificationHandler : INotificationHandler<TickNotification>
{
    private readonly ILogger<TickNotificationHandler> _logger;

    public TickNotificationHandler(ILogger<TickNotificationHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(TickNotification notification, CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            _logger.LogInformation("{NotificationCurrentTick}. tick was cancelled", notification.CurrentTick);
            cancellationToken.ThrowIfCancellationRequested();
        }
        _logger.LogInformation("Handled the {NotificationCurrentTick}. tick notification", notification.CurrentTick);
        return Task.CompletedTask;
    }
}
