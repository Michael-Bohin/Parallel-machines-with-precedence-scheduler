using System.Text;

class Job {
	public readonly string name;
	public readonly int duration;
	public readonly List<string> dependencies;

	public Job(string name, int duration, List<string> dependencies) {
		this.name = name;
		this.duration = duration;
		this.dependencies = dependencies;

		// @ Defensive programming:
		// 1. assert duration is not negative 
		// 2. assert all names, including dependencies are nonempty strings

		if(duration < 0) {
			throw new ArgumentException($"Job.Ctor() : Duration must not be negative! Duration: {duration}, Job name: {name}");
		}

		if(name == "") {
			throw new ArgumentException($"Job.Ctor() : Job name cannot be empty string! Duration: {duration}");
		}

		foreach(string d in dependencies) {
			if(d == "") {
				throw new ArgumentException($"Job.Ctor() : One of the jobs dependenceis is empty string. Duration: {duration}, Job name: {name}");
			}
		}

	}

	public override string ToString() {
		StringBuilder sb = new();
		sb.Append($"Job: {name}, {duration}, depends on:");
		foreach(string d in dependencies) {
			sb.Append($" {d}");
		}
		return sb.ToString();
	}
}

