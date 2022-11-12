using DigitalSignature.Model;
using DigitalSignature.Model.DocumentModel;

namespace DigitalSignature.Interface
{
    public interface IDocumentTypeService
    {
        Task<ResultModel> GetDocumentTypes();
        Task<ResultModel> GetDocumentTypeById(Guid id);
        Task<ResultModel> CreateDocumentType(DocumentTypeCreateModel model);
        Task<int> DeleteDocumentType(Guid id);
        Task<ResultModel> UpdateDocumentType(DocumentTypeUpdateModel model, Guid Id);
    }
}
