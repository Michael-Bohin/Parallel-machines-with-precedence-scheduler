class LPTsourcePathLayers : Scheduler {
	public LPTsourcePathLayers(List<Job> job, int machineCount) : base(job, machineCount) { }

	public override string AlgoName { get => nameof(LPTsourcePathLayers); }

	protected override void DecideSchedulingPermutation(HashSet<int> toRecompute, List<ScheduleUnit> schedule) {
		// 1. Create prio queue that will hold duration of source paths and id of source paths
		// 2. Find all source paths in to recompute and insert them in prio queue
		// 3. Remove all jobs from these source path from the toReocompute set
		// 3. Schedule all source paths in prioque
		// 4. Repeat steps 2, 3, and 4 untill toRecompute is empty

		DescendingDurationComparer dc = new();
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
	// Subroutine algorithm:
	// 1. Create set of jobs added to current layer
	// 2. Create List<SourcePath> sourcePaths
	// 3. Find List<int> of all source on toRecompute (identical to subroutine in TSL) -> move it abstract class Scheduler
	// 4. Foreach source
	// 5.	Find !>>probable<<! (possible further optimization) its source path sp (this will another subroutine)
	// 6.	Make a queue of memebers of sp and add it to sourcePaths list
	// 7.	Add its members to set from step 1
	// 8.	Remove its members from set toRecompute
	List<SourcePath> FindAllSourcePaths(HashSet<int> toRecompute) {
		HashSet<int> idsInLayer = new();
		List<SourcePath> sourcePaths = new();
		HashSet<int> sources = FindAllSources(toRecompute);

		foreach(int source in sources) {
			List<int> sourcePath = FindSourcePath(source, idsInLayer);
			SourcePath sp = new();
			foreach(int id in sourcePath) { // refactorization possible later, this foreach can be moved into the method FindSourcePath
				sp.EnqueueJob(id, jobs[id].duration);
				idsInLayer.Add(id);
				toRecompute.Remove(id);
			}
			sourcePaths.Add(sp);
		}

		return sourcePaths;
	}

	// Subroutine's algorithm:
	// 1. Create List<int> source path and append source to it
	// 2. Add source to idsInLayer
	// 3. Get List<int> of all successors of source
	// 4. Sort them in descending way
	// 5. Find first successor s that meets all constraints:
	//		i.   ScheduleState of s is waiting
	//		ii.  idsInLayer does not contain s
	//		iii. ScheduleState of all predecessors of s is not waiting (that is the state is either Computed or Scheduled)
	// 6. Append s to sourcePath, and add it to idsinlayer
	// 7. Repeat steps 3, 4, 5, 6 with the change that s now has the role of source, untill you find at least one successor that meets all constraints from step 5
	// 8. return sourcePath
	List<int> FindSourcePath(int source, HashSet<int> idsInLayer) {
		List<int> sourcePath = new() {
			source
		};
		idsInLayer.Add(source);

		int s = source;
		while(TryFindBestSuccessor(s, idsInLayer, out int bestSuccessor)) {
			sourcePath.Add(s);
			s = bestSuccessor;
		}
		
		return sourcePath;
	}

	bool TryFindBestSuccessor(int id, HashSet<int> idsInLayer, out int bestSuccessor) {
		List<int> successors = jobs[id].successors;

		// 4.: { ?? possible space for refactorization ??
		DescendingDurationComparer dc = new();
		PriorityQueue<int, int> prioQueue = new(dc);

		foreach (int succ in successors) {
			int duration = jobs[succ].duration;
			prioQueue.Enqueue(succ, duration);
		}

		List<int> sortedSuccessors = new();
		while (prioQueue.Count > 0) {
			int succ = prioQueue.Dequeue();
			sortedSuccessors.Add(succ);
		}
		// 4.: }

		foreach(int succ in sortedSuccessors) {
			if (state[succ] == ScheduleState.Waiting && !idsInLayer.Contains(succ) && AllPredecessorsAreNotWaiting(succ)) {
				bestSuccessor = succ;
				idsInLayer.Add(succ);
				return true;
			}
		}

		bestSuccessor = -1;
		return false;
	}

	bool AllPredecessorsAreNotWaiting(int id) {
		foreach(int pred in jobs[id].predecessors) {
			if (state[pred] == ScheduleState.Waiting) {
				return false;
			}
		}
		return true;
	}
}