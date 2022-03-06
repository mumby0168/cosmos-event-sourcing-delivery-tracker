using System.Security.Cryptography;
using System.Text;
using CleanArchitecture.Exceptions;

namespace DeliveryTracker.Domain.Identifiers;

/// <summary>
/// A ID that can uniquely identify a schedule
/// </summary>
/// <remarks>
/// 14 digits
/// Made up of the {DriverCode[4]}-{Day[2]}{Month[2]{Year[4]}-{RandomDigits[4]}
/// </remarks>
public record ScheduleId
{
    public ScheduleId(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new DomainException<ScheduleId>(
                "A value must be provided for a schedule ID");
        }

        if (value.Length != 14)
        {
            throw new DomainException<ScheduleId>(
                "A schedule ID must be 12 characters");
        }
        
        
        Value = value;
    }

    public static ScheduleId NewScheduleId(string driverCode)
    {
        if (string.IsNullOrWhiteSpace(driverCode) || driverCode.Length != 4)
        {
            throw new DomainException<ScheduleId>(
                "A driver code must be 4 digits");
        }

        var dateString = DateTime.UtcNow.ToString("ddMMyy");

        return new ScheduleId($"{driverCode}-{dateString}-{RandomDigits()}");
    }

    private static string RandomDigits()
    {
        var sb = new StringBuilder();
        
        for (var i = 0; i < 4; i++)
        {
            sb.Append(RandomNumberGenerator.GetInt32(0, 10));
        }

        return sb.ToString();
    }

    public string Value { get; }
}