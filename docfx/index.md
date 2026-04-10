# nuget-systems

`nuget-systems` is the target repository for refactored `Italbytz.ComputingSystems.*`, `Italbytz.Networking.*`, and `Italbytz.OperatingSystems.*` packages.

## Current Phase 3 slice

- `Italbytz.ComputingSystems.Abstractions`
- `Italbytz.ComputingSystems`

The first migrated building blocks cover binary addition, binary-to-decimal conversion, decimal-to-binary conversion, and two's-complement tasks.
## Local validation

```bash
dotnet restore nuget-systems.sln
dotnet test nuget-systems.sln -v minimal
dotnet pack nuget-systems.sln -c Release -v minimal
dotnet tool restore
dotnet tool run docfx docfx/docfx.json
```
