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
    
    /*
    public static RsaSecurityKey CreateRsaKey()
    {
        // local
        string publicKey =
            "xKBAKEGjEywbAJUxlGRmk52IdgyZi7g7A_V2Wngfq-aPI4Apu9oUu0MQAeEVNIpqaNE8UmsZPldjiOT0-osEOCsdGBMqrGjMMx-8ZzIvAQIsjX7TqToKc0MK8o-mIw55sgEGr27-NCQFaIc7816deCBX4eg2VA1jL7enw53SoBl4DInHlb2MYkUQJAP9eLKaO4j5Qe_zz4_wynsgEUYneSicWGIGF-AzIeXABFyQitQ8V9B-DbgyGLFbDTSwz-wjQMaA8do5MTD0Pu1IfucG0nYjSxx-HIfVpdxiP5rgLeU90TDnvpvXyWcOMDyDDoE3m59wiW3z4Tz_rIwul2j_yQ";
        /#1#/docker
        string publicKey =
            "ust_cElVqYP-x3OsHHP5AZKjmpyWoCcPIqrlY8QwOf4glDvlHS9DXGv2IBn5fdPjbpeT2j6EdA-P3L8YurCczdMVTME3es1HHrd15YKl98hlbBoRGTx9LtnnsByQs7pX4xj4zoNpe5Kf1bpL-hkGbti3_ffQ-SegRJMl9KEXPe8I_msWelQKKBESbBpePDz8VvXY0Sp4gSWZnWYDIJZHaTHJacFi51xMXWkmljL3vpBqXweLejN7qAq_XwcU8Y-AZyfJZ7TanuccHUvOrtcy3iPrIr1lTqyHAqHHMTToYqe_b2gNY66L5qoM8nJiVyVckVgl7Hlf8yUSHg70BcWFzw";#1#
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
    */
    
    public static RsaSecurityKey CreateRsaKey()
    {
        string publicKey =
            "xKBAKEGjEywbAJUxlGRmk52IdgyZi7g7A_V2Wngfq-aPI4Apu9oUu0MQAeEVNIpqaNE8UmsZPldjiOT0-osEOCsdGBMqrGjMMx-8ZzIvAQIsjX7TqToKc0MK8o-mIw55sgEGr27-NCQFaIc7816deCBX4eg2VA1jL7enw53SoBl4DInHlb2MYkUQJAP9eLKaO4j5Qe_zz4_wynsgEUYneSicWGIGF-AzIeXABFyQitQ8V9B-DbgyGLFbDTSwz-wjQMaA8do5MTD0Pu1IfucG0nYjSxx-HIfVpdxiP5rgLeU90TDnvpvXyWcOMDyDDoE3m59wiW3z4Tz_rIwul2j_yQ";
        string exponent = "AQAB";
        // private key
        string d =
            "qmj0ka8pRXGBSYUEPkaxvBfY7-7FmJkn1KuFUq0vTc15mtv1z5AXTaC2m9pWhX288XD95bRaYXRdmIROaWHYW1HKWlYqMth0uY40u_97S1V2BX-4s77Q1cpyD30-EL337LUE9UCt4cSiR1CFPcxvj3AAmpKlmz0rDG0QYfJDXGqRWOiCvt5jl6JKqNzxmg9OwJPBT_1Lc17HZFnGiS_Brw3BT7uPqeHmjoTaEfjEPLJSOfzIitJGZDyEtSUC8KJ7UkLTdWtsWaRSrFfBNNmDBpbk2ThXbe8Fgiikwpm278Wu7hpmSzB03k0LMjwtN9w4ym2ppLJ-3LelrxpiRXq-7Q";
        string dp = "wceEgqDODUEPYLeSU67hd3H-Chvy1F56U9-RqyPsXyDLRsO38RiDJuFoKJeNwv4Nd1VEV3EiDtwFQR4Tu7gE-0o3Ei570VvSHuJySrIP_WPHjJAWBn7cK9eI3MSf0oVOwgFRocya4jMSr2FwXvmy35Hj8H1INd4l2s_UXkr9LVs";
        string dq = "SI7EN02jVDQRtT5mH7Ogli6SlFp2jYNwipvv9wHArpkY3C1hBl2UvWVOhpuGkrOacxGlB5erKUTS3XKvfFRfb1GPFKXJBW-RfGrqiCscFVlhg5lC0ppkaAPkJCLUvHVPe-Z-N8NYcogrnqP1tIpQ9HaYeOddk6N-2dbJRKXKQAk";
        
        string p = "5vmxRvySW0MxhlDLpjg4wgpQtRI5JicOwaa29wHkiNqn_sQhyaEoo6axmQ4KGMJ5KhuD7j1YTPt_iZE4Ls0BmmSziV9spYvnvt_uLqUykOvNU2tpYbD7UvHrPrpJ57XYbiRvoLYzf7o4YHQ2U_UAeeWw_7_mUEoadbOqfR83Rdc";
        string q = "2e3Y7pxmU-caV8Jfzr6hdVxRsQeu_-HxPcPF15EmKGFTRIsQ5D-UMOyqBjgAKrs3CFrDPaxncYWoFLbNEezlnz5Brh5J_6Gowg7RP1-9LYnpiETHrt-PZcscxJ9J5871RHJARdltLguF4C1Ke5zDBjyAxLnqoquCYrY3asDd818";
        string qi = "RaZbdwuC4QIsG7rwfBdHiofQYw8y6NYgyDUHgR-BJpKHhcPKDe9zFiv4fa2RLxGsQTRKsvGPn0Az5N0oZmSF7jD6_YnYeJil0sew4G9Ft8h2RyG2UmlucHmV_KfOC384XS0NvyzRowiHvlhVjqaxAP0bCDJNvEGFjmGBpVymYqs";
        
        var publicKeyAsBytes = Base64UrlEncoder.DecodeBytes(publicKey);
        var exponentBytes = Base64UrlEncoder.DecodeBytes(exponent);
        var dAsBytes = Base64UrlEncoder.DecodeBytes(d);
        var dpAsBytes = Base64UrlEncoder.DecodeBytes(dp);
        var dqAsBytes = Base64UrlEncoder.DecodeBytes(dq);
        var pAsBytes = Base64UrlEncoder.DecodeBytes(p);
        var qAsByte = Base64UrlEncoder.DecodeBytes(q);
        var qiAsBytes = Base64UrlEncoder.DecodeBytes(qi);
        
        var rsaParameter = new RSAParameters
        {
            Modulus = publicKeyAsBytes,
            Exponent = exponentBytes,
            D = dAsBytes,
            Q = qAsByte,
            DP = dpAsBytes,
            DQ = dqAsBytes,
            P = pAsBytes,
            InverseQ = qiAsBytes
        };
        var rsaKey = RSA.Create();
        rsaKey.ImportParameters(rsaParameter);
        return new RsaSecurityKey(rsaKey);
    }
    
}
