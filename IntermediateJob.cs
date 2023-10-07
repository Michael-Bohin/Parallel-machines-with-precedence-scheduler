class IntermediateJob {
	public readonly int id;
	public readonly int duration;
	public readonly List<string> dependencies;

	public IntermediateJob(int id, int duration, List<string> dependencies) {
		this.id = id;
		this.duration = duration;
		this.dependencies = dependencies;

		// Assert:
		// 1. Id is not negative number
		// 2. Duration is greater than zero
		
		if(id < 0) {
			throw new ArgumentException($"IntermediteJob.Ctor() : Id can not be negative number. Id: {id}, duration: {duration}.");
		}

		if(duration < 1) {
			throw new ArgumentException($"IntermediteJob.Ctor() : Duration must be greater than zero. Id: {id}, duration: {duration}.");
		}
	}
}