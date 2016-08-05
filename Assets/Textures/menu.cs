using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class menu : MonoBehaviour {

	public Image tutorial;
	bool tutorialShow;

	// Use this for initialization
	void Start () {
		tutorial.gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown(0) && tutorialShow){
			tutorialShow = false;
			tutorial.gameObject.SetActive(false);
		}
	}

	public void showTutorial(){
		tutorial.gameObject.SetActive(true);
		tutorialShow = true;
	}

}
