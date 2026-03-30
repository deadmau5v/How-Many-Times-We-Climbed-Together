# How Many Times We Climbed Together

A PEAK mod that remembers how many expeditions you have shared with other players.

## Features

- Tracks how many times you have climbed with each player across sessions.
- Uses a stable player identifier when available so reconnects do not create duplicate records.
- Shows a localized join message when someone you have climbed with before enters the room.
- Adds a localized stats page to the PEAK pause menu.
- Supports Chinese and other in-game languages for the stats page and message text.

## How It Works

- When a new expedition starts, the mod records every non-local player currently in the room.
- If a player joins mid-expedition, they are counted once for that run.
- Player records are stored in `BepInEx/config/HowManyTimesWeClimbedTogether.json`.

## In-Game Usage

- Open the pause menu.
- Select `Climb Stats` or the localized equivalent shown by your game language.
- Browse the list of players you have climbed with and how many times you climbed together.

## Installation

Copy these DLLs into `PEAK/BepInEx/plugins/`:

- `com.github.d5v.HowManyTimesWeClimbedTogether.dll`
- `com.github.PEAKModding.PEAKLib.Core.dll`
- `com.github.PEAKModding.PEAKLib.UI.dll`
- `com.github.MonoDetour.dll`
- `com.github.MonoDetour.Bindings.Reorg.dll`
- `com.github.MonoDetour.Reflection.dll`

Make sure BepInEx is already installed for PEAK.

## Development

1. Copy `Config.Build.user.props.template` to `Config.Build.user.props`.
2. Update the game and plugin paths in that file.
3. Build the mod:

```sh
dotnet build src/BepInExModTemplate/BepInExModTemplate.csproj -c Debug
```

The built DLL will be written to:

`artifacts/bin/BepInExModTemplate/debug/`

## Thunderstore Packaging

Build a Thunderstore package with:

```sh
dotnet build -c Release -target:PackTS -v d
```

The package output will be written to:

`artifacts/thunderstore/`
