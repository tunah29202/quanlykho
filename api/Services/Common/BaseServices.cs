using AutoMapper;
namespace Services.Common.Repository
{
    public class BaseServices
    {
        protected IMapper _mapper { get; set; }
        protected IUnitOfWork _unitOfWork { get; set; }
        public BaseServices(IUnitOfWork unitOfWork, IMapper mapper) 
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
    }
}