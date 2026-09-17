---
name: laerdal-dfu-bindings-ios
description: Use when writing, reviewing, or debugging code in this repo (Laerdal.Dfu.Bindings.iOS — the Xamarin/MAUI binding over Nordic's native iOS/MacCatalyst DFU Pod) or when bumping the wrapped Nordic native library version. Routes to this repo's existing Copilot instructions instead of re-deriving them, and flags a cross-repo coordination step that isn't written down in any single repo.
---

# Laerdal.Dfu.Bindings.iOS orientation

Xamarin/MAUI binding over Nordic Semiconductor's native iOS/MacCatalyst `IOS-Pods-DFU-Library`,
built with the classic Objective-C binding pattern (`ApiDefinition.cs` + `StructsAndEnums.cs`,
`INVOKE_SHARPIE=NO` with a hand-maintained `ApiDefinition.cs`). Most consumers should depend on
`Laerdal.Dfu` instead, which wraps this binding (plus its Android counterpart) behind one
cross-platform API. Verify the paths below still exist before trusting them; if they don't,
this skill is stale, not the docs.

## Step 1 — do this now, before anything else

**Read `.github/copilot-instructions.md` in full, right now, before touching
`ApiDefinition.cs`, `Laerdal.targets`, or the Carthage/fat-lib build pipeline.** It already
documents the real traps here: the `[Protocol]`/`[Model]` attribute bug that only reproduces if
bindings are ever regenerated with `INVOKE_SHARPIE=YES` (do **not** follow
`Laerdal.Mac.CompileAndGenerateFatLibs.sh`'s own Sharpie guidance for the DFU interfaces — it's
written for `Laerdal.McuMgr`'s delegate-style classes and reproduces this exact bug here); the
"poisoned NuGet" missing-native-symbols failure if Restore/Rebuild runs before
`Frameworks/NordicDFU.framework` exists; why parallel builds (`/m:1` is mandatory) corrupt
native fat-lib generation; and the stale-Carthage-symlink codesign failure. Everything below
this point only adds what that file doesn't cover.

## What's not written down in this repo alone — cross-repo Nordic version bumps

Bumping the wrapped Nordic native DFU library version is a **3-repo coordinated change**, not
a single-repo one, and no single repo's docs say so:
- This repo's native version pin (`Laerdal.targets` / Carthage fetch).
- `Laerdal.Dfu.Bindings.Android`'s `Nordic_Package_Version`.
- `Laerdal.Dfu`'s `NordicDfuUuids` (Legacy/Secure DFU GATT constants) needs re-verifying
  against the new native version before republishing — a GATT UUID or attribute layout change
  upstream wouldn't be caught by either binding repo's own build.

Bump all three together and re-validate against real hardware before publishing any of them
individually.
