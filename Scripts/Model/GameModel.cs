using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Evolution;


public class GameModel{

	public ConstEnums.GameState state { get; private set; }
	public ConstEnums.PlayerId control { get; private set; }
	public ConstEnums.PlayerId winner { get; private set; }

	public static int MAX_CARD_NUM = 30;
    public static int MAX_PREDATOR_CARD_NUM = 15;
    public static int MAX_AQUATIC_CARD_NUM = 10;
    public static int MAX_FAT_CARD_NUM = 5;

    public List<CardModel> mainCardMods;
	public List<FoodModel> foodMods;
	public int totalFoodNum;
	public int turn;
	public bool isLastRound;


	public GameModel ()
	{
		mainCardMods = new List<CardModel>();
		foodMods = new List<FoodModel>();
	}

	public void Reset ()
	{
		control = ConstEnums.PlayerId.First;
		winner = ConstEnums.PlayerId.None;
		state = ConstEnums.GameState.None;
		turn = 0;
		Debug.Log("reset total card:"+mainCardMods.Count);
	}

    public CardModel initCards(ConstEnums.Skills cardType)
    {
        CardModel card = new CardModel(cardType);
        mainCardMods.Add(card);
		Debug.LogWarning("main card add card "+ cardType);
		Debug.LogWarning("main card count "+ mainCardMods.Count);
        return card;
    }

	public void setState(ConstEnums.GameState _state){
		state = _state;
	}
		

	public void addFood(FoodModel food){
		totalFoodNum++;
		foodMods.Add(food);
		food.setIndex(foodMods.Count-1);
		food.setOwner(ConstEnums.PlayerId.None);
	}

	public void eatFood(int index){
		totalFoodNum--;
		foodMods.RemoveAt(index);
		refreshFoodsIndex();
	}

    public void clearFood()
    {
		if(foodMods.Count>0){
			for (int i = foodMods.Count - 1; i > -0; i--)
			{
				foodMods.RemoveAt(i);
			}
			totalFoodNum = 0;
			foodMods.Clear();
		}

    }

    void refreshFoodsIndex(){
		for(int i =0;i<foodMods.Count;i++){
			foodMods[i].index = i;
		}
	}

	public void ChangeTurn ()
	{
		CheckIsLastRound();
		CheckForGameOver();
		if(control != ConstEnums.PlayerId.None){
			if(control == ConstEnums.PlayerId.Second){
				control = ConstEnums.PlayerId.First;
			}else{
				control++;
			}
			turn++;
		}

		Debug.Log("turn: "+turn+" change to "+control);
	}



	void CheckForGameOver ()
	{
		
		if (isLastRound==true && state == ConstEnums.GameState.Hunt && MatchController.Instance.checkAllPlayerPassHunt())
		{
			Debug.Log("last turn:"+isLastRound);
			control = ConstEnums.PlayerId.None;
		}
	}

	void CheckIsLastRound()
	{
		if(mainCardMods.Count==0){
			isLastRound = true;
		}else{
			isLastRound = false;
		}
	}
		

	public void checkWinner(){
		int maxAnimalCount = 0;
		ConstEnums.PlayerId winnerId = ConstEnums.PlayerId.None;
		foreach(PlayerController player in MatchController.Instance.players){
			int animalNum = player.getAnimalNum();
			if(animalNum > maxAnimalCount){
				maxAnimalCount = animalNum;
				winnerId = player.getPlayerId();
            }else if(animalNum == maxAnimalCount)
            {
                winnerId = ConstEnums.PlayerId.None;
            }
		}
        Debug.Log("winner:"+ winnerId+" with "+ maxAnimalCount + " animal");
		winner = winnerId;
	}



}
