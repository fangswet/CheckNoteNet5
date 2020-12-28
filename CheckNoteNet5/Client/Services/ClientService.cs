using CheckNoteNet5.Shared.Services;
using System.Net.Http;
using System.Threading.Tasks;

namespace CheckNoteNet5.Client.Services
{
    public abstract class ClientService
    {
        protected static async Task<ServiceResult<T>> Parse<T>(HttpResponseMessage response) => await ServiceResult<T>.Parse(response);
    }
}
