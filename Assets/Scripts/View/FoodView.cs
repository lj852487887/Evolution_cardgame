using UnityEngine;
using System.Collections;

public class FoodView : BaseDraggtableView {
	Material skin;

	void Update () {
		transform.Rotate (new Vector3 (15, 30, 45) * Time.deltaTime);
	}

	public override void init (BaseModel _mod)
	{
		base.init (_mod);
		skin = GetComponentInChildren<FoodSkin>().gameObject.GetComponent<Renderer>().material;
		originalColor = skin.color;
	}

	public virtual void OnMouseEnter()  
	{  
		isMouseEnter = true;
		if(checkActive()){
			skin.color = mouseOverColor;
			this.PostNotification (MouseEnter);
		}
	}


	public override void OnMouseExit ()
	{
		isMouseEnter = false;
		if(checkActive()){
			skin.color = originalColor;
			this.PostNotification (MouseExit);
		}
	}

	public override bool checkActive ()
	{
		if( GameController.Instance.getControl() == MatchController.Instance.localPlayer.getPlayerId()){
			return true;
		}else{
			return false;
		}
	}

	FoodView(){
		MouseDrop = "FoodView.MouseDrop";
		MouseDown = "FoodView.MouseDown";
		MouseEnter = "FoodView.MouseEnter";
		MouseExit = "FoodView.MouseExit";
		MouseDrag = "FoodView.MouseDrag";	
	}
}


