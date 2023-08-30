using ChatBotVK.Models;
using ChatBotVK.Models.Dtos;
using RestSharp;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;

namespace ChatBotVK.Services
{
    public class PhotoLoaderService
    {
        private readonly string _groupId;
        private readonly string _token;
        private readonly HttpClient _httpClient;
        private readonly string _vkApiPath = "https://api.vk.com/method/";

        public PhotoLoaderService(IConfiguration configuration, HttpClient httpClient)
        {
            _groupId = configuration["GroupId"];
            _token = configuration["AccessToken"];
            _httpClient = httpClient;
        }

        public async Task<SavePhotoResponce> UploadPhotoToVk(string path)
        {
            var url = await GetUploadServer();
            var uploadPhoto = await UploadPhoto(url, path);
            return await SavePhoto(uploadPhoto);
        }

        private async Task<SavePhotoResponce> SavePhoto(UploadPhotoResponse uploadPhotoResponse)
        {
            string content = await SendRequest("photos.saveMessagesPhoto", uploadPhotoResponse);
            var a  = JsonSerializer.Deserialize<SavePhotoResponce>(content);
            return a;
        }

        private async Task<UploadPhotoResponse> UploadPhoto(string uploadUrl, string path)
        {
            uploadUrl = uploadUrl.Replace("\\", "");
            var wc = new WebClient();
            var result = Encoding.ASCII.GetString(wc.UploadFile(uploadUrl, path));
            return JsonSerializer.Deserialize<UploadPhotoResponse>(result);
            //var client = new RestClient(uploadUrl);
            //var request = new RestRequest();
            //request.Method = Method.Post;
            //request.AddHeader("Authentication", $"Bearer {_token}");
            //request.AddHeader("Accept", "multipart/form-data");
            //request.AddHeader("Content-Type", "multipart/form-data");
            //request.AddParameter("photo", path);
            //request.AddFile("123.jpg", path);
            //var response = await client.ExecuteAsync(request);
            //return JsonSerializer.Deserialize<UploadPhotoResponse>(response.Content);

            //using var multipartFormContent = new MultipartFormDataContent();
            //var fileStreamContent = new StreamContent(File.OpenRead(path));
            //fileStreamContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpg");
            //multipartFormContent.Add(fileStreamContent);
            //var response = await _httpClient.PostAsync(uploadUrl, multipartFormContent);
            //var data = await response.Content.ReadAsStringAsync();
            //return JsonSerializer.Deserialize<UploadPhotoResponse>(data);
        }

        private async Task<string?> GetUploadServer()
        {
            string content = await SendRequest("photos.getMessagesUploadServer");
            var responce = JsonSerializer.Deserialize<UploadUrlResponse>(content);
            return responce.Response.UploadUrl;
        }

        private async Task<string> SendRequest(string method, UploadPhotoResponse? content = null)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-encoded"));
            var uri = new Uri($"{_vkApiPath}{method}");
            var request = new HttpRequestMessage(HttpMethod.Post, uri)
            {
                Content = new FormUrlEncodedContent(CreateQuery(content))
            };

            var answer = await _httpClient.SendAsync(request);
            return await answer.Content.ReadAsStringAsync();
        }

        private Dictionary<string, string> CreateQuery(UploadPhotoResponse? content = null)
        {
            var query = new Dictionary<string, string>();
            if (content != null)
            {
                query.Add("photo", $"{content.Photo}");
                query.Add("server", $"{content.Server}");
                query.Add("hash", $"{content.Hash}");
            }
            query.Add("group_id", $"{_groupId}");
            query.Add("v", "5.131");
            return query;
        }
    }
}
