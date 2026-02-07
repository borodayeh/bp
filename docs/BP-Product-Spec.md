# BP (Borod Player) — Production Architecture & Delivery Specification

## 1) Product Architecture Overview

### 1.1 High-level system architecture
BP is designed as a **modular desktop application** with strict separation between rendering, media pipeline, and domain logic.

```text
┌─────────────────────────────────────────────────────────────┐
│ BP.exe (WinUI 3 shell + Composition UI)                    │
│                                                             │
│  UI Layer (XAML + MVVM)                                    │
│   ├─ Playback Surface (Video/Timeline/Subtitles)           │
│   ├─ Library Views (Music/Video/Recently Played)           │
│   ├─ Mini Player / Fullscreen / PiP                        │
│   └─ Settings / Profiles / Shortcuts                       │
│                                                             │
│  Application Services                                       │
│   ├─ Playback Orchestrator                                 │
│   ├─ Smart Detection Service                               │
│   ├─ Resume/Bookmark Service                               │
│   ├─ Metadata & Artwork Service (optional internet)        │
│   ├─ Audio DSP Controller                                  │
│   └─ Library Indexer                                        │
│                                                             │
│  Core Engine (native C++)                                  │
│   ├─ LibVLC/FFmpeg playback backend                        │
│   ├─ Hardware decode abstraction (DXVA2/D3D11VA)           │
│   ├─ Subtitle parser/render pipeline                       │
│   ├─ Thumbnail extraction engine                           │
│   └─ Frame-accurate seek manager                           │
│                                                             │
│  Persistence Layer (SQLite + local cache)                  │
│   ├─ Media index / playback history                        │
│   ├─ Resume state / bookmarks / profiles                   │
│   └─ Artwork + timeline thumbnail cache                    │
└─────────────────────────────────────────────────────────────┘
```

### 1.2 Process model
- **Single process by default** for low overhead and predictable UX.
- Optional isolated helper process for heavy thumbnail generation (future safety/perf upgrade).
- Crash-safe autosave points for playback state every N seconds and on app lifecycle events.

### 1.3 Core modules
- **Shell/UI**: navigation, windows, overlays, animation states.
- **Playback Domain**: commands (`Play`, `Pause`, `Seek`, `LoadSubtitle`, `SetProfile`).
- **Media Adapter**: wraps libVLC/FFmpeg API into stable internal interfaces.
- **Library Domain**: scanning, dedupe, classification, sorting/filtering.
- **Intelligence Domain**: media-type classifier and importance scoring.
- **Persistence**: SQLite repositories + migration system.
- **Diagnostics**: structured logs + crash dumps + perf counters.

### 1.4 Data model (minimum)
- `MediaItem`: file path hash, title, duration, codec, streams, last played.
- `PlaybackState`: current position, last stable keyframe, speed, subtitle track, audio track.
- `Bookmark`: mediaId, timestamp, optional note, createdAt.
- `Profile`: EQ preset, normalization target, dialogue enhancement mode.
- `SessionEvent`: start/stop/seek/error telemetry (local only, opt-in export).

### 1.5 Offline-first behavior
- Fully functional without internet.
- Metadata/artwork fetching is explicit opt-in and can be disabled globally.
- No account system. No cloud requirement. No blocking network calls in playback path.

---

## 2) Feature Breakdown (MVP vs Future)

### 2.1 MVP (ship target)

#### Playback
- Local file playback for major formats: MP4, MKV, AVI, MOV, WEBM, FLAC, MP3, AAC, WAV, OGG.
- H.264/H.265/VP9/AV1 decode when available.
- GPU acceleration (D3D11VA/DXVA2) with software fallback.
- Frame-accurate seek (keyframe + fine correction).
- Playback speed: 0.5x–2.0x with pitch correction for audio content.
- Subtitle support: SRT, ASS/SSA, VTT (embedded + external).
- Resume playback per file across restart/reboot.

#### Smart features
- Content type heuristic: Movie / Series / Music / Tutorial.
- Profile presets:
  - **Cinema** (default dynamic range, subtle bass lift)
  - **Study** (dialogue boost + reduced dynamic swings)
  - **Night** (loudness management, reduced peaks)
  - **Music** (flat video controls, EQ-focused)
- Basic volume normalization (EBU R128-inspired target loudness).

#### Timeline/navigation
- Seekbar with chapter marks if present.
- User bookmarks with short notes.
- “Return to last meaningful point” (last segment watched >20s).

#### Library
- Watched folders + recursive scan.
- Media cards by type (Audio/Video) and last played.
- Sort/filter by duration, date added, last watched.

#### UX
- Dark-first theme.
- Contextual controls auto-hide.
- Keyboard shortcuts and wheel/gesture support.
- Fullscreen + borderless windowed playback.

### 2.2 V1.1–V2 future extensions
- Visual thumbnail timeline with precomputed strips.
- Scene-change detection for quick jump points.
- Enhanced classifier (series episode parsing, tutorial chaptering).
- Plugin SDK (C ABI + managed wrapper).
- Audio enhancement chain upgrades (voice isolation modes).
- Network streams and casting (DLNA/Chromecast equivalent) as optional modules.

---

## 3) UI/UX Layout Description

### 3.1 Design language
- **Modern, minimal, premium**: large spacing, low-contrast surfaces, high-legibility typography.
- Dark-first palette with restrained accent color.
- Motion: 120–180ms ease-out transitions; no flashy effects.

### 3.2 Main shells
1. **Home/Library Shell**
   - Left rail: Library, Music, Videos, Playlists, Settings.
   - Top command bar: search, sort, scan status, quick filters.
   - Main content: adaptive media grid/list.

2. **Now Playing (Video)**
   - Full-bleed video surface.
   - Bottom overlay (auto-hide): play controls, timeline, subtitle/audio track picker.
   - Right contextual panel (collapsible): bookmarks, chapters, playback profile.

3. **Now Playing (Audio)**
   - Artwork + transport controls + queue.
   - Expanded panel for EQ and normalization.

4. **Mini Player**
   - Compact always-on-top mode.
   - Essential controls only.

### 3.3 Key interaction rules
- Controls appear on mouse move / tap; fade out on inactivity.
- Space toggles play/pause, arrows seek, `J/K/L` quick transport.
- Scroll on timeline zooms precision for frame-level seek.
- Long files show semantic markers (bookmarks, last watched, chapters).

### 3.4 Accessibility & usability
- High-contrast mode compatibility.
- Full keyboard navigation.
- Scalable text and control density presets.
- Clear focus rings and target sizes for touchpad use.

---

## 4) Technical Stack Decision with Reasoning

### 4.1 Language and runtime
- **C# (.NET 8)** for app shell and business logic.
  - Fast iteration, strong ecosystem, stable desktop deployment.
- **C++20 native core module** for performance-critical media integration.
  - Tight control over decode/seek/timing and lower overhead where needed.

### 4.2 UI framework
- **WinUI 3 (Windows App SDK)**.
  - Native Windows 10/11 modern controls.
  - Fluent-compatible, high-quality dark-mode behavior.
  - Good composition pipeline for smooth transitions.

### 4.3 Media engine choice
- **libVLC 4.x (or latest stable 3.x initially) + FFmpeg stack**.
  - Proven broad codec/container coverage (VLC-class compatibility).
  - Mature subtitle and stream handling.
  - Reliable hardware decode pathways on Windows.

Why not pure Media Foundation only?
- MF is fast but weaker in edge-format coverage and subtitle flexibility; BP’s objective requires VLC-class support.

### 4.4 Data/storage
- **SQLite** for index, history, bookmarks, settings.
- File cache for artwork/thumbnails using deterministic hashed paths.

### 4.5 Dependency injection and modularity
- Use `Microsoft.Extensions.DependencyInjection` in app host.
- Modules behind interfaces (`IMediaEngine`, `ILibraryScanner`, `IProfileService`), enabling future plugin loading.

### 4.6 Plugin-ready design
- Internal plugin host contract:
  - discovery folder: `%ProgramFiles%/BP/plugins` and `%AppData%/BP/plugins`
  - manifest + versioned capability declarations
  - sandbox policy: no direct UI thread access, restricted APIs by capability

---

## 5) Windows Installer Strategy (.exe)

### 5.1 Installer tech
- **Inno Setup** generating single `BP-Setup-x64.exe`.
  - Mature, scriptable, reliable upgrades/uninstalls.
  - Supports VC++ runtime checks and dependency bootstrap.

### 5.2 Packaging approach
- Publish app as self-contained .NET runtime build (`win-x64`).
- Bundle native DLLs (libVLC binaries, codecs, DSP modules).
- Include signed binaries and installer (Authenticode).

### 5.3 Install behavior
- Per-machine default install to `C:\Program Files\BP`.
- Optional file association setup (`.mp4`, `.mkv`, `.mp3`, etc.).
- Start menu + desktop shortcuts configurable.
- Silent install flags for enterprise deployment.

### 5.4 Update strategy
- In-app updater service (future) downloads signed incremental package.
- MVP supports manual upgrade via newer installer over existing installation.

---

## 6) Recommended Folder Structure & Build Process

### 6.1 Repository layout

```text
bp/
├─ src/
│  ├─ BP.App/                     # WinUI 3 shell (C#)
│  ├─ BP.Application/             # use-cases/services
│  ├─ BP.Domain/                  # entities + interfaces
│  ├─ BP.Infrastructure/          # sqlite, file IO, metadata
│  ├─ BP.Media.Native/            # C++ media bridge
│  ├─ BP.Media.Interop/           # C# P/Invoke wrapper
│  └─ BP.Plugins.Abstractions/    # plugin contracts
├─ assets/
│  ├─ branding/
│  ├─ icons/
│  └─ themes/
├─ installer/
│  ├─ inno/
│  │  └─ bp.iss
│  └─ scripts/
├─ build/
│  ├─ Build.ps1
│  └─ Versioning.props
├─ tests/
│  ├─ BP.UnitTests/
│  ├─ BP.IntegrationTests/
│  └─ BP.Media.SmokeTests/
└─ docs/
   ├─ architecture/
   ├─ ux/
   └─ release/
```

### 6.2 Build pipeline (CI/CD)
1. Restore + compile C# and C++ projects.
2. Run unit tests and media smoke tests.
3. Publish self-contained app artifact.
4. Collect native media binaries.
5. Produce installer via Inno Setup CLI.
6. Sign binaries and installer.
7. Generate SBOM + checksums.

### 6.3 Quality gates
- Cold start target: <1.8s on mid-tier SSD system.
- 4K H.265 playback drop-frame threshold: <0.5% steady state.
- Memory cap targets:
  - Audio playback steady state: <250 MB
  - 4K video steady state: <600 MB

---

## 7) Development Roadmap, Risks, and Mitigations

### 7.1 Roadmap (realistic phases)

**Phase 0 — Foundations (2–3 weeks)**
- Solution scaffolding, DI, logging, settings, base UI shell.
- Integrate libVLC playback prototype.

**Phase 1 — Core Playback MVP (4–6 weeks)**
- Video/audio playback, subtitle tracks, seek, speed control.
- Resume persistence and basic profile system.

**Phase 2 — Library & UX polish (4–5 weeks)**
- Folder scan/indexing, filters/sorting, bookmarks + notes.
- Contextual controls, keyboard map, fullscreen behavior.

**Phase 3 — Performance & stability hardening (3–4 weeks)**
- Hardware decode optimization, seek accuracy tuning.
- Crash resilience, stress testing, memory leak checks.

**Phase 4 — Packaging & release (2 weeks)**
- Installer integration, signing, upgrade path validation.
- QA matrix on Windows 10/11 devices.

### 7.2 Primary risks and mitigation

1. **Codec/legal distribution complexity**
   - Mitigation: maintain explicit third-party license inventory and distribution policy; automate NOTICE generation.

2. **Hardware decode inconsistency across GPUs/drivers**
   - Mitigation: runtime capability probing + robust software fallback; telemetry logs for decode path diagnostics.

3. **Frame-accurate seek on VFR/high-bitrate media**
   - Mitigation: hybrid seek algorithm (keyframe seek + bounded decode-to-target) with configurable tolerance.

4. **UI responsiveness under heavy library scans**
   - Mitigation: background indexing queue, cancellation tokens, incremental UI updates.

5. **Corrupt media causing hangs/crashes**
   - Mitigation: watchdog timeouts around risky operations; defensive parser boundaries; crash dump pipeline.

6. **Installer false positives / SmartScreen friction**
   - Mitigation: EV code signing, reputation buildup, reproducible builds.

---

## Production-Readiness Definition (Exit Criteria)
- BP installs via a single signed `.exe` on Windows 10/11.
- Plays representative codec matrix with stable hardware acceleration.
- Recovers playback state after reboot without data loss.
- UI remains fluid during playback + library operations.
- No account, no ads, offline-first behavior validated.
