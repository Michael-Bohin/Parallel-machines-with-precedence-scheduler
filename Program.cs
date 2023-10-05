
// Load input
JobsLoader jl = new();
jl.ReadInput();

// Task 1
Scheduler s = new(jl.jobs);
List<string> jobsWithError = new() { "WhiteCaravan", "GlassLurker", "IronSunburn", "LastSunburn", "KindSunburn" };
s.FindErrorDependencies(jobsWithError); 
s.LogErrorDependencies();

// Task 2
s.ScheduleRecompute(); 
s.LogResult_2();