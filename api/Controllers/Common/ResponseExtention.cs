using Common;

namespace Controllers.Common
{
    public static class ResponseExtention
    {
        public static BaseResponse<T> ToResponse<T>(this T data, CancellationToken cancellationToken = default(CancellationToken)) where T : class
        {
            if (data != null)
            {
                return new BaseResponse<T>(data, ResponseCode.Success, "Success");
            }
            else
            {
                return new BaseResponse<T>(data, ResponseCode.NotFound, "Data not found");
            }
        }
    }
}