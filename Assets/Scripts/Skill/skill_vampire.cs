using UnityEngine;
using System.Collections;
using Evolution;
public class skill_vampire : base_skill {
	public skill_vampire(AnimalModel animal){
		thisAnimal = animal;
		Enter ();
	}
	public override void Enter ()
	{
		base.Enter ();
		thisAnimal.raiseNeed ();
		thisAnimal.raiseNeed ();
		skillName = ConstEnums.Skills.Vampire;
		Debug.Log("skill wisdom creat");
	}


}
