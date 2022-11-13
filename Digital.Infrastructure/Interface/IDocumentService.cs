using Digital.Infrastructure.Model;
using Digital.Infrastructure.Model.DocumentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digital.Infrastructure.Interface
{
    public interface IDocumentService
    {
        Task<ResultModel> CreateAsync(DocumentUploadApiRequest model);
    }
}
