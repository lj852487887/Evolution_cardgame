using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Evolution;

public abstract class BaseGameState : State 
{
	public GameController gameController;

	public GameView gameView { get { return gameController.gameView; }}
	public GameModel gameMod { get { return gameController.gameMod; }}
	public Text turnTxt { get { return gameView.turnTxt; }}
	public Text stateTxt { get { return gameView.stateTxt; }}
	public Text foodTxt { get { return gameView.foodTxt; }}
	public Text infoTxt { get { return gameView.infoTxt; }}
	public Button passBtn { get { return gameView.passBtn; }}

	public PlayerController LocalPlayer { get { return MatchController.Instance.localPlayer; }}
	public PlayerController RemotePlayer { get { return MatchController.Instance.remotePlayer; }}

    public override void Enter()
    {
        base.Enter();
        turnTxt.text = gameMod.turn.ToString();
    }

    protected virtual void Awake ()
	{
		gameController = GetComponent<GameController>();
	}

	protected void RefreshPlayerLabels ()
	{
		
	}
}
