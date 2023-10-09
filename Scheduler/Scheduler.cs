enum ScheduleState {
	Computed, Waiting, Scheduled
}

abstract class Scheduler {
	protected readonly List<Job> jobs;
	protected readonly MachineAllocator alloc;
	private readonly int machineCount;
	protected readonly List<ScheduleState> state = new();
	protected readonly Dictionary<int, int> finnishTime = new(); // Key: jobId, Value: it's finnish time in schedule
	public int Makespan { get; private set; }
	public int TotalJobsDuration { get; private set; }
	public int MakespanArea { get; private set; }
	public double MachinesUsage { get; private set;}

	public abstract string AlgoName { get; }

	protected abstract void DecideSchedulingPermutation(HashSet<int> toRecompute, List<ScheduleUnit> schedule);

	public Scheduler(List<Job> jobs, int machineCount) {
		this.jobs = jobs;
		this.machineCount = machineCount;
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

		RecordPerformance(schedule);

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

	void RecordPerformance(List<ScheduleUnit> schedule) {
		int max = int.MinValue;
		foreach(int ft in finnishTime.Values) {
			max = Math.Max(max, ft);
		}
		Makespan = max;

		int sum = 0;
		foreach(ScheduleUnit unit in schedule) {
			int id = unit.jobId;
			Job job = jobs[id];
			sum += job.duration;
		}
		TotalJobsDuration = sum;

		MakespanArea = Makespan * machineCount;

		MachinesUsage = ((double) TotalJobsDuration / MakespanArea)* 100.0;
	}

	protected void ScheduleJob(int id, List<ScheduleUnit> schedule) {
		Job next = jobs[id];
		ScheduleUnit su = Schedule(next);
		schedule.Add(su);
		state[id] = ScheduleState.Scheduled;
		finnishTime[id] = su.time + jobs[id].duration;
	}

	protected HashSet<int> FindAllSources(HashSet<int> toRecompute) {
		HashSet<int> layer = new();
		foreach (int i in toRecompute) {
			if (DependenciesScheduled(i)) {
				layer.Add(i);
			}
		}
		return layer;
	}
}