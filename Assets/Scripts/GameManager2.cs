using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;


public class GameManager2 : MonoBehaviour
{

	public GameObject craigprefab;
	public GameObject amyprefab;
	public GameObject willprefab;
	public GameObject killerprefab;

	Vector3 player1startposition = new Vector3(-3,-1,0);
	Vector3 player2startposition = new Vector3 (3, -1, 0);

	private Dictionary<PlayerName,GameObject> _dicPlayerPrefabByName = null;

	PlayerName selected;


	void Awake(){
		_dicPlayerPrefabByName = new Dictionary<PlayerName, GameObject> ();
		_dicPlayerPrefabByName.Add (PlayerName.Craig, craigprefab);
		_dicPlayerPrefabByName.Add (PlayerName.Amy, amyprefab);
		_dicPlayerPrefabByName.Add (PlayerName.Will, willprefab);
		_dicPlayerPrefabByName.Add (PlayerName.Killer, killerprefab);


	}
	// Use this for initialization
	void Start ()
	{
		selected = Select.selectedPlayer;
		//DetermineFightOpponents (selected,opponents);
	
		StartCoroutine (StartGame());
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
	void DetermineFightOpponents(){

	}
	IEnumerator StartGame(){
		yield return null;
	}

}

