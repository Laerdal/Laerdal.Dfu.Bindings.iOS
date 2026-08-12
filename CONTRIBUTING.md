# Contributing

## Branching

Work off a short-lived feature branch cut from `master`. Open a PR early and mark it as a Draft
if it's still in progress.

## Commit convention

Commit header: `type(scope): short imperative`, <= 72 chars. Body (if present): 1-2 factual
sentences (what/why) — no emojis, issue refs, or co-authors.

Types: `feat`, `fix`, `refa`, `perf`, `docs`, `ci`, `chore`, `test`, `build`.

## Pull requests

Fill out the PR template. Keep each PR scoped to one logical change. CI must pass before merge.

## Code style

Follow this repo's `.editorconfig`. Prefer clarity over cleverness; avoid unnecessary
abstraction — this is a firm preference, not a suggestion.

## Building

This repo can only be built on macOS (Carthage + Xcode + Sharpie). See
[README.md](README.md#building-locally) for the full setup and the [Known issues](README.md#known-issues)
section before touching `Laerdal.Scripts/*.targets` or the native binding definitions.
