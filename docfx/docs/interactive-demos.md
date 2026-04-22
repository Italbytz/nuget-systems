# Interactive demos

The published documentation for `nuget-systems` now includes a Blazor WebAssembly demo host under `/demos/`.

The demo project lives in `samples/Italbytz.Systems.Demos.Web` and references the concrete package projects directly. That keeps the examples aligned with the code that is built, packed, and published.

## Included routes

- `/demos/` gives a package-family overview and links into the focused demo pages.
- `/demos/computing-systems` demonstrates binary addition, decimal conversion, and two's-complement output with `Italbytz.ComputingSystems`, including algorithm traces for static exercise walkthroughs and dynamic demonstrations.
- `/demos/computing-systems` is also the natural place to surface future boolean-minimization slices such as canonical DNF/CNF, Karnaugh maps, and Quine-McCluskey once those package solvers are added.
- `/demos/networking` demonstrates line encoding, CRC calculation, and subnet decomposition with `Italbytz.Networking`, including per-solver trace walkthroughs.
- `/demos/operating-systems` demonstrates page replacement traces, CPU scheduling comparisons with solver-step traces, realtime scheduling timelines with RMS-vs-EDF comparison and deadline-miss markers, and buddy-allocation history with `Italbytz.OperatingSystems`.

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
