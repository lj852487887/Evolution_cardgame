using UnityEngine;
using System.Collections;

public class HuntPassiveGameState : BaseGameState 
{
	public override void Enter ()
	{
		base.Enter ();
		turnTxt.text = "Opponent's Turn!";
		stateTxt.text = "Hunt Passive State";
		foodTxt.text = "food:"+gameMod.totalFoodNum;
		passBtn.gameObject.SetActive(false);
		RefreshPlayerLabels();
	}
}