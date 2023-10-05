using static System.Console;

class JobsLoader {
	public List<Job> jobs = new();

	public void ReadInput() {
		string[] jobsFile = File.ReadAllLines("./in/jobs.txt");

		foreach(string line in jobsFile) { 
			Job j = ParseJobLine(line);
			jobs.Add(j);
		}

		LogStats();
	}

	Job ParseJobLine(string line) {
		string[] lineItems = line.Split('\t');

		if(lineItems.Length < 2 ) { 
			throw new ArgumentException($"JobsLoader.ParseJobLine() : Some input line contains to few arguments for a job. At least two argument are nessecary.");	
		}

		string name = lineItems[0];

		bool success = int.TryParse(lineItems[1], out int duration);
		if (!success) {
			throw new ArgumentException($"JobsLoader.ParseJobLine() : Cannot parse duration argument into an int type! Failed string: {lineItems[1]}");
		} 

		List<string> dependencies = new();
		for(int i = 2; i < lineItems.Length; i++) {
			dependencies.Add(lineItems[i]);
		}

		return new(name, duration, dependencies);
	}

	void LogStats() {
		foreach(Job job in jobs) { 
			WriteLine($"{job}");	
		}

		WriteLine($"\nTotal jobs loaded: {jobs.Count}");
	}
}

