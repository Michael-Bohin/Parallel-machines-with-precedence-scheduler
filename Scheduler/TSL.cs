﻿// TSL: topological sort layers

class TSL : Scheduler {
	public TSL(List<Job> jobs, int machineCount) : base(jobs, machineCount) { }

	public override string AlgoName { get => nameof(TSL); }

	protected override void DecideSchedulingPermutation(HashSet<int> toRecompute, List<ScheduleUnit> schedule) {
		while (toRecompute.Count > 0) {
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
				ScheduleJob(k, schedule);			
			}
		}
	}
}