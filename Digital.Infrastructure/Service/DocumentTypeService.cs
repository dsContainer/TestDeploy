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
                documentType.Id = Guid.NewGuid();
                documentType.NormalizationName = model.Name.ToUpper();
                documentType.IsActive = true;

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

        public async Task<ResultModel> DeleteDocumentType(Guid id)
        {
            var result = new ResultModel();
            var transaction = _context.Database.BeginTransaction();
            try
            {
                var docType = await _context.DocumentTypes.FirstOrDefaultAsync(x => x.IsActive && x.Id == id);

                if (docType == null)
                {
                    result.Code = 400;
                    result.IsSuccess = false;
                    result.ResponseFailed = $"Doctype with id: {id} not existed!!";
                    return result;
                }

                _context.DocumentTypes.Remove(docType);
                await _context.SaveChangesAsync();

                result.Code = 200;
                result.ResponseSuccess = "Okie Br";
                result.IsSuccess = true;
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                result.IsSuccess = false;
                result.ResponseFailed = e.InnerException != null ? e.InnerException.Message + "\n" + e.StackTrace : e.Message + "\n" + e.StackTrace;
            }

            await transaction.CommitAsync();
            return result;
        }

        public async Task<ResultModel> GetDocumentTypeById(Guid id)
        {
            var result = new ResultModel();
            try
            {
                var docTypes = _context.DocumentTypes.Where(x => !x.IsActive && x.Id == id);

                if (docTypes == null)
                {
                    result.Code = 400;
                    result.IsSuccess = false;
                    result.ResponseSuccess = $"Any DocumentType Not Found!";
                    return result;
                }

                result.Code = 200;
                result.IsSuccess = true;
                result.ResponseSuccess = await _mapper.ProjectTo<DocumentTypeViewModel>(docTypes).FirstOrDefaultAsync();

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
                var docTypes = _context.DocumentTypes.Where(x => !x.IsActive);

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


        public async Task<ResultModel> UpdateDocumentType(Guid Id, DocumentTypeUpdateModel model)
        {
            var result = new ResultModel();
            var transaction = _context.Database.BeginTransaction();
            try
            {
                var docType = await _context.DocumentTypes.FindAsync(Id);
                if (docType == null)
                {
                    result.Code = 200;
                    result.IsSuccess = true;
                    result.ResponseSuccess = new DocumentTypeUpdateModel();
                    return result;
                }

                docType.Name = model.Name;
                docType.NormalizationName = model.Name.ToUpper();
                docType.IsActive = model.IsActive;
                _context.DocumentTypes.Update(docType);
                await _context.SaveChangesAsync();
                result.IsSuccess = true;
                result.Code = 200;
                result.IsSuccess = true;
                await transaction.CommitAsync();
                result.ResponseSuccess = _mapper.Map<DocumentTypeViewModel>(docType);
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                result.IsSuccess = false;
                result.ResponseFailed = e.InnerException != null ? e.InnerException.Message + "\n" + e.StackTrace : e.Message + "\n" + e.StackTrace;
            }
            return result;
        }

        public DocumentType DeletedDocument(Guid id, bool IsActive)
        {
            var documentType = _context.DocumentTypes.Find(id);

            if (documentType != null)
            {
                documentType.DateUpdated = DateTime.Now;
                documentType.IsActive = IsActive;

                _context.DocumentTypes.Update(documentType);

                _context.SaveChanges();
            }

            return documentType;
        }
    }
}
