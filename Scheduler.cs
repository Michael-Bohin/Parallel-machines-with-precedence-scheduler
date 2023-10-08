using static System.Math;

enum ScheduleState {
	Computed, Waiting, Scheduled
}

class Scheduler {
	readonly List<Job> jobs;
	readonly MachineAllocator alloc;
	readonly List<ScheduleState> state = new();
	readonly Dictionary<int, int> finnishTime = new(); // Key: jobId, Value: it's finnish time in schedule

	public Scheduler(List<Job> jobs, int machineCount) {
		this.jobs = jobs;
		alloc = new(machineCount); // allocate with 5 machines

		for (int i = 0; i < jobs.Count; i++) {
			state.Add(ScheduleState.Computed);
		}
	}

	public List<ScheduleUnit> Schedule(HashSet<int> toRecompute) {
		List<ScheduleUnit> schedule = new();

		foreach(int id in toRecompute) {
			state[id] = ScheduleState.Waiting;	
		}

		while(toRecompute.Count > 0) {
			HashSet<int> layer = new();
			foreach(int i in toRecompute) {
				if(DependenciesScheduled(i)) {
					layer.Add(i);
				}
			}

			foreach(int j in layer) {
				toRecompute.Remove(j);	
			}

			foreach(int k in layer) { 
				ScheduleUnit su = Schedule(jobs[k]);
				schedule.Add(su);
				state[k] = ScheduleState.Scheduled;
				finnishTime[k] = su.time + jobs[k].duration;
			}
		}

		return schedule;
	}

	bool DependenciesScheduled(int id) {
		foreach(int pred in jobs[id].predecessors) {
			if (state[pred] == ScheduleState.Waiting) { // computed or scheduled state of dependency is OK
				return false;
			}
		}
		return true;
	}

	ScheduleUnit Schedule(Job job) {
		// Get earliest time the job can be scheduled relative to its dependencies:
		int minStart = int.MinValue;
		foreach(int pred in job.predecessors) {
			if (state[pred] == ScheduleState.Scheduled) {
				minStart = Max(minStart, finnishTime[pred]);
			}
		}

		(int startTime, int machineId) = alloc.GetBlock(minStart, job.duration);

		ScheduleUnit su = new(startTime, job.id, machineId);
		return su;
	}
}