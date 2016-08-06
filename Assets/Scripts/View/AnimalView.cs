using UnityEngine;
using System.Collections;
using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using Evolution;
using System.Collections.Generic;

public class AnimalView: BaseDraggtableView {
	public GameObject[] skillPrefbs;
	public List<GameObject> skillObjs = new List<GameObject>();
	public Color isFullColor = Color.green;
	bool isFull;
	public bool isFoodIn;
	Material skin;
	TextMesh foodText;
	TextMesh skillText;
	TextMesh indexText;

	AnimalView(){
		MouseDrop = "AnimalView.MouseDrop";
		MouseDown = "AnimalView.MouseDown";
		MouseEnter = "AnimalView.MouseEnter";
		MouseExit = "AnimalView.MouseExit";
		MouseDrag = "AnimalView.MouseDrag";	
		MouseOnEny = "AnimalView.MouseOnEny";
	}

	public override void init (BaseModel _mod)
	{
		base.init (_mod);
		skin = GetComponentInChildren<AnimalSkin>().gameObject.GetComponent<Renderer>().material;
		originalColor = skin.color;

		TextMesh[] texts = GetComponentsInChildren<TextMesh>();
		foreach(TextMesh text in texts){
			switch(text.gameObject.name){
			case "food":
				foodText = text;
				foodText.text = "1";
				break;
			case "skill":
				skillText = text;
				break;
			}
		}
		if(mod.ownerId == MatchController.Instance.localPlayer.getPlayerId()){
			Vector3 r = foodText.gameObject.transform.localEulerAngles;
			r.y = 270;
			Vector3 p = skillText.gameObject.transform.localPosition;
			foodText.gameObject.transform.localEulerAngles = r;
			skillText.gameObject.transform.localEulerAngles = r;
			skillText.gameObject.transform.localPosition = foodText.gameObject.transform.localPosition;
			foodText.gameObject.transform.localPosition = p;

		}
	}

	public virtual void OnMouseEnter()  
	{  
		isMouseEnter = true;
		if(checkActive()){
			skin.color = mouseOverColor;
			this.PostNotification (MouseEnter);
		}
	}

	public override bool mouseOnEnemy(){
		if(mod.ownerId == MatchController.Instance.localPlayer.getPlayerId()){
			Debug.Log(" not select enemy: "+ mod.index);
			return false;
		}else{
			Debug.Log("select enemy: "+ mod.index);
			return true;
		}
	}

	public override void OnMouseExit ()
	{
		isMouseEnter = false;
		if(checkActive()){
			if(isFull){
				skin.color = isFullColor;
			}else{
				skin.color = originalColor;
			}
			this.PostNotification (MouseExit);
		}
	}

	public void setFullColor()  
	{ 
		setFoodTextHuntMode(true);
		foodText.color = Color.green;
		Debug.Log("animal set full color");
		skin.color = isFullColor;
		isFull = true;
	}
		

    public void setOriginColor()
    {
        Debug.Log("animal set origin color");
		foodText.color = Color.white;
		skin.color = originalColor;
        isFull = false;
    }

	public void clearIsFoodIn(){
		isFoodIn = false;
	}

	public void addSkillView(ConstEnums.Skills skillType,int neededFood){
		int skillIdx = (int)skillType;
		//GameObject skillObj = Instantiate(skillPrefbs[skillIdx]);	
		//skillObjs.Add(skillObj);
		//skillObj.transform.parent=transform;
		//float z = -1f + skillObjs.Count*0.4f;
		//skillObj.transform.localPosition = new Vector3(0f,1.1f,z);
		switch(skillIdx){
		case 1:
			setColor(Color.yellow);
			break;
		case 2:
			setColor(Color.red);
			break;
		case 3:
			setColor(Color.gray);
			break;
		case 4:
			setColor(Color.blue);
			break;

		};
		skillText.text += "<"+skillType.ToString().Substring(0,3)+">";
		setFoodTextHuntMode();

	}

	public void setFoodTextHuntMode(bool isHunt=false){
		AnimalModel tempMod = (AnimalModel)mod;
		if(isHunt){
			foodText.text = tempMod.currentFood.ToString() + "/" + tempMod.neededFood.ToString();
		}else{
			foodText.text = tempMod.neededFood.ToString();
		}
	}

	void setColor(Color color){
		skin.color = color;
		originalColor = color;
	}

}  