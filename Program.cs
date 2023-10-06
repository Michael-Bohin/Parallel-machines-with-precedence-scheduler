string[] inputFileNames = { "smallSample", "jobs" };
List<string> jobsWithError = new() { "WhiteCaravan", "GlassLurker", "IronSunburn", "LastSunburn", "KindSunburn" };

foreach (string file in inputFileNames) {
	// Read & parse input
	JobsParser parser = new();
	List<Job> jobs = parser.ReadFile(file);
	List<string> jobsToString = parser.JobsToString(); // Classes DependencyCrawler and Scheduler operate over renamed jobs as ints. Only class Logger needs to know the true name of jobs as string type.
	Logger logger = new(jobsToString);
	logger.Jobs(jobs, file);

	// Task 1
	/*DependencyCrawler dc = new(jobs);
	List<int> toRecompute = dc.FindDependencies(jobsWithError);
	logger.ToRecompute(toRecompute);

	// Task 2
	Scheduler s = new(jobs);
	List<ScheduleUnit> schedule = s.Schedule(toRecompute);
	logger.Schedule(schedule);*/
}