namespace Restaurant_BE.Dto
{
    public class BaseResponse
    {
        public string Status { get; set; } = "success";
        public string Message { get; set; }
        public object Data { get; set; }

        public BaseResponse(string status, string message, object data = null)
        {
            Status = status;
            Message = message;
            Data = data;
        }
    }
}
