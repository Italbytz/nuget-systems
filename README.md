# nuget-systems

[![Documentation](https://img.shields.io/badge/Documentation-GitHub%20Pages-blue?style=for-the-badge)](https://italbytz.github.io/nuget-systems/)

`nuget-systems` bundles the refactored `Italbytz.ComputingSystems.*`, `Italbytz.Networking.*`, and `Italbytz.OperatingSystems.*` package families.

It is intended for developers who need reusable solver contracts and implementations for teaching, exam preparation, and examples around binary arithmetic, networking, and operating systems.

## Which package should I use?

- Use `Italbytz.ComputingSystems.Abstractions` for contracts such as `IBinaryAdditionSolver`, `IBinaryToDecimalSolver`, `IDecimalToBinarySolver`, and `ITwosComplementSolver`.
- Use `Italbytz.ComputingSystems` for concrete solver implementations such as `BinaryAdditionSolver`, `BinaryToDecimalSolver`, `DecimalToBinarySolver`, and `TwosComplementSolver`.
- Use `Italbytz.Networking.Abstractions` for reusable contracts such as `IBitencodingSolver`, `ICRCSolver`, and `INetmaskSolver`.
- Use `Italbytz.Networking` for ready-to-use implementations like `BitencodingSolver`, `CRCSolver`, and `NetmaskSolver`.
- Use `Italbytz.Networking.Resources` when you need the extracted subnet and IP helper utilities behind the networking tasks.
- Use `Italbytz.OperatingSystems.Abstractions` for contracts such as `ISchedulingSolver`, `IPageReplacementSolver`, `IBuddySolver`, and `IRealtimeSchedulingSolver`.
- Use `Italbytz.OperatingSystems` for concrete implementations like `PrioritySchedulingSolver`, `RoundRobinSolver`, `FIFOSolver`, `LRUSolver`, `BuddySolver`, `EDFSolver`, and `RMSSolver`.
- Use `Italbytz.OperatingSystems.Resources` for the extracted simulation helpers behind page-replacement and buddy-allocation scenarios.

## Documentation

API documentation is generated with `docfx` and can be published via GitHub Pages:

- `https://italbytz.github.io/nuget-systems/`

The published site now also exposes an interactive Blazor demo host under:

- `https://italbytz.github.io/nuget-systems/demos/`

The doc site includes a solver catalog and an interactive demos guide that map common binary-arithmetic, networking, and operating-systems topics to the relevant solver classes and sample routes, including scheduling and buddy-allocation walkthroughs on the operating-systems page.

## Quality checks

This repository includes:

- a `GitHub Actions` workflow in `.github/workflows/ci.yml`
- automated `restore`, `build`, `test`, `pack`, docs generation, and demo publication
- a `docfx` setup under `docfx/`
- a Blazor WebAssembly sample host under `samples/Italbytz.Systems.Demos.Web`
- a `Makefile` for fast local feedback loops and GitHub Pages artifact assembly

## Release model

- the current `nuget-systems` line stays on `1.0.0-preview.*` as long as it still depends on preview-stage shared AI abstractions during the migration
- a pushed tag such as `v1.0.0-preview.1` triggers the release-ready pipeline in GitHub Actions
- if the repository secret `NUGET_API_KEY` is configured, the workflow also publishes `.nupkg` and `.snupkg` files to NuGet

## Local validation

```bash
make restore
make build
make test
make pack
make docs
make demo-watch
make pages-prepare
```
