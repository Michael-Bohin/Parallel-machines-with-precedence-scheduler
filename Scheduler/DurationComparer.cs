// PriorityQueue needs custom comparer, because LPT algorithms
// sort jobs by their duration in descending order.

class DurationComparer : IComparer<int> {
	public int Compare(int x, int y) {
		return y.CompareTo(x);
	}
}