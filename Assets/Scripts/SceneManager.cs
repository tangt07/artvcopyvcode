using UnityEngine;
using System.Collections;

public class SceneManager : MonoBehaviour {

	void ChangeScene(string levelindex){
		Application.LoadLevel(levelindex);
	}
	public void ChangeToGame(){
		ChangeScene ("Game");
	}
	public void ChangeToCharacterSelect(){
		ChangeScene ("CharacterSelect");
	}
}
