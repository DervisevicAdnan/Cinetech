using Newtonsoft.Json;
using CineTech.Models;

namespace CineTech.Services
{
    public class TmdbService
    {
        private readonly string _apiKey;
        private readonly HttpClient _httpClient;

        public TmdbService(string apiKey)
        {
            _apiKey = apiKey;
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://api.themoviedb.org/3/")
            };
        }

        public async Task<MovieResponseTMDb> GetPopularMoviesAsync()
        {
            var response = await _httpClient.GetStringAsync($"movie/popular?api_key={_apiKey}");
            return JsonConvert.DeserializeObject<MovieResponseTMDb>(response);
        }

        public async Task<MovieResponseTMDb> GetTopRatedMoviesAsync()
        {
            var response = await _httpClient.GetStringAsync($"movie/top_rated?api_key={_apiKey}");
            return JsonConvert.DeserializeObject<MovieResponseTMDb>(response);
        }
    }
}
