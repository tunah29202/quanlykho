using AutoMapper;
using Common;
using Database.Entities;
using Services.Common.Enums;
using Services.Common.Repository;
using Services.Core.Interfaces;
namespace Services.Core.Services
{
    public class LogServices : BaseServices, ILogServices
    {
        private readonly IRepository<LogAction> logActionRepository;
        private readonly IRepository<LogException> logExceptionRepository;
        private readonly ICurrentUserService serviceContext;
        private readonly ILocalizeServices _ls;
        public LogServices(IUnitOfWork unitOfWork,
                            IMapper mapper,
                            ICurrentUserService serviceContext,
                            ILocalizeServices ls) : base(unitOfWork, mapper)
        {
            logActionRepository = _unitOfWork.GetRepository<LogAction>();
            logExceptionRepository = _unitOfWork.GetRepository<LogException>();
            this.serviceContext = serviceContext;
            _ls = ls;
        }
        public async Task WriteLogAction(string path, string method, string? body = null)
        {
            using (var trans = await _unitOfWork.BeginTransactionAsync())
            {
                try
                {
                    var methodString = MethodConvert(method);
                    if (string.IsNullOrWhiteSpace(methodString))
                        return;
                    var bodys = new List<string>()
                    {
                        _ls.Get(Modules.Core, LogActionFeature.LogAction, path), method, methodString,
                    };
                    // Ex: Tài khoản user_a truy cập màn hình Thông tin sản phẩm
                    var description = $"Tài khoản {serviceContext.user_name} {methodString} màn hình {_ls.Get(Modules.Core, LogActionFeature.LogAction, path)}";
                    if (!string.IsNullOrWhiteSpace(body))
                    {
                        bodys.Add(body);
                        // Ex: Tài khoản user_a (thêm mới | chỉnh sửa | xóa) CODE1234 ở màn hình Thông tin sản phẩm
                        description = $"Tài khoản {serviceContext.user_name} {methodString} {body} ở màn hình {_ls.Get(Modules.Core, LogActionFeature.LogAction, path)}";
                    }
                    var logAction = new LogAction
                    {
                        id = Guid.NewGuid(),
                        user_id = serviceContext.user_id,
                        user_name = serviceContext.user_name,
                        method = method,
                        body = string.Join('|', bodys),
                        description = description,
                    };
                    await logActionRepository.AddAsync(logAction);
                    var count = await _unitOfWork.SaveChangeAsync();
                    await trans.CommitAsync();
                    return;
                }
                catch (Exception)
                {
                    trans.Rollback();
                    throw;
                }
            }
        }
        public async Task WriteLogException(string function, string message, string stackTrace)
        {
            await logExceptionRepository.AddAsync(new LogException
            {
                function = function,
                message = message,
                stack_trace = stackTrace,
                created_at = DateTime.UtcNow
            });

            await _unitOfWork.SaveChangeAsync();
        }

        private string MethodConvert(string method)
        {
            var methodString = string.Empty;
            switch (method)
            {
                case LogActionMethod.Read:
                    methodString = StrLogActionMethod.Read;
                    break;
                case LogActionMethod.Create:
                    methodString = StrLogActionMethod.Create;
                    break;
                case LogActionMethod.Update:
                    methodString = StrLogActionMethod.Update;
                    break;
                case LogActionMethod.Delete:
                    methodString = StrLogActionMethod.Delete;
                    break;
                default:
                    return methodString;
            }
            return methodString;
        }
    }
}
