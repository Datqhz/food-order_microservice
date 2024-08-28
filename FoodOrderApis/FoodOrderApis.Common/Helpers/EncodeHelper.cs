using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;

namespace FoodOrderApis.Common.Helpers;

public static class EncodeHelper
{
    public static RsaSecurityKey CreateRsaKey()
    {
        string publicKey =
            "nUSGxUGoDNcKruJyLxG-UHy0P16d_J-DW2VX8VJGg_yjThhAFF-Hvbr1ImnCX9e8A8C522FNffy_PAXZsX7iAnY1vQniYTJL4V-LHA00o2EK42eAqhHRZxn4f8dciHAbHTA6mTLU2eP07L24r4sqN1PWWWiag81c7HCCwsiH6Vc-b6IrTV8N8a7iLCrIbzNVv4yqj4e-oEQa5-ecvSbL41rkXmqwz0JbFCqB3y94hLLGRb_XUBarcI0nahO4HcrFwCxr_7ae5XHnEJKtJpxzCNBnCYtB56fFdRGTonAPEBpOKQI5NJZUXQOshjZuNGz_X8nmbfrFbVA2x6KOI6Krmw";
        string exponent = "AQAB";
        var publicKeyAsBytes = Base64UrlEncoder.DecodeBytes(publicKey);
        var exponentBytes = Base64UrlEncoder.DecodeBytes(exponent);
        var rsaParameter = new RSAParameters
        {
            Modulus = publicKeyAsBytes,
            Exponent = exponentBytes
        };
        var rsaKey = RSA.Create();
        rsaKey.ImportParameters(rsaParameter);
        return new RsaSecurityKey(rsaKey);
    }
}
