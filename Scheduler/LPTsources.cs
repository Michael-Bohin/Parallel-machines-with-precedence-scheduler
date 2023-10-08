class LPTsources : Scheduler {
	public LPTsources(List<Job> job, int machineCount) : base(job, machineCount) { }

	public override string AlgoName { get => nameof(LPTsources); }

	protected override void DecideSchedulingPermutation(HashSet<int> toRecompute, List<ScheduleUnit> schedule) {
		DurationComparer dc = new();
		PriorityQueue<int, int> prioQueue = new(dc);

		while(toRecompute.Count > 0 || prioQueue.Count > 0) {
			foreach (int i in toRecompute) {
				if (DependenciesScheduled(i)) {
					prioQueue.Enqueue(i, jobs[i].duration);
					toRecompute.Remove(i);
					// work to do: What data structure can I use not use iterating through entire foreach in each while iteration? (so that I know schedulable jobs in faster than linear time)
				}
			}

			int id = prioQueue.Dequeue();
			ScheduleJob(id, schedule);
		}
	}
}

/* Template for more descendants:
  
class LPTsources : Scheduler {
	public LPTsources(List<Job> job, int machineCount) : base(job, machineCount) { }

	public override string AlgoName { get => nameof(); }

	protected override void DecideSchedulingPermutation(HashSet<int> toRecompute, List<ScheduleUnit> schedule) {
		throw new NotImplementedException();
	}
}

*/