using Digital.Infrastructure.Model;
using Digital.Infrastructure.Model.DocumentModel;

namespace Digital.Infrastructure.Interface
{
    public interface IDocumentService
    {
        /// <summary>
        /// This method uploads a file submitted with the request
        /// </summary>
        /// <param name="file">File for upload</param>
        /// <returns>Blob with status</returns>
        Task<ResultModel> CreateAsync(DocumentUploadApiRequest model);

        Task<ResultModel> GetDocAsync();
        Task<ResultModel> GetDocumentDetail(Guid id);
        Task<ResultModel> DeleteDocument(Guid id);
        Task<ResultModel> UpdateDocument(DocumentUpdateModel model, Guid Id);
        Task<DocumentResponse> GetContent(Guid id);
    }
}
