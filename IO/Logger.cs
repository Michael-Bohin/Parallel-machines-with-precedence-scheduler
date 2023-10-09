using System.Text;
using static System.Console;

class Logger {
	public List<string> idToName;

	public Logger(List<string> idToName) {
		this.idToName = idToName;
	}

	static void WriteFile(string content, string folderName, string fileName) {
		string folderPath = $"./out/{folderName}/";
		if (!Directory.Exists(folderPath)) {
			Directory.CreateDirectory(folderPath);
		}

		using StreamWriter sw = new($"{folderPath}{fileName}.txt");
		sw.WriteLine($"{content}");
		sw.Close();
	}

	public void Jobs(List<Job> jobs, string folderName) {
		StringBuilder sb = new();
		foreach (Job job in jobs) {
			sb.AppendLine($"{job.ToString(idToName)}");
			sb.AppendLine($"{job}");
			sb.AppendLine();
		}

		string total = $"Loaded {jobs.Count} jobs from file {folderName}.";
		sb.AppendLine($"\n{total}");
		string result = sb.ToString();

		WriteFile(result, folderName, "parsedJobs");
		WriteLine(total);
	}

	public void TopSort(TopologicalSort ts, string folderName) {
		StringBuilder sb = new();
		for(int i = 0; i < ts.jobs.Count; i++) { 
			string vertex = $"{idToName[i]}-{i},";
			string metadata = $"\tstate: {ts.state[i]}, in time: {ts.inTime[i]}, out time: {ts.outTime[i]}";

			sb.Append(vertex);
			if(vertex.Length < 16) {
				sb.Append('\t');
			}
			if(vertex.Length < 8) {
				sb.Append('\t');
			}
			sb.AppendLine(metadata);
		}
		string result = sb.ToString();

		WriteFile(result, folderName, "topSort");
	}

	public void ToRecompute(HashSet<int> toRecompute, string folderName) {
		List<string> strToRecompute = new();
		foreach(int id in toRecompute) {
			string name = idToName[id];
			strToRecompute.Add(name);
		}
		strToRecompute.Sort(); // requirement from specification to return dependencies sorted

		StringBuilder sb = new();
		foreach (string dependency in strToRecompute) {
			sb.AppendLine(dependency);
		}
		string result = sb.ToString();

		WriteFile(result, folderName, "toRecompute");

		WriteLine($"Found {strToRecompute.Count} jobs to be recomputed.");
	}

	public void Schedule(List<ScheduleUnit> schedule, string folderName, string algoName, int makespan, int sumJobsDuration, int makespanArea, double machinesUsage) {
		StringBuilder sb = new();
		string makespanLog = $"Makespan {makespan}. Jobs' duration: {sumJobsDuration}, makespan area: {makespanArea}, machines' usage: {machinesUsage:F2} %.";
		sb.AppendLine($"{makespanLog}\n");
		foreach (ScheduleUnit su in schedule) {
			string line = su.ToString(idToName);
			sb.AppendLine(line);
		}
		string result = sb.ToString();

		WriteFile(result, folderName, $"schedule-{algoName}");
		WriteLine($"Algorithm {algoName} scheduled {schedule.Count} tasks. {makespanLog}");
	}
}

