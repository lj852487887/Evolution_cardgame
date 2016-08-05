using UnityEngine;
using System.Collections;
using Evolution;
public class skill_aquatic : base_skill {

	public skill_aquatic(){
		skillName = ConstEnums.Skills.Aquatic;
		Debug.Log("skill aquatic creat");
	}

	public override bool Defend (Property attacker)
	{
		bool result = base.Defend (attacker);
		bool isAttackerAquatic = attacker.getSkill(ConstEnums.Skills.Aquatic);
		if(isAttackerAquatic){
            result =  false;
		}else{
			result = true;
		}
		return result;
	}
}
