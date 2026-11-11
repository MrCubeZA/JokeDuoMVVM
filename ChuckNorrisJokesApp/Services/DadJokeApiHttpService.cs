using ChuckNorrisJokesApp.Models;
using ChuckNorrisJokesApp.Services.Interfaces;

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace ChuckNorrisJokesApp.Services
{
    public class DadJokeApiHttpService : IDadJokeApiService
    {
        private HttpClient _httpClient;
        private IConfigurationService _configurationService;

        public DadJokeApiHttpService(IConfigurationService service)
        {
            _configurationService = service;

            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        }

        public async Task<string> GetJokeAsync()
        {
            HttpResponseMessage response;

            try
            {
                response = await _httpClient.GetAsync(_configurationService.DadJokeApiUrl);
            }
            catch (Exception)
            {
                throw new Exception("Could not reach the dad joke service. Check your network connection.");
            }

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("The dad joke request failed (HTTP " + (int)response.StatusCode + ").");
            }

            string jsonContent = await response.Content.ReadAsStringAsync();
            DadJoke joke;

            try
            {
                joke = JsonSerializer.Deserialize<DadJoke>(jsonContent);
            }
            catch (JsonException)
            {
                throw new Exception("The dad joke response was not valid JSON.");
            }

            if (joke == null || string.IsNullOrWhiteSpace(joke.joke))
            {
                throw new Exception("The dad joke response did not include a joke.");
            }

            return joke.joke;
        }
    }
}
