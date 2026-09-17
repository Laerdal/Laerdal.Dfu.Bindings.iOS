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

## Before bumping the Nordic version in this repo

Two real gotchas already in this repo's own README ("Known issues") that a version-bump task
must not skip — restated here because Claude Code won't auto-load the README the way a human
contributor reading it top-to-bottom would:
- **`Nordic_Package_Version` is defined in *both* `Laerdal.Scripts/Laerdal.targets` and
  `Laerdal.Scripts/Laerdal.Builder.targets`.** Bump both together — CI reads it from
  `Laerdal.targets`, but letting the two drift apart breaks the release step silently.
- **A Nordic version bump can silently corrupt `[Native] DFUState`.** Nordic's Swift `DFUState`
  enum has no explicit raw values — they're assigned by declaration order — so an upstream
  insertion renumbers every case declared after it (this happened going `4.16.0 → 4.17.0`,
  shifting `Completed`/`Aborted` from `6`/`7` to `8`/`9`). **Before ever bumping this version,
  diff the actual Swift source for `DFUState` (and any other `[Native] enum`-bound type)
  against the previous tag** — never assume an "additive-sounding" changelog entry is safe.

## What's not written down in this repo alone — cross-repo Nordic version bumps

`Laerdal.Dfu.Bindings.iOS` and `Laerdal.Dfu.Bindings.Android` wrap **independently-versioned**
native libraries (`IOS-Pods-DFU-Library` vs `Android-DFU-Library`) — bumping this repo's Nordic
version does **not** imply bumping the Android binding too. What *does* need re-checking
whenever either platform's native library moves is `Laerdal.Dfu`'s `NordicDfuUuids`
(Legacy/Secure DFU GATT constants) — re-verify it against the new native version before
republishing, since neither binding repo's own build would catch a drift there.
