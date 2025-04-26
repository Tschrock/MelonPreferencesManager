cd UniverseLib
.\build.ps1
cd ..


# ----------- MelonLoader IL2CPP (net6) -----------
dotnet build src/MelonPreferencesManager.sln -c Release_ML_Cpp_net6preview
$Path = "Release\MelonPrefManager.MelonLoader.IL2CPP.net6preview"
# ILRepack
UnityExplorer/lib/ILRepack.exe /target:library /lib:lib/net6 /lib:lib/unhollowed /lib:$Path /internalize /out:$Path/MelonPrefManager.ML.IL2CPP.net6preview.dll $Path/MelonPrefManager.ML.IL2CPP.net6preview.dll
# (cleanup and move files)
Remove-Item $Path/MelonPrefManager.ML.IL2CPP.net6preview.deps.json
Remove-Item $Path/MelonPrefManager.ML.IL2CPP.net6preview.pdb
Remove-Item $Path/Tomlet.dll
Remove-Item $Path/Iced.dll
Remove-Item $Path/UnhollowerBaseLib.dll
New-Item -Path "$Path" -Name "Mods" -ItemType "directory" -Force
Move-Item -Path $Path/MelonPrefManager.ML.IL2CPP.net6preview.dll -Destination $Path/Mods -Force
New-Item -Path "$Path" -Name "UserLibs" -ItemType "directory" -Force
Move-Item -Path $Path/UniverseLib.IL2CPP.Unhollower.dll -Destination $Path/UserLibs -Force
# (create zip archive)
Remove-Item $Path/../MelonPrefManager.MelonLoader.IL2CPP.net6preview.zip -ErrorAction SilentlyContinue
compress-archive .\$Path\* $Path/../MelonPrefManager.MelonLoader.IL2CPP.net6preview.zip

# ----------- MelonLoader IL2CPP CoreCLR (net6) -----------
dotnet build src/MelonPreferencesManager.sln -c Release_ML_Cpp_CoreCLR
$Path = "Release\MelonPrefManager.MelonLoader.IL2CPP.CoreCLR"
# ILRepack
UnityExplorer/lib/ILRepack.exe /target:library /lib:lib/net6 /lib:lib/interop /lib:$Path /internalize /out:$Path/MelonPrefManager.ML.IL2CPP.CoreCLR.dll $Path/MelonPrefManager.ML.IL2CPP.CoreCLR.dll
# (cleanup and move files)
Remove-Item $Path/MelonPrefManager.ML.IL2CPP.CoreCLR.deps.json
Remove-Item $Path/MelonPrefManager.ML.IL2CPP.CoreCLR.pdb
Remove-Item $Path/Tomlet.dll
Remove-Item $Path/Iced.dll
Remove-Item $Path/Il2CppInterop.Common.dll
Remove-Item $Path/Il2CppInterop.Runtime.dll
Remove-Item $Path/Microsoft.Extensions.Logging.Abstractions.dll
New-Item -Path "$Path" -Name "Mods" -ItemType "directory" -Force
Move-Item -Path $Path/MelonPrefManager.ML.IL2CPP.CoreCLR.dll -Destination $Path/Mods -Force
New-Item -Path "$Path" -Name "UserLibs" -ItemType "directory" -Force
Move-Item -Path $Path/UniverseLib.ML.IL2CPP.Interop.dll -Destination $Path/UserLibs -Force
# (create zip archive)
Remove-Item $Path/../MelonPrefManager.MelonLoader.IL2CPP.CoreCLR.zip -ErrorAction SilentlyContinue
compress-archive .\$Path\* $Path/../MelonPrefManager.MelonLoader.IL2CPP.CoreCLR.zip

# ----------- MelonLoader IL2CPP (net472) -----------
dotnet build src/MelonPreferencesManager.sln -c Release_ML_Cpp_net472
$Path = "Release/MelonPrefManager.MelonLoader.IL2CPP"
# ILRepack
UnityExplorer/lib/ILRepack.exe /target:library /lib:lib/net472 /lib:lib/net35 /lib:lib/unhollowed /lib:$Path /internalize /out:$Path/MelonPrefManager.ML.IL2CPP.dll $Path/MelonPrefManager.ML.IL2CPP.dll
# (cleanup and move files)
Remove-Item $Path/Tomlet.dll
Remove-Item $Path/Iced.dll
Remove-Item $Path/UnhollowerBaseLib.dll
Remove-Item $Path/MelonPrefManager.ML.IL2CPP.pdb
New-Item -Path "$Path" -Name "Mods" -ItemType "directory" -Force
Move-Item -Path $Path/MelonPrefManager.ML.IL2CPP.dll -Destination $Path/Mods -Force
New-Item -Path "$Path" -Name "UserLibs" -ItemType "directory" -Force
Move-Item -Path $Path/UniverseLib.IL2CPP.Unhollower.dll -Destination $Path/UserLibs -Force
# (create zip archive)
Remove-Item $Path/../MelonPrefManager.MelonLoader.IL2CPP.zip -ErrorAction SilentlyContinue
compress-archive .\$Path\* $Path/../MelonPrefManager.MelonLoader.IL2CPP.zip

# ----------- MelonLoader Mono -----------
dotnet build src/MelonPreferencesManager.sln -c Release_ML_Mono
$Path = "Release/MelonPrefManager.MelonLoader.Mono"
# ILRepack
UnityExplorer/lib/ILRepack.exe /target:library /lib:lib/net35 /lib:$Path /internalize /out:$Path/MelonPrefManager.ML.Mono.dll $Path/MelonPrefManager.ML.Mono.dll
# (cleanup and move files)
Remove-Item $Path/Tomlet.dll
Remove-Item $Path/MelonPrefManager.ML.Mono.pdb
New-Item -Path "$Path" -Name "Mods" -ItemType "directory" -Force
Move-Item -Path $Path/MelonPrefManager.ML.Mono.dll -Destination $Path/Mods -Force
New-Item -Path "$Path" -Name "UserLibs" -ItemType "directory" -Force
Move-Item -Path $Path/UniverseLib.Mono.dll -Destination $Path/UserLibs -Force
# (create zip archive)
Remove-Item $Path/../MelonPrefManager.MelonLoader.Mono.zip -ErrorAction SilentlyContinue
compress-archive .\$Path\* $Path/../MelonPrefManager.MelonLoader.Mono.zip
