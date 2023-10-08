class SourcePath {
	Queue<int> sourcePath = new();
	public int Length { get; private set; } = 0;
	public bool IsNotEmpty { get => sourcePath.Count != 0; }

	public void EnqueueJob(int id, int duration) {
		sourcePath.Enqueue(id);
		Length += duration;
	}

	public int Dequeue() {
		return sourcePath.Dequeue();
	}
}