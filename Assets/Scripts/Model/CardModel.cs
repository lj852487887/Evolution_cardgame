using UnityEngine;
using System.Collections;
using Evolution;


public class CardModel:BaseModel{


	public CardModel(){
		ownerId = ConstEnums.PlayerId.None;
		cardType = ConstEnums.Skills.None;
		index = -1;
	}

	public CardModel(ConstEnums.Skills skill){
		ownerId = ConstEnums.PlayerId.None;
		cardType = skill;
		index = -1;
	}

	public CardModel(ConstEnums.PlayerId playerId,int _index){
		ownerId = playerId;
		index = _index;
		cardType = ConstEnums.Skills.None;
	}
		



}
