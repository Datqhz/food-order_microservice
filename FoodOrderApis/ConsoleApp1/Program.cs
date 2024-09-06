// See https://aka.ms/new-console-template for more information


using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

using (RSA rsa = RSA.Create(2048)) // 2048 bit RSA key
{
    // Tạo các thuộc tính cho chứng chỉ
    var request = new CertificateRequest(
        "CN=MySelfSignedCertificate", // Common Name (CN) của chứng chỉ
        rsa,
        HashAlgorithmName.SHA256, // Thuật toán băm
        RSASignaturePadding.Pkcs1 // Padding cho chữ ký RSA
    );

    // Thêm các mục đích cho chứng chỉ (chữ ký số, mã hóa, v.v.)
    request.CertificateExtensions.Add(
        new X509KeyUsageExtension(X509KeyUsageFlags.DigitalSignature, false)
    );

    // Tạo và ký chứng chỉ, chứng chỉ sẽ có hiệu lực trong 1 năm
    var certificate = request.CreateSelfSigned(
        DateTimeOffset.Now, // Ngày bắt đầu có hiệu lực
        DateTimeOffset.Now.AddYears(1) // Ngày hết hạn
    );

    // Chuyển đổi chứng chỉ sang định dạng X509Certificate2 để dễ dàng sử dụng
    var certWithPrivateKey = new X509Certificate2(certificate.Export(X509ContentType.Pfx, "my_app_food_order"));
    
    var pfxBytes = certWithPrivateKey.Export(X509ContentType.Pfx, "my_app_food_order"); // Xuất với mật khẩu bảo vệ
    File.WriteAllBytes("myCertificate.pfx", pfxBytes);
    Console.WriteLine("Chứng chỉ đã được lưu thành công dưới dạng PFX.");
}