codex/create-bp-architecture-and-delivery-specifications-jcqw5n
#ifndef MyAppVersion
  #define MyAppVersion "0.1.0"
#endif

#ifndef SourceDir
  #define SourceDir "..\\..\\artifacts\\win-x64"
#endif

#define MyAppName "BP"
=======
#define MyAppName "BP"
#define MyAppVersion "0.1.0"
main
#define MyAppPublisher "Borod"
#define MyAppExeName "BP.App.exe"

[Setup]
AppId={{6A463D52-D4FA-4603-AF2A-E8A1F7B5EC1D}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppPublisher={#MyAppPublisher}
DefaultDirName={autopf}\BP
DefaultGroupName=BP
OutputBaseFilename=BP-Setup-x64
Compression=lzma
SolidCompression=yes
ArchitecturesInstallIn64BitMode=x64

[Files]
codex/create-bp-architecture-and-delivery-specifications-jcqw5n
Source: "{#SourceDir}\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs
=======
Source: "..\..\artifacts\win-x64\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs
main

[Tasks]
Name: "desktopicon"; Description: "Create desktop icon"; GroupDescription: "Additional icons:";

[Icons]
Name: "{group}\BP"; Filename: "{app}\{#MyAppExeName}"
Name: "{autodesktop}\BP"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "Launch BP"; Flags: nowait postinstall skipifsilent
