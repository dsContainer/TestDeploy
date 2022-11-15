using AutoMapper;
using Digital.Data.Entities;
using Digital.Infrastructure.Interface;
using Digital.Infrastructure.Model;
using Digital.Infrastructure.Model.SignatureModel;
using Digital.Infrastructure.Utilities;
using Digital.Infrastructure.Utilities.HSMServer;
using Microsoft.EntityFrameworkCore;

namespace Digital.Infrastructure.Service
{
    public class SignatureService : ISignatureService
    {
        private readonly DigitalSignatureDBContext _context;
        private readonly IMapper _mapper;

        public SignatureService(
            IMapper mapper,
            DigitalSignatureDBContext context)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResultModel> CreateSignatureByUserId(Guid userId)
        {
            var result = new ResultModel();
            try
            {
                var userToCreate = _context.Users.FirstOrDefault(x => x.Id == userId);
                if (userToCreate != null)
                {
                    string message = SignatureUtils.createCertificate(userToCreate.Username);
                    var signature = new Signature
                    {
                        Id = Guid.NewGuid(),
                        FromDate = DateTime.Now,
                        ToDate = DateTime.Now.AddYears(1),
                        IsActive = false,
                        UserId = userId
                    };
                    await _context.Signatures.AddAsync(signature);
                    await _context.SaveChangesAsync();
                    result.IsSuccess = true;
                    result.Code = 200;
                    result.ResponseSuccess = message;
                }
            }
            catch (Exception e)
            {
                result.IsSuccess = false;
                result.Code = 400;
                result.ResponseFailed = e.InnerException != null ? e.InnerException.Message + "\n" + e.StackTrace : e.Message + "\n" + e.StackTrace;
            }
            return result;
        }

        public async Task<ResultModel> GetListSignature()
        {
            var result = new ResultModel();

            try
            {
                var listSignature = await _context.Signatures.ToListAsync();
                if (listSignature != null)
                {
                    result.IsSuccess = true;
                    result.Code = 200;
                    result.ResponseSuccess = listSignature;
                }
                else
                {
                    result.IsSuccess = true;
                    result.Code = 200;
                    result.ResponseSuccess = "Signature not found";
                }
            }
            catch (Exception e)
            {
                result.IsSuccess = false;
                result.Code = 400;
                result.ResponseFailed = e.InnerException != null ? e.InnerException.Message + "\n" + e.StackTrace : e.Message + "\n" + e.StackTrace;
            }

            return result;
        }

        public async Task<ResultModel> SearchBySignatureId(Guid sigId)
        {

            var result = new ResultModel();
            try
            {
                var signature = _context.Signatures.FirstOrDefault(x => x.Id == sigId);
                if (signature != null)
                {
                    result.IsSuccess = true;
                    result.Code = 200;
                    result.ResponseSuccess = signature;
                }
                else
                {
                    result.IsSuccess = true;
                    result.Code = 200;
                    result.ResponseSuccess = "Not Found";
                }
            }
            catch (Exception e)
            {
                result.IsSuccess = false;
                result.Code = 400;
                result.ResponseFailed = e.InnerException != null ? e.InnerException.Message + "\n" + e.StackTrace : e.Message + "\n" + e.StackTrace;
            }

            return result;
        }

        public async Task<ResultModel> SearchContainUserNamePhoneOrEmail(string data)
        {
            var result = new ResultModel();
            try
            {
                /*var SignatureIdByUserName = _context.Users.Where(x => x.Username.Contains(data) && x.IsActive == true)
                                                .Select(x => x.SigId).ToList();
                var SignatureIdByMail = _context.Users.Where(x => x.Email!.Contains(data) && x.IsActive == true)
                                                .Select(x => x.SigId).ToList();
                var SignatureIdByPhone = _context.Users.Where(x => x.Phone!.Contains(data) && x.IsActive == true)
                                                .Select(x => x.SigId).ToList();

                List<Guid> listSignatureId = new List<Guid>();
                foreach (var IdAdd in SignatureIdByUserName)
                {
                    if (listSignatureId.Count == 0)
                    {
                        listSignatureId.Add(IdAdd);
                    }
                    else
                    {
                        foreach (var item in listSignatureId)
                        {
                            if (IdAdd != item)
                            {
                                listSignatureId.Add(IdAdd);
                            }
                        }
                    }
                }
                foreach (var IdAdd in SignatureIdByMail)
                {
                    if (listSignatureId.Count == 0)
                    {
                        listSignatureId.Add(IdAdd);
                    }
                    else
                    {
                        foreach (var item in listSignatureId)
                        {
                            if (IdAdd != item)
                            {
                                listSignatureId.Add(IdAdd);
                            }
                        }
                    }
                }
                foreach (var IdAdd in SignatureIdByPhone)
                {
                    if (listSignatureId.Count == 0)
                    {
                        listSignatureId.Add(IdAdd);
                    }
                    else
                    {
                        foreach (var item in listSignatureId)
                        {
                            if (IdAdd != item)
                            {
                                listSignatureId.Add(IdAdd);
                            }
                        }
                    }
                }
                List<Signature> listSignature = new List<Signature>();
                foreach (var item in listSignatureId)
                {
                    var signature = _context.Signatures.FirstOrDefault(x => x.Id == item);
                    listSignature.Add(signature);
                }

                result.IsSuccess = true;
                result.Code = 200;
                result.ResponseSuccess = listSignature;*/

            }
            catch (Exception e)
            {
                result.IsSuccess = false;
                result.Code = 400;
                result.ResponseFailed = e.InnerException != null ? e.InnerException.Message + "\n" + e.StackTrace : e.Message + "\n" + e.StackTrace;
            }

            return result;
        }

        public async Task<ResultModel> SearchRangeDate(string fromDate, string toDate)
        {

            var result = new ResultModel();
            try
            {
                DateTime fromDateToSearch = DateTime.ParseExact(fromDate, "dd/MM/yyyy", null);
                DateTime toDateToSearch = DateTime.ParseExact(toDate, "dd/MM/yyyy", null);
                int dateCompare = fromDateToSearch.CompareTo(toDateToSearch); // >=0 => true
                if (dateCompare >= 0)
                {
                    result.IsSuccess = false;
                    result.Code = 400;
                    result.ResponseSuccess = "Date not valid";
                    return result;
                }

                var listSignature = await _context.Signatures.Where(x => x.ToDate >= fromDateToSearch && x.ToDate <= toDateToSearch)
                                                        .ToListAsync();
                if (listSignature != null)
                {
                    result.IsSuccess = true;
                    result.Code = 200;
                    result.ResponseSuccess = listSignature;
                }
                else
                {
                    result.IsSuccess = true;
                    result.Code = 200;
                    result.ResponseSuccess = "not found";
                }
            }
            catch (Exception e)
            {
                result.IsSuccess = false;
                result.Code = 400;
                result.ResponseFailed = e.InnerException != null ? e.InnerException.Message + "\n" + e.StackTrace : e.Message + "\n" + e.StackTrace;
            }
            return result;
        }
        public async Task<ResultModel> SignPDF(SignModel signModel)
        {
            var result = new ResultModel();
            try
            {
                /*var newProcess = new Process
                {
                    Id = Guid.NewGuid(),
                    Name = "MelloTest1",
                    Status = "Pending",
                    CompanyLevel = "Department",
                };
                var check1 = await _context.Processes.AddAsync(newProcess);
                if (check1 != null)
                {
                    var newProcessStep = new ProcessStep
                    {
                        Id = Guid.NewGuid(),
                        OrderIndex = 1,
                        UserId = Guid.Parse(""),
                        Xpoint = 60,
                        Ypoint = 60,
                        Width = 60,
                        Height = 60,
                        PageSign = 1,
                        DateSign = DateTime.Now,
                        Message = null,
                        ProcessId = newProcess.Id,
                        XpointPercent = 0,
                        YpointPercent = 0,
                    };
                    await _context.ProcessSteps.AddAsync(newProcessStep);
                }
                else
                {
                    return result;
                }
                await _context.SaveChangesAsync();*/
                var signer = new PDFSigner("4288161d272f4622b23e52697346ad8d", "eHi3x+c4nQZYuVQRZV3d1WcB46aCzMQhNR4KcSpY", "hn1");

                var PdfBytes = Convert.FromBase64String(SignatureUtils.convertBase64FromFile("E:\\C#\\Resources\\Signing\\samplePdf.pdf"));
                var PdfImg = SignatureUtils.convertBase64FromImg("E:\\C#\\Resources\\Signing\\sampleImg.jpg");

                var docSigned = signer.Sign(
                        PdfBytes,
                        signModel.Hashalg.ToString(),
                        (int)signModel.Typesignature,
                        PdfImg,
                        "TextOutLeVinh",
                        "SignatureNameLeVinh",
                        1,
                        60,
                        60,
                        60,
                        60);

                if (docSigned == null)
                {
                    result.IsSuccess = false;
                    result.Code = 400;
                    result.ResponseFailed = "Sign PDF Failed!";
                    return result;
                }

                result.IsSuccess = true;
                result.Code = 200;
                result.ResponseSuccess = Convert.ToBase64String(docSigned);

            }
            catch (Exception e)
            {
                result.IsSuccess = false;
                result.ResponseFailed = e.InnerException != null ? e.InnerException.Message + "\n" + e.StackTrace : e.Message + "\n" + e.StackTrace;
            }
            return result;
        }
    }
}
