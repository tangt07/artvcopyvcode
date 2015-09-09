using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class Player2Select : MonoBehaviour {
	GameObject go;
	public Button craigbutton;
	public Button amybutton;
	public Button willbutton;
	public Button selectbutton;
	public Button killerbutton;
	public Text confirm;
	public Text selectbuttontext;
	private Dictionary<string,Button> _dicButtonByName = null;

	private Dictionary<PlayerName,Button> _dicButtonByPName = null;

	private Dictionary<string,PlayerName> _dicPlayerNameByName = null;


	public static PlayerName player2name = PlayerName.None;


	void Awake(){
		_dicButtonByName = new Dictionary<string, Button> ();
		_dicButtonByName.Add ("Craig", craigbutton);
		_dicButtonByName.Add ("Amy", amybutton);
		_dicButtonByName.Add ("Will", willbutton);
		_dicButtonByName.Add ("Killer", killerbutton);

		_dicButtonByPName = new Dictionary<PlayerName, Button> ();
		_dicButtonByPName.Add (PlayerName.Craig, craigbutton);
		_dicButtonByPName.Add (PlayerName.Amy, amybutton);
		_dicButtonByPName.Add (PlayerName.Will, willbutton);
		_dicButtonByPName.Add (PlayerName.Killer, killerbutton);

		_dicPlayerNameByName = new Dictionary<string, PlayerName> ();
		_dicPlayerNameByName.Add ("Craig", PlayerName.Craig);
		_dicPlayerNameByName.Add ("Amy", PlayerName.Amy);
		_dicPlayerNameByName.Add ("Will", PlayerName.Will);
		_dicPlayerNameByName.Add ("Killer", PlayerName.Killer);

		
	}
	// Use this for initialization
	void Start () {
		if (Player1Select.player1name == null || Player1Select.player1name == PlayerName.None) {
			Player1Select.player1name = PlayerName.Craig;
		}
		_dicButtonByPName[Player1Select.player1name].interactable = false;

		selectbuttontext = selectbutton.GetComponentInChildren<Text> ();
	
	}
	// Update is called once per frame
	void Update () {
		if (EventSystem.current.currentSelectedGameObject == null) {
			if(Player1Select.player1name != PlayerName.Craig){
				EventSystem.current.SetSelectedGameObject (craigbutton.gameObject);
			}else{
				EventSystem.current.SetSelectedGameObject (amybutton.gameObject);
			}
		} 



		if(EventSystem.current.currentSelectedGameObject != selectbutton.gameObject){
			selectbutton.interactable = false;
			selectbuttontext.text = "Select";
			player2name = PlayerName.None;
			confirm.enabled = false;


		}

	}
	public void SelectActivate(string name){
		var navigation = selectbutton.navigation;
		navigation.selectOnUp = _dicButtonByName[name];
		selectbutton.navigation = navigation;
		player2name = _dicPlayerNameByName[name];


		ModifyButton ();


	}


	void ModifyButton(){
		
		confirm.enabled = true;
		selectbutton.interactable = true;
		selectbuttontext.text = player2name + " Select";
		EventSystem.current.SetSelectedGameObject (selectbutton.gameObject);

	}
	public void PlaySelectedCharacter(){
		Application.LoadLevel("Game");

	}
}
