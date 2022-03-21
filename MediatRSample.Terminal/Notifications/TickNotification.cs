using MediatR;
namespace MediatRSample.Terminal.Notifications;
public record struct TickNotification(int CurrentTick) : INotification;
