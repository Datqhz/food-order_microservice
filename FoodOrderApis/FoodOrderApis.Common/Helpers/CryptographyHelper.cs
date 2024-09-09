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
        // local
        string publicKey =
            "xKBAKEGjEywbAJUxlGRmk52IdgyZi7g7A_V2Wngfq-aPI4Apu9oUu0MQAeEVNIpqaNE8UmsZPldjiOT0-osEOCsdGBMqrGjMMx-8ZzIvAQIsjX7TqToKc0MK8o-mIw55sgEGr27-NCQFaIc7816deCBX4eg2VA1jL7enw53SoBl4DInHlb2MYkUQJAP9eLKaO4j5Qe_zz4_wynsgEUYneSicWGIGF-AzIeXABFyQitQ8V9B-DbgyGLFbDTSwz-wjQMaA8do5MTD0Pu1IfucG0nYjSxx-HIfVpdxiP5rgLeU90TDnvpvXyWcOMDyDDoE3m59wiW3z4Tz_rIwul2j_yQ";
        /*//docker
        string publicKey =
            "ust_cElVqYP-x3OsHHP5AZKjmpyWoCcPIqrlY8QwOf4glDvlHS9DXGv2IBn5fdPjbpeT2j6EdA-P3L8YurCczdMVTME3es1HHrd15YKl98hlbBoRGTx9LtnnsByQs7pX4xj4zoNpe5Kf1bpL-hkGbti3_ffQ-SegRJMl9KEXPe8I_msWelQKKBESbBpePDz8VvXY0Sp4gSWZnWYDIJZHaTHJacFi51xMXWkmljL3vpBqXweLejN7qAq_XwcU8Y-AZyfJZ7TanuccHUvOrtcy3iPrIr1lTqyHAqHHMTToYqe_b2gNY66L5qoM8nJiVyVckVgl7Hlf8yUSHg70BcWFzw";*/
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
