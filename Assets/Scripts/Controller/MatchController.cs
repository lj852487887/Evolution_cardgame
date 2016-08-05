using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Evolution;

public class MatchController : BaseSingletonController<MatchController> {

	public const string MatchReady = "MatchController.Ready";

	public bool IsReady { get { return localPlayer != null && remotePlayer != null; }}
	public PlayerController localPlayer;
	public PlayerController remotePlayer;
	public PlayerController hostPlayer;
	public PlayerController clientPlayer;
	public List<PlayerController> players = new List<PlayerController>();


	void OnEnable ()
	{
		this.AddObserver(OnPlayerStarted, PlayerController.Started);
		this.AddObserver(OnPlayerStartedLocal, PlayerController.StartedLocal);
		this.AddObserver(OnPlayerDestroyed, PlayerController.Destroyed);
	}

	void OnDisable ()
	{
		this.RemoveObserver(OnPlayerStarted, PlayerController.Started);
		this.RemoveObserver(OnPlayerStartedLocal, PlayerController.StartedLocal);
		this.RemoveObserver(OnPlayerDestroyed, PlayerController.Destroyed);
	}

	void OnPlayerStarted (object sender, object args)
	{
		players.Add((PlayerController)sender);
		Configure();
	}

	void OnPlayerStartedLocal (object sender, object args)
	{
		localPlayer = (PlayerController)sender;
		Configure();
	}


	void OnPlayerDestroyed (object sender, object args)
	{
		PlayerController pc = (PlayerController)sender;
		if (localPlayer == pc)
			localPlayer = null;
		if (remotePlayer == pc)
			remotePlayer = null;
		if (hostPlayer == pc)
			hostPlayer = null;
		if (clientPlayer == pc)
			clientPlayer = null;
		if (players.Contains(pc))
			players.Remove(pc);
	}

	void Configure ()
	{
		if (localPlayer == null || players.Count < 2)
			return;

		for (int i = 0; i < players.Count; ++i)
		{
			if (players[i] != localPlayer)
			{
				remotePlayer = players[i];
				break;
			}
		}

		hostPlayer = (localPlayer.isServer) ? localPlayer : remotePlayer;
		clientPlayer = (localPlayer.isServer) ? remotePlayer : localPlayer;
		hostPlayer.setPlayerId(ConstEnums.PlayerId.First);
		clientPlayer.setPlayerId(ConstEnums.PlayerId.Second);

		clientPlayer.setOrientation();

		this.PostNotification(MatchReady);
	}

	public bool checkAllPlayerPassEvolute(){
		int passCount = 0;
		foreach(PlayerController player in players){
			if(player.checkPassEvolute()){
				passCount++;
			}
		}
		if(passCount == players.Count){
			return true;
		}else{
			return false;
		}
		Debug.Log("pass count: "+ passCount);
	}

	public bool checkAllPlayerPassHunt(){
		int count = 0;
		foreach(PlayerController player in players){
			if(player.checkPassHunt()){
				count++;
			}
		}
		Debug.Log("player take action count: "+count);
		if(count == players.Count){
			return true;
		}else{
			return false;
		}
	}

	public PlayerController getPlayerById(ConstEnums.PlayerId playerId){
		PlayerController returnPlayer = null;
		foreach(PlayerController player in players){
			if(player.getPlayerId() == playerId){
				returnPlayer = player;
				break;
			}
		}
		return returnPlayer;
	}


	public bool checkHostIsLocal(){
		return hostPlayer == localPlayer;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
