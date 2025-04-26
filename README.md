# MelonPreferencesManager

<p align="center">
In-game UI for managing MelonLoader Mod Preferences. Supports IL2CPP and Mono Unity games.
</p>
<p align="center">
Requires MelonLoader v0.5+
</p>
<p align="center">
  ✨ Powered by <a href="https://github.com/yukieiji/UniverseLib">UniverseLib</a><br>
  🛠️ This is fork of <a href="https://github.com/kafeijao/MelonPreferencesManager">MelonPreferencesManager</a> maintained by tschrock<br>
</p>

# Releases  [![](https://img.shields.io/github/downloads/tschrock/MelonPreferencesManager/total.svg)](../../releases)

[![](https://img.shields.io/github/release/tschrock/MelonPreferencesManager.svg?label=version)](../../releases/latest)
[![](https://img.shields.io/github/actions/workflow/status/tschrock/MelonPreferencesManager/build.yml?branch=main)](https://github.com/tschrock/MelonPreferencesManager/actions)
[![](https://img.shields.io/github/downloads/tschrock/MelonPreferencesManager/latest/total.svg)](../../releases/latest)


## How to use

| Release | IL2CPP | Mono |
| ------- | ------ | ---- |
| ML 0.6+  | ✅ [link](https://github.com/tschrock/MelonPreferencesManager/releases/latest/download/MelonPreferencesManager.MelonLoader.IL2CPP.CoreCLR.zip) | ✖️ |
| ML 0.6(**ONLY 0.6 ALPHA BUILD, NOT BETA**)  | ✅ [link](https://github.com/tschrock/MelonPreferencesManager/releases/latest/download/MelonPreferencesManager.MelonLoader.IL2CPP.net6preview.zip) | ✖️ |
| ML 0.5  | ✅ [link](https://github.com/tschrock/MelonPreferencesManager/releases/latest/download/MelonPreferencesManager.MelonLoader.IL2CPP.zip) | ✅ [link](https://github.com/tschrock/MelonPreferencesManager/releases/latest/download/MelonPreferencesManager.MelonLoader.Mono.zip) | 

1. Unzip the release file into a folder
2. Copy the DLL inside the `Mods` folder into your MelonLoader `Mods` folder
3. Copy all of the DLLs inside the `UserLibs` folder into your MelonLoader `UserLibs` folder

## Common issues and solutions

Although this tool should work out of the box for most Unity games, in some cases you may need to tweak the settings for it to work properly.

To adjust the settings, open the config file: `UserData\MelonPreferences.cfg`

Try adjusting the following settings and see if it fixes your issues:
* `Startup_Delay_Time` - increase to 5-10 seconds (or more as needed), can fix issues with the UI being destroyed or corrupted during startup.
* `Disable_EventSystem_Override` - if input is not working properly, try setting this to `true`.

If these fixes do not work, please create an issue in this repo and I'll do my best to look into it.

## Info for developers

The UI supports the following types by default:

* Toggle: `bool`
* Number input: `int`, `float` etc (any primitive number type)
* String input: `string`
* Key binder: `UnityEngine.KeyCode` or `UnityEngine.InputSystem.Key`
* Dropdown: `enum`
* Multi-toggle: `enum` with `[Flags]` attribute
* Color picker: `UnityEngine.Color` or `UnityEngine.Color32`
* Struct editor: `UnityEngine.Vector3`, `UnityEngine.Quaternion`, etc
* Toml input: Anything else as long as Tomlet can serialize it.

To make a slider, use a number type and provide a `ValueRange` for the Validator when creating the entry. For example:
* `myCategory.CreateEntry("SomeFloat", 0f, validator: new ValueRange<float>(-1f, 1f));`
* `myCategory.CreateEntry("SomeByte", 32, validator: new ValueRange<byte>(0, 255));`

You can override the Toml input for a Type by registering your own InteractiveValue for it. Refer to [existing classes](https://github.com/sinai-dev/MelonPreferencesManager/tree/main/src/UI/InteractiveValues) for more concrete examples.
```csharp
// Define an InteractiveValue class to handle 'Something'
public class InteractiveSomething : InteractiveValue
{
    // declaring this ctor is required
    public InteractiveSomething(object value, Type fallbackType) : base(value, fallbackType) { }

    // you could also check "if type == typeof(Something)" to be more strict
    public override bool SupportsType(Type type) => typeof(Something).IsAssignableFrom(type);

    // override other methods as necessary
}

// Register your class in your MelonMod.OnApplicationStart method
public class MyMod : MelonLoader.MelonMod
{
    public override void OnApplicationStart()
    {
        InteractiveValue.RegisterIValueType<InteractiveSomething>();
    }
}
```

# Building

1. Run the `build.ps1` powershell script to build UnityExplorer. Releases are found in the `Release` folder.

Building individual configurations from your IDE is fine, though note that the intial build process builds into `Release/<version>/...` instead of the subfolders that the powershell script uses. Batch building is not currently supported with the project.
