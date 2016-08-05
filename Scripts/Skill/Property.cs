using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Evolution;
public class Property: MonoBehaviour{
	private List<base_skill> skills=new List<base_skill>();

	public Property(){
		
	}
	public int getPropertyNumber(){
		return skills.Count;
	}


	public bool getSkill(ConstEnums.Skills name)
	{
		foreach(base_skill skill in skills){
			if(skill.getName() == name){
				Debug.Log (name+" exist!");
				return true;
			}
		}
		return false;
	}
		

	public bool addProperty(base_skill skill){
		if(skill.getName() == ConstEnums.Skills.Fat){
			skills.Add (skill);
			Debug.Log (skill.getName()+"added!");
			return true;
		}else{
			if(!getSkill(skill.getName())){
				skills.Add (skill);
				Debug.Log (skill.getName()+"added!");
				return true;
			}else{
				return false;
			}

		}

	}
		

	public void showProperty(){
		foreach (base_skill o in skills) {
			Debug.Log (o.getName() + " ");
		}
	}

	public List<base_skill> getPropertyList(){
		return skills;
	}

	public bool canAttack(){
		bool result = false;
		foreach (base_skill o in skills) {
			result = o.Attack ();
			if (result) {
				break;
			}
		}
		return result;
	}

	public bool Attack(Property defender){
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
		//具备攻击条件，开始攻击
		if (defender.Defend (this)) {//防御成功则攻击失败
			result=false;
		}
		return result;

	}

	public bool Defend(Property attacker){
		bool result = false;//默认防御失败
		foreach (base_skill o in skills) {//各个技能依次开始防御
			result = o.Defend (attacker);
			if (result) {//若其中有一项技能防御成功过则通过
				break;
			}
		}
		return result;
	}
}
