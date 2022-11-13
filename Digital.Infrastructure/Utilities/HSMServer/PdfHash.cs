using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.security;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;
using System.Text;
using Font = iTextSharp.text.Font;
using Image = iTextSharp.text.Image;
using Rectangle = iTextSharp.text.Rectangle;

namespace Digital.Infrastructure.Utilities.HSMServer
{
    /// <summary>
    /// 
    /// </summary>
    public class PdfHash : IDisposable
    {
        private readonly PdfReader _pdfReader;
        private bool _disposed;
        string default_font = "segoeui.tff";
        public PdfHash(byte[] pdfinputBytes)
        {
            _pdfReader = new PdfReader(pdfinputBytes);
        }

        public byte[] getHash(X509Certificate cer, ICollection<X509Certificate> chain, IByteSigner signer, string sHash, int typeSignature, string base64Image, string textOut, string font, string signatureName, int pageSign = 1,
            int xPoint = 100, int yPoint = 200, int width = 300, int height = 400)
        {
            PdfStamper stp = null;
            AcroFields af = _pdfReader.AcroFields;
            var signatureNameNames = af.GetSignatureNames();
            var signatureNumber = signatureNameNames.Count + 1;
            MemoryStream baos = new MemoryStream();

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            if (signatureNumber > 1)
            {

                stp = PdfStamper.CreateSignature(_pdfReader, baos, '\0', null, true);
            }
            else
            {
                stp = PdfStamper.CreateSignature(_pdfReader, baos, '\0');
            }

            var sap = stp.SignatureAppearance;
            if (af.GetFieldItem(signatureName) == null)
            {
                Rectangle rectangle1 = new Rectangle((float)xPoint, (float)yPoint, (float)width, (float)height);
                sap.SetVisibleSignature(rectangle1, pageSign, signatureName);
            }
            else
            {
                sap.SetVisibleSignature(signatureName);
            }

            sap.Certificate = cer;

            switch (typeSignature)
            {
                case 1:
                    {
                        byte[] bytePng = Convert.FromBase64String(base64Image);
                        Image image = Image.GetInstance(bytePng);
                        sap.Image = image;
                        sap.Acro6Layers = true;
                        sap.Layer2Text = "";
                        break;
                    }
                case 2:
                    {

                        if (font == null)
                        {
                            font = default_font;
                            BaseColor colorSign = new BaseColor(0, 128, 0);
                            var bytes = Resources.segoeui;
                            BaseFont bf = BaseFont.CreateFont(font, BaseFont.IDENTITY_H, BaseFont.EMBEDDED, true, bytes, null);
                            sap.Layer2Font = new Font(bf, 9, Font.NORMAL, colorSign);

                        }

                        string noidung = "\tKý Bởi:  " + signatureName + "\n\n\n\n";
                        noidung += "  Ký Ngày: " + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");

                        sap.Layer2Text = noidung;
                        //sap.Layer2Text.PadLefta(100);
                        break;
                    }
                case 3:
                    {

                        if (font != null)
                        {

                        }

                        Font f = null;
                        if (String.Compare(font.ToLower(), "segoeui.ttf") == 0)
                        {
                            BaseColor colorSign = new BaseColor(0, 128, 128);
                            var bytes = Resources.segoeui;
                            BaseFont bf = BaseFont.CreateFont("segoeui.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED,
                                true, bytes, null);
                            f = new Font(bf, 9, Font.ITALIC, colorSign);
                        }
                        else
                        {
                            BaseColor colorSign = new BaseColor(0, 128, 128);
                            var bytes = Resources.segoeui;
                            BaseFont bf = BaseFont.CreateFont("segoeui.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED,
                                true, bytes, null);
                            f = new Font(bf, 9, Font.ITALIC, colorSign);
                        }
                        sap.Layer2Font = f;
                        string noidung = "     Ký Bởi:  " + signatureName + "\n\n\n\n";
                        noidung += "    Ký Ngày: " + DateTime.Now.ToString("dd-MM-yyyy");

                        byte[] bytePng = Convert.FromBase64String(base64Image);
                        Image image = Image.GetInstance(bytePng);
                        image.ScalePercent(500);
                        image.SetAbsolutePosition(width, height);
                        sap.Image = image;
                        sap.ImageScale = 0.2f;
                        /*sap.Image.ScaleAbsoluteHeight(height/4);*/
                        Rectangle rSignature1 = new Rectangle((float)xPoint + 50, (float)yPoint + 50, (float)xPoint + 50, (float)yPoint + 50);
                        sap.Image.ScaleToFit(rSignature1);

                        sap.Acro6Layers = true;
                        sap.Layer2Text = noidung;
                        /*sap.SignatureGraphic.Border = (int)1;*/
                        /*sap.SignatureGraphic.BorderColor = BaseColor.GREEN;*/
                        /*sap.Layer2Text.PadLeft(100);*/
                        //sap.Layer4Text = noidung;
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
            var externalSignature = new HsmRsaSignature(signer, sHash);

            int estimatedSize = 8192;
            sap.CryptoDictionary = (PdfDictionary)new PdfSignature(PdfName.ADOBE_PPKLITE, PdfName.ADBE_PKCS7_DETACHED)
            {
                Reason = sap.Reason,
                Location = sap.Location,
                SignatureCreator = sap.SignatureCreator,
                Contact = sap.Contact,
                Date = new PdfDate(sap.SignDate),
            };
            sap.PreClose(new Dictionary<PdfName, int>()
            {
                [PdfName.CONTENTS] = estimatedSize * 2 + 2
            });

            string hashAlgorithm = externalSignature.GetHashAlgorithm();
            PdfPKCS7 pdfPkcS7 = new PdfPKCS7(null, chain, hashAlgorithm, false);
            DigestUtilities.GetDigest(hashAlgorithm);
            byte[] secondDigest = DigestAlgorithms.Digest(sap.GetRangeStream(), hashAlgorithm);
            byte[] authenticatedAttributeBytes = pdfPkcS7.getAuthenticatedAttributeBytes(secondDigest, null, null, CryptoStandard.CMS);
            byte[] digest = externalSignature.Sign(authenticatedAttributeBytes);
            pdfPkcS7.SetExternalDigest(digest, (byte[])null, externalSignature.GetEncryptionAlgorithm());
            byte[] encodedPkcS7 = pdfPkcS7.GetEncodedPKCS7(secondDigest);
            byte[] bytes2 = new byte[estimatedSize];
            Array.Copy((Array)encodedPkcS7, 0, (Array)bytes2, 0, encodedPkcS7.Length);
            PdfDictionary update = new PdfDictionary();
            update.Put(PdfName.CONTENTS, (PdfObject)new PdfString(bytes2).SetHexWriting(true));
            sap.Close(update);

            var bytearr = baos.ToArray();
            return bytearr;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes object
        /// </summary>
        /// <param name="disposing">Flag indicating whether managed resources should be disposed</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                // Dispose managed objects
                if (disposing)
                {
                    _pdfReader.Dispose();
                }

                // Dispose unmanaged objects

                _disposed = true;
            }
        }
    }

    public enum Pkcs11InteropHashAlgorithm
    {
        /// <summary>
        /// The SHA1 hash algorithm
        /// </summary>
        SHA1,

        /// <summary>
        /// The SHA256 hash algorithm
        /// </summary>
        SHA256,

        /// <summary>
        /// The SHA384 hash algorithm
        /// </summary>
        SHA384,

        /// <summary>
        /// The SHA512 hash algorithm
        /// </summary>
        SHA512
    }

    public class HsmRsaSignature : IExternalSignature
    {
        private Pkcs11InteropHashAlgorithm _hashAlgorihtm = Pkcs11InteropHashAlgorithm.SHA256;
        public string GetHashAlgorithm()
        {
            return _hashAlgorihtm.ToString();
        }

        private String hash = "RSA";
        private readonly IByteSigner _byteSigner;
        private readonly string algHash;
        public HsmRsaSignature(IByteSigner byteSigner, String algHash)
        {
            this._byteSigner = byteSigner;
            this.algHash = algHash;
        }
#pragma warning disable CA1024 // Use properties where appropriate
        public string GetEncryptionAlgorithm()
#pragma warning restore CA1024 // Use properties where appropriate
        {
            return hash + "";
        }
        /// <summary>
        /// Computes hash of the data
        /// </summary>
        /// <param name="digest">Hash algorithm implementation</param>
        /// <param name="data">Data that should be processed</param>
        /// <returns>Hash of data</returns>
        private byte[] ComputeDigest(IDigest digest, byte[] data)
        {
            if (digest == null)
                throw new ArgumentNullException("digest");

            if (data == null)
                throw new ArgumentNullException("data");

            byte[] hash = new byte[digest.GetDigestSize()];

            digest.Reset();
            digest.BlockUpdate(data, 0, data.Length);
            digest.DoFinal(hash, 0);

            return hash;
        }
        /// <summary>
        /// Creates PKCS#1 DigestInfo
        /// </summary>
        /// <param name="hash">Hash value</param>
        /// <param name="hashOid">Hash algorithm OID</param>
        /// <returns>DER encoded PKCS#1 DigestInfo</returns>
        private byte[] CreateDigestInfo(byte[] hash, string hashOid)
        {
            DerObjectIdentifier derObjectIdentifier = new DerObjectIdentifier(hashOid);
            AlgorithmIdentifier algorithmIdentifier = new AlgorithmIdentifier(derObjectIdentifier, null);
            DigestInfo digestInfo = new DigestInfo(algorithmIdentifier, hash);
            return digestInfo.GetDerEncoded();
        }
        public byte[] Sign(byte[] message)
        {
            byte[] digest = null;
            byte[] digestInfo = null;

            switch (_hashAlgorihtm)
            {
                case Pkcs11InteropHashAlgorithm.SHA1:
                    digest = ComputeDigest(new Sha1Digest(), message);
                    digestInfo = CreateDigestInfo(digest, "1.3.14.3.2.26");
                    break;
                case Pkcs11InteropHashAlgorithm.SHA256:
                    digest = ComputeDigest(new Sha256Digest(), message);
                    digestInfo = CreateDigestInfo(digest, "2.16.840.1.101.3.4.2.1");
                    break;
                case Pkcs11InteropHashAlgorithm.SHA384:
                    digest = ComputeDigest(new Sha384Digest(), message);
                    digestInfo = CreateDigestInfo(digest, "2.16.840.1.101.3.4.2.2");
                    break;
                case Pkcs11InteropHashAlgorithm.SHA512:
                    digest = ComputeDigest(new Sha512Digest(), message);
                    digestInfo = CreateDigestInfo(digest, "2.16.840.1.101.3.4.2.3");
                    break;
            }
            return _byteSigner.Sign(digestInfo);
        }

    }


}
