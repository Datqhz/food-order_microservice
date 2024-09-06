using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;

namespace FoodOrderApis.Common.Helpers;

public static class EncodeHelper
{
    public static RsaSecurityKey CreateRsaKey()
    {
        /*string publicKey =
            "5ZIZVzV3Nwh8Ljb-9c1kdlMXhe72B5VvNwd7xnL-zPn1QY5JufJViVXdggKWdDaQ0Lrrgz39Y1xbMWit9RtipRU8kjN44dB41ZKG2sEVkmT2kgy0YTsDWhlHBpaCI44bHBO6E8IeScQ-qMVEjUqMZc-mvaCXCLH4uRErCkLnj8LcmwjwchSgozXanL4bfoHWc3OnU_suvVJY-azy6WaYBL5-cNytB5o_lCaUwmbk34sQyH2qAh8o5sJtrQXzpi-RrYf2K9inUdAgO0N1nz_Lx-Dgk-rFkqF9nnyvwmooi4QJoxcc9casc0JtbSbSf4sysbjbFq1Mt_JV_K8HDqBWaw";*/
        //local
        string publicKey =
            "xKBAKEGjEywbAJUxlGRmk52IdgyZi7g7A_V2Wngfq-aPI4Apu9oUu0MQAeEVNIpqaNE8UmsZPldjiOT0-osEOCsdGBMqrGjMMx-8ZzIvAQIsjX7TqToKc0MK8o-mIw55sgEGr27-NCQFaIc7816deCBX4eg2VA1jL7enw53SoBl4DInHlb2MYkUQJAP9eLKaO4j5Qe_zz4_wynsgEUYneSicWGIGF-AzIeXABFyQitQ8V9B-DbgyGLFbDTSwz-wjQMaA8do5MTD0Pu1IfucG0nYjSxx-HIfVpdxiP5rgLeU90TDnvpvXyWcOMDyDDoE3m59wiW3z4Tz_rIwul2j_yQ";
        //docker
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
