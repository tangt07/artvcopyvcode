using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class Player1Select : MonoBehaviour {
	GameObject go;
	public Button craigbutton;
	public Button amybutton;
	public Button willbutton;
	public Button selectbutton;
	public Button killerbutton;
	public Image killercanvas;
	public Text confirm;
	public Text selectbuttontext;
	private Dictionary<string,Button> _dicButtonByName = null;

	private Dictionary<string,PlayerName> _dicPlayerNameByName = null;


	public static PlayerName player1name = PlayerName.None;


	void Awake(){
		_dicButtonByName = new Dictionary<string, Button> ();
		_dicButtonByName.Add ("Craig", craigbutton);
		_dicButtonByName.Add ("Amy", amybutton);
		_dicButtonByName.Add ("Will", willbutton);
		_dicButtonByName.Add ("Killer", killerbutton);

		_dicPlayerNameByName = new Dictionary<string, PlayerName> ();
		_dicPlayerNameByName.Add ("Craig", PlayerName.Craig);
		_dicPlayerNameByName.Add ("Amy", PlayerName.Amy);
		_dicPlayerNameByName.Add ("Will", PlayerName.Will);
		_dicPlayerNameByName.Add ("Killer", PlayerName.Killer);

		
	}
	// Use this for initialization
	void Start () {
		selectbuttontext = selectbutton.GetComponentInChildren<Text> ();
		if (SceneManager.players == 1) {
			killercanvas.gameObject.SetActive(false);
		}
	
	}
	
	// Update is called once per frame
	void Update () {
		if (EventSystem.current.currentSelectedGameObject == null) {
			EventSystem.current.SetSelectedGameObject (craigbutton.gameObject);
		}

		if(EventSystem.current.currentSelectedGameObject != selectbutton.gameObject){
			selectbutton.interactable = false;
			selectbuttontext.text = "Select";
			player1name = PlayerName.None;
			confirm.enabled = false;


		}

	}
	public void SelectActivate(string name){
		var navigation = selectbutton.navigation;
		navigation.selectOnUp = _dicButtonByName[name];
		selectbutton.navigation = navigation;
		player1name = _dicPlayerNameByName[name];


		ModifyButton ();


	}
	void ModifyButton(){
		
		confirm.enabled = true;
		selectbutton.interactable = true;
		selectbuttontext.text = player1name + " Select";
		EventSystem.current.SetSelectedGameObject (selectbutton.gameObject);

	}
	public void PlaySelectedCharacter(){
		if (SceneManager.players == 1) {
			Application.LoadLevel ("Game");
		} else {
			Application.LoadLevel ("Player2Select");
		}
	}
}
