using UnityEngine;
using System.Collections;
using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using Evolution;

public class CardView: BaseDraggtableView {


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
			setOriginColor(Color.yellow);
			break;
		case ConstEnums.Skills.Predator:
			setOriginColor(Color.red);
			break;
		case ConstEnums.Skills.Hide:
			setOriginColor(Color.gray);
			break;
		case ConstEnums.Skills.Aquatic:
			setOriginColor(Color.blue);
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