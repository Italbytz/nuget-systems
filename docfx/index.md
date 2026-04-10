# nuget-systems

`nuget-systems` is the target repository for refactored `Italbytz.ComputingSystems.*`, `Italbytz.Networking.*`, and `Italbytz.OperatingSystems.*` packages.

## Current Phase 3 slices

- `Italbytz.ComputingSystems.Abstractions`
- `Italbytz.ComputingSystems`
- `Italbytz.Networking.Abstractions`
- `Italbytz.Networking`
- `Italbytz.Networking.Resources`

The migrated building blocks now cover binary addition, binary-to-decimal conversion, decimal-to-binary conversion, two's-complement tasks, line encoding, CRC calculation, and subnetting helpers.
## Local validation

```bash
dotnet restore nuget-systems.sln
dotnet test nuget-systems.sln -v minimal
dotnet pack nuget-systems.sln -c Release -v minimal
dotnet tool restore
dotnet tool run docfx docfx/docfx.json
```
