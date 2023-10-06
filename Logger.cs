using System.Text;
using static System.Console;

class Logger {
	public static void Jobs(List<string> toRecompute) {
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

	public static void Schedule(List<ScheduleUnit> schedule) {
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
	}
}

