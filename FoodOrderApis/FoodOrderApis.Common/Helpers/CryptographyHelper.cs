using System.Security.Cryptography;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace FoodOrderApis.Common.Helpers;

public static class CryptographyHelper
{
    /*public static JsonWebKey CreateJsonWebKey(int keySize = 2048)
    {
        var key = CryptoHelper.CreateRsaSecurityKey();
        var jwk = JsonWebKeyConverter.ConvertFromRSASecurityKey(key);
        jwk.Alg = IdentityServerConstants.RsaSigningAlgorithm.RS256.ToString();
        
        File.WriteAllText("myjwk.jwk", JsonConvert.SerializeObject(jwk));
        return jwk;
    }*/
    
    public static RsaSecurityKey CreateRsaKey()
    {
        /*string publicKey =
            "5ZIZVzV3Nwh8Ljb-9c1kdlMXhe72B5VvNwd7xnL-zPn1QY5JufJViVXdggKWdDaQ0Lrrgz39Y1xbMWit9RtipRU8kjN44dB41ZKG2sEVkmT2kgy0YTsDWhlHBpaCI44bHBO6E8IeScQ-qMVEjUqMZc-mvaCXCLH4uRErCkLnj8LcmwjwchSgozXanL4bfoHWc3OnU_suvVJY-azy6WaYBL5-cNytB5o_lCaUwmbk34sQyH2qAh8o5sJtrQXzpi-RrYf2K9inUdAgO0N1nz_Lx-Dgk-rFkqF9nnyvwmooi4QJoxcc9casc0JtbSbSf4sysbjbFq1Mt_JV_K8HDqBWaw";*/
        //local
        string publicKey =
            "qmj0ka8pRXGBSYUEPkaxvBfY7-7FmJkn1KuFUq0vTc15mtv1z5AXTaC2m9pWhX288XD95bRaYXRdmIROaWHYW1HKWlYqMth0uY40u_97S1V2BX-4s77Q1cpyD30-EL337LUE9UCt4cSiR1CFPcxvj3AAmpKlmz0rDG0QYfJDXGqRWOiCvt5jl6JKqNzxmg9OwJPBT_1Lc17HZFnGiS_Brw3BT7uPqeHmjoTaEfjEPLJSOfzIitJGZDyEtSUC8KJ7UkLTdWtsWaRSrFfBNNmDBpbk2ThXbe8Fgiikwpm278Wu7hpmSzB03k0LMjwtN9w4ym2ppLJ-3LelrxpiRXq-7Q\n";
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
