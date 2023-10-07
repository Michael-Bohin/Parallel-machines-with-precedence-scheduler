class DependencyCrawler {
	public readonly List<Job> jobs;

	public DependencyCrawler(List<Job> jobs) {
		this.jobs = jobs;
	}

	public List<int> JobsToRecompute(List<int> idsWithError) {
		List<int> toRecompute = new();
		foreach(int id in idsWithError) {
			if(!toRecompute.Contains(id)) {
				toRecompute.Add(id);
			}
			GetAllSuccessors(id, toRecompute);
		}
		return toRecompute;
	}

	void GetAllSuccessors(int id, List<int> toRecompute) {
		foreach(int succ in jobs[id].successors) {
			if(!toRecompute.Contains(succ)) {
				toRecompute.Add(succ);
				GetAllSuccessors(succ, toRecompute);
			}
		}
	}
}