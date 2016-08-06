﻿using UnityEngine;
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
		bool result = base.Defend (attacker, defender);
		bool isAttackerAquatic = attacker.getSkill(ConstEnums.Skills.Aquatic);
		bool isDefenderAquatic = defender.getSkill(ConstEnums.Skills.Aquatic);
		if(isDefenderAquatic && isDefenderAquatic){
			result =  false;
		}else{
			result = true;
		}
		return result;
	}

}
