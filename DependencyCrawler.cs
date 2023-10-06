using System.Security.Cryptography.X509Certificates;

class DependencyCrawler {
	public readonly List<Job> jobs;

	// Task 1 results
	List<string> allDependencies = new();
	Dictionary<string, List<string>> jobsMap = new();

	public DependencyCrawler(List<Job> jobs) {
		this.jobs = jobs;
	}

	public List<string> FindDependencies(List<string> jobsWithError) {
		// use different structure then from input: List -> Dictionary
		foreach (Job job in jobs) {
			jobsMap[job.name] = job.dependencies;
		}

		// search all unique dependencies
		foreach (string job in jobsWithError) {
			if (!allDependencies.Contains(job)) {
				allDependencies.Add(job);
			}
			GetAllDependencies(job);
		}

		return allDependencies;
	}

	void GetAllDependencies(string name) {
		foreach (string dependency in jobsMap[name]) {
			// only recurse and add to list, if the dependency has not been added yet:
			if (!allDependencies.Contains(dependency)) {
				allDependencies.Add(dependency);
				GetAllDependencies(dependency);
			}
		}
	}
}

