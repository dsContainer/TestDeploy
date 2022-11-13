using AutoMapper;
using Digital.Data.Entities;
using Digital.Infrastructure.Model.DocumentModel;
using Digital.Infrastructure.Model.ProcessModel;
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

            #region Process
            CreateMap<Process, ProcessViewModel>();
            CreateMap<ProcessCreateModel, Process>();
            CreateMap<ProcessUpdateModel, Process>();
            #endregion

            #region ProcessStep
            CreateMap<ProcessStep, ProcessStepViewModel>();
            CreateMap<ProcessStepCreateModel, ProcessStep>();
            CreateMap<ProcessStepUpdateModel, ProcessStep>();
            #endregion
        }
    }
}
