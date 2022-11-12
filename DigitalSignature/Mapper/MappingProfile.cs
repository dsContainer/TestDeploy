using AutoMapper;
using DigitalSignature.Entities;
using DigitalSignature.Model;
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
