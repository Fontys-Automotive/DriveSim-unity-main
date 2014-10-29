using UnityEngine;
using System.Collections;
using System.IO;

public class LoadSaveBox : Object
{
	
	private Texture2D bgTex = new Texture2D (1, 1);
	private GUIStyle bgStyle = new GUIStyle ();
	private Rect loadSaveBox;
	private Rect inputField;
	private Rect name;
	private Rect loadSaveButton;
	private Rect scrollRect;

	private int boxWidth = 400;
	private int boxHeight = 200;
	
	private bool showBox = false;
	
	private string inputName = "";
	
	private bool loading = true;
	
	private ArrayList xmlFileList = new ArrayList ();
	private int fileWidth;
	private int fileHeight = 20;
	private int fileX;
	private int fileY;
	private int selectedFileIndex = -1;
	private float scrollPos = 0;
	private Texture2D deselectedFileTex = new Texture2D (1, 1);
	private Texture2D selectedFileTex = new Texture2D (1, 1);
	private GUIStyle deselectedStyle = new GUIStyle ();
	private GUIStyle selectedStyle = new GUIStyle ();

	private string extension = ".nlb";

	//Needed for saving/loading
	private GridXmlSerializer ser;
	
	public LoadSaveBox ()
	{
		init ();
	}
	
	// Use this for initialization
	private void init ()
	{
		bgTex.SetPixel (0, 0, new Color (0f, 0f, 0f, .75f));
		bgTex.Apply ();
		bgStyle.normal.background = bgTex;
		
		int boxX = (int)(Screen.width * .5 - boxWidth * .5) - 200;
		int boxY = (int)(Screen.height * .5 - boxHeight * .5) - 100;
		
		loadSaveBox = new Rect (boxX, boxY, boxWidth, boxHeight);
		inputField = new Rect (boxX + 60, boxY + boxHeight - 50, boxWidth - 65, 20);
		name = new Rect (boxX + 5, boxY + boxHeight - 50, 50, 20);
		loadSaveButton = new Rect (boxX + boxWidth - (200 + 50), boxY + boxHeight - 25, 50, 20);
		scrollRect = new Rect (boxX, boxY - 25, boxWidth, 16);
		
		fileX = boxX + 5;
		fileY = boxY + 5;
		fileWidth = (int)(boxWidth * .5) - 15;
		
		selectedFileTex.SetPixel (0, 0, new Color (.2f, .8f, 1f, .5f));
		selectedFileTex.Apply ();
		selectedStyle.normal.background = selectedFileTex;
		
		deselectedFileTex.SetPixel (0, 0, new Color (0f, 0f, 0f, 0f));
		deselectedFileTex.Apply ();
		deselectedStyle.normal.background = deselectedFileTex;
		
		ser = new GridXmlSerializer (); //Add serializer for saving/loading
	}
	
	public void drawGUI ()
	{
		if (showBox) {
			GUI.Label (loadSaveBox, bgTex, bgStyle);
			GUI.Label (name, "Name: ");
			//Load screen
			if (loading) {
				if (!legitLoadName ()) {
					GUI.contentColor = Color.red;
				}
				string preInputName = inputName;
				inputName = GUI.TextField (inputField, initLoadName ());
				if (preInputName != inputName) {
					string path = Directory.GetCurrentDirectory () + "\\" + inputName;
					if (!path.EndsWith (extension)) {
						path += extension;
					}
					
					if (xmlFileList.Contains (path)) {
						selectedFileIndex = xmlFileList.IndexOf (path);
					} else {
						selectedFileIndex = -1;
					}
				}
				GUI.contentColor = Color.white;
				if (GUI.Button (loadSaveButton, "Load")) {
					if (legitLoadName ()) {
						string path = inputName;
						if (!inputName.ToLower ().EndsWith (extension)) {
							path += extension;
						}
						ser.ReadXml (path);
						showBox = false;
					}
				}
			} else { //Save screen
				inputName = GUI.TextField (inputField, initSaveName ());
				string path = inputName;
				if (!inputName.ToLower ().EndsWith (extension)) {
					path += extension;
				}
				if (!legitSaveName ()) {
					GUI.contentColor = Color.red;
				} else if (fileExists (path)) {
					GUI.contentColor = Color.yellow;
				}
				if (GUI.Button (loadSaveButton, "Save")) {
					if (legitSaveName ()) {
						ser.SaveXml (path);
						showBox = false;
					}
				}
				GUI.contentColor = Color.white;
			}
			//Cancel button
			Rect cancelButton = new Rect (loadSaveButton.x + 55, loadSaveButton.y, loadSaveButton.width, loadSaveButton.height);
			if (GUI.Button (cancelButton, "Cancel")) {
				showBox = false;
			}

			int scrollIndex = 0;
			//Scrollbar
			if (xmlFileList.Count > 18) {
				int fitAmount = Mathf.CeilToInt ((float)xmlFileList.Count / 9);
				float scrollBarWidth = 100 / fitAmount;
				scrollPos = GUI.HorizontalScrollbar (scrollRect, scrollPos, scrollBarWidth, 0, 100);
				scrollIndex = (int)((((float)fitAmount / 100F) * scrollPos) + .1);
				scrollIndex = scrollIndex * 9;
				if (scrollIndex < 0) {
					scrollIndex = 0;
				}
			}

			showFiles (scrollIndex);
		}
	}
	
	private ArrayList getXMLFiles ()
	{
		string[] s = Directory.GetFiles (Directory.GetCurrentDirectory ());
		ArrayList returnValue = new ArrayList ();
		foreach (string x in s) {
			if (x.EndsWith (extension)) {
				returnValue.Add (x);
			}
		}
		return returnValue;
	}
	
	private void showFiles (int startIndex)
	{
		int endIndex = startIndex + 18;
		if (endIndex > xmlFileList.Count) {
			endIndex = xmlFileList.Count;
		}
		int textIndex = 0;

		for (int i = startIndex; i < endIndex; i++) {
			string pathName = xmlFileList [i].ToString ();
			string fileName = pathName.Substring (pathName.LastIndexOf ('\\') + 1);
			if (textIndex >= 9) {
				Rect rect = new Rect (fileX + fileWidth + 10, fileY + (15 * (textIndex - 9)), fileWidth, fileHeight);
				GUI.Label (rect, fileName);
				if (selectedFileIndex == i) {
					rect = new Rect (fileX + fileWidth + 10, fileY + (15 * (textIndex - 9)) + 4, fileWidth, 14);
					GUI.Label (rect, "", selectedStyle);
				} else {
					if (GUI.Button (rect, "", deselectedStyle)) {
						rect = new Rect (fileX + fileWidth + 10, fileY + (15 * (textIndex - 9)) + 4, fileWidth, 14);
						selectedFileIndex = i;
						inputName = fileName;
					}
				}
			} else {
				Rect rect = new Rect (fileX, fileY + (15 * textIndex), fileWidth, fileHeight);
				GUI.Label (rect, fileName);
				if (selectedFileIndex == i) {
					rect = new Rect (fileX, fileY + (15 * textIndex) + 4, fileWidth, 14);
					GUI.Label (rect, "", selectedStyle);
				} else {
					if (GUI.Button (rect, "", deselectedStyle)) {
						rect = new Rect (fileX, fileY + (15 * textIndex) + 4, fileWidth, 14);
						selectedFileIndex = i;
						inputName = fileName;
					}
				}
			}
			textIndex++;
		}
	}
	
	private bool fileExists (string path)
	{
		if (File.Exists (path)) {
			return true;
		} else {
			return false;
		}
	}
	
	private bool legitLoadName ()
	{
		bool returnValue = false;
		string path = inputName;
		if (path.Length > 0) {
			if (!inputName.ToLower ().EndsWith (extension)) {
				path += extension;
			}
			if (fileExists (path)) {
				returnValue = true;
			}
		}
		return returnValue;
	}
	
	private bool legitSaveName ()
	{
		bool returnValue = false;
		string path = inputName;
		if (path.Length > 0) {
			returnValue = true;
		}
		return returnValue;
	}
	
	private string initLoadName ()
	{
		return inputName;
	}
	
	private string initSaveName ()
	{
		return inputName;
	}
	
	public void openLoadDialog ()
	{
		loading = true;
		showBox = true;
		xmlFileList = getXMLFiles ();
	}
	
	public void openSaveDialog ()
	{
		loading = false;
		showBox = true;
		xmlFileList = getXMLFiles ();
	}
	
	public bool getShowBox ()
	{
		return showBox;
	}
}
