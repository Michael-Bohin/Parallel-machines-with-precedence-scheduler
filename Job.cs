using System.Text;

class Job {
	public readonly int name;
	public readonly int duration;
	public readonly List<int> predecessors = new();
	public readonly List<int> successors = new();

	public Job(int name, int duration) {
		this.name = name;
		this.duration = duration;

		// Assert:
		// 1. Dependency name is not negative number
		// 2. Duration is greater then zero

		if(name < 0) { 
			throw new ArgumentException($"Job.Ctor() : Name must not be negative number! Job name: {name}, duration: {duration}.");	
		}

		if (duration < 1) {
			throw new ArgumentException($"Job.Ctor() : Duration must be greater than zero! Job name: {name}, duration: {duration}.");
		}
	}

	public override string ToString() {
		StringBuilder sb = new();
		sb.Append($"Job: {name}, d:{duration}, pred:");

		foreach(int pred in predecessors) {
			sb.Append($" {pred}");
		}

		sb.Append(", succ:");
		foreach(int succ in successors) {
			sb.Append($" {succ}");
		}

		return sb.ToString();
	}

	public string ToString(List<string> jobToString) {
		StringBuilder sb = new();
		sb.Append($"Job: {jobToString[name]}, d: {duration}, pred:");

		foreach (int pred in predecessors) {
			sb.Append($" {jobToString[pred]}");
		}

		sb.Append(", succ:");
		foreach (int succ in successors) {
			sb.Append($" {jobToString[succ]}");
		}

		return sb.ToString();
	}

	public bool ContainsPredecessor(int value) {
		foreach(int pred in predecessors) {
			if(pred == value) {
				return true;
			}
		}
		return false;
	}

	public bool ContainsSuccessor(int value) {
		foreach(int succ in successors) { 
			if(succ == value) { 
				return true;
			}	
		}
		return false;
	}

	// Foreach new predecessor or successor, assert:
	// 1. Value is not negative number
	// 2. Value is not equal to name
	// 3. Value is different from all other existing dependency records
	public void AddPredecessor(int value) {
		AssertRecordValidity(value);
		predecessors.Add(value);
	}

	public void AddSuccessor(int value) {
		AssertRecordValidity(value);
		successors.Add(value);
	}

	void AssertRecordValidity(int value) {
		if(value < 0) {
			throw new ArgumentException($"Job.AssertRecordValidity() : Dependency record can not be negative number. Name: {name}, duration: {duration}, value: {value}");
		}

		if(value == name) {
			throw new ArgumentException($"Job.AssertRecordValidity() : Attemped to add dependency record to a job with name equal to the value. Name: {name}, duration: {duration}, value: {value}");
		}

		foreach(int pred in predecessors) {
			if(pred == value) {
				throw new ArgumentException($"Job.AssertRecordValidity() : Attemped to add already existing predecessor. Name: {name}, duration: {duration}, value: {value}");
			}
		}

		foreach(int succ in successors) {
			if(succ == value) {
				throw new ArgumentException($"Job.AssertRecordValidity() : Attemped to add already existing successor. Name: {name}, duration: {duration}, value: {value}");
			}
		}
	}
}