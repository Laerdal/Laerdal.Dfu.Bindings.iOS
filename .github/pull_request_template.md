## Summary

Describe the change in 2-6 lines.

## Why

Explain the reason for this change.

## Change Type

- [ ] Feature
- [ ] Bug fix
- [ ] Refactor (`refa`)
- [ ] Docs only
- [ ] CI/build/tooling

## Affected Areas

- [ ] `Laerdal.Dfu.Bindings.iOS` (device)
- [ ] `Laerdal.Dfu.Bindings.MacCatalyst`
- [ ] `Laerdal.Dfu.Bindings.iOSSimulator.Arm64` / `.x64`
- [ ] Native binding definitions (`ApiDefinition.cs` / `StructsAndEnums.cs`)
- [ ] Build scripts (`Laerdal.Scripts/*.targets`, `*.sh`)
- [ ] CI/CD (`.github/workflows`)
- [ ] Documentation

## Behavior And Compatibility

- [ ] Public API changed
- [ ] Native/runtime behavior changed
- [ ] Nordic native library version bumped
- [ ] No externally visible behavior change

If any box above is checked, describe impact:

## Platform Notes

List platform-specific behavior differences introduced or touched (iOS vs MacCatalyst vs
simulator builds).

## Tests

- [ ] Built locally end-to-end (`dotnet msbuild Laerdal.Scripts/Laerdal.Builder.targets /m:1`)
- [ ] Manual validation performed against real hardware
- [ ] Not applicable (explain)

Validation notes:

## Documentation

- [ ] Docs updated in same PR (including README's "Known issues" if a new gotcha was found/fixed)
- [ ] Not applicable (explain)

## Risks And Follow-ups

Risk level:
- [ ] Low
- [ ] Medium
- [ ] High

Known limitations or deferred follow-ups:
-

## Checklist

- [ ] Commit header follows `type(scope): short imperative` and is <= 72 chars
- [ ] Commit type is one of: feat, fix, refa, perf, docs, ci, chore, test, build
- [ ] Commit body is 1-2 factual sentences (what/why), no emojis, refs, or co-authors
