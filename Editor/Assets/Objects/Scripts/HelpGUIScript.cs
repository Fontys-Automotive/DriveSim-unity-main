using UnityEngine;
using System.Collections;

public class HelpGUIScript : MonoBehaviour
{
	private GUIStyle style;
	private Texture2D bgTex;
	private int guiHeight = 0;
	private int guiWidth = 0;
	private bool showHelp = false;
	private bool f1PressedLastFrame = false;
	// Use this for initialization
	void Start ()
	{
		style = new GUIStyle ();
		guiHeight = (int)(Screen.height - 150);
		guiWidth = 400;
		bgTex = new Texture2D (guiWidth, guiHeight);
		for (int i = 0; i < guiHeight; i++) {
			for (int i2 = 0; i2 < guiWidth; i2++) {
				bgTex.SetPixel (i2, i, new Color (0, 0, 0, 0.75f));
			}
		}
		bgTex.Apply ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKey (KeyCode.F1)) {
			if(!f1PressedLastFrame) {
				showHelp = !showHelp;
				f1PressedLastFrame = true;
			}
		}
		else {
			f1PressedLastFrame = false;
		}
	}

	string[] helpArray = {"F1 - show/hide this screen", "Arrow keys/Mouse - Navigation", "G key - Toggle Grid", 
		"O key - Toggle Overlay", "Q & E - Rotate Object Left & Right", "Del key - Toggle Delete Mode",
		"Enter - Toggle Simulation Mode", "Left Click - Place selected object", "Right Click - Select object in world",
		"Backspace - Reset camera", "Left Shift + Del key - Clear full grid", "Left Ctrl + S - Save world",
		"Left Ctrl + O - Load world"};

	void OnGUI ()
	{
		if (showHelp) {
			GUI.Label (new Rect (0, 0, 500, 500), bgTex, style);
			for (int i = 0; i < helpArray.Length; i++) {
				GUI.Label (new Rect (10, (i + 1) * 15, 500, 25), helpArray[i]);
			}
		}
	}
	void Show()
	{
		GUI.Label (new Rect (0, 0, 500, 500), bgTex, style);
		for (int i = 0; i < helpArray.Length; i++) {
			GUI.Label (new Rect (10, (i + 1) * 15, 500, 25), helpArray [i]);
		}
	}
}
