using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace CheckNoteNet5.Shared.Services
{
    public class ServiceResult
    {
        public ServiceError error;
        public bool IsOk { get => error == null; }

        public HttpStatusCode statusCode;
        public HttpStatusCode StatusCode 
        { 
            get => IsOk ? statusCode : error.StatusCode; 
            set => statusCode = value; 
        }

        public ServiceResult(HttpStatusCode statusCode = HttpStatusCode.OK) => this.statusCode = statusCode;
        public ServiceResult(ServiceError error) => this.error = error;

        public static ServiceResult MakeOk(HttpStatusCode statusCode = HttpStatusCode.OK) => new ServiceResult(statusCode);
        public static ServiceResult<T> MakeOk<T>(T value = default, HttpStatusCode statusCode = HttpStatusCode.OK) => new ServiceResult<T>(value, statusCode);
        public static ServiceResult MakeError<E>() where E : ServiceError, new() => new ServiceResult(new E());
        public static ServiceResult MakeError<E>(string message) where E : ServiceError, new() => new ServiceResult(new E { Message = message });
        public static ServiceResult<T> NullCheck<T>(T value) where T : class 
            => value == null ? ServiceResult<T>.MakeError<NotFoundError>() : MakeOk(value);

        public static async Task<ServiceResult> Parse(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsByteArrayAsync();

            if (response.IsSuccessStatusCode) 
                return new ServiceResult(statusCode: response.StatusCode);

            var error = content.Length > 0 ? JsonSerializer.Deserialize<ServiceError>(content) : new ServiceError(statusCode: response.StatusCode);

            return new ServiceResult(error);
        }

        public static implicit operator ServiceResult(HttpResponseMessage response) => Parse(response).Result;
    }

    public class ServiceResult<T> : ServiceResult
    {
        private T _value;
        public bool hasValue = false;
        public T Value
        {
            get => _value;
            set { _value = value; hasValue = true; }
        }

        public ServiceResult(T value = default, HttpStatusCode statusCode = HttpStatusCode.OK) : base(statusCode) => Value = value;
        public ServiceResult(ServiceError error) : base(error) { }
        public new static ServiceResult<T> MakeError<E>() where E : ServiceError, new() => new ServiceResult<T>(new E());
        public new static ServiceResult<T> MakeError<E>(string message) where E : ServiceError, new() => new ServiceResult<T>(new E { Message = message });
        public ServiceResult<T> Ok(T value = default, HttpStatusCode statusCode = HttpStatusCode.OK) => ServiceResult.MakeOk(value, statusCode);
        public ServiceResult<T> Error<E>() where E : ServiceError, new() => new ServiceResult<T>(new E());
        public ServiceResult<T> Error<E>(string message) where E : ServiceError, new() => new ServiceResult<T>(new E { Message = message });

        public new static async Task<ServiceResult<T>> Parse(HttpResponseMessage response)
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

            var error = content.Length > 0 ? JsonSerializer.Deserialize<ServiceError>(content) : new ServiceError(statusCode: response.StatusCode);

            return new ServiceResult<T>(error);
        }

        public static implicit operator ServiceResult<T>(HttpResponseMessage response) => Parse(response).Result;
    }
}
