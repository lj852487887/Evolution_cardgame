using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.UI;
using Evolution;

public class GameView : MonoBehaviour,IPointerClickHandler {

    static float CARD_MARGIN = 0.01f;
    public const string BoardClickedNotification = "GameView.BoardClickedNotification";
	public const string PassBtnClickedNotification = "GameView.PassBtnClickedNotification";

	public Text turnTxt;
	public Text stateTxt;
	public Text foodTxt;
	public Text infoTxt;
	public Button passBtn;
	public GameObject canvas;
	public GameObject animalActionBtn;

	public Transform[] playerCardPostions;
	public Transform[] playerAnimalPostions;
    

	public List<GameObject> foods = new List<GameObject>();
    public List<GameObject> mainCards = new List<GameObject>();
	public List<GameObject> actionBtns = new List<GameObject>();
    public GameObject foodPrefab;
    public GameObject cardPrefab;
    public GameObject foodArea;
    public GameObject mainCardArea;
    GameModel gameMod;

	public void init(GameModel mod){
		gameMod = mod;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}



	public void showFood(FoodModel foodMod, Vector3 position){
		GameObject food = Instantiate<GameObject> (foodPrefab);
		food.GetComponent<FoodView>().init(foodMod);
		food.transform.position = position;
		foods.Add(food);
		foodTxt.text = "food:"+foods.Count;
	}

	public void eatFood(int index){
        Debug.Log("foods count" + foods.Count);
        Debug.Log("destroy food"+ index);
        GameObject food = foods[index];
        foods.Remove(food);
        Destroy(food);
        foodTxt.text = "food:"+foods.Count;
	}

    public void clearFood()
    {
        for (int i = foods.Count-1;i>=0;i--)
        {
            Destroy(foods[i]);
            foods.RemoveAt(i);
            foodTxt.text = "food:" + foods.Count;
        }
    }



    public void initCard(CardModel card)
    {
        Vector3 pos = mainCardArea.transform.position;
        GameObject cardObj = Instantiate<GameObject>(cardPrefab);
        mainCards.Add(cardObj);
        cardObj.GetComponent<CardView>().init(card);
        int i = mainCards.Count - 1;
        Vector3 cardPos = new Vector3(pos.x, pos.y + 0.04f + i * CARD_MARGIN, pos.z + i * CARD_MARGIN);
        cardObj.transform.position = cardPos;
        
    }

	public List<GameObject> removeCard(int num){
		List<GameObject> cardObjList = new List<GameObject>();
		if(mainCards.Count>=num){
			for (int i = 0; i < num ; i++)
			{
				int idx = mainCards.Count - 1 -i;
				GameObject card = mainCards[idx];
				mainCards.Remove(card);
				cardObjList.Add(card);
				//Destroy(card);
			}
		}
		return cardObjList;
	}


	public void setPassBtnActive(bool active){
		passBtn.gameObject.SetActive(active);
	}

	public void onPassBtnClick(){
		setPassBtnActive(false);
		this.PostNotification(PassBtnClickedNotification);

	}

	public bool isActionShow = false;

	public void setActionActive(bool enable){
		isAttackClicked = enable;
		Debug.Log("attack is clicked! "+ enable);
	}

	public void setActionBtnActive(int animalIdx, Vector3 position){
		if(isActionShow == false){
			AnimalModel animal =  MatchController.Instance.localPlayer.playerMod.animalMods[animalIdx];
			if(animal.property.canAttack()){
				GameObject btn =  (GameObject)Instantiate(animalActionBtn,position,Quaternion.identity);
				actionBtns.Add(btn);
				btn.GetComponentInChildren<Text>().text = "Attack";
				btn.transform.SetParent(canvas.transform);
				btn.GetComponent<Button>().onClick.AddListener(() => setAttackActive(true));
			}
		}
		isActionShow = !isActionShow;
	}

	public bool isAttackClicked = false;

	public void setAttackActive(bool enable){
		isAttackClicked = enable;
		Debug.Log("attack is clicked! "+ enable);
	}


	void IPointerClickHandler.OnPointerClick (PointerEventData eventData)
	{
		Vector3 pos = eventData.pointerCurrentRaycast.worldPosition;

		this.PostNotification(BoardClickedNotification);
	}
}
