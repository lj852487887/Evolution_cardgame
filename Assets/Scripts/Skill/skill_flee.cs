using UnityEngine;
using System.Collections;
using Evolution;

public class skill_flee : base_skill {
	public skill_flee(AnimalModel animal){
		thisAnimal = animal;
		Enter ();
	}
	public override void Enter ()
	{
		base.Enter ();
		skillName = ConstEnums.Skills.Flee;
		Debug.Log("skill flee creat");
	}
	public override bool Defend (Property attacker,Property defender)
	{
		bool result = base.Defend (attacker, defender);
		int rnd = Random.Range (0, 99);
		if (rnd > 49) {
			result = true;
		} else {
			result = false;
		}
		return result;
	}
}
