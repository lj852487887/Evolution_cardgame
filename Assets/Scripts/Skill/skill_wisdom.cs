using UnityEngine;
using System.Collections;
using Evolution;
public class skill_wisdom : base_skill {
	public skill_wisdom(AnimalModel animal){
		thisAnimal = animal;
		Enter ();
	}
	public override void Enter ()
	{
		base.Enter ();
		thisAnimal.raiseNeed ();
		skillName = ConstEnums.Skills.Wisdom;
		Debug.Log("skill wisdom creat");
	}


}
