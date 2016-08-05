
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
		property = new Property();
	}

	public AnimalModel(ConstEnums.PlayerId playerId, int id){
		neededFood = 1;
		currentFood = 0;
		ownerId = playerId;
		index = id;
		fatNum = 0;
		currentFatFood = 0;
		ANIMAL_NUM++;
		property = new Property();
	}




	public bool eatFood(ConstEnums.Food food,int num =1) {
		if(currentFood<neededFood){
			if(currentFood + num > neededFood){
				currentFood = neededFood;
			}else{
				currentFood += num;
			}
			return true;
		}else{
			return false;
		}
	}

    public void resetFood()
    {
        currentFood = 0;
    }

    public bool isFull(){
        if (neededFood > currentFood + currentFatFood)
        {
            return false;
        }
        else{
            if (neededFood > currentFood)
            {
                currentFatFood = currentFatFood - (neededFood - currentFood);
                if (currentFatFood >= fatNum)
                {
                    currentFatFood = fatNum;
                }
            }
            return true;
        }
    }

	public void addSkillToProperty(ConstEnums.Skills skillType){
		switch(skillType){
		case ConstEnums.Skills.Fat:
			property.addProperty(new skill_fat());
			break;
		case ConstEnums.Skills.Predator:
			property.addProperty(new skill_predator());
			break;
		case ConstEnums.Skills.Aquatic:
			property.addProperty(new skill_aquatic());
			break;
		case ConstEnums.Skills.Hide:
			property.addProperty(new skill_hide());
			break;
		default:
			break;
		}
	}



}