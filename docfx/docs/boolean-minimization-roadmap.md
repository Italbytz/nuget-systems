# Boolean Minimization Roadmap

This roadmap defines the missing digital-logic solver slice for `Italbytz.ComputingSystems` so the package can support both static teaching material and dynamic demo consumers without depending on browser-only JavaScript code.

## Goal

Build one reusable C# pipeline for boolean-function analysis and minimization:

1. truth table input
2. canonical DNF and CNF generation
3. Karnaugh-map layout and grouping
4. Quine-McCluskey minimization
5. Petrick-method selection for cyclic coverings

## Existing anchors

- `ISDCompanion.Shared/Pages/NormalForm.razor.js`
- `ISDCompanion.Shared/Pages/KVMap.razor.js`
- `ISDCompanion.Shared/Pages/QMCAlgorithm.razor.js`

These implementations are useful as behavioral references, but the algorithmic core should live in `nuget-systems` as package code.

## Recommended slice order

1. `NormalFormSolver`
   - truth table to DNF and CNF
   - row-by-row trace output
2. `KarnaughMapSolver`
   - Gray-code layout
   - cell mapping, wrap-around grouping candidates
3. `QuineMcCluskeySolver`
   - implicant grouping and combination trace
   - prime implicants and essential prime implicants
4. `PetrickSolver`
   - minimal covering selection for cyclic remainder problems
5. expression parser and equivalence helpers
   - formula to truth table
   - expression equivalence checks

## First implemented slice

`NormalFormSolver` is the first C# slice in the package.

It provides:

- `INormalFormSolver`
- `INormalFormParameters`
- `INormalFormSolution`
- truth-table based canonical DNF and CNF generation
- trace output via `ITracedSolution`

## Design constraints

- keep the solver core independent from UI or SVG concerns
- keep trace output deterministic for tests and generated exercises
- use ASCII-safe operator strings in package output (`!`, `&`, `|`)
- preserve compatibility with future KV and QMC slices by reusing one shared truth-table model