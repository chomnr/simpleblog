using Application.Common.Models;
using Application.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.BlogUser.EventHandlers;

public class UserRegisterCompletedEventHandler : INotificationHandler<DomainEventNotification<BlogUserCompletedEvent>>
{
    private readonly ILogger<UserRegisterCompletedEventHandler> _logger;
    
    public UserRegisterCompletedEventHandler(ILogger<UserRegisterCompletedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(DomainEventNotification<BlogUserCompletedEvent> notification,
        CancellationToken cancellationToken)
    {
        var domainEvent = notification.DomainEvent;
        _logger.LogInformation("Domain Event: {DomainEvent}", domainEvent.GetType().Name);
        
        return Task.CompletedTask;
    }
}