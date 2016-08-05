using UnityEngine;
using System.Collections;

public class EvolutePassiveGameState : BaseGameState 
{
	public override void Enter ()
	{
		base.Enter ();
		turnTxt.text = "Opponent's Turn!";
		stateTxt.text = "Evolute Passive State";
		foodTxt.text = "no food";
		passBtn.gameObject.SetActive(false);
		RefreshPlayerLabels();
	}
}