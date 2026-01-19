using System.Security.Claims;
using System.Text.Json;

public static class JwtClaimsParser
{
    public static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
    {
        var parts = jwt.Split('.');
        if (parts.Length != 3) return [];

        var payload = parts[1];
        var jsonBytes = ParseBase64WithoutPadding(payload);
        var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes) ?? [];

        var claims = new List<Claim>();
        foreach (var kvp in keyValuePairs)
        {
            if (kvp.Key is "role" or "roles")
            {
                if (kvp.Value is JsonElement el)
                {
                    if (el.ValueKind == JsonValueKind.Array)
                    {
                        foreach (var r in el.EnumerateArray())
                            claims.Add(new Claim(ClaimTypes.Role, r.GetString() ?? ""));
                    }
                    else if (el.ValueKind == JsonValueKind.String)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, el.GetString() ?? ""));
                    }
                    else
                    {
                        claims.Add(new Claim(ClaimTypes.Role, el.ToString() ?? ""));
                    }
                }
                else
                {
                    claims.Add(new Claim(ClaimTypes.Role, kvp.Value.ToString() ?? ""));
                }

                continue;
            }

            claims.Add(new Claim(kvp.Key, kvp.Value.ToString() ?? ""));
        }

        return claims;
    }

    private static byte[] ParseBase64WithoutPadding(string base64)
    {
        base64 = base64.Replace('-', '+').Replace('_', '/');
        return Convert.FromBase64String(base64.PadRight(base64.Length + (4 - base64.Length % 4) % 4, '='));
    }
}
