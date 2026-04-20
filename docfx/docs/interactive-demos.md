# Interactive demos

The published documentation for `nuget-systems` now includes a Blazor WebAssembly demo host under `/demos/`.

The demo project lives in `samples/Italbytz.Systems.Demos.Web` and references the concrete package projects directly. That keeps the examples aligned with the code that is built, packed, and published.

## Included routes

- `/demos/` gives a package-family overview and links into the focused demo pages.
- `/demos/computing-systems` demonstrates binary addition, decimal conversion, and two's-complement output with `Italbytz.ComputingSystems`.
- `/demos/networking` demonstrates line encoding, CRC calculation, and subnet decomposition with `Italbytz.Networking`.
- `/demos/operating-systems` demonstrates page replacement traces, CPU scheduling comparisons, and buddy-allocation history with `Italbytz.OperatingSystems`.

## Local workflow

Use the repository `Makefile` for the common feedback loop:

```bash
make restore
make demo-watch
make docs
make pages-prepare
make pages-serve
```

`make pages-prepare` builds the combined GitHub Pages artifact in `artifacts/pages`, with the DocFX site at the root and the Blazor demo host under `demos/`.
