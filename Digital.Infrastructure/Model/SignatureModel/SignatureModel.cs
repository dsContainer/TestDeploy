using Digital.Data.Entities;
using Digital.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace Digital.Infrastructure.Model.SignatureModel
{
    public class SignModel
    {
        public Guid DocumentId { get; set; }
        [Required]
        public string Base64image { get; set; }
        [Required]
        public HashAlgorithm Hashalg { get; set; } = HashAlgorithm.SHA256;
        [Required]
        public SignType Typesignature { get; set; } = SignType.TEXTIMA;
    }
    public class SignatureViewModel
    {
        public Guid Id { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public bool IsDelete { get; set; }
    }
    public class SignatureDetailModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string AccessKey { get; set; }
        public string SecretKey { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public bool IsActive { get; set; }
    }

    public class SignatureSearchModel
    {
        public Guid UserId { get; set;}
        public List<SignatureDetailModel> SignatureDetailModel { get; set; }
    }
}
