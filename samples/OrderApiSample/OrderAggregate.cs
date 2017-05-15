using EasyEventSourcing.Aggregate;

namespace OrderApiSample
{
    public class OrderAggregate : AggregateRoot
    {
        public string Description { get; private set; }

        public OrderAggregate(string description) =>
            RaiseEvent(new OrderCreatedEvent(description));
        
        public void Apply(OrderCreatedEvent @event) => Description = @event.Description;
    }
}