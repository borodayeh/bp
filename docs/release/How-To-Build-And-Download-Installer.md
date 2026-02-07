# Build and Download BP Installer (.exe)

## Fastest path (GitHub Actions)
1. Push your branch.
2. Open **Actions → build-windows → Run workflow**.
3. After success, download artifacts:
   - `BP-win-x64-portable` (self-contained app files)
   - `BP-Setup-x64` (installer `.exe`)

## Local Windows build
Requirements:
- .NET SDK 8
- Inno Setup (`iscc` in PATH)

Run:
```powershell
./build/Build.ps1 -Configuration Release -Runtime win-x64 -Version 0.1.0
```

Outputs:
- `artifacts/win-x64/BP.App.exe` (portable app executable)
- `artifacts/BP-Setup-x64.exe` (installer executable)
