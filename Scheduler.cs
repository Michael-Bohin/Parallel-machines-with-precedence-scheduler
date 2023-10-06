using System.Text;
using static System.Console;
using static System.Math;

class Scheduler {
	public readonly List<Job> jobs;

	public Scheduler(List<Job> jobs) {
		this.jobs = jobs;
	}

	// Task 1 results
	List<string> allDependencies = new();
	Dictionary<string, Job> jobsMap = new();

	// Task 2 results
	List<ScheduleUnit> schedule = new();
	List<int> machinesTime = new();
	Dictionary<string, int> finnishTimes = new();

	public List<ScheduleUnit> Schedule(List<string> toRecompute) {
		allDependencies = toRecompute;

		foreach (Job job in jobs) {
			jobsMap[job.name] = job;
		}

		// 0. Jobs from pseudocode is now allDependencies (and not a set but list)
		// 0. 
		for (int i = 0; i < 5; i++) {
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
				Schedule(jobsMap[k]); // 8
			}
		}

		return schedule;
	}

	bool DependenciesFinnished(string name) {
		foreach(string dependency in jobsMap[name].dependencies) {
			if(allDependencies.Contains(dependency)) {
				return false;
			}
		}

		return true;
	}

	void Schedule(Job job) { 
		int minTime = int.MaxValue;
		int machineId = -1;
		for(int i = 0; i < 5; i++) {
			int time = machinesTime[i];
			if(time < minTime) { 
				minTime = time;
				machineId = i;
			}
		}

		// check that all dependencies finnished
		// in case some are still running, shift the schedule time, after the maximum of finnishing times of all dependencies:
		int startTime = minTime;
		foreach(string dependency in job.dependencies) {
			int dependencyFinnishTime = finnishTimes[dependency];
			startTime = Max(startTime, dependencyFinnishTime);
		}

		// increment time on scheduled machine:
		int endTime = startTime + job.duration;
		machinesTime[machineId] = endTime;
		finnishTimes[job.name] = endTime;

		ScheduleUnit su = new(startTime, job.name, machineId);
		schedule.Add(su);
	}
}