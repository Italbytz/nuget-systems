# nuget-systems

`nuget-systems` bundles the refactored `Italbytz.ComputingSystems.*`, `Italbytz.Networking.*`, and `Italbytz.OperatingSystems.*` package families.

It is intended for developers who need reusable solver contracts and implementations for teaching, exam preparation, and examples around binary arithmetic, networking, and operating systems.

## Current migration status

The first Phase 3 wave now includes:

- `Italbytz.ComputingSystems.Abstractions`
- `Italbytz.ComputingSystems`

The next waves will extend the repository with `Italbytz.Networking.*` and `Italbytz.OperatingSystems.*`.

## Which package should I use?

- Use `Italbytz.ComputingSystems.Abstractions` for contracts such as `IBinaryAdditionSolver`, `IBinaryToDecimalSolver`, `IDecimalToBinarySolver`, and `ITwosComplementSolver`.
- Use `Italbytz.ComputingSystems` for the first concrete solver implementations such as `BinaryAdditionSolver`, `BinaryToDecimalSolver`, `DecimalToBinarySolver`, and `TwosComplementSolver`.

## Migration notice

Older repositories and articles may still refer to names such as:

- `Italbytz.Ports.Exam.ComputingSystems`
- `Italbytz.Adapters.Exam.ComputingSystems`
- `nuget-ports-exam-computing-systems`
- `nuget-adapters-exam-computing-systems`

For all new development, please use the new `Italbytz.ComputingSystems.*` package names.

## Documentation

API documentation is generated with `docfx` and can be published via GitHub Pages:

- `https://italbytz.github.io/nuget-systems/`

## Quality checks

This repository includes:

- a `GitHub Actions` workflow in `.github/workflows/ci.yml`
- automated `restore`, `build`, `test`, `pack`, and docs generation
- a `docfx` setup under `docfx/`

## Local validation

```bash
dotnet restore nuget-systems.sln
dotnet test nuget-systems.sln -v minimal
dotnet pack nuget-systems.sln -c Release -v minimal
dotnet tool restore
dotnet tool run docfx docfx/docfx.json
```