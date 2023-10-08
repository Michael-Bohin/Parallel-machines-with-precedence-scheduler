// LCPTfixedPaths -> create a set of disjoint paths in the topological sort
// track their sources and check the set of schedulable sources on those paths in cycles
// prioritize schedulable sources in longest critical paths
// key question how do I choose the permutation of disjoint paths in the topological sort?
//		1st idea: find the lcpt and cut it off, then again find the next lcpt in the complement

class LCPTfixedPaths : Scheduler {
	public LCPTfixedPaths(List<Job> job, int machineCount) : base(job, machineCount) { }

	public override string AlgoName { get => nameof(LCPTfixedPaths); }

	protected override void DecideSchedulingPermutation(HashSet<int> toRecompute, List<ScheduleUnit> schedule) {
		throw new NotImplementedException();
	}
}