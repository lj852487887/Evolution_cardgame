using UnityEngine;
using System.Collections;
using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using Evolution;

public abstract class BaseDraggtableView: MonoBehaviour {

	public string MouseDrop = ".MouseDrop";
	public string MouseDown = ".MouseDown";
	public string MouseEnter = ".MouseEnter";
	public string MouseExit = ".MouseExit";
	public string MouseDrag = ".MouseDrag";
	public string MouseOnEny = ".MouseOnEny";

	public BaseModel mod;
	public Color mouseOverColor = Color.cyan;
	public Color originalColor;
	public string className;
	public bool isMouseDown;
	public bool isMouseEnter;
	public TextMesh text;
	public Quaternion originTextRotation;
	public Vector3 originTextPos;

    public BaseDraggtableView()
    {
        mod = new BaseModel();
    }

    void Start()
	{
		text = gameObject.GetComponentInChildren<TextMesh>();
		if(text){
			originTextRotation = text.gameObject.transform.rotation;
		}
    }  

	void LateUpdate(){
		if(text){
			text.gameObject.transform.rotation = originTextRotation;
		}

	}

	public virtual void init(BaseModel _mod){
		originalColor = GetComponent<Renderer>().material.color;
		mod = _mod;
		//refreshText();
	}

	public void setOriginColor(Color color){
		originalColor = color;
		GetComponent<Renderer>().material.color = originalColor;
	}



	public virtual bool checkActive(){
		if(mod.ownerId == MatchController.Instance.localPlayer.getPlayerId() && 
			GameController.Instance.getControl() == MatchController.Instance.localPlayer.getPlayerId()){
			return true;
		}else{
			return false;
		}
	}



	public virtual void OnMouseEnter()  
	{  
		isMouseEnter = true;
		if(checkActive()){
			GetComponent<Renderer>().material.color = mouseOverColor;
			this.PostNotification (MouseEnter);
		}
	}

	public virtual void OnMouseExit()
	{  
		isMouseEnter = false;
		if(checkActive()){
			GetComponent<Renderer>().material.color = originalColor;
			this.PostNotification (MouseExit);
		}
	}

	public virtual void clearColor(){
		GetComponent<Renderer>().material.color = originalColor;
		isMouseEnter = false;
	}


	public virtual bool mouseOnEnemy(){
		return false;
	}

	public virtual IEnumerator OnMouseDown()  
	{  
		isMouseDown = true;
		if(checkActive()){
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


	public virtual void OnMouseUp(){
		clearColor();
		isMouseDown = false;
		if(checkActive()){
			this.PostNotification (MouseDrop, mod.index);
		}
	}
}  