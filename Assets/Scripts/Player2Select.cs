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
	public Button killerbutton;
	
	public Text charName;
	public Text charInfo;
	private Dictionary<PlayerName,Button> _dicButtonByName = null;
	
	private Dictionary<string,PlayerName> _dicPlayerNameByName = null;
	
	
	public static PlayerName player2name = PlayerName.None;
	
	
	void Awake(){
		_dicButtonByName = new Dictionary<PlayerName, Button> ();
		_dicButtonByName.Add (PlayerName.Craig, craigbutton);
		_dicButtonByName.Add (PlayerName.Amy, amybutton);
		_dicButtonByName.Add (PlayerName.Will, willbutton);
		_dicButtonByName.Add (PlayerName.Killer, killerbutton);
		
		_dicPlayerNameByName = new Dictionary<string, PlayerName> ();
		_dicPlayerNameByName.Add ("Craig", PlayerName.Craig);
		_dicPlayerNameByName.Add ("Amy", PlayerName.Amy);
		_dicPlayerNameByName.Add ("Will", PlayerName.Will);
		_dicPlayerNameByName.Add ("Killer", PlayerName.Killer);
		
		
	}
	// Use this for initialization
	void Start () {
		if (SceneManager.players == 0) {
			SceneManager.players = 2;
		}
		if (SceneManager.players == 1) {
			killerbutton.gameObject.SetActive(false);
		}
		if (Player1Select.player1name == null || Player1Select.player1name == PlayerName.None) {
			Player1Select.player1name = PlayerName.Craig;
		}
		_dicButtonByName[Player1Select.player1name].interactable = false;
		
	}
	
	// Update is called once per frame
	void Update () {
		if (EventSystem.current.currentSelectedGameObject == null) {
			EventSystem.current.SetSelectedGameObject (craigbutton.gameObject);
		}
		if (EventSystem.current.currentSelectedGameObject == craigbutton.gameObject) {
			charName.text = "Craig the Copywriter";
			charInfo.text ="Craig believes that anything and everything in this world is a story\nwaiting to be told -- and after a few beers, he's ready to tell them\nall. He can hypnotize with words, and craft another person's thoughts\nwith the right combination of storytelling and masterful facial hair.\nHis toolbox is simple, but powerful: 26 tiny instruments that know no\nboundaries and can transform dreams into reality.";
		}
		if (EventSystem.current.currentSelectedGameObject == amybutton.gameObject) {
			charName.text = "Amy the Art Director";
			charInfo.text ="An artist at heart, Amy the AD always dreamed of sharing her talents\nwith the world. But the life of a "+'"'+"starving artist"+'"'+" didn't suit her,\nso when the advertising world came calling, she traded her crayons\nand brushes for Adobe Suite and a Wacom tablet. Whoever thought\nthat Keynote couldn't be a canvas never saw Amy sculpt a presentation\ndeck like Michelangelo, either. She is a visually - driven idea builder,\ncapable of using her designer toolkit to create almost anything.";
		}
		if (EventSystem.current.currentSelectedGameObject == willbutton.gameObject) {
			charName.text = "Will the Creative Technologist";
			charInfo.text ="Will the CT stands at the cusp of new technology, using it freely and\nexpertly in his creative pursuits. This makes him something of a\nrenaissance man in the ad agency world, maneuvering seamlessly\nbetween 'creative' and 'coder' -- although he couldn't tell you how or\nwhy. His achilles heel is answering the question, 'What do you do?'\nBut otherwise, he's a fearless creator, constantly experimenting and\ndreaming of pioneering his own brand of 'new tech.'";
		}
		if (EventSystem.current.currentSelectedGameObject == killerbutton.gameObject) {
			charName.text = "The Idea Killer";
			charInfo.text ="The Idea Killer hides in the dark corners of the creative world,\nwaiting patiently for the perfect time to strike -- epiphanies. He\ngobbles them up and spits them back out, taking pleasure in the\nsoul-crushing disappointment of Creatives. Even great ideas don't\nstand a chance, as he can tear them down with little more than a\nconfused glance, or a "+'"'+"meh"+'"'+" expression. He reviles creativity, but\ndoesn't hesitate to use it himself -- to kill great ideas.";
		}
		
		
		
	}
	public void SelectOnHover(GameObject g){
		EventSystem.current.SetSelectedGameObject (g);
	}
	public void SelectActivate(string name){
		
		player2name = _dicPlayerNameByName[name];
		
		Application.LoadLevel ("Game");
		
		
		
	}
	

}
