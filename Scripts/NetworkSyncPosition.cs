using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class NetworkSyncPosition: NetworkBehaviour {

	[SyncVar]
	private Vector3 syncPos;

	[SerializeField] Transform myTransform;
	[SerializeField] float lerpRate = 15;

	private Vector3 lastPos;
	private float threshold = 0.5f;

	GameController gameController;
	MatchController matchController;
	CardView cardView;

	// Use this for initialization
	void Start () {
		gameController = GameObject.FindGameObjectWithTag("gameController").GetComponent<GameController>();
		matchController = GameObject.FindGameObjectWithTag("matchController").GetComponent<MatchController>();
		cardView = GetComponent<CardView>();

	}

	// Update is called once per frame
	void Update () {
		LerpPosition ();
	}

	void FixedUpdate(){
		TransmitPosition ();

	}

	public bool checkActive(){
		if(cardView.mod.ownerId == matchController.localPlayer.getPlayerId() && 
			gameController.getControl() == matchController.localPlayer.getPlayerId()){
			return true;
		}else{
			return false;
		}
	}

	void LerpPosition(){
		if(!isLocalPlayer){
			myTransform.position = Vector3.Lerp (myTransform.position, syncPos, Time.deltaTime * lerpRate);
		}
	}

	[Command]
	void CmdProvidePositionToServer(Vector3 pos){
		syncPos = pos;
	}

	[ClientCallback]
	void TransmitPosition(){
		if(checkActive() && Vector3.Distance(myTransform.position,lastPos)> threshold){
			CmdProvidePositionToServer (myTransform.position);
			lastPos = myTransform.position;
		}

	}
}
