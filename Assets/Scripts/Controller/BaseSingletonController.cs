using UnityEngine;
using System.Collections;

public class BaseSingletonController<T> : StateMachine where T : Component {

	private static T instance;
	public static T Instance {
		get {
			if (instance == null) {
				instance = FindObjectOfType<T> ();
				if (instance == null) {
					Debug.LogError("init singleton");
					GameObject obj = new GameObject ();
					obj.name = typeof(T).Name;
					instance = obj.AddComponent<T>();
				}
			}
			return instance;
		}
	}

	public virtual void Awake ()
	{
		if (instance == null) {
			instance = this as T;
			DontDestroyOnLoad (this.gameObject);
		} else {
			//Destroy (gameObject);
		}
	}
}
