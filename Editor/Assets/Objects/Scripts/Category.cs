using UnityEngine;
using System.Collections;

public class Category : Object
{
	
	private string name;
	private ArrayList thumbnails;
	
	private GUIStyle textStyle;
	private Rect buttonRect;
	
	private Texture2D button;
	private Texture2D buttonSelected;
	
	public Category (string name, ArrayList thumbnails, int xPos)
	{
		this.name = name;
		this.thumbnails = thumbnails;
		
		textStyle = new GUIStyle ();
		textStyle.alignment = TextAnchor.MiddleCenter;
		
		int yPos = Screen.height - 40;
		int width = (int)20 + (name.Length * 10);
		int height = 30;
		
		buttonRect = new Rect (xPos, yPos, width, height);
		
		button = new Texture2D (width, height);
		for (int y = 0; y < button.height; y++) {
			for (int x = 0; x < button.width; x++) {
				if (x == (int)((button.height - y) * .2f) || x == (int)(button.width - (button.height - y) * .2f)) {
					button.SetPixel (x, y, Color.black);
				} else if (x > (int)((button.height - y) * .2f) && x < (int)(button.width - (button.height - y) * .2f)) {
					button.SetPixel (x, y, Color.grey);
					if (y == 0) {
						button.SetPixel (x, y, Color.black);
					}
				} else {
					button.SetPixel (x, y, new Color (0, 0, 0, 0));
				}
			}
		}
		button.Apply ();
		
		buttonSelected = new Texture2D (width, height);
		for (int y = 0; y < buttonSelected.height; y++) {
			for (int x = 0; x < buttonSelected.width; x++) {
				if (x == (int)((buttonSelected.height - y) * .2f) || x == (int)(buttonSelected.width - (buttonSelected.height - y) * .2f)) {
					buttonSelected.SetPixel (x, y, Color.black);
				} else if (x > (int)((buttonSelected.height - y) * .2f) && x < (int)(buttonSelected.width - (buttonSelected.height - y) * .2f)) {
					buttonSelected.SetPixel (x, y, new Color (.7f - .02f * y, .7f - .02f * y, 1f, 1f));
					if (y == 0) {
						buttonSelected.SetPixel (x, y, Color.black);
					}
				} else {
					buttonSelected.SetPixel (x, y, new Color (0, 0, 0, 0));
				}
			}
		}
		buttonSelected.Apply ();
	}
	
	public bool DrawButton ()
	{
		bool returnValue = GUI.Button (buttonRect, button, "");
		GUI.Label (buttonRect, name, textStyle);
		
		return returnValue;
	}
	
	public void DrawSelectedButton ()
	{
		GUI.Button (buttonRect, buttonSelected, "");
		GUI.Label (buttonRect, name, textStyle);
	}
	
	public ArrayList GetThumbnails ()
	{
		return thumbnails;
	}
}
