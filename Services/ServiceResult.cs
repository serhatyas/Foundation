using System.Net;
using System.Text.Json.Serialization;

namespace App.Services
{
    public class ServiceResult<T>
    {
        //data boşsa hata, hata boşsa data dönecek o yüzden nullable 
        public T? Data { get; set; }

        public List<string>? ErrorMessage { get; set; }

        //sadece get olduğu için lambdayla yazıldı
        [JsonIgnore]
        public bool IsSuccess =>  ErrorMessage == null || ErrorMessage.Count == 0;

        [JsonIgnore]
        public bool IsFail => !IsSuccess;

        [JsonIgnore]
        public HttpStatusCode StatusCode { get; set; }

        //static factory method
        public static ServiceResult<T> Success(T data, HttpStatusCode statusCode=HttpStatusCode.OK)
        {
            return new ServiceResult<T>()
            {
                Data = data,
                StatusCode = statusCode
            };
        }

        public static ServiceResult<T> Fail(List<String> errorMessage, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        {
            return new ServiceResult<T>()
            {
                ErrorMessage = errorMessage,
                StatusCode = statusCode
            };
        }

        public static ServiceResult<T> Fail(string errorMessage, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        {
            return new ServiceResult<T>()
            {
                //ErrorMessage = new List<string>() { errorMessage }
                //.net 8 ile gelen yeni özellik
                ErrorMessage = [errorMessage ],
                StatusCode = statusCode
            };
        }
    }

    public class ServiceResult
    {
        public List<string>? ErrorMessage { get; set; }

        //sadece get olduğu için lambdayla yazıldı

        [JsonIgnore]
        public bool IsSuccess => ErrorMessage == null || ErrorMessage.Count == 0;
        [JsonIgnore]
        public bool IsFail => !IsSuccess;
        [JsonIgnore]
        public HttpStatusCode StatusCode { get; set; }

        //static factory method
        public static ServiceResult Success(HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            return new ServiceResult()
            {
                StatusCode = statusCode
            };
        }

        public static ServiceResult Fail(List<String> errorMessage, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        {
            return new ServiceResult()
            {
                ErrorMessage = errorMessage,
                StatusCode = statusCode
            };
        }

        public static ServiceResult Fail(string errorMessage, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        {
            return new ServiceResult()
            {
                //ErrorMessage = new List<string>() { errorMessage }
                //.net 8 ile gelen yeni özellik
                ErrorMessage = [errorMessage],
                StatusCode = statusCode
            };
        }
    }
}
