class IntermediateJob {
	public readonly int name;
	public readonly int duration;
	public readonly List<string> dependencies;

	public IntermediateJob(int name, int duration, List<string> dependencies) {
		this.name = name;
		this.duration = duration;
		this.dependencies = dependencies;

		// Assert:
		// 1. Name is not negative number
		// 2. Duration is greater than zero
		
		if(name < 0) {
			throw new ArgumentException($"IntermediteJob.Ctor() : Name can not be negative number. Name: {name}, duration: {duration}.");
		}

		if(duration < 1) {
			throw new ArgumentException($"IntermediteJob.Ctor() : Duration must be greater than zero. Name: {name}, duration: {duration}.");
		}
	}

	public Job CopyJob() => new(name, duration);
}