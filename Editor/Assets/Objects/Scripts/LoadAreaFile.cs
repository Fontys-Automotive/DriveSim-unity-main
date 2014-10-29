using UnityEngine;
using System.Collections;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

public class LoadAreaFile : Object {
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
	
	private ArrayList pngFileList = new ArrayList ();
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

	private readonly string defaultPath = "C:\\DriveSim_exports\\";
	private readonly string fileName;
	private readonly string imageExtension = ".png";
	private readonly string areaExtension = ".dsae";
	private Texture2D overlay = new Texture2D(0,0);
	private AreaFile areaFile;

	private string areaFileName = "";

	public LoadAreaFile() {
		init();
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
	}

	public void drawGUI() {
		if (showBox) {
			GUI.Label (loadSaveBox, bgTex, bgStyle);
			GUI.Label (name, "Name: ");

			if (!legitLoadName ()) {
				GUI.contentColor = Color.red;
			}
			string preInputName = inputName;
			inputName = GUI.TextField (inputField, initLoadName ());
			if (preInputName != inputName) {
				string path = defaultPath + inputName;
				if (!path.EndsWith (imageExtension)) {
					path += imageExtension;
				}
				
				if (pngFileList.Contains (path)) {
					selectedFileIndex = pngFileList.IndexOf (path);
				} else {
					selectedFileIndex = -1;
				}
			}
			GUI.contentColor = Color.white;
			if (GUI.Button (loadSaveButton, "Load")) {
				if (legitLoadName ()) {
					areaFileName = inputName;
					//Load file here
					initArea();
					initImage();
					showBox = false;
				}
			}

			//Cancel button
			Rect cancelButton = new Rect (loadSaveButton.x + 55, loadSaveButton.y, loadSaveButton.width, loadSaveButton.height);
			if (GUI.Button (cancelButton, "Cancel")) {
				showBox = false;
			}
			
			int scrollIndex = 0;
			//Scrollbar
			if (pngFileList.Count > 18) {
				int fitAmount = Mathf.CeilToInt ((float)pngFileList.Count / 9);
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

	private void showFiles (int startIndex)
	{
		int endIndex = startIndex + 18;
		if (endIndex > pngFileList.Count) {
			endIndex = pngFileList.Count;
		}
		int textIndex = 0;
		
		for (int i = startIndex; i < endIndex; i++) {
			string pathName = pngFileList [i].ToString ();
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

	private bool legitLoadName ()
	{
		bool returnValue = false;
		string path = defaultPath + inputName;
		if (path.Length > 0) {
			if (!inputName.ToLower ().EndsWith (imageExtension)) {
				path += imageExtension;
			}
			if (File.Exists (path)) {
				returnValue = true;
			}
		}
		return returnValue;
	}

	private ArrayList getPNGFiles ()
	{
		string[] s = Directory.GetFiles (defaultPath);
		ArrayList returnValue = new ArrayList ();
		foreach (string x in s) {
			if (x.EndsWith (imageExtension)) {
				string fileName = x.Substring(0, x.Length - 4);
				returnValue.Add (fileName);
			}
		}
		return returnValue;
	}

	private void initArea() {
		string path = defaultPath + inputName + areaExtension;

		byte[] bytes = File.ReadAllBytes(path);
		string classAsText = System.Text.Encoding.UTF8.GetString(bytes);

		string[] variables = classAsText.Split('\n');
		int gridSizeX = int.Parse(variables[0]);
		int gridSizeY = int.Parse(variables[1]);
		double nwLat = double.Parse(variables[2]);
		double nwLon = double.Parse(variables[3]);
		double neLat = double.Parse(variables[4]);
		double neLon = double.Parse(variables[5]);
		double seLat = double.Parse(variables[6]);
		double seLon = double.Parse(variables[7]);
		double swLat = double.Parse(variables[8]);
		double swLon = double.Parse(variables[9]);

		AreaFile af = new AreaFile(gridSizeX, gridSizeY, nwLat, nwLon, neLat, neLon, seLat, seLon, swLat, swLon);

//		AreaFile c = new AreaFile();
//		FileStream s = File.OpenRead(path);
//		BinaryFormatter b = new BinaryFormatter();
//		c = (AreaFile)b.Deserialize(s);
//		s.Close();

		new GridXmlSerializer().resizeGrid(af.getGridSizeY(), af.getGridSizeX());
		GameObject.FindGameObjectWithTag("MainScripts").GetComponent<SimulationScript>().setNewLatLon(af);
	}

	private void initImage() {
		string path = defaultPath + inputName + imageExtension;

		Texture2D tex = new Texture2D (0, 0);
		tex.LoadImage (File.ReadAllBytes (path));
		tex.name = path.Substring (path.LastIndexOf ("\\") + 1).Replace (".png", "");

		overlay = tex;
		showOverlay();
	}

	public void showOverlay() {
		GameObject.FindGameObjectWithTag("ground").GetComponent<WorldScript>().setOverlay(overlay, true);
	}

	private string initLoadName ()
	{
		return inputName;
	}

	public void show() {
		showBox = true;
		pngFileList = getPNGFiles();
	}

	public void loadFile() {

	}

	public Texture2D getOverlay() {
		return overlay;
	}

	public AreaFile getAreaFile() {
		return areaFile;
	}

	public string getAreaFileName() {
		return areaFileName;
	}

	public void loadAreaFile(string areaFileName) {
		this.areaFileName = areaFileName;
		this.inputName = areaFileName;
		if(legitLoadName()) {
			initArea();
			initImage();
		}
	}
}
