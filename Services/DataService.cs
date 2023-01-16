using Azure.Core;
using BlazorTWProject.Data;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace BlazorTWProject.Services
{
    public class DataService
    {
        private readonly HttpClient _httpClient;
        public DataService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<List<Message>> GetDataAsync()
        {
            var response = await _httpClient.GetAsync("https://rest-apiproiect2utcn20221212231729.azurewebsites.net/api/Messages");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<List<Message>>(content);

            return data;
        }
        public async Task<List<Message>> GetDataAsync(String? extras)
        {
            var response = await _httpClient.GetAsync("https://rest-apiproiect2utcn20221212231729.azurewebsites.net/api/Messages/"+extras);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<List<Message>>(content);

            return data;
        }

        public async Task<T> GetAsync<T>(string url)
        {
            var response = await _httpClient.GetAsync(url);
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(json);
        }

        public async Task<List<String>> GetPatients()
        {

            _httpClient.BaseAddress = new Uri("https://rest-apiproiect2utcn20221212231729.azurewebsites.net/api/AppUsers");
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            var response = _httpClient.GetAsync("").Result;
            if (!response.IsSuccessStatusCode) return null;
            var json = await response.Content.ReadAsStringAsync();
            var deserialized = JsonConvert.DeserializeObject<List<AppUser>>(json);
            List<String> patients = new();
            deserialized.ForEach(user => patients.Add(user.Username));
            return patients;
        }
    }
}
