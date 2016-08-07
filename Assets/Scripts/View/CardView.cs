using UnityEngine;
using System.Collections;
using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using DG.Tweening;
using Evolution;

public class CardView: BaseDraggtableView {
	public Texture predator,hide,aquatic,fat,vampire,sharpeye,wisdom,flee,fastrun;

	CardView(){
		MouseDrop = "CardView.MouseDrop";
		MouseDown = "CardView.MouseDown";
		MouseEnter = "CardView.MouseEnter";
		MouseExit = "CardView.MouseExit";
		MouseDrag = "CardView.MouseDrag";
	}
	public Vector3 originPos;

	public override void init (BaseModel _mod)
	{
		base.init (_mod);

		if(mod.ownerId == MatchController.Instance.hostPlayer.getPlayerId()){
			Vector3 r = gameObject.transform.eulerAngles;
			r.y = 180;
			gameObject.transform.eulerAngles = r;
		}

		switch(mod.cardType){
		case ConstEnums.Skills.Fat:
			GetComponent<Renderer> ().material.SetTexture ("_MainTex", fat);
			break;
		case ConstEnums.Skills.Predator:
			GetComponent<Renderer> ().material.SetTexture ("_MainTex", predator);
			break;
		case ConstEnums.Skills.Hide:
			GetComponent<Renderer> ().material.SetTexture ("_MainTex", hide);
			break;
		case ConstEnums.Skills.Aquatic:		
			GetComponent<Renderer> ().material.SetTexture ("_MainTex", aquatic);
			break;
		case ConstEnums.Skills.Vampire:		
			GetComponent<Renderer> ().material.SetTexture ("_MainTex", vampire);
			break;
		case ConstEnums.Skills.FastRun:		
			GetComponent<Renderer> ().material.SetTexture ("_MainTex", fastrun);
			break;
		case ConstEnums.Skills.SharpEye:		
			GetComponent<Renderer> ().material.SetTexture ("_MainTex", sharpeye);
			break;
		case ConstEnums.Skills.Wisdom:		
			GetComponent<Renderer> ().material.SetTexture ("_MainTex", wisdom);
			break;
		case ConstEnums.Skills.Flee:		
			GetComponent<Renderer> ().material.SetTexture ("_MainTex", flee);
			break;
		}
	}


	public override void OnMouseEnter()  
	{  

	}

	public override void OnMouseExit()
	{  

	}


	bool viewCard = false ;


	public override IEnumerator OnMouseDown ()
	{
		
		isMouseDown = true;
		if(checkActive() && !viewCard){
			this.PostNotification (MouseDown,mod.index);
			Vector3 screenSpace = Camera.main.WorldToScreenPoint(transform.position);

			var offset = transform.position - Camera.main.ScreenToWorldPoint(
				new Vector3(Input.mousePosition.x, 
					Input.mousePosition.y, 
					screenSpace.z));  

			while (Input.GetMouseButton(0))  
			{  
				if(isMouseDown == true){
					Vector3 curScreenSpace = new Vector3(Input.mousePosition.x, 
						Input.mousePosition.y, 
						screenSpace.z);  
					var curPosition = Camera.main.ScreenToWorldPoint(curScreenSpace) + offset;  
					transform.position = curPosition; 
					//Debug.Log(MouseDrag);
					this.PostNotification (MouseDrag, mod.index);
					//Debug.LogWarning("mouse move!!!");

					yield return new WaitForFixedUpdate();  
				}

			}  
		}else if(mouseOnEnemy()) {
			Debug.Log("mouse on enemy post"+ MouseOnEny);
			this.PostNotification (MouseOnEny, mod.index);
		}
	}

	public override void OnMouseUp ()
	{
		float disOrigin = Vector3.Distance(gameObject.transform.position,originPos);
		float disShow = Vector3.Distance(gameObject.transform.position,GameController.Instance.cardShowPos.position);
		Debug.Log(disOrigin);
		Debug.Log(disShow);
		if(disOrigin>0.1 && !viewCard){
			base.OnMouseUp ();
		}else if(disOrigin<0.1 && !viewCard){
			Transform cardShowTrans = GameController.Instance.cardShowPos;
			gameObject.transform.DOMove(cardShowTrans.position,0.3f);
			gameObject.transform.DOScaleX(9,0.3f);
			gameObject.transform.DOScaleZ(14,0.3f);
			viewCard = true;
		}else if(disShow<0.1 && viewCard){
			gameObject.transform.DOMove(originPos,0.3f);
			gameObject.transform.DOScaleX(0.9f,0.3f);
			gameObject.transform.DOScaleZ(1.4f,0.3f);
			viewCard = false;
		}
	}



    public override bool checkActive()
    {
        bool result = base.checkActive();
        if (GameController.Instance.gameMod.state == ConstEnums.GameState.Evolute)
        {
            return result;
        }
        else
        {
            return false;
        }

    }



}  