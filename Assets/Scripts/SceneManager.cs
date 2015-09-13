using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class SceneManager : MonoBehaviour {
	public static int players;
	public GameObject defaultbutton;

	void Update () {
		if(EventSystem.current.currentSelectedGameObject == null){
			EventSystem.current.SetSelectedGameObject (defaultbutton);
			
			
		}
		
	}
	public void SelectOnHover(GameObject g){
		EventSystem.current.SetSelectedGameObject (g);
	}
	public void CharacterSelect(int p){
		players = p;
		Application.LoadLevel("Player2Select");
	}
	public void ChangeScene(string scene){
		Application.LoadLevel(scene);

	}


}
