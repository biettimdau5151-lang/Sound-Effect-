# SoundEffect

A .NET 4.8 WPF application for playing sound effects, featuring a modern UI built with MahApps.Metro and audio handling via NAudio.

## Features
- Modern WPF UI (MahApps.Metro)
- Global Hotkeys for sound triggers
- Audio playback management (NAudio)
- Customizable sound effect groups

## Prerequisites
- **Operating System**: Windows 7 SP1 or later
- **Runtime**: [.NET Framework 4.8 Runtime](https://dotnet.microsoft.com/download/dotnet-framework/net48)
- **IDE**: Visual Studio 2019 or later (recommended)

## Build Instructions

### 1. Clone the repository
```bash
git clone https://github.com/biettimdau5151-lang/Sound-Effect-.git
cd Sound-Effect-
```

### 2. Restore NuGet Packages
Open the solution in Visual Studio. NuGet packages should restore automatically on build. Alternatively, run:
```bash
nuget restore SoundEffect.csproj
```

### 3. Build the Solution
- Open `SoundEffect.csproj` in Visual Studio.
- Select your target configuration (e.g., `Debug` or `Release`) and platform (`x64` is preferred).
- Press `Ctrl+Shift+B` or go to `Build > Build Solution`.

### 4. Run the Application
The executable will be located in:
- `bin\Debug\SoundEffect.exe`
- or `bin\Release\SoundEffect.exe`

## Technologies Used
- **UI Framework**: WPF with [MahApps.Metro](https://mahapps.com/)
- **Audio Library**: [NAudio](https://github.com/naudio/NAudio)
- **JSON Serialization**: [Newtonsoft.Json](https://www.newtonsoft.com/json)
- **Framework**: .NET Framework 4.8

## License
This project is licensed under the GNU General Public License v3.0 - see the [LICENSE](LICENSE) file for details.
