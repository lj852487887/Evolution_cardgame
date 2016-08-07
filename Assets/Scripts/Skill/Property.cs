using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Evolution;
public class Property: MonoBehaviour{
	private List<base_skill> skills=new List<base_skill>();
	public AnimalModel thisAnimal;
	public Property(AnimalModel remoteAnimal){
		thisAnimal = remoteAnimal;
	}
	public int getPropertyNumber(){
		return skills.Count;
	}


	public bool getSkill(ConstEnums.Skills name)
	{
		foreach(base_skill skill in skills){
			//Debug.Log ("*******"+skill.getName ());
			if(skill.getName() == name){
				Debug.Log (name+" exist!");
				return true;
			}
		}
		return false;
	}

	public bool addProperty(base_skill newSkill){
		if(newSkill.getName() != ConstEnums.Skills.Fat){
			if(getSkill(newSkill.getName()) == false){
				skills.Add (newSkill);
				Debug.Log (newSkill.getName()+"added!");
				return true;
			}else{
				return false;
			}
		}else{
			skills.Add (newSkill);
			Debug.Log (newSkill.getName()+"added!");
			return true;		
		}
	}
		

	public void showProperty(){
		foreach (base_skill o in skills) {
			//Debug.Log (o.getName() + " ");
		}
	}


	public List<base_skill> getPropertyList(){
		return skills;
	}

	public bool Attack(Property defender){
		Debug.Log("check attack: ");
		Debug.Log("attacker skills:");
		showProperty();
		Debug.Log("defender skills:");
		defender.showProperty();
		bool result = false;
		foreach (base_skill o in skills) {
			result = o.Attack ();
			if (result) {
				break;
			}
		}
		if (!result) {//不具备攻击条件
			return false;
		}
		Debug.Log("can attack:"+result);
		//具备攻击条件，开始攻击
		if (defender.Defend (this,defender)) {//防御成功则攻击失败
			result=false;
		}
		return result;

	}

	public bool Defend(Property attacker,Property defender){
		bool result = false;//默认防御失败
		bool hasWisdom=attacker.getSkill(ConstEnums.Skills.Wisdom);
		foreach (base_skill o in skills) {//各个技能依次开始防御
			Debug.Log("check defend skill:"+o.getName());
			result = o.Defend (attacker,defender);
			if (result) {//若其中有一项技能防御成功过则通过
				if(hasWisdom){//若有智慧，本次防御无视
					result = false;
					hasWisdom = false;
					continue;
				}
				break;
			}
		}
		return result;
	}

	public bool canAttack(){
		if (thisAnimal.isFull()) {
			Debug.Log ("full");
			return false;
		}
		showProperty ();
		//Debug.Log ("could attack???"+getSkill (ConstEnums.Skills.Predator));
		return getSkill (ConstEnums.Skills.Predator);
	}
}
