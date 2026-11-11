using System;
using System.Collections.Generic;
using System.Text;

namespace ChuckNorrisJokesApp.Services.Interfaces
{
    public  interface IPopUps
    {
        Task ShowPopupAsync(string title, string message, string buttons);
    }
}
