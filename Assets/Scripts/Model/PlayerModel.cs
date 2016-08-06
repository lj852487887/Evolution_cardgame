using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Evolution;


public class PlayerModel {

	public ConstEnums.PlayerId playerId;
	public List<CardModel> cardMods;
	public List<AnimalModel> animalMods;
	public int score;


	public PlayerModel(){
		cardMods = new List<CardModel>();
		animalMods  = new List<AnimalModel>();
	}

	public void addCard(CardModel card){
		cardMods.Add(card);
		card.setIndex(cardMods.Count-1);
	}

	public void addCards( List<CardModel> cards){
		for(int i = 0;i<cards.Count;i++){
			cardMods.Add(cards[i]);
			cards[i].setIndex(cardMods.Count-1);
		}

	}

	public void removeCard(int index){
		cardMods.RemoveAt(index);
		refreshCardsIndex();
	}

	public int getCardNum(){
		return cardMods.Count;
	}

	void refreshCardsIndex(){
		for(int i =0;i<cardMods.Count;i++){
			cardMods[i].setIndex(i);
		}
	}

	void refreshAnimalsIndex(){
		for(int i =0;i<animalMods.Count;i++){
			animalMods[i].index = i;
		}
	}

	public void addAnimal(AnimalModel animal){
		animalMods.Add(animal);
		animal.setIndex(animalMods.Count-1);
	}

	public void removeAnimal(int index){
		animalMods.RemoveAt(index);
		refreshAnimalsIndex();
	}

	public Property getAnimalPropertyByIdx(int index){
		
		return animalMods[index].property;
	}

	public bool checkAnimalAttack(int attackerIdx,int defenderIdx){
		bool result = true;
		Property defenderProperty =  MatchController.Instance.remotePlayer.playerMod.getAnimalPropertyByIdx(defenderIdx);
		result = animalMods [attackerIdx].property.Attack (defenderProperty);
		return result;
	}

    public void resetAnimal(int index)
    {
        animalMods[index].resetFood();
    }

    public bool addFoodToAnimal(int index,int num = 1){
		animalMods[index].eatFood(ConstEnums.Food.Meat,num);
		return animalMods[index].isFull();
	}

	public int addSkillToAniml(int animalIdx,ConstEnums.Skills skill){
		return animalMods[animalIdx].addSkillToProperty(skill);
	}


}


