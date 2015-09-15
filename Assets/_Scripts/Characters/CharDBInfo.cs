using UnityEngine;
using System.Collections;

public class CharDBInfo {
	private StatsHolder _transformStats;

	public CharDBInfo(StatsHolder transformStats){
		_transformStats = transformStats;
	}
	
	public StatsHolder transformationStatsBuff{
		get{return _transformStats;}
	}
}
