// Its only use is intended in JobsParser. Because of that I can afford to do some nongeneral implementation specifics.
class BidirectionalDictionary {
	// Bidirectional dictionary name <-> id
	readonly List<string> idToName = new(); // id -> name
	readonly Dictionary<string, int> nameToId = new(); // name -> id
	
	public void Add(int id, string name) {
		// Strict invariant: adding ids in incremental way as we read the file.
		if (id != idToName.Count()) {
			throw new ArgumentException();
		}

		if(nameToId.ContainsKey(name)) {
			throw new ArgumentException();
		}

		idToName.Add(name);
		nameToId[name] = id;
	}

	public string IdToName(int id) {
		return idToName[id];
	}

	public int NameToId(string name) {
		return nameToId[name];
	}

	public List<string> GetIdToNameList() => idToName; // Logger class needs to save the direction after the lifespan of JobsParser ends.
}
