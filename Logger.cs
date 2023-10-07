using System.Text;
using static System.Console;

class Logger {
	public List<string> jobsToString;

	public Logger(List<string> jobsToString) {
		this.jobsToString = jobsToString;
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
			sb.AppendLine($"{job.ToString(jobsToString)}");
			sb.AppendLine($"{job}");
			sb.AppendLine();
		}

		string total = $"Loaded {jobs.Count} jobs from file {folderName}.";
		sb.AppendLine($"\n{total}");

		WriteFile(sb.ToString(), folderName, "parsedJobs");
		WriteLine(total);
	}

	public void TopSort(TopologicalSort ts, string folderName) {
		StringBuilder sb = new();
		for(int i = 0; i < ts.jobs.Count; i++) { 
			string vertex = $"{jobsToString[i]}-{i},";
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

		WriteFile(sb.ToString(), folderName, "topSort");
	}

	/*public void ToRecompute(List<int> toRecompute) {
		toRecompute.Sort(); // requirement from specification to return dependencies sorted

		StringBuilder sb = new();
		foreach (string dependency in toRecompute) {
			sb.AppendLine(dependency);
		}
		string result = sb.ToString();

		using StreamWriter sw = new("./out/toRecompute.txt");
		sw.WriteLine(result);
		sw.Close();

		WriteLine($"\nFound {toRecompute.Count} dependices of jobs with error:");
		WriteLine(result);
	}

	public void Schedule(List<ScheduleUnit> schedule) {
		StringBuilder sb = new();
		foreach (ScheduleUnit unit in schedule) {
			sb.AppendLine($"{unit}");
		}
		string result = sb.ToString();

		using StreamWriter sw = new("./out/scheduler.txt");
		sw.WriteLine(result);
		sw.Close();

		WriteLine($"\nSchedules tasks: {schedule.Count}");
		WriteLine(result);
	}*/
}

