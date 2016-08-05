using UnityEngine;
using System.Collections;

public class HuntActiveGameState : BaseGameState 
{
	public override void Enter ()
	{
		base.Enter ();
		turnTxt.text = "Your Turn!";
		stateTxt.text = "Hunt Active State";
		foodTxt.text = "food:"+gameMod.totalFoodNum;
		checkPassHunt();
		RefreshPlayerLabels();
		LocalPlayer.curGameFoodNum = gameMod.totalFoodNum;
	}

	protected override void AddListeners ()
	{
		base.AddListeners ();
		this.AddObserver(OnFoodViewDrop, "FoodView.MouseDrop");
		this.AddObserver(OnFoodViewMove, "FoodView.MouseDrag");
		this.AddObserver(OnFoodViewChoosen, "FoodView.MouseDown");
        this.AddObserver(OnAnimalChoosen, "AnimalView.MouseDown");
		this.AddObserver(OnEnemyAnimalChoosen, "AnimalView.MouseOnEny");
        this.AddObserver(OnAnimalDroped, "AnimalView.MouseDrop");
        this.AddObserver(OnAnimalDraged, "AnimalView.MouseDrag");
        this.AddObserver(OnPassBtnClicked, "GameView.PassBtnClickedNotification");
	}

	protected override void RemoveListeners ()
	{
		base.RemoveListeners ();
		this.RemoveObserver(OnFoodViewDrop, "FoodView.MouseDrop");
		this.RemoveObserver(OnFoodViewMove, "FoodView.MouseDrag");
		this.RemoveObserver(OnFoodViewChoosen, "FoodView.MouseDown");
        this.RemoveObserver(OnAnimalChoosen, "AnimalView.MouseDown");
		this.RemoveObserver(OnEnemyAnimalChoosen, "AnimalView.MouseOnEny");
        this.RemoveObserver(OnAnimalDroped, "AnimalView.MouseDrop");
        this.RemoveObserver(OnAnimalDraged, "AnimalView.MouseDrag");
        this.RemoveObserver(OnPassBtnClicked, "GameView.PassBtnClickedNotification");
	}

	void checkPassHunt(){
		if(LocalPlayer.checkPassHunt()){
			passBtn.gameObject.SetActive(false);
			LocalPlayer.CmdPassHunt();
		}else{
			passBtn.gameObject.SetActive(true);
		}
	}

	void OnFoodViewChoosen(object sender, object args){
		int index = (int)args;
		gameController.setLatestChoosenFoodPosition(index);
	}

	void OnFoodViewMove(object sender, object args){
		int index = (int)args;
		Vector3 position = gameView.foods [index].transform.position;
		LocalPlayer.CmdMoveFood(index, position);
	}

	void OnFoodViewDrop(object sender, object args){
		int index = (int)args;
		gameController.foodEaten(index);
	}

	void OnAnimalChoosen(object sender, object args){

		Vector3 pos = LocalPlayer.getAnimalPosition((int)args);
		pos.x = pos.x - 1.5f;
		pos.z = pos.z + 1.5f;
		Vector3 screenSpace = Camera.main.WorldToScreenPoint (pos);
		gameController.latestChoosenAnimalIndex = (int)args;
		gameView.setActionBtnActive((int)args, screenSpace);

	}

	void OnEnemyAnimalChoosen(object sender, object args){
		Debug.Log("get enemy choose");
		if (gameController.latestChoosenAnimalIndex >= 0 && gameView.isAttackClicked){
			LocalPlayer.CmdAnimalAuto(gameController.latestChoosenAnimalIndex, (int)args, LocalPlayer.getPlayerId());
			gameController.latestChoosenAnimalIndex = -1;
			gameView.setAttackActive(false);
		}

	}

    void OnAnimalDraged(object sender, object args)
    {
        Debug.Log("animal draged!");
        Vector3 pos = LocalPlayer.getAnimalPosition((int)args);
        LocalPlayer.CmdMoveAnimal((int)args, pos);
    }

    void OnAnimalDroped(object sender, object args)
    {
        Debug.Log("animal droped!");
        Vector3 pos = LocalPlayer.getAnimalPosition((int)args);
        LocalPlayer.CmdMoveAnimal((int)args, pos);
    }

    void OnPassBtnClicked (object sender, object args)
	{
		LocalPlayer.CmdPassHunt();
	}


}