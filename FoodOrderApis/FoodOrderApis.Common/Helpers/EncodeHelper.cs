using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;

namespace FoodOrderApis.Common.Helpers;

public static class EncodeHelper
{
    public static RsaSecurityKey CreateRsaKey()
    {
        string publicKey =
            "x5VNfbyqcld4Wn_IczjOnsZjTcisaCN1ttoqAxRHXZwiAj-qY-SU-cGiOslduEfUsIbqY6dtQJhnsP32yHiPqZDYyC2PNGavD4VANzewyXGaptO3-ojMv9n0G7y8pn031DXw14tP3t1mjPqqcU-aDvTfvEOY0vJ5fqfov5Odqf4jX264LbeKk9Njuz3VWGlFhtVCaQ7ySNAzx_WWln6aE_QC4Vq9ycqvi7y1jce1PJvoK6mYlzXkeAR-yix9XjropgsTklBmx-a0pSzT7vBKVri0asBME4tkwpG-byomDp_XP8jne2Wc8_eIwTrbmEV18KMxZI-7V_voYeF36lUe4w";
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
