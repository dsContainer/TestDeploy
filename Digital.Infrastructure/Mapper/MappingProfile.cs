using AutoMapper;
using Digital.Data.Entities;
using Digital.Infrastructure.Model.DocumentModel;
using Digital.Infrastructure.Model.Requests;

namespace Digital.Infrastructure.Mapper
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
