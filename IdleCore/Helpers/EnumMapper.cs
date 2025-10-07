namespace IdleCore.Helpers;

public interface IEnumMapper
{
    /// <summary>
    /// Get a mapping of the enum from strings to values.
    ///
    /// Must be strings to values instead of the other way around
    /// because typescript only supports string keys.
    /// </summary>
    /// <typeparam name="T">The enum to get a mapping for.</typeparam>
    /// <returns>A mapping of enum strings to values.</returns>
    Dictionary<string, T> GetEnumMapping<T>() where T : struct, Enum;
}

public class EnumMapper : IEnumMapper
{
    public Dictionary<string, T> GetEnumMapping<T>() where T : struct, Enum
    {
        return Enum.GetValues<T>().ToDictionary(v => v.ToString(), v => v);
    }
}
