namespace api.makebe.agenda.domain.Helpers
{
    public static class PropiedadesHelper
    {
        public static Guid ParseGuidOrDefault(string? value)
        {
            return Guid.TryParse(value, out var result) ? result : Guid.Empty;
        }

        public static string GuidToStringOrEmpty(Guid? value)
        {
            return value.HasValue && value.Value != Guid.Empty
                ? value.Value.ToString()
                : string.Empty;
        }

        public static string DefaultIfNull(string? value)
        {
            return value ?? string.Empty;
        }
    }
}
