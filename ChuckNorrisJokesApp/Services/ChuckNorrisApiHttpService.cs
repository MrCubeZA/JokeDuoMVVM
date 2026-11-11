using ChuckNorrisJokesApp.Models;
using ChuckNorrisJokesApp.Services.Interfaces;

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace ChuckNorrisJokesApp.Services
{
    public class ChuckNorrisApiHttpService : IChuckNorrisApiService
    {
        private HttpClient _httpClient;
        private IConfigurationService _configurationService;
        public ChuckNorrisApiHttpService(IConfigurationService service)
        {
            _configurationService = service;

            _httpClient = new HttpClient();
        }

        public async Task<string> GetJokeAsync()
        {
            HttpResponseMessage response;

            try
            {
                response = await _httpClient.GetAsync(_configurationService.ChuckNorrisApiUrl);
            }
            catch (Exception)
            {
                throw new Exception("Could not reach the Chuck Norris joke service. Check your network connection.");
            }

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("The Chuck Norris joke request failed (HTTP " + (int)response.StatusCode + ").");
            }

            string jsonContent = await response.Content.ReadAsStringAsync();
            ChuckNorrisJoke joke;

            try
            {
                joke = JsonSerializer.Deserialize<ChuckNorrisJoke>(jsonContent);
            }
            catch (JsonException)
            {
                throw new Exception("The Chuck Norris joke response was not valid JSON.");
            }

            if (joke == null || string.IsNullOrWhiteSpace(joke.value))
            {
                throw new Exception("The Chuck Norris joke response did not include a joke.");
            }

            return joke.value;
        }
    }
}
