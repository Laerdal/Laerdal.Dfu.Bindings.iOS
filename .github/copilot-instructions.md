# GitHub Copilot Instructions for Laerdal.Dfu.Bindings.iOS

## Project Overview

**Laerdal.Dfu.Bindings.iOS** is a .NET MAUI binding library for iOS/MacCatalyst over Nordic
Semiconductor's native [`IOS-Pods-DFU-Library`](https://github.com/NordicSemiconductor/IOS-Pods-DFU-Library).
It's the iOS/MacCatalyst half of the binding layer consumed by
[`Laerdal.Dfu`](https://github.com/Laerdal/Laerdal.Dfu) — most consumers should depend on
`Laerdal.Dfu` directly rather than this package (`Laerdal.Dfu.Bindings.Android` is the Android
counterpart).

Ships as three package flavours from this repo:
- `Laerdal.Dfu.Bindings.iOS` — device build
- `Laerdal.Dfu.Bindings.MacCatalyst` — MacCatalyst build
- iOS Simulator (Apple Silicon / Intel) — published under the **same** package ID as the main
  iOS package, differentiated only by a prerelease version postfix, because `lipo` can't merge
  two arm64 slices (device + simulator) into one binary.

## Technology Stack

- **Framework:** `net10.0-ios` (SDK `10.0.100`, see `global.json`)
- **Type:** Xamarin/MAUI binding library — `ApiDefinition.cs` + `StructsAndEnums.cs`, the classic
  Objective-C binding pattern (not ordinary C#)
- **Build platform:** macOS only, with Xcode, Carthage, and the `ios`/`maccatalyst` MAUI
  workloads
- **CI/CD:** GitHub Actions

## Project Structure

```
Laerdal.Dfu.Bindings.iOS/
├── .github/
│   ├── workflows/ci.yml
│   └── copilot-instructions.md          # This file
├── Laerdal.Dfu.Bindings.iOS/
│   ├── ApiDefinition.cs                  # Hand-maintained ObjC binding definitions
│   ├── StructsAndEnums.cs
│   └── Laerdal.Dfu.Bindings.iOS.csproj
├── Laerdal.Dfu.Bindings.MacCatalyst/
├── Laerdal.Dfu.Bindings.iOSSimulator.Arm64/
├── Laerdal.Dfu.Bindings.iOSSimulator.x64/
├── Laerdal.Dfu.Native/                   # Carthage-fetched NordicDFU.framework lives here
└── Laerdal.Scripts/                       # Shared build targets (also used by Laerdal.Dfu, McuMgr)
```

## Building Locally

Requires **macOS** with Xcode (SDK matching `TargetPlatformVersion` in
`Laerdal.Scripts/Laerdal.targets`, currently `26.5` — check with `xcodebuild -showsdks`),
**.NET SDK feature band `10.0.400`+** (older bands' `ios`/`maccatalyst` workload manifests don't
support platform version `26.5` and fail with `NETSDK1140`), and **Carthage**
(`brew install carthage`).

**Always build with `/m:1`** — see Known Gotchas below; this is not optional.

## Known Gotchas (see README "Known issues" for full detail)

- **`Foundation.You_Should_Not_Call_base_In_This_Method` at runtime.** Caused by `[Protocol]`/
  `[Model]` attributes on the DFU `ApiDefinition.cs` interfaces (`DFUFirmware`,
  `DFUServiceController`, `DFUServiceInitiator`, etc.). Fixed by removing those attributes and
  relying on `[DisableDefaultCtor]` instead — this is the **opposite** of what
  `Laerdal.Scripts/Laerdal.Mac.CompileAndGenerateFatLibs.sh`'s own comments recommend, because
  that script's Sharpie post-processing step was written for `Laerdal.McuMgr`'s delegate-style
  classes, not this repo's DFU classes. If the Nordic library is ever bumped and bindings
  regenerated with `INVOKE_SHARPIE=YES`, do **not** follow that script's `[Protocol]` guidance
  for the DFU interfaces — it reproduces this exact bug. Current pipeline uses
  `INVOKE_SHARPIE=NO` with a hand-maintained `ApiDefinition.cs`.
- **"Poisoned" nuget — missing native symbols at runtime** if `Restore`/`Rebuild` run before
  `Frameworks/NordicDFU.framework` exists on disk. Fixed by `Laerdal.Builder.targets` issuing 3
  separate MSBuild calls per project, with `EnsureFrameworkFolderIsCreated` always running first.
- **Parallel builds corrupt native fat-lib generation.** `Laerdal.targets` hard-fails if
  `MSBuildNodeCount > 1` — Carthage/Sharpie's Swift-to-fat-lib pipeline isn't safe under MSBuild
  parallelization.
- **Codesign failures in consuming MAUI apps** from stale symlinks Carthage/`xcodebuild` leave
  inside the generated `Frameworks/*.framework` folder. `Laerdal.targets` explicitly removes them
  (`find . -type l -delete`) right after fat-lib generation — don't drop that step.

## Coding Standards

Follow this repo's `.editorconfig`. Prefer clarity over cleverness; avoid unnecessary
abstraction — this is a firm preference, not a suggestion.

## Commit Message Format

`type (scope): short imperative`, <= 72 characters, matching the sibling repos in this DFU
family (`Laerdal.Dfu`, `Laerdal.Dfu.Bindings.Android`).

## Useful Resources

- **Repository:** https://github.com/Laerdal/Laerdal.Dfu.Bindings.iOS
- **NuGet Package:** https://www.nuget.org/packages/Laerdal.Dfu.Bindings.iOS
- **Nordic IOS-Pods-DFU-Library:** https://github.com/NordicSemiconductor/IOS-Pods-DFU-Library
