using UnityEngine;
using System.Collections;
using Evolution;

public class EndGameState : BaseGameState 
{
	public override void Enter ()
	{
		base.Enter ();
		stateTxt.text = "End Game State";
		foodTxt.text = "food:"+gameMod.totalFoodNum;
		if (gameMod.winner == ConstEnums.PlayerId.None)
		{
			infoTxt.text = "Tie Game!";
		}
		else if (gameMod.winner == LocalPlayer.playerMod.playerId)
		{
			infoTxt.text = "You Win!";
		}
		else
		{
			infoTxt.text = "You Lose!";
		}

		RefreshPlayerLabels();

		if (!LocalPlayer.isServer){
			//StartCoroutine(Restart());
		}
			
	}

	IEnumerator Restart ()
	{
		yield return new WaitForSeconds(5);
		//LocalPlayer.CmdCreatRandom();
	}
}