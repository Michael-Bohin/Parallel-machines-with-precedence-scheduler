﻿// Note: I will likely reuse some subroutines of top. sort in the LCPT (longest critical path time) adaptation of LPT algorithm

enum DFSState {
	Undiscovered, Opened, Closed
}

class TopologicalSort {
	public readonly List<Job> jobs;

	// DFS metadata:
	public readonly List<DFSState> state = new();
	public readonly List<int> inTime = new();
	public readonly List<int> outTime = new();
	int time = 0;
	public bool DAG = true;

	public TopologicalSort(List<Job> jobs) {
		this.jobs = jobs;
	}

	public void AssertDAG() {
		DFS();
		if(!DAG) {
			throw new ArgumentException($"TopologicalSort.AssertDAG() : Input dependencies contain cycle. Invalid input. Jobs count: {jobs.Count}");
		}
	}

	void DFS() {
		for (int i = 0; i < jobs.Count; i++) {
			state.Add(DFSState.Undiscovered);
			inTime.Add(0);
			outTime.Add(0);
		}

		for(int i = 0; i < jobs.Count; i++) {
			if (state[i] == DFSState.Undiscovered) {
				DFS_Visit(i);
			}
		}
		
		Console.WriteLine("DFS finnished");
	}

	void DFS_Visit(int name) {
		state[name] = DFSState.Opened;
		time++;
		inTime[name] = time;

		foreach(int succ in jobs[name].successors) {
			if (state[succ] == DFSState.Undiscovered) {
				DFS_Visit(succ);
			} else if (state[succ] == DFSState.Opened) {
				DAG = false; // In DAG it is impossible to meet opened vertex. Cycle detected.
			}
		}

		state[name] = DFSState.Closed;
		time++;
		outTime[name] = time;
	}
}