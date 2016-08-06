using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Networking;
using System.Collections;
using Evolution;


//玩家控制器，管理玩家行为、所有玩家动物的行动、所有玩家卡牌的行动
using System;


public class PlayerController : NetworkBehaviour {
	
	public const string Started = "PlayerController.Start";
	public const string StartedLocal = "PlayerController.StartedLocal";
	public const string Destroyed = "PlayerController.Destroyed";

	public const string RequestCreatAnimal = "PlayerController.RequestCreatAnimal";
	public const string RequestPassEvolute = "PlayerController.RequestPassEvolute";
	public const string RequestPassHunt = "PlayerController.RequestPassHunt";
	public const string RequestTakeAction = "PlayerController.RequestTakeAction";



	public const string RequestAnimalGetFood = "PlayerController.RequestAnimalGetFood";
	public const string FinishInitMainCards = "PlayerController.FinishInitMainCards";
	public const string FinishInitLocalPlayerCard = "PlayerController.FinishInitLocalPlayerCard";
	public const string FinishInitRemotePlayerCard = "PlayerController.FinishInitRemotePlayerCard";


    public const string InitFood = "PlayerController.InitFood";
    public const string ClearFood = "PlayerController.ClearFood";
    
    public PlayerModel playerMod;
	public PlayerView playerView;

    public bool isControl;
    //是否跳过进化阶段
	public bool isEvolutePass;
    //是否跳过猎食阶段
    public bool isHuntPass;
    //当前游戏场景剩余食物
	public int curGameFoodNum;

	void Awake () {
		playerMod = new PlayerModel();
		playerView = GetComponent<PlayerView>();
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
		

	public override void OnStartClient ()
	{
		base.OnStartClient ();
		this.PostNotification(Started);
	}

	public override void OnStartLocalPlayer ()
	{
		base.OnStartLocalPlayer ();
		this.PostNotification(StartedLocal);
	}


	public void setOrientation(){
		gameObject.transform.rotation = Quaternion.Euler(0f,180.0f,0f);
		if(!isServer){
			Camera.main.transform.rotation = Quaternion.Euler(90f,180f,0f);	
		}
	}

	void OnDestroy ()
	{
		this.PostNotification(Destroyed);
	}

	public void init(){
		isEvolutePass = false;
		isHuntPass = false;
	}


    //开始新一轮，重置之前所有跳过标记
	public void initNextRound(){
		isEvolutePass = false;
		isHuntPass = false;

    }

    //将所有动物的设为没吃饱
    public void resetAnimals()
    {
        //Debug.Log(playerMod.animalMods.Count);
        for (int i = playerMod.animalMods.Count - 1; i >= 0; i--)
        {
            playerMod.resetAnimal(i);
            playerView.resetAnimal(i);
        }
    }


	//检查玩家的动物是否吃饱，杀掉没吃饱的
    public void checkAnimalAlive(){
        //Debug.Log(playerMod.animalMods.Count);
		if(playerMod.animalMods.Count>0){
			for (int i = playerMod.animalMods.Count-1;i>=0;i--)
			{
				Debug.Log(playerMod.animalMods[i].index);
				if (playerMod.animalMods[i].isFull() == false)
				{
					killAnimal(i);
				}
			}
			//playerView.refreshAnimalText();
		}

	}


	public bool checkAnimalFull(int animalIndex){
		return playerMod.animalMods[animalIndex].isFull();
	}

	public void killAnimal(int index){
		playerMod.removeAnimal(index);
		playerView.removeAnimal(index);
	}

	public bool checkPassEvolute(){
		if(isEvolutePass){
			return true;
		}else{
			return false;
		}
	}

	public bool checkPassHunt(){
		if(isHuntPass){
			return true;
		}else{
			return false;
		}
	}

	bool checkCardEmpty(){
		if(playerMod.getCardNum() == 0){
			return true;
		}else{
			return false;
		}
	}


	bool checkNoActionTake(){
		int fullCount = 0;
		foreach(AnimalModel animal in playerMod.animalMods){
			if(animal.isFull()){
				fullCount ++;
			}
		}
		if(fullCount == playerMod.animalMods.Count){
			return true;
		}
		if(curGameFoodNum==0){
			return true;
		}
		return false;
	}

	public void setPlayerId(Evolution.ConstEnums.PlayerId id){
		playerMod.playerId = id;
	}


	public Evolution.ConstEnums.PlayerId getPlayerId(){
		return playerMod.playerId;
	}

	public void setIsControl(bool control){
		isControl = control;
	}



    /*-------------------------------card ------------------------------------*/
    

    //游戏初始化，由主机玩家初始化牌堆
    [Command]
    public void CmdInitMainCards()
    {
        int skillRange = ConstEnums.getSkillCount();
		///////////////
		//skillRange = 3;
		/////////////////
		for (int i =0;i<GameController.MAX_CARD_NUM;i++)
        {
			int randomType = UnityEngine.Random.Range(1, skillRange);
            Debug.Log("init main card " + (ConstEnums.Skills)randomType);
			RpcInitMainCards(randomType,GameController.MAX_CARD_NUM);
        }
    }

    [ClientRpc]
	public void RpcInitMainCards(int cardType,int TotalNum)
    {
		GameController.Instance.initCardToMainCard(cardType);
		Debug.Log("rpc init cards"+ TotalNum);
		if(GameController.Instance.getMainCardNum() == TotalNum){
			Debug.LogWarning("main cards init complete: "+ TotalNum);
			this.PostNotification(FinishInitMainCards);
		}
    }

	public int tempCount;

    //由主机玩家随机发牌
    [Command]
	public void CmdAddCardToPlayer(ConstEnums.PlayerId playerId, int cardNum)
    {
		List<CardModel> mainCardMods =  GameController.Instance.gameMod.mainCardMods;
		//tempCount = mainCardMods.Count;
		for (int i = 0; i < cardNum; ++i){
			if(tempCount>=0){
				int randomIndex = UnityEngine.Random.Range(0,tempCount);
				tempCount --;
				Debug.Log("remove random index:"+ randomIndex+" from main cards");
				RpcInitCardToPlayer(playerId,randomIndex,cardNum,i);
			}else{
				Debug.LogWarning("main cards empty!");
			}
		}
    }

    [ClientRpc]
	public void RpcInitCardToPlayer(ConstEnums.PlayerId playerId,int cardIndex,int TotalNum,int i)
    {
		if(i==0){
			tempCardModels.Clear();
		}
		Debug.Log("card from host to "+playerId+" , no."+i);
		Debug.Log("temp count "+tempCount);
		List<CardModel> mainCardMods =  GameController.Instance.gameMod.mainCardMods;
		Debug.Log("cur maincard count: "+mainCardMods.Count);
		Debug.Log("draw card index: "+cardIndex);
		CardModel cardMod = mainCardMods[cardIndex];
		cardMod.setOwner(playerId);
		mainCardMods.Remove(cardMod);
		tempCardModels.Add(cardMod);
		Debug.Log("remaining main card:"+mainCardMods.Count);

		if(i == TotalNum - 1){
			initCards(playerId,tempCardModels);
		}
		//
		//initCards(playerId,cardModels,cardObjList,TotalNum);

		//rpcTest();
    }

	//public List<GameObject> tempCardObjList = 
	public List<CardModel> tempCardModels = new List<CardModel>();

	void rpcTest(){
		Debug.LogWarning("temp card model count "+tempCardModels.Count);
	}
		

	public void initCards(ConstEnums.PlayerId playerId,List<CardModel> cards){
		PlayerController player = MatchController.Instance.getPlayerById(playerId);
		Debug.Log("init cards to "+ playerId);
		List<GameObject> cardObjList =  GameController.Instance.gameView.removeCard(cards.Count);

		player.playerMod.addCards(cards);
		player.playerView.addCards(cards,cardObjList);
		if(player.playerMod.cardMods.Count == cards.Count){
			if(player.isLocalPlayer){
				this.PostNotification(FinishInitLocalPlayerCard);
			}else{
				this.PostNotification(FinishInitRemotePlayerCard);
			}
		}
	}

	public int getAddCardNum(){
		return playerMod.animalMods.Count + 1;
	}


	public Vector3 getCardPosition(int index){
		return playerView.getCardPosition(index);
	}

	public int checkOnAnimal(Vector3 position,int cardIdx){
		int animalIndex = ConstEnums.NOT_ON_Animal;
		Evolution.ConstEnums.Skills cardType = playerMod.cardMods[cardIdx].cardType;

		foreach (GameObject animal in playerView.animals) {
			Vector3 animalSize = animal.GetComponent<Collider> ().bounds.size;
			Vector3 animalCenter = animal.transform.position;

			float left = animalCenter.x - animalSize.x / 2;
			float right = animalCenter.x + animalSize.x / 2;
			float top = animalCenter.z + animalSize.z / 2;
			float bottom = animalCenter.z - animalSize.z / 2;

			if (position.x <  right && position.x > left) {
				if (position.z < top && position.z > bottom) {
					AnimalModel mod =  (AnimalModel)animal.GetComponent<AnimalView>().mod;
					if(!mod.property.getSkill(cardType)){
						animalIndex = mod.index;
					}else{
						animalIndex = ConstEnums.ANIMAL_HAS_SKILL;
					}
					break;
				}
			}
		}
		return animalIndex;
	}

	public bool checkPosition(Vector3 position){
		Vector3 boardSize = playerView.animalArea.GetComponent<Renderer> ().bounds.size;
		Vector3 boardCenter = playerView.animalArea.transform.position;

		float left = boardCenter.x - boardSize.x / 2;
		float right = boardCenter.x + boardSize.x / 2;
		float top = boardCenter.z + boardSize.z / 2;
		float bottom = boardCenter.z - boardSize.z / 2;
		Debug.Log ("size:" + boardSize);
		Debug.Log ("center:" + boardCenter);
		Debug.Log ("area:(" + top + "," + right + "," + bottom + "," + left + ")");
		Debug.Log ("my position:" + position);
		if (position.x <  right && position.x > left) {
			if (position.z < top && position.z > bottom) {
				return true;
			}
		}
		return false;
	}

	[Command]
	public void CmdPutCard (int cardIndex,Vector3 pos, bool inArea, int animalIdx)
	{
		if (inArea) {
			if (animalIdx>=0) {
				Evolution.ConstEnums.Skills skillType = playerMod.cardMods[cardIndex].cardType;
				Debug.Log("create skill:"+skillType);
				RpcRemoveCard (cardIndex);
				Debug.Log ("i am on animal");
				RpcCreateSkill (animalIdx,skillType);
			} else if(animalIdx == ConstEnums.ANIMAL_HAS_SKILL){
				RpcMoveCard (cardIndex, GameController.Instance.latestChoosenCardPosition);
			} else if(animalIdx == ConstEnums.NOT_ON_Animal){
				RpcRemoveCard (cardIndex);
				RpcCreateAnimal (pos);
			}
		} else {
			RpcMoveCard (cardIndex, GameController.Instance.latestChoosenCardPosition);
		}
	}

	[ClientRpc]
	void RpcCreateSkill(int animalIdx,Evolution.ConstEnums.Skills skillType) {
        Debug.Log("rpc create skill:" + skillType);
        int neededFood = playerMod.addSkillToAniml(animalIdx,skillType);
		playerView.addSkillToAniml(animalIdx,skillType,neededFood);
		Debug.Log( getPlayerId() + " creat skill ");
		this.PostNotification(RequestCreatAnimal);
		//Instantiate (animalStatus, new Vector3 (pos.x, pos.y + 3.0f, pos.z), Quaternion.identity);
	}

	[ClientRpc]
	void RpcRemoveCard (int cardIndex)
	{
		playerMod.removeCard(cardIndex);
		playerView.removeCard(cardIndex);
		Debug.Log( getPlayerId() + " remove card ");
	}

	[ClientRpc]
	void RpcCreateAnimal (Vector3 pos)
	{
		AnimalModel animal = new AnimalModel(getPlayerId());
		playerMod.addAnimal(animal);
		if(this == MatchController.Instance.clientPlayer){
			playerView.showAnimal(animal,pos,true);
		}else{
			playerView.showAnimal(animal,pos,false);
		}

		//Debug.Log( getPlayerId() + " create animal ");
		this.PostNotification(RequestCreatAnimal);
		Debug.Log("create animal "+ animal.index);
	}

	[Command]
	public void CmdMoveCard (int cardIndex,Vector3 pos)
	{
		//Debug.LogWarning("move card!!!");
		RpcMoveCard(cardIndex,pos);
	}

	[ClientRpc]
	void RpcMoveCard (int cardIndex,Vector3 pos)
	{
		playerView.moveCard(cardIndex,pos);
	}

	[Command]
	public void CmdSyncLatestCardPosition(Vector3 position){
		RpcSyncLatestCardPosition (position);
	}
	[ClientRpc]
	void RpcSyncLatestCardPosition(Vector3 position){
		GameController.Instance.setLatestChoosenCardPosition(position);

	}

	/*-------------------------------card end------------------------------------*/

	/*-------------------------------animal------------------------------------*/


	public Vector3 getAnimalPosition(int index){
		return playerView.getAnimalPosition(index);
	}

	public int getAnimalNum(){
		return playerMod.animalMods.Count;
	}

	public bool checkAnimalAttck(int attackerIdx,int defenderIdx){
		return playerMod.checkAnimalAttack(attackerIdx,defenderIdx);
	}

	[Command]
	public void CmdAnimalAttackSuccess(int attackerIdx,int defenderIdx,ConstEnums.PlayerId attackerPlayerId){			
		RpcAnimalAttackSuccess(attackerIdx,defenderIdx,attackerPlayerId);

	}

	[ClientRpc]
	public void RpcAnimalAttackSuccess(int attackerIdx, int defenderIdx,ConstEnums.PlayerId attackerPlayerId){
		if(MatchController.Instance.localPlayer.getPlayerId() == attackerPlayerId){
			Vector3 pos =  MatchController.Instance.remotePlayer.getAnimalPosition (defenderIdx);
			Debug.Log ("local attack  playerid :  " + getPlayerId());
			MatchController.Instance.localPlayer.dealAnimalAttack(attackerIdx,defenderIdx,pos);
		}else{
			Vector3 pos = MatchController.Instance.localPlayer.getAnimalPosition (defenderIdx);
			Debug.Log ("remote attack playerid :  " + getPlayerId());
			PlayerController remotePlayer = MatchController.Instance.remotePlayer;
			remotePlayer.dealAnimalAttack(attackerIdx,defenderIdx,pos);
		}
	}

	void dealAnimalAttack(int attackerIdx, int defenderIdx,Vector3 defenderPos){
		//给该动物吃食物，并获取该动物是否吃饱
		Debug.Log( getPlayerId() + "'s cur animal count:"+playerMod.animalMods.Count);
		Debug.Log(getPlayerId() + "'s animal :"+attackerIdx+ "eat enemy's animal" +  defenderIdx);
		bool isFull = playerMod.addFoodToAnimal(attackerIdx,2);
		playerView.animalAttackAnimation(attackerIdx,defenderIdx,defenderPos,isFull);
	}



	[Command]
	public void CmdEndTurn(){			
		RpcEndTurn ();
	}

	[ClientRpc]
	public void RpcEndTurn(){
		GameController.Instance.endTurn ();
	}


	//同步拖动动物
	[Command]
	public void CmdMoveAnimal (int index,Vector3 pos)
	{
		//Debug.LogWarning("move card!!!");
		RpcMoveAnimal(index,pos);
	}

	[ClientRpc]
	void RpcMoveAnimal (int index,Vector3 pos)
	{
		playerView.moveAnimal(index,pos);
	}

    /*-------------------------------animal end------------------------------------*/


    /*-------------------------------food------------------------------------*/


    //猎食阶段初始化，产生随机食物
    [Command]
    public void CmdgenerateFood()
    {
        int num = UnityEngine.Random.Range(1, 7) + 2;
        while (num > 0)
        {
            GameObject foodArea = GameController.Instance.gameView.foodArea;
            Vector3 foodAreaCenter = foodArea.transform.position;
            Vector3 foodAreaSize = foodArea.GetComponent<Renderer>().bounds.size;
            float left = foodAreaCenter.x - foodAreaSize.x / 2;
            float right = foodAreaCenter.x + foodAreaSize.x / 2;
            float top = foodAreaCenter.z + foodAreaSize.z / 2;
            float bottom = foodAreaCenter.z - foodAreaSize.z / 2;
            Debug.Log("area:(" + top + "," + right + "," + bottom + "," + left + ")");
            Vector3 position = new Vector3(UnityEngine.Random.Range(left, right), 1.4f, UnityEngine.Random.Range(bottom, top));
            RpcShowFood(position);
            num--;
        }
    }
    [ClientRpc]
    public void RpcShowFood(Vector3 position)
    {
		MatchController.Instance.refreshAllPlayerFoodText(true);
        this.PostNotification(InitFood, position);
    }


    //猎食阶段结束化，清空剩余食物
    [Command]
    public void CmdClearFood()
    {
        RpcClearFood();
    }
    [ClientRpc]
    public void RpcClearFood()
    {
		MatchController.Instance.refreshAllPlayerFoodText();
        this.PostNotification(ClearFood);
    }


    //动物吃食物
    [Command]
	public void CmdEatFood (int animalIndex,int foodIndex)
	{
		RpcEatFood(animalIndex,foodIndex);
	}

	[ClientRpc]
	void RpcEatFood (int animalIndex,int foodIndex)
	{
		//Debug.Log("animal "+animalIndex+" eat "+" food "+ foodIndex);
        //给该动物吃食物，并获取该动物是否吃饱
		bool isFull = playerMod.addFoodToAnimal(animalIndex);
		playerView.refreshAnimalFoodText(true);
		if(isFull){
            //如果吃饱，变成吃饱的颜色
			playerView.setAnimalFull(animalIndex);
		}
        //让gameController销毁食物
        GameController.Instance.eatFood(foodIndex);
		this.PostNotification(RequestTakeAction);
		Debug.Log(getPlayerId() + "'s animal eat food");
	}


    //拖动食物
	[Command]
	public void CmdMoveFood(int index,Vector3 pos){
		//Debug.Log ("here move food");
		RpcMoveFood (index, pos);
	}

	[ClientRpc]
	public void RpcMoveFood(int index,Vector3 pos){
		try {
			if(index<GameController.Instance.gameView.foods.Count){
				GameController.Instance.gameView.foods [index].transform.position = pos;
			}
		}
		catch (Exception e) {
			Debug.Log(index);
			Debug.LogError("move food error");
		}  


	}

    //食物复位
    [Command]
    public void CmdRefreshFoodPosition(int foodIndex, Vector3 position)
    {
        RpcMoveFood(foodIndex, position);
    }

    /*-------------------------------food end------------------------------------*/



    /*------------------------------- pass evolute------------------------------------*/

    [Command]
	public void CmdPassEvolute ()
	{
		RpcPassEvolute();
	}

	[ClientRpc]
	void RpcPassEvolute ()
	{
		Debug.Log( getPlayerId() + " pass evolute ");
		isEvolutePass = true;
		this.PostNotification(RequestPassEvolute);
	}

	/*-------------------------------pass evolute end------------------------------------*/
	/*-------------------------------pass hunt------------------------------------*/

	[Command]
	public void CmdPassHunt ()
	{
		RpcPassHunt();
	}

	[ClientRpc]
	void RpcPassHunt ()
	{
		Debug.Log( getPlayerId() + " pass hunt ");
		isHuntPass = true;
		this.PostNotification(RequestPassHunt);
	}


	/*-------------------------------pass hunt end------------------------------------*/





    



}
