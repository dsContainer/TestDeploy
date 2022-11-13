using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digital.Infrastructure.Model.DocumentModel
{
    public class AzureBlobStorageModel
    {
        public string? Uri { get; set; }
        public string? Name { get; set; }
        public string? ContentType { get; set; }
        public Stream? Content { get; set; }
    }

    public class BlobResponseDto
    {
        public string? Status { get; set; }
        public bool Error { get; set; }
        public AzureBlobStorageModel Blob { get; set; }

        public BlobResponseDto()
        {
            Blob = new AzureBlobStorageModel();
        }
    }
}
