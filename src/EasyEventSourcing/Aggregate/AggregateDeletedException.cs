using System;

namespace EasyEventSourcing.Aggregate
{
    internal class AggregateDeletedException : DomainException
    {
        private object id;
        private Type type;

        public AggregateDeletedException(object id, Type type)
        {
            this.id = id;
            this.type = type;
        }
    }
}