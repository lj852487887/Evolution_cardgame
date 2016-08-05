using UnityEngine;
using System.Collections;

public class LoadGameState : BaseGameState 
{
	public override void Enter ()
	{
		base.Enter ();
		stateTxt.text = "Waiting For Players";
		turnTxt.text = "";
		foodTxt.text = "";
		infoTxt.text = "";
		passBtn.gameObject.SetActive(false);
	}

	public override void Exit ()
	{
		base.Exit ();
		RefreshPlayerLabels();
	}
}