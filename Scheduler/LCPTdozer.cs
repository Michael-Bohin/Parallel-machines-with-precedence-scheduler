// LCPTdozer
// Find the lcpt
// Start scheduling it from its source, when you meet a branch that needs to be resolved first, call on its entire dependency subtree recursively
// Repeat for complement after resolving initial lcpt and its dependent branches

class LCPTdozer : Scheduler {
	public LCPTdozer(List<Job> job, int machineCount) : base(job, machineCount) { }

	public override string AlgoName { get => nameof(LCPTdozer); }

	protected override void DecideSchedulingPermutation(HashSet<int> toRecompute, List<ScheduleUnit> schedule) {
		throw new NotImplementedException();
	}
}