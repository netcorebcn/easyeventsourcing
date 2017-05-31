using System;

namespace EasyEventSourcing.Aggregate
{
    public class DomainException : Exception
    {
        public DomainException()
        {
            
        }

        public DomainException(string message) : base(message)
        {
            
        }
    }
}