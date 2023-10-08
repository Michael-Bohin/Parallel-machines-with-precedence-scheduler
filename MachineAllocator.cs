interface IMachineAllocator { 
	(int startTime, int machineId) GetBlock(int from, int size);
}

enum AllocType {
	InsideFragment, MachineTime, CreateFragment, Undefined
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
		int startTime = int.MaxValue;
		int machineId = 0;
		AllocType allocType = AllocType.Undefined;
		int? fragmentId = 0;

		for(int i = 0; i < machineTime.Count; i++) {
			(int minMachineTime, AllocType at, int? fId) = MachinesFirstAllocOption(i, from, blockSize);
			if(minMachineTime < startTime) {
				machineId = i;
				startTime = minMachineTime;
				allocType = at;
				fragmentId = fId;
			}
		}

		SaveAllocation(machineId, allocType, startTime, blockSize, fragmentId);

		return (startTime, machineId); 
	}

	(int startTime, AllocType allocType, int? fragmentIndex) MachinesFirstAllocOption(int machineId, int from, int blockSize) {
		int fragmentIndex = 0;
		foreach(Fragment f in fragments[machineId]) {
			if((from <= f.from && f.Size <= blockSize)) {
				return (f.from, AllocType.InsideFragment, fragmentIndex); // placed at the beginning of the fragment
			}
				
			if(from + blockSize <= f.to) {
				int startTime = f.to - blockSize;
				return (startTime, AllocType.InsideFragment, fragmentIndex); // placed at the end of the fragment
			}
			fragmentIndex++;
		}

		// job does not fit into any fragment
		// it can be placed either at machine time, or earliest time based on its dependencies
		if (from <= machineTime[machineId]) {
			//
			// work to do
			//
			// is there a better way of handling the fact, that machine time and create fragment situation dont need to carry the null fragmentId value?
			return (machineTime[machineId], AllocType.MachineTime, null); 
		}
		//
		// work to do
		//
		// is there a better way of handling the fact, that machine time and create fragment situation dont need to carry the null fragmentId value?

		Console.WriteLine("Does runtime come here?");
		return (from, AllocType.CreateFragment, null); 
	}

	void SaveAllocation(int machineId, AllocType allocType, int startTime, int blockSize, int? fragmentIndex) {
		if(allocType == AllocType.MachineTime) {
			machineTime[machineId] = startTime + blockSize;
			return;
		}

		if(allocType == AllocType.CreateFragment) {
			int from = machineTime[machineId];
			int to = startTime;
			Fragment f = new(from, to);
			fragments[machineId].Add(f);

			machineTime[machineId] = startTime + blockSize;
			return;			
		}

		// AllocType must now be InsideFragment
#if DEBUG
		if(allocType != AllocType.InsideFragment) {
			throw new ArgumentException($"MachineAllocator.SaveAllocation() : Code ran into unexpected allocType: {allocType}");
		}
#endif
		// Allocation inside fragment can be either at the beginning or end of the fragment:
		if(fragmentIndex != null) {
			Fragment f = fragments[machineId][(int)fragmentIndex];
			if(f.from == startTime) {
				// place block at the start of fragment:
				f.from = startTime + blockSize;
			} else {
				// place block at the end of fragment:
				f.to = f.to - blockSize;
			}

			if (f.Size == 0) {
				// fragment was filled, remove it:
				fragments[machineId].RemoveAt((int)fragmentIndex);
			} else {
				// fragment was only made smaller, save the updated fragment to fragments:
				fragments[machineId][(int)fragmentIndex] = f;
			}

		} else {
			throw new ArgumentException();
		}
	}
}