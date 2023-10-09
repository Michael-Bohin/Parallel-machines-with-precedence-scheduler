// SimpleProbability ->
// While to recompute is not empty
// Update the set of sources
// From the set of sources choose random source to schedule


class SimpleProbability : Scheduler {
	public SimpleProbability(List<Job> job, int machineCount) : base(job, machineCount) { }

	public override string AlgoName { get => nameof(SimpleProbability); }

	int seed = 38;

	protected override void DecideSchedulingPermutation(HashSet<int> toRecompute, List<ScheduleUnit> schedule) {
		List<int> sources = new();

		Random rand = new(seed);

		while (toRecompute.Count > 0 || sources.Count > 0) {
			foreach (int i in toRecompute) {
				if (DependenciesScheduled(i)) {
					sources.Add(i);
					toRecompute.Remove(i);
					// work to do: What data structure can I use not use iterating through entire foreach in each while iteration? (so that I know schedulable jobs in faster than linear time)
				}
			}

			if(sources.Count > 0) {
				int sourcesIndex = rand.Next(sources.Count);
				int jobId = sources[sourcesIndex];
				sources.RemoveAt(sourcesIndex);
				ScheduleJob(jobId, schedule);
			}
		}

		seed = rand.Next();
	}
}