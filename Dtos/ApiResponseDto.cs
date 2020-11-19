using System;

namespace Youpay.API.Dtos
{
    public class ApiResponseDto<T>
    {
        public ApiResponseDto(){}
        public ApiResponseDto(int status, String message, String error, T data)
        {
            this.Status = status;
            this.Message = message;
            this.Error = error;
            this.Data = data;
            this.Timestamp = DateTime.Now;
        }
        public int Status { get; set; }
        public String Message { get; set; }
        public String Error { get; set; }
        public T Data { get; set; }
        public DateTime Timestamp { get; set; }
    }
}