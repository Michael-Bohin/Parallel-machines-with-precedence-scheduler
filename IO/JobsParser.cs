﻿class JobsParser {
	readonly BidirectionalDictionary dict = new();
	int id = 0;

	public List<Job> ReadFile(string file) {
		string[] jobsFile = File.ReadAllLines($"./../../../in/{file}.txt");
		List<Job> jobs = new();
		List<List<string>> predecessors = new();

		foreach (string line in jobsFile) {
			(Job job, List<string> pred) = ParseJobLine(line);
			jobs.Add(job);
			predecessors.Add(pred);
		}

		for (int i = 0; i < jobs.Count; i++) {
			foreach (string pred in predecessors[i]) {
				int predId = dict.NameToId(pred);

				// this check is necessary, because large sample size contains jobs with duplicit dependencies
				if (!jobs[i].ContainsPredecessor(predId)) {
					jobs[i].AddPredecessor(predId);
					jobs[predId].AddSuccessor(i);
				}
			}
		}
#if DEBUG
		AssertCorrectnes(jobs);
#endif
		return jobs;
	}

	public List<string> JobsToString() => dict.GetIdToNameList();

	public HashSet<int> NamesToIds(List<string> names) {
		HashSet<int> ids = new();
		foreach (string name in names) {
			int id = dict.NameToId(name);
			ids.Add(id);
		}
		return ids;
	}

	(Job job, List<string> predecessors) ParseJobLine(string line) {
		string[] lineItems = line.Split('\t');

		if(lineItems.Length < 2 ) { 
			throw new ArgumentException($"JobsLoader.ParseJobLine() : Some input line contains too few arguments for a job. At least two argument are nessecary.");	
		}

		string name = lineItems[0].Trim();
		int id = ParseId(name);
		dict.Add(id, name);

		int duration = ParseDuration(lineItems[1]);
		Job job = new(id, duration);

		List<string> predecessors = ReadDependencies(lineItems);

		return (job, predecessors);
	}

	int ParseId(string name) {
		if (name.Length == 0) {
			throw new ArgumentException("JobsLoader.ParseName() : Trimmed name is an empty string. Name has to contain at least one nonwhitespace character.");
		}
		return id++; // increment after returning value
	}

	static int ParseDuration(string strDuration) {
		bool success = int.TryParse(strDuration, out int duration);
		if (!success) {
			throw new ArgumentException($"JobsLoader.ParseDuration() : Cannot parse duration argument into an int type. Failed string: {strDuration}.");
		}
		return duration;
	}

	static List<string> ReadDependencies(string[] lineItems) {
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

#if DEBUG
	static void AssertCorrectnes(List<Job> jobs) {
		// 1. All jobs are at the index of their name
		// 2. Foreach predecessor record, counterpart successor record exists
		// 3. Foreach successor record, counterpart predecessor record exists
		for (int i = 0; i < jobs.Count; i++) {
			if(i != jobs[i].id) {
				throw new ArgumentException($"JobsLoader.AssertCorrectnes() : Some job is not at the index of it's name. Index: {i}, name: {jobs[i].id}.");
			}
		}

		foreach(Job job in jobs) {
			foreach(int pred in job.predecessors) {
				if (!jobs[pred].ContainsSuccessor(job.id)) {
					throw new ArgumentException($"JobsLoader.AssertCorrectnes() : Found predecessor record that does not have counterpart successor record. Job name: {job.id}, predecessor: {pred}, successor name: {jobs[pred]}.");
				}
			}
		}

		foreach(Job job in jobs) {
			foreach(int succ in job.successors) {
				if (!jobs[succ].ContainsPredecessor(job.id)) {
					throw new ArgumentException($"JobsLoader.AssertCorrectnes() : Found successor record that does not have counterpart predecessor record. Job name: {job.id}, successor: {succ}, predecessor name: {jobs[succ]}.");
				}
			}
		}
	}
#endif
}