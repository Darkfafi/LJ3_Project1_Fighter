using UnityEngine;
using System.Collections;

public class CharDBInfo {

	private Animator _charAnimator;
	private StatsHolder _transformStats;
	private SpecialAttack _specialAttack;

	public CharDBInfo(Animator animator, StatsHolder transformStats,SpecialAttack specialAttack){
		_charAnimator = animator;
		_transformStats = transformStats;
		_specialAttack = specialAttack;
	}

	public Animator animator{
		get{return _charAnimator;}
	}
	public StatsHolder transformationStatsBuff{
		get{return _transformStats;}
	}
	public SpecialAttack specialAttack{
		get{return _specialAttack;}
	}
}
