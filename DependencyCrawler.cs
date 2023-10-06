/*
class DependencyCrawler {
	public readonly List<Job> jobs;

	// Task 1 results
	List<string> allDependencies = new();
	Dictionary<string, List<string>> predecessors = new();

	public DependencyCrawler(List<Job> jobs) {
		this.jobs = jobs;
	}

	public List<int> FindDependencies(List<string> jobsWithError) {
		// use different structure then from input: List -> Dictionary
		foreach (Job job in jobs) {
			predecessors[job.name] = job.dependencies;
		}

		// search all unique dependencies
		foreach (string job in jobsWithError) {
			if (!allDependencies.Contains(job)) {
				allDependencies.Add(job);
			}
			GetAllDependencies(job, predecessors);
		}

		// now search all jobs that depend on jobs with error
		// first, get jobs map with reversed edges, so that we can apply recursion used for predecessors
		Dictionary<string, List<string>> successors = new();
		foreach (Job job in jobs) {
			successors[job.name] = new();
		}


		foreach (Job job in jobs) {
			foreach(string dependency in job.dependencies) {
				successors[dependency].Add(job.name);
			}
		}

		// second, now traverse all secors using same recursion function as for predecessors:
		// search all unique dependencies
		foreach (string job in jobsWithError) {
			if (!allDependencies.Contains(job)) {
				allDependencies.Add(job);
			}
			GetAllDependencies(job, successors);
		}


		return allDependencies;
	}

	void GetAllDependencies(string name, Dictionary<string, List<string>> edges) {
		foreach (string dependency in edges[name]) {
			// only recurse and add to list, if the dependency has not been added yet:
			if (!allDependencies.Contains(dependency)) {
				allDependencies.Add(dependency);
				GetAllDependencies(dependency, edges);
			}
		}
	}
}
*/