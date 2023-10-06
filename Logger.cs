﻿using System.Text;
using static System.Console;

class Logger {
	public List<string> jobsToString;

	public Logger(List<string> jobsToString) {
		this.jobsToString = jobsToString;
	}

	public void Jobs(List<Job> jobs, string fileName) {
		StringBuilder sb = new();
		foreach (Job job in jobs) {
			sb.AppendLine($"{job.ToString(jobsToString)}");
			sb.AppendLine($"{job}");
			sb.AppendLine();
		}

		string total = $"Loaded {jobs.Count} jobs from file {fileName}.";
		sb.AppendLine($"\n{total}");

		using StreamWriter sw = new($"./out/parsedJobs-{fileName}.txt");
		sw.WriteLine($"{sb}");
		sw.Close();

		WriteLine(total);
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

