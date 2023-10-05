class ScheduleUnit {
	public int time;
	public string name;
	public int machineId;

	public ScheduleUnit(int time, string name, int machineId) {
		this.time = time;
		this.name = name;
		this.machineId = machineId;
	}

	public override string ToString() {
		return $"{time}\t{name}\t{machineId}";
	}
}

