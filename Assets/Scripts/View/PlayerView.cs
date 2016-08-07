using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Evolution;
using DG.Tweening;

public class PlayerView : MonoBehaviour {

	public static string OnAnimalAttackComplete = "PlayerView.OnAnimalEaten";
	public GameObject cardPrefab;
	public GameObject animalPrefab;
	public GameObject animalArea;
	public GameObject cardArea;
	public Transform[] playerCardPostions;
	public Transform[] playerAnimalPostions;
	public List<GameObject> cards = new List<GameObject>();
	public List<GameObject> animals = new List<GameObject>();
    Vector3 cardSize;


	public List<int> latestChoosenLocalAnimalIndex = new List<int>();
	public List<int> latestChoosenRemoteAnimalIndex = new List<int>();
	public List<Vector3> latestChoosenRemoteAnimalPos = new List<Vector3>();

    // Use this for initialization
    void Start () {
		DOTween.Init(false, true, LogBehaviour.ErrorsOnly);
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void addCards(List<CardModel> addCards,List<GameObject> cardObjList)
	{
        for (int i =0;i<addCards.Count;i++){
			GameObject card = cardObjList[i];
            cardSize = card.GetComponent<Renderer>().bounds.size;
            card.GetComponent<CardView>().init(addCards[i]);
			cards.Add(card);
		}
		refreshCardsPosition();
	}

	public void removeCard(int cardIndex)
	{
		Destroy(cards[cardIndex]);
		cards.RemoveAt(cardIndex);
		refreshCardsPosition();
	}

	public void refreshCardsPosition(){
        //Debug.Log("refresh card count:"+ cards.Count);
		if(cards.Count>0){
            Vector3 pos = new Vector3(cardArea.transform.position.x, cardArea.transform.position.y+1, cardArea.transform.position.z);
			Vector3 areaSize = cardArea.GetComponent<Renderer>().bounds.size;
			float margin = (areaSize.x - (cards.Count*cardSize.x))/(cards.Count-1==0?1: cards.Count - 1);
			float originX = pos.x - (areaSize.x/2) + (cardSize.x/2);
			Sequence mySequence = DOTween.Sequence();
			for(int i =0;i<cards.Count;i++){
				pos.x =  originX + i*margin + i*cardSize.x;
				mySequence.Append(cards[i].transform.DOMove(pos,0.3f,false));
			}
		}
	}
		

	public void moveCard(int cardIndex,Vector3 pos){
		cards[cardIndex].transform.position = pos;
	}

	public Vector3 getCardPosition(int index){
		return cards[index].transform.position;
	}

	public Vector3 getAnimalPosition(int index){
		return animals[index].transform.position;
	}

	public void moveAnimal(int index,Vector3 pos){
		animals[index].transform.position = pos;
	}

	public void showAnimal(AnimalModel animalMod,Vector3 pos,bool isRemote)
	{
		Vector3 newPos = new Vector3(pos.x,1.5f,pos.z);
		GameObject animal = Instantiate<GameObject>(animalPrefab);
		animal.GetComponent<AnimalView>().init(animalMod);
		Debug.Log("creat animal as "+newPos);

		animal.transform.position = newPos;
		if(isRemote){
			animal.transform.rotation = Quaternion.Euler(0f,270f,0f);
		}else{
			animal.transform.rotation = Quaternion.Euler(0f,90f,0f);
		}
		animals.Add(animal);
	}

	public void removeAnimal(int animalIndex)
	{
		Destroy(animals[animalIndex]);
		animals.RemoveAt(animalIndex);
	}

	public void refreshAnimalFoodText(bool enable = false){
		foreach(GameObject animal in animals){
			animal.GetComponent<AnimalView>().setFoodTextHuntMode(enable);
		}
	}

	public void setAnimalFull(int animalIndex)
	{
		Debug.Log("animal " + animalIndex + "is full!!");
		animals[animalIndex].GetComponent<AnimalView>().setFullColor();
	}


    public void resetAnimal(int animalIndex)
    {
        //Debug.Log("animal" + animalIndex + "is empty!!");
        animals[animalIndex].GetComponent<AnimalView>().setOriginColor();
    }

	public void addSkillToAniml(int animalIdx,ConstEnums.Skills skill,int neededFood){
		animals[animalIdx].GetComponent<AnimalView>().addSkillView(skill,neededFood);
	}

	public void animalAttackAnimation(int attackerIdx,int defenderIdx, Vector3 defenderPos,bool isFull){
		Vector3 originPos = animals[attackerIdx].transform.position;
		animals[attackerIdx].transform.DOJump(defenderPos,5,1,0.5f).OnComplete(()=>jumpBack(attackerIdx,defenderIdx,originPos,isFull));
	}

	void jumpBack(int attackerIdx,int defenderIdx,Vector3 originPos,bool isFull){
		animals[attackerIdx].transform.DOJump(originPos,5,1,0.5f);
		if(isFull){
			//如果吃饱，变成吃饱的颜色
			setAnimalFull(attackerIdx);
		}
		this.PostNotification (OnAnimalAttackComplete,defenderIdx);
	}




    public void Clear ()
	{
		
	}


}
