using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Notifier.Core.Firebase;

public class FirebaseSettingConfig
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("project_id")]
    public string ProjectId { get; set; } = string.Empty;

    [JsonPropertyName("private_key_id")]
    public string PrivateKeyId { get; set; } = string.Empty;

    [JsonPropertyName("private_key")]
    public string PrivateKey { get; set; } = string.Empty;

    [JsonPropertyName("client_email")]
    public string ClientEmail { get; set; } = string.Empty;

    [JsonPropertyName("client_id")]
    public string ClientId { get; set; } = string.Empty;

    [JsonPropertyName("auth_uri")]
    public string AuthUri { get; set; } = string.Empty;

    [JsonPropertyName("token_uri")]
    public string TokenUri { get; set; } = string.Empty;

    [JsonPropertyName("auth_provider_x509_cert_url")]
    public string AuthProviderX509CertUrl { get; set; } = string.Empty;

    [JsonPropertyName("client_x509_cert_url")]
    public string ClientX509CertUrl { get; set; } = string.Empty;

    [JsonPropertyName("universe_domain")]
    public string UniverseDomain { get; set; } = string.Empty;

    public override string ToString()
    {
        string value = JsonSerializer.Serialize(this);
        return value;
    }
}
