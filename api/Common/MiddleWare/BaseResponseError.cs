using Helpers;

namespace Common
{
    public class BaseResponseError
    {
        public ResponseCode code { get; set; }

        public string message { get; set; }

        public BaseResponseError(ResponseCode _code, string _message)
        {
            code = _code;
            message = _message;
        }

    }
}
