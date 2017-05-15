namespace OrderApiSample
{
    public class OrderCreatedEvent
    {
        public string Description { get; }
        public OrderCreatedEvent(string description) => Description = description;
    }
}