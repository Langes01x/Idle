using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace IdleDB.Converters;

public class NullableDateTimeValueConverter : ValueConverter<DateTime?, DateTime?>
{
    public NullableDateTimeValueConverter() : base(
        modelDate => modelDate == null ? modelDate : modelDate.Value.ToUniversalTime(),
        providerDate => providerDate == null ? providerDate : DateTime.SpecifyKind(providerDate.Value, DateTimeKind.Utc))
    {
    }
}
