using UnityEngine;
using System.Collections;
using Evolution;

public class skill_predator : base_skill {

	public skill_predator(){
		skillName = ConstEnums.Skills.Predator;
		Debug.Log("skill predator creat");
	}


	public override bool Attack ()
	{
		return true;
	}
}
