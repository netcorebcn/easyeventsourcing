using System;

namespace OrderApiSample
{
    public class CreateOrderCommand
    {
        public string Description { get; }

        public CreateOrderCommand(string description) => Description = description;
    }
}