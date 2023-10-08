enum ScheduleState {
	Computed, Waiting, Scheduled
}

abstract class Scheduler {
	protected readonly List<Job> jobs;
	protected readonly MachineAllocator alloc;
	protected readonly List<ScheduleState> state = new();
	protected readonly Dictionary<int, int> finnishTime = new(); // Key: jobId, Value: it's finnish time in schedule
	public int Makespan { get; protected set; }

	public abstract string AlgoName { get; }

	protected abstract void DecideSchedulingPermutation(HashSet<int> toRecompute, List<ScheduleUnit> schedule);

	public Scheduler(List<Job> jobs, int machineCount) {
		this.jobs = jobs;
		alloc = new(machineCount);

		for (int i = 0; i < jobs.Count; i++) {
			state.Add(ScheduleState.Computed);
		}
	}

	public List<ScheduleUnit> Schedule(HashSet<int> toRecompute) {
		List<ScheduleUnit> schedule = new();

		foreach(int id in toRecompute) {
			state[id] = ScheduleState.Waiting;	
		}

		DecideSchedulingPermutation(toRecompute, schedule);

		RecordMakespan();

		return schedule;
	}

	protected bool DependenciesScheduled(int id) {
		foreach(int pred in jobs[id].predecessors) {
			if (state[pred] == ScheduleState.Waiting) { // computed or scheduled state of dependency is OK
				return false;
			}
		}
		return true;
	}

	protected ScheduleUnit Schedule(Job job) {
		// Get earliest time the job can be scheduled relative to its dependencies:
		int minStart = int.MinValue;
		foreach(int pred in job.predecessors) {
			if (state[pred] == ScheduleState.Scheduled) {
				minStart = Math.Max(minStart, finnishTime[pred]);
			}
		}

		(int startTime, int machineId) = alloc.GetBlock(minStart, job.duration);

		ScheduleUnit su = new(startTime, job.id, machineId);
		return su;
	}

	void RecordMakespan() {
		int max = int.MinValue;
		foreach(int ft in finnishTime.Values) {
			max = Math.Max(max, ft);
		}
		Makespan = max;
	}
}