using Digital.Data.Entities;
using Digital.Infrastructure.Model;
using Digital.Infrastructure.Model.DocumentModel;

namespace Digital.Infrastructure.Interface
{
    public interface IDocumentTypeService
    {
        Task<ResultModel> GetDocumentTypes();
        Task<ResultModel> GetDocumentTypeById(Guid id);
        Task<ResultModel> CreateDocumentType(DocumentTypeCreateModel model);
        Task<ResultModel> DeleteDocumentType(Guid id);
        Task<ResultModel> UpdateDocumentType(DocumentTypeUpdateModel model, Guid Id);
        DocumentType DeletedDocument(Guid id, bool isDeleted);
    }
}
