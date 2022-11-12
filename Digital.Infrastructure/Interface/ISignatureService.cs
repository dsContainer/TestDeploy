using Digital.Infrastructure.Model;

namespace Digital.Infrastructure.Interface
{
    public interface ISignatureService
    {
        Task<ResultModel> GetListSignature();
        Task<ResultModel> CreateSignatureByUserId(Guid userId);
        Task<ResultModel> SearchContainUserNamePhoneOrEmail(string data);
        Task<ResultModel> SearchBySignatureId(Guid sigId);
        Task<ResultModel> SearchRangeDate(string fromDate, string toDate);
    }
}
