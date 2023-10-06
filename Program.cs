// Load input
List<Job> jobs = JobsLoader.ReadInput("./in/jobs.txt");

// Task 1
DependencyCrawler dc = new(jobs);
List<string> jobsWithError = new() { "WhiteCaravan", "GlassLurker", "IronSunburn", "LastSunburn", "KindSunburn" };
List<string> toRecompute = dc.FindDependencies(jobsWithError); 
Logger.Jobs(toRecompute);

// Task 2
Scheduler s = new(jobs);
List<ScheduleUnit> schedule = s.Schedule(toRecompute); 
Logger.Schedule(schedule);