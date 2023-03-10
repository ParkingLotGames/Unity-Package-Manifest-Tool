# Unity Package Manifest Tool
![Unity Version](https://img.shields.io/badge/Unity-5.6%2B-blue?style=plastic) ![License](https://img.shields.io/github/license/ParkingLotGames/Unity-Package-Manifest-Tool?style=plastic) ![Size](https://img.shields.io/github/repo-size/ParkingLotGames/Unity-Package-Manifest-Tool?style=plastic) ![package.json version (branch)](https://img.shields.io/github/package-json/v/ParkingLotGames/Unity-Package-Manifest-Tool/main?style=plastic) ![Last commit](https://img.shields.io/github/last-commit/ParkingLotGames/Unity-Package-Manifest-Tool?style=plastic)

![package.json dynamic](https://img.shields.io/github/package-json/keywords/ParkingLotGames/Unity-Package-Manifest-Tool?style=plastic)

![Issues](https://img.shields.io/github/issues-raw/ParkingLotGames/Unity-Package-Manifest-Tool?style=plastic) ![Pull requests](https://img.shields.io/github/issues-pr-raw/ParkingLotGames/Unity-Package-Manifest-Tool?style=plastic)

### An editor tool to create and modify JSON files for your Unity Packages.
![previe](https://github.com/ParkingLotGames/Unity-Package-Manifest-Tool/blob/main/preview.png)
In 2021+ it uses the Newtonsoft.Json library to add Author and Dependency keys

## Installation 
### Package manifest
* Add the following to your manifest.json under "dependencies":

```"com.parkinglotgames.packagemanifesttool": "1.1.1",```
### Git URL
* In Unity, go to Window > Package Manager > "+" button > "Add package from git URL..." and enter:
```https://github.com/ParkingLotGames/Unity-Package-Manifest-Tool.git```

# Usage

"Assets/Create/Package Manifest" or Right click in Project view>"Create/Package Manifest" will open a window to save a new [package manifest](https://docs.unity3d.com/Manual/upm-manifestPkg.html) file. It will open the window to customize the values and in 2021+ the UI has the option to add dependencies and author information.

"Tools/Package Manifest Tool" will open the Tool window, you can load a package manifest file to edit

# //TODO

#### Add summaries, documentation, comments, examples.
