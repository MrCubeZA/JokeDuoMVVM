# Joke Duo (Chuck Norris + Dad Jokes App)

> **⚠️ For educational purposes only.**
> This solution builds on the Chuck Norris Jokes starter app to practise .NET MAUI, MVVM and dependency injection. It is not production software and comes with no warranty or support. It is not affiliated with or endorsed by Chuck Norris, [chucknorris.io](https://api.chucknorris.io) or [icanhazdadjoke](https://icanhazdadjoke.com). Jokes are fetched live from those third-party APIs, so content is outside this project's control.

## What it does

A small cross-platform .NET MAUI app. Tap the button and it fetches a random Chuck Norris joke from `https://api.chucknorris.io/jokes/random` and a random dad joke from `https://icanhazdadjoke.com/`, and displays both with clear labels. A loading indicator shows while the jokes load. If either request fails, an error message is shown for that joke and the other joke stays on screen.

## API investigation

- **Chuck Norris jokes**: `GET https://api.chucknorris.io/jokes/random` with no extra headers. The JSON response has a `value` field containing the joke, which is the field the app shows.
- **Dad jokes**: `GET https://icanhazdadjoke.com/` with the header `Accept: application/json` (without it the API returns plain text). The JSON response has a `joke` field containing the joke, which is the field the app shows.

## Solution layout

```
ChuckNorrisJokesAppSln.slnx          Solution file
└── ChuckNorrisJokesApp/             Single MAUI project
    ├── MauiProgram.cs               App startup and dependency injection registration
    ├── App.xaml / AppShell.xaml     Application and Shell navigation
    ├── Models/
    │   ├── ChuckNorrisJoke.cs       DTO matching the Chuck Norris API's JSON response
    │   └── DadJoke.cs               DTO matching the dad joke API's JSON response
    ├── Services/
    │   ├── Interfaces/              IChuckNorrisApiService, IDadJokeApiService, IConfigurationService, IPopUps
    │   ├── ChuckNorrisApiHttpService.cs   Calls the Chuck Norris API with HttpClient and deserializes the JSON
    │   ├── DadJokeApiHttpService.cs       Calls the dad joke API with HttpClient and deserializes the JSON
    │   ├── ConfigurationService.cs        Holds the API URLs
    │   └── MauiPopupService.cs            Shows alerts through MAUI
    ├── ViewModels/
    │   ├── BaseViewModel.cs         INotifyPropertyChanged helper
    │   └── JokesViewModel.cs        Exposes both jokes, error messages, loading state and GetJokeCommand
    ├── Views/
    │   └── JokesView.xaml(.cs)      UI bound to JokesViewModel
    ├── Platforms/                   Android, iOS, Mac Catalyst and Windows entry points
    └── Resources/                   Icons, splash screen, fonts, images, styles
```

## Responsibilities

- **View** (`JokesView`): shows the two labelled jokes, the error text, the loading indicator and the button. It only binds to the view model.
- **ViewModel** (`JokesViewModel`): holds both jokes, an error message for each and the loading flag. `GetJokeCommand` asks both services for new jokes and will not run again while a fetch is in progress.
- **Models** (`ChuckNorrisJoke`, `DadJoke`): match each API's JSON so the services can deserialize the response.
- **API services** (`ChuckNorrisApiHttpService`, `DadJokeApiHttpService`): make the HTTP requests and deserialize the JSON, keeping that out of the view model and UI. Unsuccessful HTTP responses and invalid or unexpected data are handled as errors.

## Concepts demonstrated

- **MVVM**: the `JokesView` binds to `JokesViewModel`, which exposes `ChuckNorrisJoke`, the dad joke and `GetJokeCommand`.
- **Dependency injection**: services, view and view model are registered in `MauiProgram.cs` and constructor-injected.
- **Interface-based services**: the API, configuration and pop-up logic sit behind interfaces, so they can be swapped or mocked in tests.
- **Consuming a REST API**: `HttpClient` plus `System.Text.Json`, including a request header for the dad joke API.
- **Cross-platform targeting**: one project targets Android, iOS, Mac Catalyst and Windows.

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- The .NET MAUI workload: `dotnet workload install maui`
- Visual Studio 2026 with the ".NET Multi-platform App UI development" workload (recommended on Windows)
- Platform tooling for whichever target you run (Android SDK/emulator, Xcode on macOS for iOS and Mac Catalyst)

## Running

From Visual Studio, open `ChuckNorrisJokesAppSln.slnx`, pick a target platform and press F5. From the command line:

```bash
dotnet build ChuckNorrisJokesApp/ChuckNorrisJokesApp.csproj -f net10.0-windows10.0.19041.0
dotnet run --project ChuckNorrisJokesApp/ChuckNorrisJokesApp.csproj -f net10.0-windows10.0.19041.0
```

Replace the `-f` value with `net10.0-android`, `net10.0-ios` or `net10.0-maccatalyst` for the other targets. iOS and Mac Catalyst require a Mac. An internet connection is needed to fetch jokes.

## Testing

- **Successful responses**: both APIs returned JSON and the app read `value` and `joke`.
- **API errors**: a failed or invalid response from one API returned an error for that joke while the other joke still loaded.
- **No network access**: with the APIs unreachable, each request failed with an error message instead of crashing.

## Known limitations (learning opportunities)

- `HttpClient` is created directly rather than via `IHttpClientFactory`.
- The API URLs are hard-coded in `ConfigurationService` rather than read from settings.
- There are no automated tests.

## Disclaimer

This project is provided "as is" for learning and demonstration only. Do not use it in production or rely on it for anything important. Review the terms of the [chucknorris.io](https://api.chucknorris.io) and [icanhazdadjoke](https://icanhazdadjoke.com/api) APIs before using them beyond personal learning.
