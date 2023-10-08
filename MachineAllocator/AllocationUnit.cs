record AllocationUnit {
	public int MachineId { get; set; }
	public AllocType AllocType { get; set; }
	public int StartTime { get; set; }
	public int BlockSize { get; set; }
	public int FragmentId { get; set; }

	public AllocationUnit(int MachineId, AllocType AllocType, int StartTime, int BlockSize, int FragmentId) { 
		this.MachineId = MachineId;
		this.AllocType = AllocType;
		this.StartTime = StartTime;
		this.BlockSize = BlockSize;
		this.FragmentId = FragmentId;
	}

	public void UpdateValues(int MachineId, AllocType AllocType, int StartTime, int FragmentId) {
		this.MachineId = MachineId;
		this.AllocType = AllocType;
		this.StartTime = StartTime;
		this.FragmentId = FragmentId;
	}
}