namespace Application.Common.Interface;

public interface IDomainEventService
{
    Task Publish(DomainEvent domainEvent);
}