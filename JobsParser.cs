class JobsParser {
	readonly List<string> jobsToString = new();
	readonly Dictionary<string, int> jobsToInt = new();
	int jobIntName = 0;

	public List<Job> ReadFile(string file) {
		string[] jobsFile = File.ReadAllLines($"./../../../in/{file}.txt");

		List<IntermediateJob> intermediateJobs = new();
		List<Job> jobs = new();

		foreach(string line in jobsFile) {
			IntermediateJob ij = ParseJobLine(line);
			intermediateJobs.Add(ij);
			Job j = ij.CopyJob();
			jobs.Add(j);
		}

		// All predecessor dependencies are created, add all coresponding successors records
		SetDependencyRecords(intermediateJobs, jobs);
		AssertCorrectnes(jobs);

		return jobs;
	}

	public List<string> JobsToString() => jobsToString;

	IntermediateJob ParseJobLine(string line) {
		string[] lineItems = line.Split('\t');

		if(lineItems.Length < 2 ) { 
			throw new ArgumentException($"JobsLoader.ParseJobLine() : Some input line contains too few arguments for a job. At least two argument are nessecary.");	
		}

		int name = ParseName(lineItems[0]);
		int duration = ParseDuration(lineItems[1]);
		List<string> predecessors = ReadDependencies(lineItems);

		return new(name, duration, predecessors);
	}

	int ParseName(string name) {
		string trimmedName = name.Trim();
		if( trimmedName.Length == 0 ) {
			throw new ArgumentException("JobsLoader.ParseName() : Trimmed name is an empty string. Name has to contain at least one nonwhitespace character.");
		}
		jobsToString.Add(trimmedName);
		jobsToInt[trimmedName] = jobIntName;
		return jobIntName++; // increment after returning value
	}

	static int ParseDuration(string strDuration) {
		bool success = int.TryParse(strDuration, out int duration);
		if (!success) {
			throw new ArgumentException($"JobsLoader.ParseDuration() : Cannot parse duration argument into an int type. Failed string: {strDuration}.");
		}
		return duration;
	}

	List<string> ReadDependencies(string[] lineItems) {
		List<string> dependencies = new();
		for (int i = 2; i < lineItems.Length; i++) {
			string dependency = lineItems[i].Trim();
			if (dependency.Length == 0) {
				throw new ArgumentException($"JobsLoader.ReadDependencies() : Dependency after trimming is empty string. Failed string: {lineItems[i]}, at index: {i}.");
			}
			dependencies.Add(dependency);
		}
		return dependencies;
	}

	void SetDependencyRecords(List<IntermediateJob> intermediateJobs, List<Job> jobs) {
		foreach(IntermediateJob ij in intermediateJobs) {
			foreach(string dependency in ij.dependencies) {
				int pred = jobsToInt[dependency];
				int name = ij.name;

				// this check is necessary, because large sample size contains jobs with duplicit dependencies
				if (!jobs[name].ContainsPredecessor(pred)) {
					// set as predecessor:
					jobs[name].AddPredecessor(pred);

					// set as successor:
					jobs[pred].AddSuccessor(name);
				}
				 
			}
		}
	}

	void AssertCorrectnes(List<Job> jobs) {
		// 1. All jobs are at the index of their name
		// 2. Foreach predecessor record, counterpart successor record exists
		// 3. Foreach successor record, counterpart predecessor record exists
		// 4. Dependcies graph is DAG (directed acyclic graph)

		for (int i = 0; i < jobs.Count; i++) {
			if(i != jobs[i].name) {
				throw new ArgumentException($"JobsLoader.AssertCorrectnes() : Some job is not at the index of it's name. Index: {i}, name: {jobs[i].name}.");
			}
		}

		foreach(Job job in jobs) {
			foreach(int pred in job.predecessors) {
				if (!jobs[pred].ContainsSuccessor(job.name)) {
					throw new ArgumentException($"JobsLoader.AssertCorrectnes() : Found predecessor record that does not have counterpart successor record. Job name: {job.name}, predecessor: {pred}, successor name: {jobs[pred]}.");
				}
			}
		}

		foreach(Job job in jobs) {
			foreach(int succ in job.successors) {
				if (!jobs[succ].ContainsPredecessor(job.name)) {
					throw new ArgumentException($"JobsLoader.AssertCorrectnes() : Found successor record that does not have counterpart predecessor record. Job name: {job.name}, successor: {succ}, predecessor name: {jobs[succ]}.");
				}
			}
		}

		AssertDAG(jobs);
	}

	void AssertDAG(List<Job> jobs) {

	}
}