using ChuckNorrisJokesApp.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace ChuckNorrisJokesApp.ViewModels
{
    public class JokesViewModel : BaseViewModel
    {
        public ICommand GetJokeCommand { get; set; }

        private string _chuckNorrisJoke = "";

        public string ChuckNorrisJoke
        {
            get { return _chuckNorrisJoke; }
            set
            {
                _chuckNorrisJoke = value;
                OnPropertyChanged();
            }
        }

        private string _dadJoke = "";

        public string DadJoke
        {
            get { return _dadJoke; }
            set
            {
                _dadJoke = value;
                OnPropertyChanged();
            }
        }

        private string _chuckNorrisError = "";

        public string ChuckNorrisError
        {
            get { return _chuckNorrisError; }
            set
            {
                _chuckNorrisError = value;
                OnPropertyChanged();
            }
        }

        private string _dadJokeError = "";

        public string DadJokeError
        {
            get { return _dadJokeError; }
            set
            {
                _dadJokeError = value;
                OnPropertyChanged();
            }
        }

        private bool _isLoading;

        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                _isLoading = value;
                OnPropertyChanged();

                if (GetJokeCommand is Command command)
                {
                    command.ChangeCanExecute();
                }
            }
        }

        private IChuckNorrisApiService _chuckNorrisApiService;
        private IDadJokeApiService _dadJokeApiService;

        public JokesViewModel(IChuckNorrisApiService chuckNorrisApiService, IDadJokeApiService dadJokeApiService)
        {
            _chuckNorrisApiService = chuckNorrisApiService;
            _dadJokeApiService = dadJokeApiService;

            GetJokeCommand = new Command(FetchJoke, CanFetchJokes);
        }

        private bool CanFetchJokes(object param)
        {
            return !IsLoading;
        }

        private async void FetchJoke(object param)
        {
            if (IsLoading)
            {
                return;
            }

            IsLoading = true;

            try
            {
                try
                {
                    ChuckNorrisJoke = await _chuckNorrisApiService.GetJokeAsync();
                    ChuckNorrisError = "";
                }
                catch (Exception ex)
                {
                    ChuckNorrisError = ex.Message;
                }

                try
                {
                    DadJoke = await _dadJokeApiService.GetJokeAsync();
                    DadJokeError = "";
                }
                catch (Exception ex)
                {
                    DadJokeError = ex.Message;
                }
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}
