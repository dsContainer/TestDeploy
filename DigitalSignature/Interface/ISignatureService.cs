using DigitalSignature.Model;

namespace DigitalSignature.Interface
{
    public interface ISignatureService
    {
        Task<ResultModel> GetListSignature();
        Task<ResultModel> CreateSignatureByUserId(Guid userId);
        Task<ResultModel> SearchContainUserNamePhoneOrEmail(string data);
        Task<ResultModel> SearchBySignatureId(Guid sigId);
        Task<ResultModel> SearchRangeDate(DateTime fromDate, DateTime toDate);
    }
}
