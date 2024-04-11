# 🏠 Laerdal.Dfu.Bindings.iOS

This is an Xamarin binding library for the Nordic Semiconductors iOS library for updating the firmware of their devices over the air via Bluetooth Low Energy.

The native iOS Pod library is located here: https://github.com/NordicSemiconductor/IOS-Pods-DFU-Library

## 🚀 Getting Started

You'll need :

- **MacOS**
  - with **XCode**
  - with **.NET6-ios**
  - with **Carthage**
  - [with **ObjectiveSharpie**] (optional)

## 🛠️ Build

```bash
brew cask install objectivesharpie
```

[More about Objective Sharpie](https://docs.microsoft.com/en-us/xamarin/cross-platform/macios/binding/objective-sharpie/get-started)

## Steps to build on Local-Dev

### 1) Checkout

```bash
git clone https://github.com/Laerdal/Laerdal.Dfu.Bindings.iOS.git
```

### 2) Build

```bash
dotnet build
```

You'll find the nuget in `Output/`

### ❗ Known issues

- [**Invalid Swift support when submitted to the Apple AppStore**](https://github.com/Laerdal/Laerdal.Dfu.iOS/issues/3) |

Fix : https://github.com/Laerdal/Laerdal.Dfu.iOS/issues/3#issuecomment-783298581 | 

```shell
#!/usr/bin/env sh

xcode_lib_path="/Applications/Xcode.app/Contents/Developer/Toolchains/XcodeDefault.xctoolchain/usr/lib/swift-5.0/iphoneos"
app_path=$1
app_name=<insert app name>
libs=("$app_path/Products/Applications/$app_name/Frameworks/"*.dylib)

for i in "${libs[@]}"
do
  cp "$xcode_lib_path/$(basename "$i")" "$app_path/SwiftSupport/iphoneos/"
  cp "$xcode_lib_path/$(basename "$i")" "$app_path/Products/Applications/$app_name/Frameworks/"
done
```

> -- Thanks [@OliverFlecke](https://github.com/OliverFlecke)

- [**ObjCRuntime.RuntimeException: Can't register the class XXX when the dynamic registrar has been linked away"**](https://github.com/Laerdal/Laerdal.Dfu.iOS/issues/1)

Fix : You might need to add "--optimize=-remove-dynamic-registrar" to your apps mtouch args.
