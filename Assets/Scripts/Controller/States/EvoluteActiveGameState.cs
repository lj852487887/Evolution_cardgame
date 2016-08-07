using UnityEngine;
using System.Collections;

public class EvoluteActiveGameState : BaseGameState 
{
	public override void Enter ()
	{
		base.Enter ();
		turnTxt.text = "Your Turn!";
		stateTxt.text = "Evolute Active State";

		checkPassEvolute();
		RefreshPlayerLabels();
	}

	protected override void AddListeners ()
	{
		base.AddListeners ();
		this.AddObserver(OnCardChoosen, "CardView.MouseDown");
		this.AddObserver(OnAnimalChoosen, "AnimalView.MouseDown");
		this.AddObserver(OnCardDraged, "CardView.MouseDrag");
		this.AddObserver(OnCardDroped, "CardView.MouseDrop");
		this.AddObserver(OnAnimalDroped, "AnimalView.MouseDrop");
		this.AddObserver(OnAnimalDraged, "AnimalView.MouseDrag");
		this.AddObserver(OnPassBtnClicked, "GameView.PassBtnClickedNotification");
	}

	protected override void RemoveListeners ()
	{
		base.RemoveListeners ();
		this.RemoveObserver(OnCardChoosen, "CardView.MouseDown");
		this.RemoveObserver(OnAnimalChoosen, "AnimalView.MouseDown");
		this.RemoveObserver(OnCardDraged, "CardView.MouseDrag");
		this.RemoveObserver(OnCardDroped, "CardView.MouseDrop");
		this.RemoveObserver(OnAnimalDroped, "AnimalView.MouseDrop");
		this.RemoveObserver(OnAnimalDraged, "AnimalView.MouseDrag");
		this.RemoveObserver(OnPassBtnClicked, "GameView.PassBtnClickedNotification");
	}

	void checkPassEvolute(){
		if(LocalPlayer.checkPassEvolute()){
			passBtn.gameObject.SetActive(false);
			LocalPlayer.CmdPassEvolute();
		}else{
			passBtn.gameObject.SetActive(true);
		}
	}

	void OnCardChoosen(object sender, object args){
		Vector3 pos =  LocalPlayer.getCardPosition((int)args);
		LocalPlayer.CmdSyncLatestCardPosition (pos);
	}

	void OnCardDraged (object sender, object args)
	{

		//Debug.Log("card draged!");
		//Debug.Log((string)sender);
		Vector3 pos =  LocalPlayer.getCardPosition((int)args);

		LocalPlayer.CmdMoveCard((int)args,pos);
	}

	void OnCardDroped (object sender, object args)
	{
		//Debug.Log(AnimalView.MouseDrop);
		//Debug.Log(CardView.MouseDrop);

		Vector3 pos =  LocalPlayer.getCardPosition((int)args);
		bool inArea = LocalPlayer.checkPosition (pos);
		int animalIdx = LocalPlayer.checkOnAnimal (pos,(int)args);
		Debug.Log ("on animal: " + animalIdx);

		LocalPlayer.CmdPutCard((int)args, pos, inArea, animalIdx);

	}


	void OnAnimalChoosen(object sender, object args) {
	}

	void OnAnimalDraged (object sender, object args)
	{
		Debug.Log("animal draged!");
		Vector3 pos =  LocalPlayer.getAnimalPosition((int)args);
		LocalPlayer.CmdMoveAnimal((int)args,pos);
	}

	void OnAnimalDroped (object sender, object args)
	{
		Debug.Log("animal droped!");
		Vector3 pos =  LocalPlayer.getAnimalPosition((int)args);
		LocalPlayer.CmdMoveAnimal((int)args,pos);
	}


	void OnPassBtnClicked (object sender, object args)
	{
		Debug.Log(LocalPlayer.getPlayerId() + " pass btn clicked");
		LocalPlayer.CmdPassEvolute();
	}
}