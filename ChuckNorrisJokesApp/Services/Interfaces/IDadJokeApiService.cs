using System;
using System.Collections.Generic;
using System.Text;

namespace ChuckNorrisJokesApp.Services.Interfaces
{
    public interface IDadJokeApiService
    {
        Task<string> GetJokeAsync();
    }
}
