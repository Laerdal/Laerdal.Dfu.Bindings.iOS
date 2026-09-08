---
name: code-review
description: Use for reviewing pull requests on Laerdal.Dfu.Bindings.iOS — checks the ApiDefinition.cs [Protocol]/[Model] regression, native-framework build-ordering, parallel-build safety, and codesign symlink cleanup, all documented in the README's Known Issues.
---

# Code Review — Laerdal.Dfu.Bindings.iOS

This repo's own README ["Known issues" section](../../../README.md#known-issues) is the
canonical list of non-obvious gotchas — read it in full. The checklist below exists so a
reviewer (human or Copilot) actively checks new PRs against each one, not just the person who
originally wrote them down.

## Checklist for changes in this repo

- [ ] **`ApiDefinition.cs` regen or edits**: if `[Protocol]`/`[Model]` attributes appear on any
      of the DFU interfaces (`DFUFirmware`, `DFUFirmwareSize`, `DFUServiceController`,
      `DFUServiceInitiator`, `DFUUuid`, `DFUUuidHelper`, `IntelHex2BinConverter`,
      `LegacyDFUServiceInitiator`, `SecureDFUServiceInitiator`), that's the exact bug shape that
      causes `Foundation.You_Should_Not_Call_base_In_This_Method` at runtime — flag it even if
      it came from a Sharpie regen following `Laerdal.Mac.CompileAndGenerateFatLibs.sh`'s own
      comments, since those comments are wrong for this repo's classes (they're written for
      `Laerdal.McuMgr`'s delegate-style classes instead).
- [ ] **Build target ordering**: any change to `Laerdal.Builder.targets`/`Laerdal.targets` must
      keep `EnsureFrameworkFolderIsCreated` running as its own separate MSBuild call *before*
      `Restore`/`Rebuild` — collapsing this into a single combined build reintroduces the
      "poisoned nuget" missing-native-symbols bug.
- [ ] **Parallelism guard**: don't remove or weaken the `MSBuildNodeCount > 1` hard-fail in
      `Laerdal.targets` — Carthage/Sharpie's fat-lib generation isn't safe under MSBuild
      parallelization, and this is the only thing enforcing `/m:1`.
- [ ] **Symlink cleanup step**: don't remove the `find . -type l -delete` step that runs after
      fat-lib generation, even if it looks redundant — it prevents codesign failures in
      consuming MAUI apps caused by stale Carthage/`xcodebuild` symlinks.
- [ ] **`TargetPlatformVersion`/SDK version bumps**: cross-check against
      `Laerdal.Scripts/Laerdal.targets` and the README's "Building locally" section — a bump
      here needs the CI runner's Xcode/SDK and the required .NET SDK feature band to actually
      match, or builds fail with `NETSDK1140`.

## What to flag as a real risk, not a nit

- Any change to the simulator-package versioning trick (same package ID, prerelease postfix for
  arm64/x64 simulator builds) — this is a deliberate workaround for a `lipo` limitation, not
  something to "simplify" without understanding why it's shaped this way.
