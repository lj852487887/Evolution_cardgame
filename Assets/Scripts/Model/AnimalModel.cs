
using Evolution;

public class AnimalModel:BaseModel{

	public static int ANIMAL_NUM = 0;

	public int neededFood;
	public int currentFood;
	public int currentFatFood;
	public int fatNum;
	public Property property;

	public AnimalModel(ConstEnums.PlayerId playerId){
		neededFood = 1;
		currentFood = 0;
		ownerId = playerId;
		fatNum = 0;
		currentFatFood = 0;
		ANIMAL_NUM++;
		property = new Property(this);
	}

	public AnimalModel(ConstEnums.PlayerId playerId, int id){
		neededFood = 1;
		currentFood = 0;
		ownerId = playerId;
		index = id;
		fatNum = 0;
		currentFatFood = 0;
		ANIMAL_NUM++;
		property = new Property(this);
	}
	//脂肪
	public void getFat(int num=1){
		fatNum += num;
	}//吸血
	public void raiseNeed(int num=1){
		neededFood+=num;
	}

	public bool eatFood(ConstEnums.Food food,int num =1) {
		if(currentFood + num <= neededFood){
			currentFood += num;
			return false;
		}else{
			currentFood = neededFood;
			if(fatNum>0 && currentFatFood<fatNum){
				int diff = currentFood + num - neededFood;
				if(currentFatFood + diff <= fatNum){
					currentFatFood += diff;
				}else{
					currentFatFood = fatNum;
				}
			}
			return true;
		}
	}

	public void resetFood()
	{
		currentFood = 0;
	}


	public bool isFull(){
		if (neededFood > currentFood + currentFatFood){
			return false;
		}
		else{
			if (neededFood > currentFood )
			{
				int diff = neededFood - currentFood;
				currentFatFood -= diff;
			}
			return true;
		}
	}

	public bool canEat(){
		bool result = true;
		if(neededFood <= currentFood){
			if (fatNum > currentFatFood)
			{
				result = true;
			}else{
				result = false;
			}
		}
		return result;
	}


	public int addSkillToProperty(ConstEnums.Skills skillType){
		switch(skillType){
		case ConstEnums.Skills.Fat:
			property.addProperty(new skill_fat(this));
			break;
		case ConstEnums.Skills.Predator:
			property.addProperty (new skill_predator (this));
			break;
		case ConstEnums.Skills.Aquatic:
			property.addProperty(new skill_aquatic(this));
			break;
		case ConstEnums.Skills.Hide:
			property.addProperty(new skill_hide(this));
			break;
		case ConstEnums.Skills.FastRun:
			property.addProperty(new skill_fastRun(this));
			break;
		case ConstEnums.Skills.Vampire:
			property.addProperty(new skill_vampire(this));
			break;
		case ConstEnums.Skills.Flee:
			property.addProperty(new skill_flee(this));
			break;
		case ConstEnums.Skills.SharpEye:
			property.addProperty(new skill_sharpEye(this));
			break;
		case ConstEnums.Skills.Wisdom:
			property.addProperty(new skill_wisdom(this));
			break;
		default:
			break;
		}
		return neededFood;
	}



}