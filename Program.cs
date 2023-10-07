string[] inputFileNames = { "smallSample", "mediumSample" };
List<string> jobsWithError = new() { "WhiteCaravan", "GlassLurker", "IronSunburn", "LastSunburn", "KindSunburn" };

foreach (string file in inputFileNames) {
	// Read & parse input
	JobsParser parser = new();
	List<Job> jobs = parser.ReadFile(file);
	List<int> idsWithError = parser.NamesToIds(jobsWithError);
	List<string> idToName = parser.JobsToString(); // Classes DependencyCrawler and Scheduler operate over renamed jobs as int id. Only class Logger needs to know the true name of jobs as string type.
	Logger logger = new(idToName);
	logger.Jobs(jobs, file);

	// Assert dependencies graph is acyclic
	TopologicalSort ts = new(jobs);
	ts.AssertDAG();
	logger.TopSort(ts, file);

	// Task 1
	DependencyCrawler dc = new(jobs);
	List<int> toRecompute = dc.JobsToRecompute(idsWithError);
	logger.ToRecompute(toRecompute, file);

	// Task 2
	/*Scheduler s = new(jobs);
	List<ScheduleUnit> schedule = s.Schedule(toRecompute);
	logger.Schedule(schedule);*/
}