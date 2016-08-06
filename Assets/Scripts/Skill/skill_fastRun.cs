using UnityEngine;
using System.Collections;
using Evolution;
public class skill_fastRun : base_skill {
	public skill_fastRun(AnimalModel animal){
		thisAnimal = animal;
		Enter ();
	}
	public override void Enter ()
	{
		base.Enter ();
		skillName = ConstEnums.Skills.FastRun;
		Debug.Log("skill fast run creat");
	}

	public override bool Defend (Property attacker,Property defender)
	{
		bool result = base.Defend (attacker, defender);
		bool isAttackerAquatic = attacker.getSkill(ConstEnums.Skills.Aquatic);
		bool isDefenderAquatic = defender.getSkill(ConstEnums.Skills.Aquatic);
		int attackerSkillNum = attacker.getPropertyNumber ();
		int defenderSkillNum = defender.getPropertyNumber ();
		if(defenderSkillNum>attackerSkillNum){
			result =  false;
		}else{
			result = true;
		}
		return result;
	}

}
