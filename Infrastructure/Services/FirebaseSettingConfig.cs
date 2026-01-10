using System.Text.Json;

namespace Application.Notifier.Core.Firebase;

public class FirebaseSettingConfig
{
    public string type { get; set; } = string.Empty;
    public string project_id { get; set; } = string.Empty;
    public string private_key_id { get; set; } = string.Empty;
    public string private_key { get; set; } = string.Empty;
    public string client_email { get; set; } = string.Empty;
    public string client_id { get; set; } = string.Empty;
    public string auth_uri { get; set; } = string.Empty;
    public string token_uri { get; set; } = string.Empty;
    public string auth_provider_x509_cert_url { get; set; } = string.Empty;
    public string client_x509_cert_url { get; set; } = string.Empty;
    public string universe_domain { get; set; } = string.Empty;

    public string ToJson()
    {
        // The private_key in appsettings might have actual \n characters or escaped \\n.
        // The binder reads it correctly, but we ensure it's in the format Google expects.
        if (!string.IsNullOrEmpty(private_key))
        {
            private_key = private_key.Replace("\\n", "\n");
        }

        return JsonSerializer.Serialize(this);
    }
}
