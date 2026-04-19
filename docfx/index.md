# nuget-systems

`nuget-systems` provides reusable solver packages for computing systems, networking, and operating systems topics.

This documentation is intended for package consumers who need reusable abstractions and implementations for teaching, exam preparation, and example applications.

## Packages at a glance

- `Italbytz.ComputingSystems.Abstractions`
- `Italbytz.ComputingSystems`
- `Italbytz.Networking.Abstractions`
- `Italbytz.Networking`
- `Italbytz.Networking.Resources`
- `Italbytz.OperatingSystems.Abstractions`
- `Italbytz.OperatingSystems`
- `Italbytz.OperatingSystems.Resources`

The migrated building blocks now cover binary addition, binary-to-decimal conversion, decimal-to-binary conversion, two's-complement tasks, line encoding, CRC calculation, subnetting helpers, CPU scheduling, page replacement, buddy allocation, and realtime scheduling scenarios.

## Guide

Use `Guides > Solver catalog` for a quick map from teaching or exam topics to the concrete solver classes that now live in this repo.

## Use nuget-systems if you want to

- reuse solver abstractions and implementations across learning or assessment scenarios
- map topic-oriented exercises to concrete solver classes
- keep computing systems, networking, and operating systems helpers in one package family

## Local validation

```bash
dotnet restore nuget-systems.sln
dotnet test nuget-systems.sln -v minimal
dotnet pack nuget-systems.sln -c Release -v minimal
dotnet tool restore
dotnet tool run docfx docfx/docfx.json
```
