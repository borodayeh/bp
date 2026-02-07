PRAGMA journal_mode=WAL;
PRAGMA foreign_keys=ON;

CREATE TABLE IF NOT EXISTS media_items (
    id TEXT PRIMARY KEY,
    file_path TEXT NOT NULL,
    file_path_hash TEXT NOT NULL UNIQUE,
    title TEXT NOT NULL,
    duration_ms INTEGER NOT NULL,
    container TEXT NOT NULL,
    video_codec TEXT,
    audio_codec TEXT,
    has_video INTEGER NOT NULL,
    has_audio INTEGER NOT NULL,
    added_at TEXT NOT NULL,
    last_played_at TEXT
);

CREATE TABLE IF NOT EXISTS playback_states (
    media_id TEXT PRIMARY KEY,
    position_ms INTEGER NOT NULL,
    last_stable_keyframe_ms INTEGER NOT NULL,
    speed REAL NOT NULL,
    subtitle_track TEXT,
    audio_track TEXT,
    updated_at TEXT NOT NULL,
    FOREIGN KEY(media_id) REFERENCES media_items(id) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS bookmarks (
    id TEXT PRIMARY KEY,
    media_id TEXT NOT NULL,
    timestamp_ms INTEGER NOT NULL,
    note TEXT,
    created_at TEXT NOT NULL,
    FOREIGN KEY(media_id) REFERENCES media_items(id) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS profiles (
    id TEXT PRIMARY KEY,
    name TEXT NOT NULL UNIQUE,
    normalization_target_lufs REAL NOT NULL,
    dialogue_enhancement_enabled INTEGER NOT NULL,
    bass_gain_db REAL NOT NULL,
    created_at TEXT NOT NULL
);

CREATE TABLE IF NOT EXISTS session_events (
    id TEXT PRIMARY KEY,
    type TEXT NOT NULL,
    media_id TEXT,
    timestamp TEXT NOT NULL,
    payload TEXT NOT NULL
);
