using AutoMapper;
using Digital.Data.Entities;
using Digital.Infrastructure.Interface;
using Digital.Infrastructure.Model;
using Digital.Infrastructure.Model.BatchModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digital.Infrastructure.Service
{
    public class BatchService : IBatchService
    {
        private readonly DigitalSignatureDBContext _context;
        private readonly IMapper _mapper;

        public BatchService(
            IMapper mapper,
            DigitalSignatureDBContext context)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResultModel> createBatch(CreateBatchModel model)
        {
            var result = new ResultModel();
            var transaction = _context.Database.BeginTransaction();
            try
            {
                var process = _context.Processes.FirstOrDefault(x => x.Id == model.processId);
                if (process != null) { 
                    
                }
                var batchToCreate = new Batch
                {
                    Id = Guid.NewGuid(),
                    Name = model.name,
                    DateCreated = DateTime.Now,
                    StartDate = DateTime.ParseExact(model.startDate, "dd/MM/yyyy", null),
                    EndDate = DateTime.ParseExact(model.endDate, "dd/MM/yyyy", null),
                    Description= model.description,
                    IsActive = true,
                    ProcessId = model.processId,

                };
                _context.Batches.Add(batchToCreate);
                _context.SaveChanges();
                result.IsSuccess = true;
                result.Code = 200;
                result.ResponseSuccess = batchToCreate;
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                result.IsSuccess = false;
                result.Code = 400;
                result.ResponseFailed = e.InnerException != null ? e.InnerException.Message + "\n" + e.StackTrace : e.Message + "\n" + e.StackTrace;
            }
            await transaction.RollbackAsync();
            return result;
        }
    }
}
