using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Evolution;

public class GameController : BaseSingletonController<GameController> {

	const int INIT_CARD_NUM = 6;

	public const string DidBeginGameNotification = "GameModel.DidBeginGameNotification";
	public const string EvolutionCompleteNotification = "GameModel.EvolutionCompleteNotification";
	public const string DidGetFoodNotification = "GameModel.DidGetFoodNotification";
	public const string DidChangeControlNotification = "GameModel.DidChangeControlNotification";
	public const string DidEndGameNotification = "GameModel.DidEndGameNotification";


	public GameModel gameMod;
	public GameView gameView;

	bool initEvolute;
	bool isInitFood;
	bool isLocalPlayerInitCard = false;
	bool isRemotePlayerInitCard = false;


	public Vector3 latestChoosenFoodPosition;
	public Vector3 latestChoosenCardPosition;
	public int curActingAnimalIndex = -1;

	void Awake(){
		gameMod = new GameModel();
		gameView = GetComponentInChildren<GameView>();
		gameView.init(gameMod);
	}

	// Use this for initialization
	void Start () {
		CheckState();
		//gameMod.Reset();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnEnable ()
	{
		this.AddObserver(OnMatchReady, MatchController.MatchReady);
		this.AddObserver (OnFinishInitMainCards, PlayerController.FinishInitMainCards);
		this.AddObserver (OnFinishInitLocalPlayerCard, PlayerController.FinishInitLocalPlayerCard);
		this.AddObserver (OnFinishInitRemotePlayerCard, PlayerController.FinishInitRemotePlayerCard);
        this.AddObserver(OnInitFood, PlayerController.InitFood);
        this.AddObserver(OnClearFood, PlayerController.ClearFood);
		this.AddObserver (OnAnimalEaten, PlayerView.OnAnimalEaten);
        //this.AddObserver(OnDidBeginGame, GameModel.DidBeginGameNotification);
        //this.AddObserver(OnDidPlaceCard, GameModel.DidPutCardNotification);
        //this.AddObserver(OnDidChangeControl, GameModel.DidChangeControlNotification);
        //this.AddObserver(OnDidEndGame, GameModel.DidEndGameNotification);
        //this.AddObserver(OnRandomCreated, PlayerController.RandomCreated);
        this.AddObserver(OnRequestPutAnimal, PlayerController.RequestCreatAnimal);
		this.AddObserver(OnRequestPassEvolute, PlayerController.RequestPassEvolute);
		this.AddObserver(OnRequestTakeAction, PlayerController.RequestTakeAction);
		this.AddObserver(OnRequestPassHunt, PlayerController.RequestPassHunt);
	}

	void OnDisable ()
	{
		this.RemoveObserver(OnMatchReady, MatchController.MatchReady);
		this.RemoveObserver(OnFinishInitMainCards, PlayerController.FinishInitMainCards);
		this.RemoveObserver (OnFinishInitLocalPlayerCard, PlayerController.FinishInitLocalPlayerCard);
		this.RemoveObserver (OnFinishInitRemotePlayerCard, PlayerController.FinishInitRemotePlayerCard);
        this.RemoveObserver (OnInitFood, PlayerController.InitFood);
        this.RemoveObserver(OnClearFood, PlayerController.ClearFood);
		this.RemoveObserver (OnAnimalEaten, PlayerView.OnAnimalEaten);
        //this.RemoveObserver(OnDidBeginGame, GameModel.DidBeginGameNotification);
        //this.RemoveObserver(OnDidPlaceCard, GameModel.DidPlaceCardNotification);
        //this.RemoveObserver(OnDidChangeControl, GameModel.DidChangeControlNotification);
        //this.RemoveObserver(OnDidEndGame, GameModel.DidEndGameNotification);
        //this.RemoveObserver(OnRandomCreated, PlayerController.RandomCreated);
        this.RemoveObserver(OnRequestPutAnimal, PlayerController.RequestCreatAnimal);
		this.RemoveObserver(OnRequestPassEvolute, PlayerController.RequestPassEvolute);
		this.RemoveObserver(OnRequestTakeAction, PlayerController.RequestTakeAction);
		this.RemoveObserver(OnRequestPassHunt, PlayerController.RequestPassHunt);
	}

	IEnumerator waitInit() {
		Debug.Log("Before Waiting 2 seconds");
		yield return new WaitForSeconds(2);
		Debug.Log("After Waiting 2 Seconds");
		InitEvoluteState();
	}

    //玩家匹配成功响应
	void OnMatchReady (object sender, object args)
	{
		Debug.Log("gameStart");
		StartCoroutine(waitInit());
	}
    


	public int getMainCardNum(){
		return gameMod.mainCardMods.Count;
	}
    
    //初始化进化阶段，给每位玩家分配初始手牌
	void InitEvoluteState(){
		if(!initEvolute && MatchController.Instance.checkHostIsLocal()){
			MatchController.Instance.hostPlayer.CmdInitMainCards();
		}
	}
		
    
    //收到主机玩家初始化牌堆完毕的响应
    void OnFinishInitMainCards(object sender, object args)
    {
		gameMod.Reset();
		foreach (PlayerController player in MatchController.Instance.players){
			player.init();
			Debug.Log("init  player! "+ player.getPlayerId() );
		}
		if(MatchController.Instance.checkHostIsLocal()){
			//Debug.LogError(gameMod.mainCardMods.Count);
			PlayerController hostPlayer = MatchController.Instance.hostPlayer;
			PlayerController clientPlayer = MatchController.Instance.clientPlayer;
			hostPlayer.tempCount = 30;
			hostPlayer.CmdAddCardToPlayer(hostPlayer.getPlayerId(), INIT_CARD_NUM);
			//Debug.LogError(gameMod.mainCardMods.Count);
			hostPlayer.tempCount = 24;
			hostPlayer.CmdAddCardToPlayer(clientPlayer.getPlayerId(),INIT_CARD_NUM);
			//Debug.LogError(gameMod.mainCardMods.Count);
		}
		//Debug.LogError(gameMod.mainCardMods.Count);
		initEvolute = true;
    }



	//收到本地玩家初始化手牌完毕的响应
	void OnFinishInitLocalPlayerCard(object sender, object args)
	{
		Debug.LogWarning("local player init cards !");
		isLocalPlayerInitCard = true;
		if(isLocalPlayerInitCard && isRemotePlayerInitCard){
			resetInitCardState();
			CheckState ();
		}
	}


	//收到远端玩家初始化手牌完毕的响应
	void OnFinishInitRemotePlayerCard(object sender, object args)
	{
		Debug.LogWarning("remote player init cards !");
		isRemotePlayerInitCard = true;
		if(isLocalPlayerInitCard && isRemotePlayerInitCard){
			resetInitCardState();
			CheckState ();
		}
	}

	void resetInitCardState(){
		isLocalPlayerInitCard = false;
		isRemotePlayerInitCard = false;
	}


	public void initCardToMainCard(int skillIdx){
		ConstEnums.Skills skillType =  (ConstEnums.Skills)skillIdx;
		Debug.LogWarning("on init main card " + skillType);
		CardModel card = gameMod.initCards(skillType);
		gameView.initCard(card);
	}


    //结算猎食阶段
    void checkHuntState(){
        //遍历每个玩家，销毁没吃饱的动物
		foreach(PlayerController player in MatchController.Instance.players){
			player.checkAnimalAlive();
            player.resetAnimals();
		}
		Debug.Log("check animal alive for players");
        //清空场上的食物
        gameMod.totalFoodNum = 0;
		if (MatchController.Instance.hostPlayer.isLocalPlayer)
        {
			MatchController.Instance.hostPlayer.CmdClearFood();
        }
        //重置初始化食物标记
        isInitFood = false;
    }


    //新的一轮开始，为每位玩家补牌
	void addCards(){
		foreach(PlayerController player in MatchController.Instance.players){
			player.initNextRound();
			Debug.Log("init Next Round for player!");
			int num = player.getAddCardNum();
		}
		if(MatchController.Instance.checkHostIsLocal()){
			//Debug.LogError(gameMod.mainCardMods.Count);

			PlayerController hostPlayer = MatchController.Instance.hostPlayer;
			PlayerController clientPlayer = MatchController.Instance.clientPlayer;
			int hostAddNum = hostPlayer.getAddCardNum();
			int clientAddNum = clientPlayer.getAddCardNum();
			hostPlayer.tempCount = gameMod.mainCardMods.Count;
			hostPlayer.CmdAddCardToPlayer(hostPlayer.getPlayerId(), hostAddNum);
			//Debug.LogError(gameMod.mainCardMods.Count);
			hostPlayer.tempCount = gameMod.mainCardMods.Count - hostAddNum;
			hostPlayer.CmdAddCardToPlayer(clientPlayer.getPlayerId(),clientAddNum);
			//Debug.LogError(gameMod.mainCardMods.Count);
		}


	}

    //初始化猎食阶段
	void initHuntState(){
		if(!isInitFood){
			gameMod.totalFoodNum = 0;
			if (MatchController.Instance.hostPlayer.isLocalPlayer) {
				MatchController.Instance.hostPlayer.CmdgenerateFood ();
			}
			isInitFood = true;
		}
	}


    //收到主机玩家初始化食物的响应
	void OnInitFood(object sender, object args){
		//List<Vector3> positions = (List<Vector3>)args;
		Vector3 position = (Vector3)args;
		FoodModel food = new FoodModel();
		gameMod.addFood(food);
		gameView.showFood (food,position);

	}


    //收到主机玩家清空食物的响应
    void OnClearFood(object sender, object args)
    {
        Debug.Log("clear food!");
        gameView.clearFood();
        gameMod.clearFood();
    }
    



	public void onCurAnimalChoosen(int animalIdx,Vector3 pos){
		curActingAnimalIndex = animalIdx;
		gameView.setActionBtnActive(animalIdx, pos);
	}

	public void onEnemyAnimalChoosen(int enemyAnimalIndex){
		if (curActingAnimalIndex >= 0 && gameView.actionBtnClicked){
			PlayerController localPlayer = MatchController.Instance.localPlayer;
			bool result = localPlayer.checkAnimalAttck(curActingAnimalIndex,enemyAnimalIndex);

			if (result) {
				localPlayer.CmdAnimalAuto (curActingAnimalIndex, enemyAnimalIndex, localPlayer.getPlayerId ());
				curActingAnimalIndex = -1;
				gameView.actionBtnClicked = false;
			} else {
				localPlayer.CmdEndTurn ();
			}

		}
	}


	void OnAnimalEaten(object sender, object args){
		Debug.Log("animal eaten "+ (int)args);
		int index = (int)args;
		if(_currentState is HuntActiveGameState){
			MatchController.Instance.remotePlayer.playerView.removeAnimal (index);
			MatchController.Instance.remotePlayer.playerMod.removeAnimal (index);
		}else{
			MatchController.Instance.localPlayer.playerView.removeAnimal (index);
			MatchController.Instance.localPlayer.playerMod.removeAnimal (index);
		}
		endTurn ();

	}

	public void endTurn(){
		gameMod.ChangeTurn();
		gameView.setPassBtnActive(false);
		CheckState();
	}

    //动物吃食物
    public void eatFood(int index){
        Debug.Log("eat food"+index);
		gameView.eatFood(index);
		gameMod.eatFood(index);
	}
		
    //获取当前的控制玩家
	public ConstEnums.PlayerId getControl(){
		return gameMod.control;
	}


    //食物被放下时响应，判断食物是否会被动物吃掉
	public void foodEaten(int foodIndex){
		GameObject currentPointedFood = gameView.foods[foodIndex];
		Vector3 foodPosition = currentPointedFood.transform.position;
		List<GameObject> animals = null;
		PlayerController currentPlayer = null;
		foreach (PlayerController playerController in MatchController.Instance.players) {
			if (gameMod.control == playerController.playerMod.playerId) {
				animals = playerController.playerView.animals;
				currentPlayer = playerController;
				break;
			}
		}
		if (animals != null && currentPlayer!=null) {
			bool isFoodEat = false;
			foreach (GameObject animal in animals) {
				if(!isFoodEat){
					float x = animal.GetComponent<SphereCollider>().bounds.size.x/2;
					float y = animal.GetComponent<SphereCollider> ().bounds.size.y;
					float z = animal.GetComponent<SphereCollider> ().bounds.size.z/2;

					Debug.LogWarning ("x:" + x + " y:" + y + " z:" + z);
					AnimalView animalView= animal.GetComponent<AnimalView>();
					Vector3 animalPosition = animal.transform.position;
					float left = animalPosition.x - x;
					float right = animalPosition.x + x;
					float top = animalPosition.z + z;
					float bottom = animalPosition.z - z;
					//				if ((foodPosition.z >= animalPosition.z - z && foodPosition.x <= animalPosition.z + z) && 
					//					(foodPosition.x >= animalPosition.x - x && foodPosition.x <= animalPosition.x + x)) {
					if ((foodPosition.x <  right && foodPosition.x > left) && 
						(foodPosition.z < top && foodPosition.z > bottom) && animalView.isMouseEnter) 
					{
						int animalIndex = animal.GetComponent<AnimalView> ().mod.index;
						if(currentPlayer.checkAnimalFull(animalIndex) == false){
							currentPlayer.CmdEatFood (animalIndex, foodIndex);
							isFoodEat = true;
							Debug.Log("animal "+animalIndex+"eat food "+foodIndex);		
						}else{
							currentPlayer.CmdRefreshFoodPosition (foodIndex, latestChoosenFoodPosition);
							//Debug.Log("refresh to "+latestChoosenFoodPosition);	
						}
					}else {
						currentPlayer.CmdRefreshFoodPosition (foodIndex, latestChoosenFoodPosition);
						//Debug.Log("refresh to "+latestChoosenFoodPosition);	
					}
				}

			}
		} 
	}


	public void setLatestChoosenFoodPosition(int index){
		latestChoosenFoodPosition = gameView.foods[index].transform.position;
	}

	public void setLatestChoosenCardPosition(Vector3 position){
		latestChoosenCardPosition = position;
	}


	void OnRequestPutAnimal (object sender, object args)
	{
		gameMod.ChangeTurn();
		gameView.setPassBtnActive(false);
		CheckState();
	}

	void OnRequestPassEvolute (object sender, object args)
	{
		gameMod.ChangeTurn();
		CheckState();
	}


	void OnRequestTakeAction (object sender, object args)
	{
		gameMod.ChangeTurn();
		gameView.setPassBtnActive(false);
		CheckState();
	}


	void OnRequestPassHunt (object sender, object args)
	{
		gameMod.ChangeTurn();
		CheckState();
	}



	void OnDidChangeControl (object sender, object args)
	{
		CheckState ();
	}

	void OnDidEndGame (object sender, object args)
	{
		CheckState ();
	}

	void CheckState ()
	{
		if (!MatchController.Instance.IsReady){
			//玩家没齐，进入load状态
			Debug.LogWarning("enter load state");
			gameMod.setState(ConstEnums.GameState.None);
			ChangeState<LoadGameState>();
		}else if (gameMod.control == Evolution.ConstEnums.PlayerId.None){
            //游戏结束，进入end状态
            checkHuntState();
            gameMod.checkWinner();
            Debug.LogWarning("enter end state");
			gameMod.setState(ConstEnums.GameState.End);
			ChangeState<EndGameState>();
		}else if(gameMod.state == ConstEnums.GameState.Evolute){
			if(MatchController.Instance.checkAllPlayerPassEvolute()){
				//进入猎食阶段
				Debug.LogWarning("enter hunt state");
				gameMod.setState(ConstEnums.GameState.Hunt);
				initHuntState();
				if(gameMod.control == MatchController.Instance.localPlayer.getPlayerId()){
					ChangeState<HuntActiveGameState>();
				}else{
					ChangeState<HuntPassiveGameState>();
				}
			}else{
				//重复进化阶段
				Debug.LogWarning("repeat evolute state");
				gameMod.setState(ConstEnums.GameState.Evolute);
				if(gameMod.control == MatchController.Instance.localPlayer.getPlayerId()){
					ChangeState<EvoluteActiveGameState>();
				}else{
					ChangeState<EvolutePassiveGameState>();
				}
			}
		}else if(gameMod.state == ConstEnums.GameState.None){
			if(!MatchController.Instance.checkAllPlayerPassEvolute()){
				Debug.LogWarning("enter evolute state");
				gameMod.setState(ConstEnums.GameState.Evolute);
				if(gameMod.control == MatchController.Instance.localPlayer.getPlayerId()){
					ChangeState<EvoluteActiveGameState>();
				}else{
					ChangeState<EvolutePassiveGameState>();
				}
			}
		}else if(gameMod.state == ConstEnums.GameState.Hunt){
			if(MatchController.Instance.checkAllPlayerPassHunt()){
				Debug.LogWarning("next round enter evolute state");
				gameMod.setState(ConstEnums.GameState.Evolute);
				Debug.LogWarning("check animal alive");
				checkHuntState();
				Debug.LogWarning("add next round cards");
				addCards();
				if(gameMod.control == MatchController.Instance.localPlayer.getPlayerId()){
					ChangeState<EvoluteActiveGameState>();
				}else{
					ChangeState<EvolutePassiveGameState>();
				}
			}else{
				Debug.LogWarning("repeat hunt state");
				gameMod.setState(ConstEnums.GameState.Hunt);
				if(gameMod.control == MatchController.Instance.localPlayer.getPlayerId()){
					ChangeState<HuntActiveGameState>();
				}else{
					ChangeState<HuntPassiveGameState>();
				}
			}
		}

		foreach(PlayerController player in MatchController.Instance.players){
			player.setIsControl(gameMod.control == player.getPlayerId());
		}
			
	}

}
