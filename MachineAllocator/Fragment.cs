struct Fragment {
	public int from;
	public int to;

	public Fragment(int from, int to) {
		if(to <= from) {
			throw new ArgumentException($"Fragment.Ctor() : Fragment duration must be at least 1. from {from}, to: {to}.");
		}

		this.from = from;
		this.to = to;
	}

	public int Size => to - from;	
}