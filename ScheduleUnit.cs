struct ScheduleUnit {
	public int time;
	public int jobId;
	public int machineId;

	public ScheduleUnit(int time, int jobId, int machineId) {
		this.time = time;
		this.jobId = jobId;
		this.machineId = machineId;
	}

	public override string ToString() {
		return $"{time}\t{jobId}\t{machineId}";
	}

	public string ToString(List<string> idToName) {
		return $"{time}\t{idToName[jobId]}\t{machineId}";
	}
}