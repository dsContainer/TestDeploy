using AutoMapper;
using Digital.Data.Entities;
using Digital.Infrastructure.Interface;
using Digital.Infrastructure.Model;
using Digital.Infrastructure.Model.DocumentModel;
using Microsoft.EntityFrameworkCore;

namespace Digital.Infrastructure.Service
{
    public class DocumentTypeService : IDocumentTypeService
    {
        private readonly DigitalSignatureDBContext _context;
        private readonly IMapper _mapper;
        public DocumentTypeService(
            IMapper mapper,
            DigitalSignatureDBContext context)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResultModel> CreateDocumentType(DocumentTypeCreateModel model)
        {
            var result = new ResultModel();
            var transaction = _context.Database.BeginTransaction();
            try
            {
                var documentType = _context.DocumentTypes.FirstOrDefault(x => x.NormalizationName == model.Name.ToUpper());

                if (documentType != null)
                {
                    result.Code = 400;
                    result.IsSuccess = false;
                    result.ResponseFailed = "DocumentType Existed!";
                    return result;
                }

                documentType = _mapper.Map<DocumentType>(model);
                documentType.NormalizationName = model.Name.ToUpper();

                await _context.DocumentTypes.AddAsync(documentType);
                await _context.SaveChangesAsync();

                result.IsSuccess = true;
                result.Code = 200;
                result.ResponseSuccess = _mapper.Map<DocumentTypeViewModel>(documentType);

                await transaction.CommitAsync();
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                result.IsSuccess = false;
                result.ResponseFailed = e.InnerException != null ? e.InnerException.Message + "\n" + e.StackTrace : e.Message + "\n" + e.StackTrace;
            }

            return result;
        }

        public async Task<int> DeleteDocumentType(Guid DocId)
        {
            var res = await _context.DocumentTypes.FindAsync(DocId);
            if (res == null)
                throw new Exception($"Cannot find an Doctype with id {DocId}");
            _context.DocumentTypes.Remove(res);
            return await _context.SaveChangesAsync();
        }

        public async Task<ResultModel> GetDocumentTypeById(Guid id)
        {
            var result = new ResultModel();
            try
            {
                var docTypes = _context.DocumentTypes.Where(x => !x.IsDeleted && x.Id == id);

                if (docTypes == null)
                {
                    result.Code = 400;
                    result.IsSuccess = false;
                    result.ResponseSuccess = $"Any DocumentType Not Found!";
                    return result;
                }

                result.Code = 200;
                result.IsSuccess = true;
                result.ResponseSuccess = await _mapper.ProjectTo<DocumentTypeViewModel>(docTypes).ToListAsync();

            }
            catch (Exception e)
            {
                result.IsSuccess = false;
                result.ResponseFailed = e.InnerException != null ? e.InnerException.Message + "\n" + e.StackTrace : e.Message + "\n" + e.StackTrace;
            }

            return result;
        }


        public async Task<ResultModel> GetDocumentTypes()
        {
            var result = new ResultModel();
            try
            {
                var docTypes = _context.DocumentTypes.Where(x => !x.IsDeleted);

                if (docTypes == null)
                {
                    result.Code = 400;
                    result.IsSuccess = false;
                    result.ResponseSuccess = $"Any DocumentType Not Found!";
                    return result;
                }

                result.Code = 200;
                result.IsSuccess = true;
                result.ResponseSuccess = await _mapper.ProjectTo<DocumentTypeViewModel>(docTypes).ToListAsync();

            }
            catch (Exception e)
            {
                result.IsSuccess = false;
                result.ResponseFailed = e.InnerException != null ? e.InnerException.Message + "\n" + e.StackTrace : e.Message + "\n" + e.StackTrace;
            }

            return result;
        }


        public async Task<ResultModel> UpdateDocumentType(DocumentTypeUpdateModel model, Guid Id)
        {
            var result = new ResultModel();
            var transaction = _context.Database.BeginTransaction();
            try
            {
                var docTypes = _context.DocumentTypes.Where(x => !x.IsDeleted);

                if (docTypes == null)
                {
                    result.Code = 400;
                    result.IsSuccess = false;
                    result.ResponseSuccess = $"Any DocumentType Not Found!";
                    return result;
                }

                result.Code = 200;
                result.IsSuccess = true;
                result.ResponseSuccess = await _mapper.ProjectTo<DocumentTypeViewModel>(docTypes).ToListAsync();

            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                result.IsSuccess = false;
                result.ResponseFailed = e.InnerException != null ? e.InnerException.Message + "\n" + e.StackTrace : e.Message + "\n" + e.StackTrace;
            }
            return result;
        }
    }
}
