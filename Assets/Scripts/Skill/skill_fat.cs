using UnityEngine;
using System.Collections;
using Evolution;

public class skill_fat : base_skill {
	public skill_fat(AnimalModel animal){
		thisAnimal = animal;
		Enter ();
	}
	public override void Enter ()
	{
		base.Enter ();
		thisAnimal.getFat ();
		skillName = ConstEnums.Skills.Fat;

		Debug.Log("skill fat creat");
	}

}
