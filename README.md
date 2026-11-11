# Joke Duo

A small .NET MAUI app that shows a Chuck Norris joke and a dad joke. The screen uses the MVVM pattern. Jokes come from two public HTTP APIs.

## APIs

### Chuck Norris jokes

The app sends `GET https://api.chucknorris.io/jokes/random` with no extra headers.

A successful body looks like this:

```json
{
  "categories": [],
  "created_at": "2020-01-05 13:42:24.40636",
  "icon_url": "https://api.chucknorris.io/img/avatar/chuck-norris.png",
  "id": "B1Rz0sqqT3a7sP2I71eXhg",
  "updated_at": "2020-01-05 13:42:24.40636",
  "url": "https://api.chucknorris.io/jokes/B1Rz0sqqT3a7sP2I71eXhg",
  "value": "The joke text is here."
}
```

The joke text is the `value` field. The other fields are on the model so the JSON can be deserialized. The screen shows `value`.

### Dad jokes

The app sends `GET https://icanhazdadjoke.com/` with the header `Accept: application/json`. That header asks for JSON. Without it, the service returns plain text.

A successful body looks like this:

```json
{
  "id": "RfiGei3E6Ed",
  "joke": "The joke text is here.",
  "status": 200
}
```

The joke text is the `joke` field. The screen shows `joke`.

## Build and run

You need the .NET 10 SDK and the .NET MAUI workload (`dotnet workload install maui`). Use Visual Studio with the .NET Multi-platform App UI workload, or the command line. You also need the tools for the platform you run (Android SDK, or a Mac for iOS and Mac Catalyst, or Windows for the Windows target).

The project targets Android on every operating system. iOS and Mac Catalyst are included when you are not on Linux. The Windows target is included on Windows.

From this folder:

```bash
dotnet build ChuckNorrisJokesApp/ChuckNorrisJokesApp.csproj -f net10.0-android
```

On Windows you can build and run with:

```bash
dotnet build ChuckNorrisJokesApp/ChuckNorrisJokesApp.csproj -f net10.0-windows10.0.19041.0
dotnet run --project ChuckNorrisJokesApp/ChuckNorrisJokesApp.csproj -f net10.0-windows10.0.19041.0
```

Use `-f net10.0-ios` or `-f net10.0-maccatalyst` on a Mac. You can also open `ChuckNorrisJokesAppSln.slnx` in Visual Studio and press F5.

The phone or computer needs a network connection to load jokes.

## What each part does

- **View** (`Views/JokesView.xaml`): shows the title, the two joke labels, red error text under each joke, a loading indicator, and the Get Jokes button. It only binds to the view model. It does not call the APIs or read JSON.
- **View model** (`ViewModels/JokesViewModel.cs`): holds the Chuck Norris joke, the dad joke, an error message for each, and a loading flag. The button runs `GetJokeCommand`, which asks the two API services for new jokes. While a fetch is in progress the command will not run again, so the button cannot fire a second request. A joke already on screen is replaced only when that request returns a joke. If a request fails, the old joke stays and the error for that joke is shown.
- **Models** (`Models/ChuckNorrisJoke.cs`, `Models/DadJoke.cs`): match the JSON from each API so the services can deserialize a response.
- **API services** (`Services/ChuckNorrisApiHttpService.cs`, `Services/DadJokeApiHttpService.cs`): make the HTTP calls and deserialize the JSON. They do not run inside the view or the view model. A failed HTTP status, JSON that cannot be read, or a body with no joke text becomes an error. The URLs live in `Services/ConfigurationService.cs`.

`MauiProgram.cs` registers the services, the view, and the view model.

## Testing

The API services were called against the live endpoints and against bad responses.

- **Success.** `https://api.chucknorris.io/jokes/random` returned JSON and the service read `value`. `https://icanhazdadjoke.com/` with `Accept: application/json` returned JSON and the service read `joke`.
- **One API fails, the other works.** A Chuck Norris id that does not exist returned HTTP 404. That call failed with a message, and the dad joke call in the same run still returned a joke. A dad joke id that does not exist came back as HTTP 200 JSON with no `joke` field. That call failed, and the Chuck Norris call still returned a joke. The view model only writes a joke property after a successful call, so the joke that loaded stays on screen.
- **Invalid or unexpected data.** Plain text and HTML were rejected as invalid JSON. JSON with a blank `value` or `joke`, and JSON with those fields missing, was rejected. HTTP 503 was rejected before the body was treated as a joke.
- **No network.** Both URLs were pointed at `http://127.0.0.1:9/`, where nothing is listening. Each call failed with a message that the service could not be reached. A joke that had already been stored is left as it is, because the view model does not clear it when the call throws.

## Layout

```
ChuckNorrisJokesAppSln.slnx
└── ChuckNorrisJokesApp/
    ├── MauiProgram.cs
    ├── App.xaml / AppShell.xaml
    ├── Models/
    │   ├── ChuckNorrisJoke.cs
    │   └── DadJoke.cs
    ├── Services/
    │   ├── Interfaces/
    │   ├── ChuckNorrisApiHttpService.cs
    │   ├── DadJokeApiHttpService.cs
    │   ├── ConfigurationService.cs
    │   └── MauiPopupService.cs
    ├── ViewModels/
    │   ├── BaseViewModel.cs
    │   └── JokesViewModel.cs
    ├── Views/
    │   └── JokesView.xaml
    ├── Platforms/
    └── Resources/
```

Jokes are loaded from third-party APIs. This project is not affiliated with those APIs.
