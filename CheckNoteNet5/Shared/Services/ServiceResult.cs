using CheckNoteNet5.Shared.Models;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace CheckNoteNet5.Shared.Services
{
    public class ServiceResult
    {
        public Error error;
        private HttpStatusCode statusCode;
        public bool IsOk { get => error == null; }
        public HttpStatusCode StatusCode 
        { 
            get => IsOk ? statusCode : error.StatusCode; 
            set => statusCode = value; 
        }

        public ServiceResult(HttpStatusCode statusCode = HttpStatusCode.OK) => this.statusCode = statusCode;
        public ServiceResult(Error error) => this.error = error;

        public static ServiceResult MakeOk(HttpStatusCode statusCode = HttpStatusCode.OK) => new ServiceResult(statusCode);
        public static ServiceResult<T> MakeOk<T>(T value = default, HttpStatusCode statusCode = HttpStatusCode.OK) => new ServiceResult<T>(value, statusCode);
        public static ServiceResult MakeError<E>() where E : Error, new() => new ServiceResult(new E());
        public static ServiceResult MakeError<E>(string message) where E : Error, new() => new ServiceResult(new E { Message = message });
        public static ServiceResult<T> NullCheck<T>(T value) where T : class 
            => value == null ? ServiceResult<T>.MakeError<NotFoundError>() : MakeOk(value);
    }

    public class ServiceResult<T> : ServiceResult
    {
        public T Value { get; set; }
        public dynamic GetValue() => IsOk ? Value : error;

        public ServiceResult(T value = default, HttpStatusCode statusCode = HttpStatusCode.OK) : base(statusCode) => Value = value;
        public ServiceResult(Error error) : base(error) { }
        public new static ServiceResult<T> MakeError<E>() where E : Error, new() => new ServiceResult<T>(new E());
        public new static ServiceResult<T> MakeError<E>(string message) where E : Error, new() => new ServiceResult<T>(new E { Message = message });
        public ServiceResult<T> Ok(T value = default, HttpStatusCode statusCode = HttpStatusCode.OK) => ServiceResult.MakeOk(value, statusCode);
        public ServiceResult<T> Error<E>() where E : Error, new() => new ServiceResult<T>(new E());
        public ServiceResult<T> Error<E>(string message) where E : Error, new() => new ServiceResult<T>(new E { Message = message });

        public static async Task<ServiceResult<T>> Parse(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsByteArrayAsync();

            if (response.IsSuccessStatusCode)
            {
                if (content.Length > 0)
                {
                    var result = await response.Content.ReadFromJsonAsync<T>();
                    return MakeOk(result);
                }

                return new ServiceResult<T>(statusCode: response.StatusCode);
            }

            var error = content.Length > 0 ? JsonSerializer.Deserialize<Error>(content) : new Error(statusCode: response.StatusCode);

            return new ServiceResult<T>(error);
        }
    }
}
