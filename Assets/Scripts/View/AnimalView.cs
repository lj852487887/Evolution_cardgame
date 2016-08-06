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
		Debug.Log("animal set full color");
		skin.color = isFullColor;
		isFull = true;
	}
		

    public void setOriginColor()
    {
        Debug.Log("animal set origin color");
		skin.color = originalColor;
        isFull = false;
    }

	public void clearIsFoodIn(){
		isFoodIn = false;
	}

	public void addSkillObj(ConstEnums.Skills skillType){
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
		text.text += skillType.ToString();
	}

	void setColor(Color color){
		skin.color = color;
		originalColor = color;
	}

}  