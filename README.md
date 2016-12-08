# What is NaturalShift?
NaturalShift is a .NET library useful to compute workshifts. Computation is based on genetic algoritms. Number of days, shifts and shifters can be configured, together with many other contraints and directives.

# Quick example
The following code starts computation of a workshift scheme 30 days long, with 14 shifts and 18 employees. The computation lasts 60 seconds.

```C#
var problem = ProblemBuilder.Configure()
  .WithDays(30)
  .WithSlots(14)
  .WithItems(18)
  .WithMaxConsecutiveWorkingDaysEqualTo(5)
  .RestAfterMaxWorkingDaysReached(2)
  .Build();
  
var solvingEnvironment = SolvingEnvironmentBuilder.Configure()
  .ForProblem(problem)
  .WithPopulationSize(100)
  .RenewingPopulationAfterSameFitnessEpochs(10)
  .StoppingComputationAfter(60).Seconds
  .Build();

var solution = solvingEnvironment.Solve(); //takes about 60 seconds

Console.WriteLine(solution);
```

The best solution found during computation is printed to the console.

Too tricky? Read the [wiki](https://github.com/supix/NaturalShift/wiki)! :-)
Wiki clearly explains the concepts behind this library and all its features.

#License
NaturalShift is released under the terms of AGPL-3.0 license. This library is Free Software because of [this](https://www.youtube.com/watch?v=DjqGvUcPDZs).

#Author
Write me at esposito.marce@gmail.com
