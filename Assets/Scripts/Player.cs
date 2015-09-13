using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;


public class Player : MonoBehaviour { 

	private PlayerNumber number;

	private PlayerName name;

	private PlayerMovement movement;

	private PlayerKeys keys;

	private PlayerHealth health;
	
	private int wins;
	
	private Animator anim;

	private Rigidbody2D rb;

	private GameObject go;

	//UI

	private RectTransform healthbar;
	
	private Text textname;
	
	private Image win1;

	private Image win2;
	
	public static Dictionary<int,Player> players = new Dictionary<int, Player>();

	public Player(){
	
	
	}

	public bool SetPlayer(int num, PlayerName nam){
		//prepare player to be set
		if (players [num].name == nam) {
			Debug.Log ("Picking the same character... okay?");
			return true;
		}

		foreach (KeyValuePair<int,Player> player in players){
			if(player.Value.name == nam){
				Debug.Log ("Player already chosen by "+player.Value.number.ToString());
				return false;
			}
		}
		Debug.Log ("Set!");

		players [num].name = nam;

		return true;
	}

}