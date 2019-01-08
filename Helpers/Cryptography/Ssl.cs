using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace ADL.Cryptography.SSL
{
    public class Ssl
    {
        /// <summary>
        /// @TODO implement a way to collect password from console
        /// </summary>
        /// <param name="password"></param>
        /// <param name="commonName"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static X509Certificate2 CreateCertificate(string password, string commonName)
        {
            var sanBuilder = new SubjectAlternativeNameBuilder();
            sanBuilder.AddIpAddress(IPAddress.Loopback);
            sanBuilder.AddIpAddress(IPAddress.IPv6Loopback);
            sanBuilder.AddDnsName("localhost");
            sanBuilder.AddDnsName(Environment.MachineName);

            var distinguishedName = new X500DistinguishedName($"CN={commonName}");

            using (var rsa = RSA.Create(2048))//@TODO get the key size from configs
            {
                var request = new CertificateRequest(distinguishedName, rsa, HashAlgorithmName.SHA256,RSASignaturePadding.Pkcs1);

                request.CertificateExtensions.Add(
                    new X509KeyUsageExtension(X509KeyUsageFlags.DataEncipherment | X509KeyUsageFlags.KeyEncipherment | X509KeyUsageFlags.DigitalSignature , false));

                request.CertificateExtensions.Add(
                    new X509EnhancedKeyUsageExtension(
                        new OidCollection { new Oid("1.3.6.1.5.5.7.3.1") }, false));

                request.CertificateExtensions.Add(sanBuilder.Build());

                var certificate = request.CreateSelfSigned(new DateTimeOffset(DateTime.UtcNow.AddDays(-1)), new DateTimeOffset(DateTime.UtcNow.AddDays(3650)));
Console.WriteLine("gen ssl");
Console.WriteLine(password);
                return new X509Certificate2(certificate.Export(X509ContentType.Pfx, password), password, X509KeyStorageFlags.Exportable);
//                return new X509Certificate2(certificate.Export(X509ContentType.Pfx, password), password, X509KeyStorageFlags.MachineKeySet);//@TODO this doesnt work on macosx https://github.com/dotnet/corefx/issues/19508
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataDir"></param>
        /// <param name="fileName"></param>
        /// <param name="certificate"></param>
        /// <returns></returns>
        public static bool WriteCertificateFile(DirectoryInfo dataDir, string fileName, byte[] certificate)
        {
            Console.WriteLine(dataDir.FullName+"/"+fileName);
            File.WriteAllBytes(dataDir.FullName+"/"+fileName, certificate);
            return true;
        }
        
        private static X509Certificate2 LoadPrivateKey(string fileName, string password)
        {
            return new X509Certificate2(fileName, password);
        }

        public static X509Certificate2 LoadPublicKey(string fileName, string password)
        {
            return new X509Certificate2(fileName, password);
        }
        
        public static bool Verify(byte[] data, X509Certificate2 publicKey, byte[] signature)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            if (publicKey == null)
            {
                throw new ArgumentNullException("publicKey");
            }

            if (signature == null)
            {
                throw new ArgumentNullException("signature");
            }

            var provider = (RSACryptoServiceProvider)publicKey.PublicKey.Key;
            return provider.VerifyData(data, new SHA1CryptoServiceProvider(), signature);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="certPath"></param>
        /// <returns></returns>
        public static byte[] Sign(byte[] data, X509Certificate2 privateKey)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            if (privateKey == null)
            {
                throw new ArgumentNullException("privateKey");
            }

            if (!privateKey.HasPrivateKey)
            {
                throw new ArgumentException("invalid certicate", "privateKey");
            }

            var provider = (RSACryptoServiceProvider)privateKey.PrivateKey;
            return provider.SignData(data, new SHA1CryptoServiceProvider());
        }
    }
}