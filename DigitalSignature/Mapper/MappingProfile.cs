using AutoMapper;
using DigitalSignature.Entities;
using DigitalSignature.Model.DocumentModel;
using DigitalSignature.Model.Requests;

namespace DigitalSignature.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            #region DocumentType
            CreateMap<DocumentType, DocumentTypeViewModel>();
            CreateMap<DocumentTypeCreateModel, DocumentType>();
            #endregion

            #region User
            CreateMap<UserRequest, User>();
            #endregion
        }
    }
}
