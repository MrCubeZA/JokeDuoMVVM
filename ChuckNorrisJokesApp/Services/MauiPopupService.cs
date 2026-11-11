using ChuckNorrisJokesApp.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChuckNorrisJokesApp.Services
{
    public class MauiPopupService : IPopUps
    {
        public async Task ShowPopupAsync(string title, string message, string buttons)
        {
            await Shell.Current.DisplayAlertAsync(title, message, buttons);
        }
    }
}
