using UnityEngine;
using System.Collections;
using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
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

	public override void init (BaseModel _mod)
	{
		base.init (_mod);
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