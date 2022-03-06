using CleanArchitecture.Exceptions;

namespace DeliveryTracker.Domain.ValueObjects;

public record Location
{
    public Location(int houseNumber, string addressLine, string postCode)
    {
        if (string.IsNullOrWhiteSpace(addressLine))
        {
            throw new DomainException<Location>(
                "A location must provide an address line");
        }

        if (string.IsNullOrWhiteSpace(postCode))
        {
            throw new DomainException<Location>(
                "A location must have a postcode");
        }

        HouseNumber = houseNumber;
        AddressLine = addressLine;
        PostCode = postCode;
    }

    public int HouseNumber { get; init; }
    public string AddressLine { get; init; }
    public string PostCode { get; init; }
}