# BP MVP Implementation Plan

## Scope delivered in this scaffold
- Architecture baseline aligned with `docs/BP-Product-Spec.md`.
- Domain entities and interfaces for playback, state persistence, and profiles.
- Application services for orchestration and basic smart content classification.
- SQLite schema for core data model.
- Installer and build pipeline stubs for Windows shipping flow.

## Immediate next coding steps
1. Add `.sln` and project files targeting `.NET 8` and WinUI 3.
2. Replace `NotImplementedMediaEngine` with libVLC-backed implementation.
3. Add migration runner and concrete repositories in `BP.Infrastructure`.
4. Introduce background folder indexing service + cancellation model.
5. Implement keyboard shortcut map and auto-hide playback controls in `BP.App`.

## Validation targets
- Unit tests for `SmartDetectionService` and `PlaybackOrchestrator` behaviors.
- Integration tests around SQLite repository CRUD.
- Smoke tests with representative codec files in CI.
