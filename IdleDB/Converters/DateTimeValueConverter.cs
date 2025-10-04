using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace IdleDB.Converters;

public class DateTimeValueConverter : ValueConverter<DateTime, DateTime>
{
    public DateTimeValueConverter() : base(
        modelDate => modelDate.ToUniversalTime(),
        providerDate => DateTime.SpecifyKind(providerDate, DateTimeKind.Utc))
    {
    }
}
