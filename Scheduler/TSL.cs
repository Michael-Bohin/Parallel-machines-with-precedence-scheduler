class TSL : Scheduler {
	public TSL(List<Job> jobs, int machineCount) : base(jobs, machineCount) { }

	protected override void DecideSchedulingPermutation(HashSet<int> toRecompute, List<ScheduleUnit> schedule) {
		HashSet<int> layer = new();
		foreach (int i in toRecompute) {
			if (DependenciesScheduled(i)) {
				layer.Add(i);
			}
		}

		foreach (int j in layer) {
			toRecompute.Remove(j);
		}

		foreach (int k in layer) {
			ScheduleUnit su = Schedule(jobs[k]);
			schedule.Add(su);
			state[k] = ScheduleState.Scheduled;
			finnishTime[k] = su.time + jobs[k].duration;
		}
	}
}