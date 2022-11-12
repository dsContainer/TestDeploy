using AutoMapper;
using DigitalSignature.Entities;
using DigitalSignature.Interface;
using DigitalSignature.Model;
using DigitalSignature.Utilities;
using Microsoft.EntityFrameworkCore;

namespace DigitalSignature.Service
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

        public Task<ResultModel> CreateSignatureByUserId(Guid userId)
        {
            var result = new ResultModel();
            try
            {
                var userToCreate = _context.Users.FirstOrDefault(x => x.Id == userId);
                if (userToCreate != null)
                {
                    string message = SignatureUtils.createCertificate(userToCreate.Username);
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


            throw new NotImplementedException();
        }

        public Task<ResultModel> GetListSignature()
        {
            var result = new ResultModel();

            try
            {
                var listSignature = _context.DocumentTypes.ToList();
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

            throw new NotImplementedException();
        }

        public Task<ResultModel> SearchBySignatureId(Guid sigId)
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

            throw new NotImplementedException();
        }

        public Task<ResultModel> SearchContainUserNamePhoneOrEmail(string data)
        {
            var result = new ResultModel();
            try
            {
                var SignatureIdByUserName = _context.Users.Where(x => x.Username!.Contains(data) && x.IsDeleted != false)
                                                .Select(x => x.SigId).ToList();
                /*var SignatureIdByMail = _context.Users.Where(x => x.Username!.Contains(data))
                                                .Select(x => x.SigId).ToListAsync();*/
                var SignatureIdByPhone = _context.Users.Where(x => x.Phone!.Contains(data) && x.IsDeleted != false)
                                                .Select(x => x.SigId).ToList();
                List<Guid> listSignatureId = new List<Guid>();
                foreach (var item in SignatureIdByUserName)
                {
                    listSignatureId.Add(item);
                }

                foreach (var item in SignatureIdByPhone)
                {
                    listSignatureId.Add(item);
                }


                result.IsSuccess = true;
                result.Code = 200;
                result.ResponseSuccess = listSignatureId;

            }
            catch (Exception e)
            {
                result.IsSuccess = false;
                result.Code = 400;
                result.ResponseFailed = e.InnerException != null ? e.InnerException.Message + "\n" + e.StackTrace : e.Message + "\n" + e.StackTrace;
            }

            throw new NotImplementedException();
        }

        public Task<ResultModel> SearchRangeDate(DateTime fromDate, DateTime toDate)
        {

            var result = new ResultModel();
            try
            {
                var listSignature = _context.Signatures.Where(x => x.FromDate >= fromDate)
                                                        .Where(x => x.ToDate <= toDate).ToListAsync();
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
            throw new NotImplementedException();
        }
    }
}
