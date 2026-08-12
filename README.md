# 🏠 Laerdal.Dfu.Bindings.iOS

[![CI](https://img.shields.io/github/actions/workflow/status/Laerdal/Laerdal.Dfu.Bindings.iOS/ci.yml?branch=master&logo=github&label=build)](https://github.com/Laerdal/Laerdal.Dfu.Bindings.iOS/actions/workflows/ci.yml)
[![NuGet](https://img.shields.io/nuget/v/Laerdal.Dfu.Bindings.iOS?logo=nuget&color=004880)](https://www.nuget.org/packages/Laerdal.Dfu.Bindings.iOS/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Laerdal.Dfu.Bindings.iOS?logo=nuget&color=004880)](https://www.nuget.org/packages/Laerdal.Dfu.Bindings.iOS/)
[![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![License](https://img.shields.io/github/license/Laerdal/Laerdal.Dfu.Bindings.iOS?color=blue)](LICENSE)

A .NET MAUI binding library for iOS/MacCatalyst over Nordic Semiconductor's native
[`IOS-Pods-DFU-Library`](https://github.com/NordicSemiconductor/IOS-Pods-DFU-Library), for
updating the firmware of Nordic-based devices over the air via BLE.

This is the iOS/MacCatalyst half of the binding layer consumed by
[`Laerdal.Dfu`](https://github.com/Laerdal/Laerdal.Dfu) — most consumers should depend on
`Laerdal.Dfu` directly rather than this package. `Laerdal.Dfu.Bindings.Android` is the Android
counterpart.

## Platform Support

| Platform                | Package                                    | Supported |
|--------------------------|---------------------------------------------|-----------|
| iOS                      | `Laerdal.Dfu.Bindings.iOS`                  | ✅        |
| MacCatalyst              | `Laerdal.Dfu.Bindings.MacCatalyst`          | ✅        |
| iOS Simulator (Apple Silicon) | `Laerdal.Dfu.Bindings.iOS` (`-ios-sim-arm64` prerelease) | ✅ |
| iOS Simulator (Intel)    | `Laerdal.Dfu.Bindings.iOS` (`-ios-sim-x64` prerelease)   | ✅ |

The two simulator flavours are published under the *same* package ID as the main iOS package,
differentiated only by a version postfix — `lipo` can't merge two arm64 slices (device +
simulator) into one binary, so they ship as separate prerelease packages instead. See
[Known issues](#known-issues) for how to consume them.

## Installation

```bash
dotnet add package Laerdal.Dfu.Bindings.iOS
dotnet add package Laerdal.Dfu.Bindings.MacCatalyst
```

Again: unless you're building your own DFU abstraction from scratch, install
[`Laerdal.Dfu`](https://www.nuget.org/packages/Laerdal.Dfu/) instead, which references these
automatically for the right target platform.

## Building locally

Requires **macOS** with:

- **Xcode 16.1+**
- **.NET 10 SDK** with the `ios`/`maccatalyst` workloads (`dotnet workload install maui`)
- **Carthage** (`brew install carthage`)

```bash
git clone https://github.com/Laerdal/Laerdal.Dfu.Bindings.iOS.git
cd Laerdal.Dfu.Bindings.iOS

dotnet msbuild Laerdal.Scripts/Laerdal.Builder.targets \
    /m:1 \
    /p:Laerdal_Github_Access_Token=<your GitHub token, needed by Carthage>
```

`/m:1` is not optional — see [Known issues](#known-issues). The resulting `.nupkg` files land in
`Artifacts/`. To pin an explicit version:

```bash
dotnet msbuild Laerdal.Scripts/Laerdal.Builder.targets /m:1 /p:Laerdal_Version_Full=4.16.0.123
```

If Carthage fails outright, clear its cache and retry:

```bash
rm -rf ~/Library/Caches/org.carthage.CarthageKit
```

## Known issues

A running list of non-obvious build/runtime gotchas this repo has hit before, so they don't get
silently reintroduced.

- **`Foundation.You_Should_Not_Call_base_In_This_Method` at runtime.**
  Caused by the `[Protocol]`/`[Model]` attributes on the `ApiDefinition.cs` interfaces that
  represent the native Swift classes (`DFUFirmware`, `DFUFirmwareSize`, `DFUServiceController`,
  `DFUServiceInitiator`, `DFUUuid`, `DFUUuidHelper`, `IntelHex2BinConverter`,
  `LegacyDFUServiceInitiator`, `SecureDFUServiceInitiator`). The fix (already applied in the
  current `ApiDefinition.cs`) is to remove `[Protocol]`/`[Model]` from these interfaces and rely
  on `[DisableDefaultCtor]` instead.

  ⚠️ **This is the opposite of what `Laerdal.Scripts/Laerdal.Mac.CompileAndGenerateFatLibs.sh`
  recommends in its own comments.** That script is shared with `Laerdal.McuMgr` and its Sharpie
  post-processing step (`sed`-patching `[Protocol]`/`[Model]` onto specific interface names) was
  written for McuMgr's delegate-style classes (`IOSFileUploader`, `IOSFileDownloader`,
  `IOSDeviceResetter`, `IOSFirmwareEraser`, `IOSFirmwareInstaller`, and their `IOSListenerFor*`
  counterparts) — none of which exist in this DFU binding. If the Nordic native library is ever
  bumped and bindings are regenerated with `INVOKE_SHARPIE=YES`, **do not** blindly follow that
  script's `[Protocol]` guidance for the DFU classes above; it reproduces this exact bug. Today's
  build pipeline sets `INVOKE_SHARPIE=NO` and ships the hand-maintained `ApiDefinition.cs` as-is,
  so this only bites on a future manual bindings regen.

- **"Poisoned" nuget — missing native symbols at runtime.**
  Happens if the `Restore`/`Rebuild` MSBuild targets run before the
  `Frameworks/NordicDFU.framework` folder exists on disk. Fixed by always invoking
  `EnsureFrameworkFolderIsCreated` as its own separate `<MSBuild>` call *before* `Restore`/
  `Rebuild` — this is why `Laerdal.Builder.targets` issues 3 separate MSBuild calls per project
  instead of one combined build.

- **Parallel builds corrupt the native fat-lib generation.**
  `Laerdal.targets` hard-fails the build if `MSBuildNodeCount > 1`. Carthage/Sharpie's
  Swift-to-fat-lib pipeline isn't safe under MSBuild parallelization — always build with `/m:1`
  (see [Building locally](#building-locally)).

- **Codesign failures in consuming MAUI apps.**
  Carthage/`xcodebuild` leave stale symlinks inside the generated `Frameworks/*.framework`
  folder, which break codesigning in apps that consume the resulting nuget. `Laerdal.targets`
  removes them explicitly (`find . -type l -delete`) right after fat-lib generation — don't
  remove that step even though it looks superfluous.

- **Consuming this library from a desktop/UI-testing simulator build fails to compile.**
  Work around it the same way `Laerdal.Dfu` documents — override just the native binding with the
  simulator-specific prerelease package for your architecture:

  ```xml
  <PackageReference Include="Laerdal.Dfu.Bindings.iOS" Version="4.16.0.123-ios-sim-arm64">
      <NoWarn>$(NoWarn);NU1605</NoWarn>
  </PackageReference>
  ```

- **Manual version-sync footgun.**
  `Nordic_Package_Version` (currently `4.16.0`) is defined in both `Laerdal.Builder.targets` and
  `Laerdal.targets`, and letting the two drift apart breaks the GitHub release step silently. CI
  (`ci.yml`) reads this value directly out of `Laerdal.targets` rather than keeping its own copy,
  so there are only 2 places left to keep in sync, not 3 — but they're still manual. Bump the
  Nordic version in both `.targets` files together.

## License

[BSD 3-Clause](LICENSE)
