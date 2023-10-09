string[] inputFileNames = { "smallSample", "mediumSample" };
List<string> jobsWithError = new() { "WhiteCaravan", "GlassLurker", "IronSunburn", "LastSunburn", "KindSunburn" };
const int machineCount = 5;

foreach (string file in inputFileNames) {
	// Read & parse input
	JobsParser parser = new();
	List<Job> jobs = parser.ReadFile(file);
	HashSet<int> idsWithError = parser.NamesToIds(jobsWithError);
	List<string> idToName = parser.JobsToString(); // Classes DependencyCrawler and Scheduler operate over renamed jobs as int id. Only class Logger needs to know the true name of jobs as string type.
	Logger logger = new(idToName);
	logger.Jobs(jobs, file);

	// Assert dependencies graph is acyclic
	TopologicalSort ts = new(jobs);
	ts.AssertDAG();
	logger.TopSort(ts, file);

	// Task 1
	DependencyCrawler dc = new(jobs);
	HashSet<int> toRecompute = dc.JobsToRecompute(idsWithError);
	logger.ToRecompute(toRecompute, file);

	// Task 2
	List<Scheduler> schedulers = new() {
		new TSL(jobs, machineCount),
		new LPTsources(jobs, machineCount),
		new LPTsourcePathLayers(jobs, machineCount)
	};

	foreach(Scheduler s in schedulers) {
		HashSet<int> copy = new();
		foreach(int id in toRecompute) {
			copy.Add(id);
		}
		List<ScheduleUnit> schedule = s.Schedule(copy);
		logger.Schedule(schedule, file, s.AlgoName, s.Makespan, s.TotalJobsDuration, s.MakespanArea, s.MachinesUsage);
	}

	Console.WriteLine();
}