using Digital.Infrastructure.Model;
using Digital.Infrastructure.Model.DocumentModel;

namespace Digital.Infrastructure.Interface
{
    public interface IDocumentService
    {
        Task<ResultModel> CreateAsync(DocumentUploadApiRequest model);
    }
}
