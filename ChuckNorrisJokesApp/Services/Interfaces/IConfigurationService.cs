using System;
using System.Collections.Generic;
using System.Text;

namespace ChuckNorrisJokesApp.Services.Interfaces
{
    public interface IConfigurationService
    {
         string ChuckNorrisApiUrl { get; }
         string DadJokeApiUrl { get; }
          
    }
}
