// TSL: topological sort layers

class TSL : Scheduler {
	public TSL(List<Job> jobs, int machineCount) : base(jobs, machineCount) { }

	public override string AlgoName { get => nameof(TSL); }

	protected override void DecideSchedulingPermutation(HashSet<int> toRecompute, List<ScheduleUnit> schedule) {
		while (toRecompute.Count > 0) {
			HashSet<int> layer = FindAllSources(toRecompute);
			foreach (int id in layer) {
				toRecompute.Remove(id);
			}

			foreach (int id in layer) {
				ScheduleJob(id, schedule);			
			}
		}
	}
}