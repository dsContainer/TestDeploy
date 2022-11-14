using AutoMapper;
using Digital.Data.Entities;
using Digital.Infrastructure.Interface;
using Digital.Infrastructure.Model.ProcessModel;
using Digital.Infrastructure.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using static iTextSharp.text.pdf.AcroFields;

namespace Digital.Infrastructure.Service
{
    public class ProcessService : IProcessService
    {
        private readonly DigitalSignatureDBContext _context;
        private readonly IMapper _mapper;
        public ProcessService(
            IMapper mapper,
            DigitalSignatureDBContext context)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<ResultModel> CreateProcess(ProcessCreateModel model)
        {
            var result = new ResultModel();
            var transaction = _context.Database.BeginTransaction();
            try
            {
                var process = _mapper.Map<Process>(model);
                process.Id = Guid.NewGuid();
                process.DateCreated = DateTime.Now;
                process.DateUpdated = DateTime.Now;
                if (process.TemplateId != null)
                {
                    var template = await _context.Templates.FindAsync(process.TemplateId);
                    if(template != null)
                    {
                        process.Template = template;
                    }
                }
                var list = process.ProcessSteps;
                if(list!= null)
                {
                    foreach (var item in list)
                    {
                        item.Id = Guid.NewGuid();
                        item.DateCreated = DateTime.Now;
                        item.DateUpdated = DateTime.Now;
                        var user = await _context.Users.FindAsync(item.UserId);
                        if(user!=null)
                        {
                            item.User = user;
                        }
                        else
                        {
                            result.IsSuccess = false;
                            result.Code = 404;
                            result.ResponseFailed = "Could not find this user";
                        }
                    }
                }
                await _context.Processes.AddAsync(process);
                await _context.SaveChangesAsync();

                result.IsSuccess = true;
                result.Code = 200;
                result.ResponseSuccess = process;

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

        public async Task<int> DeleteProcess(Guid id, bool isDeleted)
        {
            var res = await _context.Processes.FindAsync(id);
            if (res == null)
                throw new Exception($"Cannot find a process with id {id}");
            var processSteps = await _context.ProcessSteps.Where(x => x.ProcessId == id).ToListAsync();
            foreach (var step in processSteps)
            {
                step.IsDeleted = isDeleted;
                step.DateUpdated = DateTime.Now;
            }
            res.IsDeleted = isDeleted;
            res.DateUpdated = DateTime.Now;
            _context.ProcessSteps.UpdateRange(processSteps);
            _context.Processes.Update(res);
            return await _context.SaveChangesAsync();
        }

        public async Task<ResultModel> GetProcessById(Guid id)
        {
            var result = new ResultModel();
            try
            {
                var processes = await _context.Processes.
                    Include(e => e.ProcessSteps).
                    Include(e => e.Documents).
                    Include(e => e.BatchProcesses).
                    Include(e => e.ProcessData).
                    Where(x => x.Id == id).
                    FirstOrDefaultAsync();

                if (processes == null)
                {
                    result.Code = 400;
                    result.IsSuccess = false;
                    result.ResponseSuccess = $"Any Processes Not Found!";
                    return result;
                }

                result.Code = 200;
                result.IsSuccess = true;
                var process = _mapper.Map<ProcessViewModel>(processes);
                DocumentType? docType;
                if (process.TemplateId == null)
                {
                    docType = await _context.DocumentTypes.Where(x => x.Name == "Contract").FirstOrDefaultAsync();
                }
                else
                {
                    docType = await _context.DocumentTypes.FindAsync(processes.Template!.DocumentTypeId);
                }
                if (docType != null)
                {
                    process.DocumentType = docType;
                }
                result.ResponseSuccess = process;

            }
            catch (Exception e)
            {
                result.IsSuccess = false;
                result.ResponseFailed = e.InnerException != null ? e.InnerException.Message + "\n" + e.StackTrace : e.Message + "\n" + e.StackTrace;
            }

            return result;
        }

        public async Task<ResultModel> GetProcesses(ProcessSearchModel searchModel)
        {
            var result = new ResultModel();
            try
            {
                var processes = await _context.Processes.
                    Include(e => e.ProcessSteps).
                    Include(e => e.Documents).
                    Include(e => e.BatchProcesses).
                    Include(e => e.ProcessData).
                    ToListAsync();

                if (!processes.Any())
                {
                    result.Code = 404;
                    result.IsSuccess = false;
                    result.ResponseSuccess = $"Any Processes Not Found!";
                    return result;
                }

                if (searchModel.CreatedDate != null)
                {
                    DateTime CreatedDateToSearch = DateTime.
                        ParseExact(searchModel.CreatedDate, "dd/MM/yyyy", null);
                    processes = await _context.Processes.
                        Include(e => e.ProcessSteps).
                        Include(e => e.Documents).
                        Include(e => e.BatchProcesses).
                        Include(e => e.ProcessData).
                        Where(x => x.DateCreated.Date == CreatedDateToSearch.Date).
                        ToListAsync();
                    if (!processes.Any())
                    {
                        result.Code = 404;
                        result.IsSuccess = false;
                        result.ResponseSuccess = $"Any Processes Not Found!";
                        return result;
                    }
                }

                result.Code = 200;
                result.IsSuccess = true;
                var p = new List<ProcessViewModel>();
                foreach (var item in processes)
                {
                    var process = _mapper.Map<ProcessViewModel>(item);
                    DocumentType? docType;
                    if(process.TemplateId == null)
                    {
                        docType = await _context.DocumentTypes.Where(x => x.Name == "Contract").FirstOrDefaultAsync();
                    }
                    else
                    {
                        docType = await _context.DocumentTypes.FindAsync(item.Template!.DocumentTypeId);
                    }
                    if (docType != null)
                    {
                        process.DocumentType = docType;
                    }
                    p.Add(process);
                }
                result.ResponseSuccess = p;

            }
            catch (Exception e)
            {
                result.Code = 400;
                result.IsSuccess = false;
                result.ResponseFailed = e.InnerException != null ? e.InnerException.Message + "\n" + e.StackTrace : e.Message + "\n" + e.StackTrace;
            }

            return result;
        }

        public async Task<ResultModel> UpdateProcess(ProcessUpdateModel model)
        {
            var result = new ResultModel();
            var transaction = _context.Database.BeginTransaction();
            try
            {
                var process = await _context.Processes.FindAsync(model.Id);
                if (process == null)
                {
                    result.Code = 200;
                    result.IsSuccess = true;
                    result.ResponseSuccess = new ProcessUpdateModel();
                    return result;
                }

                process.Name = model.Name;
                process.CompanyLevel = model.CompanyLevel;
                process.Status = model.Status;
                process.DateUpdated = DateTime.Now;
                var list = model.ProcessSteps;
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        var processStep = await _context.ProcessSteps.FindAsync(item.Id);
                        if (processStep != null)
                        {
                            processStep.OrderIndex = item.OrderIndex;
                            processStep.UserId = item.UserId;
                            processStep.Xpoint = item.Xpoint;
                            processStep.Ypoint = item.Ypoint;
                            processStep.XpointPercent = item.XpointPercent;
                            processStep.YpointPercent = item.YpointPercent;
                            processStep.Width = item.Width;
                            processStep.Height = item.Height;
                            processStep.PageSign = item.PageSign;
                            _context.ProcessSteps.Update(processStep);
                        }
                    }

                }
                _context.Processes.Update(process);
                await _context.SaveChangesAsync();
                result.IsSuccess = true;
                result.Code = 200;
                result.IsSuccess = true;
                await transaction.CommitAsync();
                result.ResponseSuccess = process;
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
