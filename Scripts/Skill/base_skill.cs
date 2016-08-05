using UnityEngine;
using System.Collections;
using Evolution;
public abstract class base_skill 
{
	public int checkOrder;
	public ConstEnums.Skills skillName;

	public base_skill(){
		//Debug.Log("base skill start");
		skillName = ConstEnums.Skills.None;
	}


	//主动攻击型属性  食肉  伏击 等
	//默认总是失败 因为初始是食草动物
	//仅用来判断当前条件是否具备攻击动作的条件
	public virtual bool Attack ()
	{
		return false;
	}

	//被动防御型属性  伪装 飞奔 水生 等
	//默认没有防御
	//每项防御都会寻找各自有可能攻破本防御的技能，若找到了则防御成功，否则防御失败
	public virtual bool Defend (Property attacker)
	{
		return false;
	}

	//其余被动属性 胎生 放牧 等
	//默认总是成功
	public virtual bool PassiveAction ()
	{
		return true;
	}

	//其余非攻击型主动属性  特化 掠食 等
	//默认总是成功
	public virtual bool ActiveAction (AnimalModel animal)
	{
		return true;
	}

	public void setName(ConstEnums.Skills name){
		skillName = name;
	}

	public ConstEnums.Skills getName(){
		return skillName;
	}
}