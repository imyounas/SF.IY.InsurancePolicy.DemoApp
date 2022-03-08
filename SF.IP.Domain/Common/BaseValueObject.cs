using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF.IP.Domain.Common
{

    // Sample implementation from MS Docs 
    //https://docs.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/implement-value-objects
    public abstract class BaseValueObject
    {
        protected static bool EqualOperator(BaseValueObject left, BaseValueObject right)
        {
            if (ReferenceEquals(left, null) ^ ReferenceEquals(right, null))
            {
                return false;
            }
            return ReferenceEquals(left, null) || left.Equals(right);
        }

        protected static bool NotEqualOperator(BaseValueObject left, BaseValueObject right)
        {
            return !(EqualOperator(left, right));
        }

        protected abstract IEnumerable<object> GetEqualityComponents();

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != GetType())
            {
                return false;
            }

            var other = (BaseValueObject)obj;

            return this.GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
        }

        public override int GetHashCode()
        {
            return GetEqualityComponents()
                .Select(x => x != null ? x.GetHashCode() : 0)
                .Aggregate((x, y) => x ^ y);
        }

        public static bool operator ==(BaseValueObject one, BaseValueObject two)
        {
            return one?.Equals(two) ?? (one is null && two is null ? true : false);
        }

        public static bool operator !=(BaseValueObject one, BaseValueObject two)
        {
            return !(one?.Equals(two) ?? (one is null && two is null ? true : false));
        }

    }
}
