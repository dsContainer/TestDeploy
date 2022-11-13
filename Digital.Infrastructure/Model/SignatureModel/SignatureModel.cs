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
}
