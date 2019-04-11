# DifferencesSearch - Library for .NET 
DifferencesSearch is a library for finding differences between two objects using reflection.

## Usage examples

### Custom search:
```c#
// STEP 1: Initialize difference controller.
DifferenceController differenceController = new DifferenceController();

// STEP 2: Set up custom search with builder.
differenceController.CustomBuilder<People>()
    .All(x => x)
    .One(x => x.SweetHome.Address)
    .One(x => x.SweetHome.RoomNumber)
    .Build();
    
People p1 = /* ... */;
People p2 = /* ... */;

// STEP 3: Get objects differences.
PropertyDifference[] differences = differenceController.GetCustomDifferences(p1, p2);
```

### Auto search:
```c#
// STEP 1: Initialize difference controller.
DifferenceController differenceController = new DifferenceController();

// STEP 2: Set up auto search with builder (you can skip this step).
differenceController.AutoBuilder<People>()
    .All(x => x)
    .One(x => x.SweetHome.Address)
    .One(x => x.SweetHome.RoomNumber)
    .Build();
    
People p1 = /* ... */;
People p2 = /* ... */;

// STEP 3: Get objects differences.
PropertyDifference[] differences = differenceController.GetAutoDifferences(p1, p2);
```
