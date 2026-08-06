namespace api.makebe.agenda.Configurations;

public sealed class ApiSecurityOptions
{
    public const string SectionName = "ApiSecurity";
    public bool Enabled { get; set; }
    public string HeaderName { get; set; } = "ApiSecurity";
    public string ApiKey { get; set; } = string.Empty;
}
