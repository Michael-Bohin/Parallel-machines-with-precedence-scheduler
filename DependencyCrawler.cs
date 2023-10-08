class DependencyCrawler {
	public readonly List<Job> jobs;

	public DependencyCrawler(List<Job> jobs) {
		this.jobs = jobs;
	}

	public HashSet<int> JobsToRecompute(HashSet<int> idsWithError) {
		HashSet<int> toRecompute = new();
		foreach(int id in idsWithError) {
			toRecompute.Add(id);
			GetAllSuccessors(id, toRecompute);
		}
		return toRecompute;
	}

	void GetAllSuccessors(int id, HashSet<int> toRecompute) {
		foreach(int succ in jobs[id].successors) {
			if (toRecompute.Add(succ)) {
				GetAllSuccessors(succ, toRecompute);
			}
		}
	}
}