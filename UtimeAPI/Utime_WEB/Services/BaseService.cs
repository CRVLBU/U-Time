using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System.Text;
using static Utime_utility.SD;
using Utime_utility;

using System.Net;

using System.Net.Http.Headers;
using Utime_WEB.Models;
using Utime_WEB.Services.IServices;
namespace Utime_WEB.Services
{
    public class BaseService : IBaseService
    {
        public IHttpClientFactory _HttpClient { get; set; }
        public BaseService(IHttpClientFactory Httpclient)
        {
            _HttpClient = Httpclient;

        }
        public async Task<T> SendAsync<T>(Request request)
        {
            T res;
            try
            {
                HttpClient client = _HttpClient.CreateClient("Utime");
                HttpRequestMessage message = new HttpRequestMessage();
                message.Headers.Add("Accept", "application/json");
                message.RequestUri = new Uri(request.url);
                message.Content = new StringContent(JsonConvert.SerializeObject(request.Contents), Encoding.UTF8, "application/json");
                switch (request.apiType)
                {
                    case ApiType.GET:
                        message.Method = HttpMethod.Get;
                        break;
                    case ApiType.POST:
                        message.Method = HttpMethod.Post;
                        break;
                    case ApiType.PUT:
                        message.Method = HttpMethod.Put;
                        break;
                    case ApiType.DELETE:
                        message.Method = HttpMethod.Delete;
                        break;
                }
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", request.Token);
                var output = await client.SendAsync(message);
                var response = await output.Content.ReadAsStringAsync();



                try
                {
                    Response api_response = JsonConvert.DeserializeObject<Response>(response);


                    if (api_response != null && (api_response.StatusCode == HttpStatusCode.BadRequest || api_response.StatusCode == HttpStatusCode.NotFound))
                    {
                        Response outcome = new();
                        outcome.StatusCode = api_response.StatusCode;
                        outcome.ErrorMessages = api_response.ErrorMessages;
                        outcome.IsSuccess = false;
                        var result = JsonConvert.SerializeObject(outcome);
                        res = JsonConvert.DeserializeObject<T>(result);
                        return res;
                    }
                    res = JsonConvert.DeserializeObject<T>(response);
                    return res;

                }
                catch (Exception ex)
                {

                    res = JsonConvert.DeserializeObject<T>(response);
                    return res;
                }

            }
            catch (Exception ex)
            {
                Response resp = new();
                resp.IsSuccess = false;
                resp.StatusCode = HttpStatusCode.BadRequest;
                resp.ErrorMessages = new List<string> { ex.ToString() };
                var result = JsonConvert.SerializeObject(resp);
                res = JsonConvert.DeserializeObject<T>(result);
                return res;
            }
        }
    }
}
