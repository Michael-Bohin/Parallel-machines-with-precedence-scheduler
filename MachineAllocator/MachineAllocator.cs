interface IMachineAllocator { 
	(int startTime, int machineId) GetBlock(int from, int size);
}

enum AllocType {
	Undefined, FragmentStart, FragmentEnd, MachineTime, CreateFragment
}

class MachineAllocator : IMachineAllocator {
	readonly List<List<Fragment>> fragments = new();
	readonly List<int> machineTime = new(); 

	public MachineAllocator(int machineCount) {
		for(int i = 0; i < machineCount; i++) {
			fragments.Add(new());
			machineTime.Add(0);
		}
	}

	public (int startTime, int machineId) GetBlock(int from, int blockSize) {
		AllocationUnit a = FindBestBlock(from, blockSize);

		if (a.AllocType == AllocType.MachineTime) {
			machineTime[a.MachineId] = a.StartTime + a.BlockSize;
		} else if(a.AllocType == AllocType.CreateFragment) {
			SaveCreateFragmentAllocation(a);
		} else {
			SaveInsideFragmentAllocation(a);
		}

		return (a.StartTime, a.MachineId); 
	}

	AllocationUnit FindBestBlock(int from, int blockSize) {
		AllocationUnit a = new(-1, AllocType.Undefined, int.MaxValue, blockSize, -1);

		for (int i = 0; i < machineTime.Count; i++) {
			(int minMachineTime, AllocType at, int fId) = MachineFirstAllocOption(i, from, blockSize);
			if (minMachineTime < a.StartTime) {
				a.UpdateValues(i, at, minMachineTime, fId);
			} else if(minMachineTime == a.StartTime) {
				int prevAllocValue = (int) a.AllocType; // in this rare case, prefer allocation types in the order of their enum value (Fragment < MachinteTime < CreateFragement)
				int currentAllocValue = (int) at;
				if(currentAllocValue < prevAllocValue) {
					a.UpdateValues(i, at, minMachineTime, fId);
				}
			}
		}

		return a;
	}

	(int startTime, AllocType allocType, int fragmentId) MachineFirstAllocOption(int machineId, int from, int blockSize) {
		int fragmentId = 0;
		foreach (Fragment f in fragments[machineId]) {
			if ((from <= f.from && f.Size <= blockSize)) {
				return (f.from, AllocType.FragmentStart, fragmentId);
			}
			if (from + blockSize <= f.to) {
				int startTime = f.to - blockSize;
				return (startTime, AllocType.FragmentEnd, fragmentId);
			}
			fragmentId++;
		}

		if (from <= machineTime[machineId]) {
			return (machineTime[machineId], AllocType.MachineTime, -1); 
		}

		return (from, AllocType.CreateFragment, -1); 
	}

	void SaveCreateFragmentAllocation(AllocationUnit a) {
		int from = machineTime[a.MachineId];
		int to = a.StartTime;
		Fragment f = new(from, to);
		fragments[a.MachineId].Add(f);

		machineTime[a.MachineId] = a.StartTime + a.BlockSize;
	}

	void SaveInsideFragmentAllocation(AllocationUnit a) {
		Fragment f = fragments[a.MachineId][a.FragmentId];
		if (a.AllocType == AllocType.FragmentStart) { 
			f.from += a.BlockSize; // place block at the start of fragment
		} else {
			f.to -= a.BlockSize; // place block at the end of fragment
		}

		if (f.Size == 0) { 
			fragments[a.MachineId].RemoveAt(a.FragmentId); // fragment was filled, remove it
		} else { 
			fragments[a.MachineId][a.FragmentId] = f;  // fragment was only made smaller, save the updated fragment to fragments
		}
	}
}