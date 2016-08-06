using UnityEngine;
using System.Collections;
using Evolution;
public class skill_aquatic : base_skill {
	public skill_aquatic(AnimalModel animal){
		thisAnimal = animal;
		Enter ();
	}
	public override void Enter ()
	{
		base.Enter ();
		skillName = ConstEnums.Skills.Aquatic;
		Debug.Log("skill aquatic creat");
	}

	public override bool Defend (Property attacker,Property defender)
	{
		Debug.Log("has aquatic!!!");
		bool result = base.Defend (attacker, defender);
		bool isAttackerAquatic = attacker.getSkill(ConstEnums.Skills.Aquatic);
		bool isDefenderAquatic = defender.getSkill(ConstEnums.Skills.Aquatic);
		if(isDefenderAquatic && isDefenderAquatic){
			Debug.Log("botn aquatic or botn not!!! attack success");
			result =  false;
		}else{
			Debug.Log("aquatic!!!  attack fail");
			result = true;
		}
		return result;
	}

}
