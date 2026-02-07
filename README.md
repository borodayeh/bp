# BP (Borod Player)

BP is an offline-first modular desktop media player architecture scaffold targeting Windows 10/11.

## What's included in this repository
- Layered architecture skeleton (`Domain`, `Application`, `Infrastructure`, `App`).
- MVP-oriented entity and interface contracts for playback, library, profiles, and persistence.
- Initial SQLite schema for media index, playback state, bookmarks, and profiles.
- Build and installer pipeline (`build/Build.ps1`, Inno Setup script, GitHub Actions Windows packaging workflow).
- Test-project placeholders for unit/integration/media smoke phases.

## Build installer (.exe) for easy download
- CI/CD artifact flow: run `.github/workflows/build-windows.yml` and download `BP-Setup-x64` from workflow artifacts.
- Local Windows build: run `./build/Build.ps1 -Configuration Release -Runtime win-x64 -Version 0.1.0`.
- Full release instructions: `docs/release/How-To-Build-And-Download-Installer.md`.

## Next implementation milestones
1. Wire `BP.App` to WinUI 3 shell and Composition surface.
2. Implement `IMediaEngine` over libVLC/FFmpeg in `BP.Media.Native` + `BP.Media.Interop`.
3. Add folder indexing workers and incremental library sync.
4. Connect schema migrations and repository implementations.
5. Expand automated tests once UI/runtime integration is complete.
