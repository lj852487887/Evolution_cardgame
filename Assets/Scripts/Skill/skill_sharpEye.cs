using UnityEngine;
using System.Collections;
using Evolution;
public class skill_sharpEye : base_skill {
	public skill_sharpEye(AnimalModel animal){
		thisAnimal = animal;
		Enter ();
	}
	public override void Enter ()
	{
		base.Enter ();
		skillName = ConstEnums.Skills.SharpEye;
		Debug.Log("skill sharp eye creat");
	}


}
