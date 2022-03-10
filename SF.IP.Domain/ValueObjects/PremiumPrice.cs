using SF.IP.Domain.Common;
using System.Collections.Generic;


namespace SF.IP.Domain.ValueObjects;
public class PremiumPrice : BaseValueObject
{
    public double Price { get; private set; }
    public string Currency { get; private set; }

    public PremiumPrice() { }

    public PremiumPrice(double price, string currency)
    {
        Price = price;
        Currency = currency;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        // Using a yield return statement to return each element one at a time
        yield return Price;
        yield return Currency;
    }
}

