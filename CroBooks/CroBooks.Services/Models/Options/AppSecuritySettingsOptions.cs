namespace CroBooks.Services.Models.Options
{
    public class AppSecuritySettingsOptions
    {
        public const string AppSecuritySettings = "AppSecuritySettings";

        public string Key { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
    }
}
