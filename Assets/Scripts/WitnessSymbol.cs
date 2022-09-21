
public class WitnessSymbol {

	public enum SymbolType
    {
		Empty,
		PathHex,
		Square,
		Sun,
		Mino,
		Y,

    }
	public SymbolType selectedSymbol;
	public int idxGroupType;
	public string metadata;
	public WitnessSymbol(string data = "")
    {
		selectedSymbol = SymbolType.Empty;
		idxGroupType = -1;
		metadata = data;
    }
	public WitnessSymbol(SymbolType xSymbol, string data = "")
    {
		selectedSymbol = xSymbol;
		idxGroupType = -1;
		metadata = data;
	}
	public WitnessSymbol(SymbolType xSymbol, int groupIdx, string data = "")
    {
		selectedSymbol = xSymbol;
		idxGroupType = groupIdx;
		metadata = data;
	}
	public WitnessSymbol(int groupIdx, SymbolType xSymbol, string data = "")
    {
		selectedSymbol = xSymbol;
		idxGroupType = groupIdx;
		metadata = data;
	}

}
