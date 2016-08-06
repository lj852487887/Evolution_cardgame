﻿using UnityEngine;
using System.Collections;
using Evolution;

public class skill_hide : base_skill {
	public skill_hide(AnimalModel animal){
		thisAnimal = animal;
		Enter ();
	}
	public override void Enter ()
	{
		base.Enter ();
		skillName = ConstEnums.Skills.Hide;
		Debug.Log("skill hide creat");
	}
	public override bool Defend (Property attacker,Property defender)
	{
		bool result = base.Defend (attacker, defender);
		bool isAttackerSharpEye = attacker.getSkill(ConstEnums.Skills.SharpEye);
		if(isAttackerSharpEye){
			result =  false;
		}else{
			result = true;
		}
		return result;
	}
}
