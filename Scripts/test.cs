using UnityEngine;
using System.Collections;

public class test : MonoBehaviour {

	public enum Player{
		None,
		One,
		Two
	}

	Player player;

	// Use this for initialization
	void Start () {
//		Debug.Log(player);
//		player++;
//		Debug.Log(player);
//		player++;
//		Debug.Log(player);
//		player++;
//		Debug.Log(player);

		base_zero zero = new base_zero();
		base_zero one = new base_one();
		//Debug.Log(zero.base_name);
		//Debug.Log(one.base_name);

	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log(UnityEngine.Random.Range(0,5));
	}
}
