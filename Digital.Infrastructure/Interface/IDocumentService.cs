using Digital.Infrastructure.Model;
using Digital.Infrastructure.Model.DocumentModel;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        Task<ResultModel> DeleteDocument(Guid id);
        Task<ResultModel> UpdateDocument(DocumentUpdateModel model, Guid Id);
    }
}
