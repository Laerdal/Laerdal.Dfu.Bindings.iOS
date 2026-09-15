## Summary

Describe the change in 2-6 lines.

## Why

Explain the reason for this change.

## Change Type

- [ ] Feature
- [ ] Bug fix
- [ ] Refactor
- [ ] Docs only
- [ ] CI/build/tooling

## Affected Areas

- [ ] iOS binding (`Laerdal.Dfu.Bindings.iOS`)
- [ ] MacCatalyst binding (`Laerdal.Dfu.Bindings.MacCatalyst`)
- [ ] iOS Simulator bindings (`iOSSimulator.Arm64` / `iOSSimulator.x64`)
- [ ] Native framework (`Frameworks/NordicDFU.framework`)
- [ ] Build scripts (`Laerdal.Scripts`)
- [ ] Central package management (`Directory.Packages.props`)
- [ ] Documentation

## Behavior And Compatibility

- [ ] Public API changed
- [ ] Native framework version bumped
- [ ] No externally visible behavior change

If any box above is checked, describe impact:

## Tests

- [ ] Ran the end-to-end `/m:1` build (`dotnet msbuild Laerdal.Scripts/Laerdal.Builder.targets /m:1 ...`) — mandatory per README; parallel MSBuild corrupts native fat-library generation
- [ ] Manual validation performed (device/simulator, against real hardware where applicable)
- [ ] Not applicable (explain)

Validation notes:

## Documentation

- [ ] README updated
- [ ] Not applicable (explain)

## Checklist

- [ ] Commit header follows `type(scope): short imperative` and is <= 72 chars
- [ ] Commit type is one of: feat, fix, refa, perf, docs, ci, chore, test, build
- [ ] Commit body is 1-2 factual sentences (what/why), no emojis, refs, or co-authors
- [ ] CI passes
- [ ] Change is scoped to one logical unit of work
