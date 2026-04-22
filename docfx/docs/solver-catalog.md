# Solver catalog

This guide gives a quick map from common teaching, exercise, or exam topics to the solver classes in `nuget-systems`.

For a browser-based walkthrough, pair this catalog with the `Interactive demos` guide.

## Computing systems

| Topic | Contracts | Implementations |
| --- | --- | --- |
| Binary addition | `IBinaryAdditionSolver` | `BinaryAdditionSolver` |
| Binary to decimal conversion | `IBinaryToDecimalSolver` | `BinaryToDecimalSolver` |
| Decimal to binary conversion | `IDecimalToBinarySolver` | `DecimalToBinarySolver` |
| Two's complement | `ITwosComplementSolver` | `TwosComplementSolver` |
| Canonical boolean normal forms | `INormalFormSolver` | `NormalFormSolver` |
| Karnaugh map layout and groups | `IKarnaughMapSolver` | `KarnaughMapSolver` |
| Quine-McCluskey minimization | `IQuineMcCluskeySolver` | `QuineMcCluskeySolver` |

Use `Italbytz.ComputingSystems.Abstractions` when you only need the contracts, and `Italbytz.ComputingSystems` when you want the working implementations.

The computing solutions now also expose `Steps` collections to make intermediate results explicit for exam preparation, generated tasks, and trace-based demos.

## Cross-domain trace contract

Use `Italbytz.Systems.Abstractions` when you need one shared trace shape across domains.

- `ITracedSolution` defines a common `Steps` collection and is implemented by computing, networking, and scheduling solution contracts.

## Networking

| Topic | Contracts | Implementations |
| --- | --- | --- |
| Line or bit encoding | `IBitencodingSolver` | `BitencodingSolver` |
| CRC calculation | `ICRCSolver` | `CRCSolver` |
| Subnet masks and addressing | `INetmaskSolver` | `NetmaskSolver` |

`Italbytz.Networking.Resources` contains the extracted subnet and IP helper logic used by the networking slice.

## Operating systems

| Topic | Contracts | Implementations |
| --- | --- | --- |
| CPU scheduling | `ISchedulingSolver` | `FCFSSolver`, `PrioritySchedulingSolver`, `ShortestJobFirstSolver`, `RoundRobinSolver` |
| Page replacement | `IPageReplacementSolver` | `FIFOSolver`, `LRUSolver`, `ClockSolver`, `OptimalSolver` |
| Buddy allocation | `IBuddySolver` | `BuddySolver` |
| Realtime scheduling | `IRealtimeSchedulingSolver` | `EDFSolver`, `RMSSolver` |

`Italbytz.OperatingSystems.Resources` contains the simulation-oriented helpers that support the operating-systems exercises.

## Demo host routes

- `/demos/` for the overview and package map
- `/demos/computing-systems` for binary arithmetic and conversion examples
- `/demos/networking` for bit encoding, CRC, and subnetting examples
- `/demos/operating-systems` for page replacement, scheduling, realtime scheduling, and buddy-allocation simulations

## Historical mapping

If you come from the older repository layout, the following rule helps:

- former `Ports.Exam.*` packages now map to the corresponding `*.Abstractions` packages
- former `Adapters.Exam.*` packages now map to the domain implementation packages
- former `Infrastructure.Exam.*` packages now map to `*.Resources`

That keeps the same teaching content, but presents it under clearer domain names.