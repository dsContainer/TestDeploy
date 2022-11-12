using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.OpenSsl;
using System.Security.Cryptography.X509Certificates;

namespace DigitalSignature.Utilities
{
    public class SignatureUtils
    {
        public static string createCertificate(string userName)
        {
            string subject = "C=GB, ST=Berkshire, L=Reading, O=MY COMPANY";
            string localLocation = "E:\\C#\\Resources\\Signing\\Certificate\\testGenCer";
            try
            {
                AsymmetricCipherKeyPair CertificateKey;

                //let us first generate the root certificate
                X509Certificate2 X509RootCert = Cryptography.CreateCertificate(userName, subject, 12, out CertificateKey);

                //now let us write the certificates files to the folder 
                File.WriteAllBytes(localLocation + "\\" + "X509Cert.der", X509RootCert.RawData);

                string PublicPEMFile = localLocation + "\\" + "X509Cert-public.pem";
                string PrivatePEMFile = localLocation + "\\" + "X509Cert-private.pem";

                //now let us also create the PEM file as well in case we need it
                using (TextWriter textWriter = new StreamWriter(PublicPEMFile, false))
                {
                    PemWriter pemWriter = new PemWriter(textWriter);
                    pemWriter.WriteObject(CertificateKey.Public);
                    pemWriter.Writer.Flush();
                }

                //now let us also create the PEM file as well in case we need it
                using (TextWriter textWriter = new StreamWriter(PrivatePEMFile, false))
                {
                    PemWriter pemWriter = new PemWriter(textWriter);
                    pemWriter.WriteObject(CertificateKey.Private);
                    pemWriter.Writer.Flush();
                }

                return ("Success: The security certificates have been succcessfully generated.");
            }
            catch (Exception ex)
            {
                return ("Error while generating certificates. ");
            }
        }
    }
}
