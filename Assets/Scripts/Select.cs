using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Select : MonoBehaviour {
	GameObject go;
	public Button craigbutton;
	public Button amybutton;
	public Button willbutton;
	public Button selectbutton;
	public Text confirm;


	public enum Player{None,Craig,Amy,Will};
	public static Player selectedPlayer = Player.None;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(EventSystem.current.currentSelectedGameObject != selectbutton.gameObject){
			selectbutton.interactable = false;
			selectbutton.GetComponentInChildren<Text>().text = "Select";
			selectedPlayer = Player.None;
			confirm.enabled = false;


		}

	}
	public void CraigSelectActivate(){
		var navigation = selectbutton.navigation;
		navigation.selectOnUp = craigbutton;
		selectbutton.navigation = navigation;
		selectedPlayer = Player.Craig;


		ModifyButton ();


	}
	public void AmySelectActivate(){
		var navigation = selectbutton.navigation;
		navigation.selectOnUp = amybutton;
		selectbutton.navigation = navigation;
		selectedPlayer = Player.Amy;

		ModifyButton ();

	}
	public void WillSelectActivate(){
		var navigation = selectbutton.navigation;
		navigation.selectOnUp = willbutton;
		selectbutton.navigation = navigation;
		selectedPlayer = Player.Will;

		ModifyButton ();
	}
	void ModifyButton(){
		
		confirm.enabled = true;
		selectbutton.interactable = true;
		selectbutton.GetComponentInChildren<Text>().text = selectedPlayer + " Select";
		EventSystem.current.SetSelectedGameObject (selectbutton.gameObject);

	}
	public void PlaySelectedCharacter(){
		Application.LoadLevel("Game");

	}
}
