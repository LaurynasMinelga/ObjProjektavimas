using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net;

namespace Craft_client
{
    class Requests
    {
        public async Task<Uri> CreateAsync(Object obj, HttpClient client, string url)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync(client.BaseAddress.PathAndQuery + url, obj);
            response.EnsureSuccessStatusCode();

            // return URL of the created resource.
            return response.Headers.Location;
        }

        public async Task<ICollection<Object>> GetAllAsync(HttpClient client, string url)
        {
            ICollection<Object> objects = null;
            HttpResponseMessage response = await client.GetAsync(client.BaseAddress.PathAndQuery + url);
            if (response.IsSuccessStatusCode)
            {
                objects = await response.Content.ReadAsAsync<ICollection<Object>>();
            }
            return objects;
        }

        public async Task<Object> GetOneAsync(HttpClient client, string url, long id)
        {
            Object objects = null;
            HttpResponseMessage response = await client.GetAsync(client.BaseAddress.PathAndQuery + url + id);
            if (response.IsSuccessStatusCode)
            {
                objects = await response.Content.ReadAsAsync<Object>();
            }
            return objects;
        }

        public async Task<HttpStatusCode> UpdateAsync(Object obj, HttpClient client, string url, long id)
        {
            HttpResponseMessage response = await client.PutAsJsonAsync(client.BaseAddress.PathAndQuery + url + $"{id}", obj);
            response.EnsureSuccessStatusCode();

            return response.StatusCode;
        }

        public async Task<HttpStatusCode> DeleteAsync(HttpClient client, string url, long id)
        {
            HttpResponseMessage response = await client.DeleteAsync(client.BaseAddress.PathAndQuery + url + $"{id}");
            return response.StatusCode;
        }
    }
}
