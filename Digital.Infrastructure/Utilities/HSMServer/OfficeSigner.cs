
using Newtonsoft.Json;

namespace Digital.Infrastructure.Utilities.HSMServer
{
    public class OfficeSigner
    {
        private readonly string inputPDF = "";
        private readonly string outputPDF = "";
        private readonly string appid;
        private readonly string secret;
        private readonly string region;

        public OfficeSigner(string appid, string secret, string region = "demo")
        {
            this.appid = appid;
            this.secret = secret;
            this.region = region;
        }


        public ApiResp Verify(byte[] input)
        {
            var signClient = new SignClient(appid, secret, "sign", region);
            string base64pdf = Convert.ToBase64String(input);
            var responseString = signClient.UploadString("/api/v2/pdf/verify", "POST", base64pdf);
            var verifyResult = JsonConvert.DeserializeObject<ApiResp>(responseString);
            if (verifyResult!.status == 0)
            {
                return verifyResult;
            }

            return null!;
        }


        public byte[] Sign(byte[] input, string sHash, int typeSignature, string base64Image, string textOut, string signatureName, int pageSign = 1,
            int xPoint = 100, int yPoint = 200, int width = 100, int height = 100)
        {
            var signClient = new SignClient(appid, secret, "sign", "demo");

            string base64pdf = Convert.ToBase64String(input);


            var pdfOriginalData = new
            {
                base64image = base64Image,
                hashalg = sHash,
                textout = textOut,
                pagesign = pageSign,
                signaturename = signatureName,
                xpoint = xPoint,
                ypoint = yPoint,
                width = width,
                height = height,
                typesignature = typeSignature,
                base64pdf = base64pdf
            };
            string pdfStringJson = JsonConvert.SerializeObject(pdfOriginalData);
            var responseString = signClient.UploadString("/api/v2/pdf/sign/originaldata", "POST", pdfStringJson);
            var dataSigned = JsonConvert.DeserializeObject<ApiResp>(responseString);
            if (dataSigned!.status == 0)
            {
                var bytesout = Convert.FromBase64String(dataSigned.obj.ToString()!);
                return bytesout;
            }

            return null!;
        }



    }
}
