/*class LPTsourcePathLayers : Scheduler {
	public LPTsourcePathLayers(List<Job> job, int machineCount) : base(job, machineCount) { }

	public override string AlgoName { get => nameof(LPTsourcePathLayers); }

	protected override void DecideSchedulingPermutation(HashSet<int> toRecompute, List<ScheduleUnit> schedule) {
		// 1. Create prio queue that will hold duration of source paths and id of source paths
		// 2. Find all source paths in to recompute and insert them in prio queue
		// 3. Remove all jobs from these source path from the toReocompute set
		// 3. Schedule all source paths in prioque
		// 4. Repeat steps 2, 3, and 4 untill toRecompute is empty

		DurationComparer dc = new();
		PriorityQueue<int, int> prioQueue = new(dc);

		while (toRecompute.Count > 0) {

			// 2.:
			List<SourcePath> sourcePaths = FindAllSourcePaths(toRecompute); // 3.: step 3 of the algorithm must be done inside the method due to the nature of queues
			// only use index of source path when inserting in prio queue:
			for(int i = 0; i < sourcePaths.Count; i++) {
				int duration = sourcePaths[i].Length;
				prioQueue.Enqueue(i, duration);
			}

			while(prioQueue.Count > 0) {
				int spId = prioQueue.Dequeue();
				SourcePath sp = sourcePaths[spId];

				while(sp.IsNotEmpty) {
					int id = sp.Dequeue();
					ScheduleJob(id, schedule);
				}
			}
		}
	}

	// combined steps 2 and 3: find all source paths and remove their jobs from toRecompute
	List<SourcePath> FindAllSourcePaths(HashSet<int> toRecompute) {

		// work to do

		return new();
	}
}*/

// LCPTfixedPaths -> create a set of disjoint paths in the topological sort
// track their sources and check the set of schedulable sources on those paths in cycles
// prioritize schedulable sources in longest critical paths
// key question how do I choose the permutation of disjoint paths in the topological sort?
//		1st idea: find the longest cp and cut it off, then again find the next longest cp in the complement


// LCPTdozer
// Find the lcp 
// Start scheduling it from its source, when you meet a branch that needs to be resolved first, call on its entire dependency subtree recursively
// Repeat for complement after resolving initial lcp and its dependent branches

