using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FrameworkBaseService.Interfaces
{
    public interface IRestService<T>
    {

        Task<IEnumerable<T>> GetAsync(Uri url);
        Task<T> GetAsync(Uri url, int id);

        Task<T> PostAsync(Uri url, T model);

        Task<T> PutAsync(Uri url, T model, int id);

        Task<T> DeleteAsync(Uri url, int id);

        Uri CreateRequestUri(string relativePath, string queryString);

        HttpContent CreateHttpContent(T content);

        JsonSerializerSettings MicrosoftDateFormatSettings { get; }

        void addCustomHeaders();
    }
}
