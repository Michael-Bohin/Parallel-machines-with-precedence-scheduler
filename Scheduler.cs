using System.Text;
using static System.Console;
using static System.Math;

class Scheduler {
	public readonly List<Job> jobs;

	// Task 1 results
	List<string> allDependencies = new();
	Dictionary<string, List<string>> jobsMap = new();

	// Task 2 result
	List<ScheduleUnit> schedule = new();
	List<int> machinesTime = new();
	Dictionary<string, int> finnishTimes = new();


	public Scheduler(List<Job> jobs) {
		this.jobs = jobs;
	}

	public void FindErrorDependencies(List<string> toRecompute) {
		// use different structure then from input: List -> Dictionary
		foreach(Job job in jobs) {
			jobsMap[job.name] = job.dependencies;
		}
		
		// search all unique dependencies
		foreach(string job in toRecompute) {
			if(!allDependencies.Contains(job)) {
				allDependencies.Add(job);
			}
			GetAllDependencies(job);
		}

		allDependencies.Sort();
	}

	void GetAllDependencies(string name) {
		foreach(string dependency in jobsMap[name]) {
			// only recurse and add to list, if the dependency has not been added yet:
			if(!allDependencies.Contains(dependency)) {
				allDependencies.Add(dependency);
				GetAllDependencies(dependency);
			}
		}
	}

	public void LogErrorDependencies() {
		StringBuilder sb = new();
		foreach(string dependency in allDependencies) {
			sb.AppendLine(dependency);
		}
		string result = sb.ToString();	

		using StreamWriter sw = new("./out/toRecompute.txt");
		sw.WriteLine(result);
		sw.Close();

		WriteLine($"\nFound {allDependencies.Count} dependices of jobs with error:");
		WriteLine(result);
	}

	public void ScheduleRecompute() {
		// Naive algorithm:
		// 0. initiate machines times
		// 0. Create hashset Jobs of jobs to recompute
		// 1. while Jobs is nonempty:
		// 2. 		create empty list of jobs "layer"
		// 3. 		foreach job j in Jobs:
		// 4. 			if(all dependencies of j have finnished):
		// 5. 				remove j from Jobs
		// 6. 				add j to layer
		//		
		// 7.		foreach job k in layer
		// 8. 			schedule k 
		// 9. print output

		// 0. Jobs from pseudocode is now allDependencies (and not a set but list)
		// 0. 
		for(int i = 0; i < 5; i++) {
			machinesTime.Add(0);
		}

		while(allDependencies.Count > 0) { // 1
			List<string> layer = new(); // 2
			foreach(string j in allDependencies) { // 3
				if(DependenciesFinnished(j)) { // 4
					layer.Add(j); // 6
				}
			}

			foreach(string k in layer) { // 5a
				allDependencies.Remove(k); // 5b
			}

			foreach(string k in layer) { // 7
				Schedule(k); // 8
			}
		}
	}

	bool DependenciesFinnished(string name) {
		foreach(string dependency in jobsMap[name]) {
			if(allDependencies.Contains(dependency)) {
				return false;
			}
		}

		return true;
	}

	void Schedule(string name) { 
		int minTime = int.MaxValue;
		int machineId = -1;
		for(int i = 0; i < 5; i++) {
			int time = machinesTime[i];
			if(time < minTime) { 
				minTime = time;
				machineId = i;
			}
		}

		// increment time on scheduled machine:

		// pardon za tohle:
		int duration = int.MaxValue;
		List<string> directDependencis = new();
		foreach(Job j in jobs) {
			if(j.name == name) {
				duration= j.duration;	
				foreach(string d in j.dependencies) {
					directDependencis.Add(d);
				}
				break;
			}
		}

		// check that all dependencies finnished
		// in case some are still running, shift the schedule time, after the maximum of finnishing times of all dependencies:
		int startTime = minTime;
		foreach(string dependency in directDependencis) {
			int dependencyFinnishTime = finnishTimes[dependency];
			startTime = Max(startTime, dependencyFinnishTime);
		}

		int endTime = startTime + duration;
		machinesTime[machineId] = endTime;
		finnishTimes[name] = endTime;

		ScheduleUnit su = new(startTime, name, machineId);
		schedule.Add(su);
	}

	public void LogResult_2() {
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
