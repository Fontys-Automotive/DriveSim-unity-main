using UnityEngine;
using System.Collections;
using System.Windows.Forms;

public class GUIScript : MonoBehaviour {
	
	private Categories categories;
	private LoadSaveBox loadSaveBox;
	private LoadAreaFile loadAreaFile;
	private GUIStyle style;
	private Texture2D bgTex;
	private int guiWidth;
	private int guiHeight;
	private GUIStyle selectedStyle;
	private Texture2D selectedTex;
	private int selectedThumbnail = -1;
	private GUIStyle hoverStyle;
	private Texture2D hoverTex;
	private int scrollIndex = 0;
	private int scrollArrowHeight;
	
	private GUIStyle arrowBGStyle;
	private Texture2D scrollArrowBG;
	
	private int scrollArrowWidth = 40;
	
	private int border = 5;
	
	private int editorBtnsWidth = 32;
	private int editorBtnsHeight = 32;
	
	private int thumbnailWidth = 64;
	private int thumbnailHeight = 64;
	
	private int menuBtnsWidth = 32;
	private int menuBtnsHeight = 32;

	private int guiOffset = 50;
	
	//Arraylists for each collection of textures
	private ArrayList editorButtons;
	private ArrayList editorPressedButtons;
	private ArrayList menuButtons;
	private ArrayList menuPressedButtons;
	private ArrayList thumbnails;
	
	//editor textures
	public Texture2D btnRotateLeft;
	public Texture2D btnPressedRotateLeft;
	public Texture2D btnRotateRight;
	public Texture2D btnPressedRotateRight;
	public Texture2D btnDelete;
	public Texture2D btnPressedDelete;
	
	//menu textures
	public Texture2D btnImport;
	public Texture2D btnPressedImport;
	public Texture2D btnLocked;
	public Texture2D btnPressedLocked;
	public Texture2D btnBuild;
	public Texture2D btnPressedBuild;
	public Texture2D btnSettings;
	public Texture2D btnPressedSettings;
	public Texture2D btnLoad;
	public Texture2D btnPressedLoad;
	public Texture2D btnSave;
	public Texture2D btnPressedSave;
	public Texture2D btnQuit;
	public Texture2D btnPressedQuit;
	//new buttons
	public Texture2D btnShortCut;
	public Texture2D btnPressedShortCut;
	
	//Scroll textures
	public Texture2D btnArrowLeft;
	public Texture2D btnArrowRight;

	//Bool to check if current world is already saved
	public bool worldSaved;

	//Bluetooth textures
	public Texture2D bluetoothEmpty;
	public Texture2D bluetoothFilled;
	private int bluetoothLabelWidth = 131;
	private int bluetoothLabelHeight = 262;
	//Animation variables for Bluetooth textures
	//bluetoothEmpty:
	private int btEmptyShown = 0;
	private int btEmptyMaxShown = 3;
	private int btEmptyTimeBetween = 0;
	private int btEmptyMaxTimeBetween = 1000;
	private bool btEmptyShow = true;
	private bool btHadConnection = true;
	//bluetoothFilled:
	private float btFilledAlpha = 2;
	private int btFilledTimeBetween = 0;
	private int btFilledMaxTimeBetween = 25;
	private bool btFilledShow = true;
	private bool bthasLostConnection = false;
	
	void Start() {
		categories = new Categories();
		loadSaveBox = new LoadSaveBox();
		loadAreaFile = new LoadAreaFile();
		worldSaved = false;
		
		style = new GUIStyle();
		guiWidth = UnityEngine.Screen.width;
		guiHeight = 200;
		bgTex = new Texture2D(1, guiHeight);
		selectedStyle = new GUIStyle();
		hoverStyle = new GUIStyle();
		arrowBGStyle = new GUIStyle();
		
		int borderStart = guiHeight - (border + editorBtnsHeight + border - 13);
		int borderEnd = (guiHeight - border - border - thumbnailHeight);
		
		for (int y = 0; y < bgTex.height; ++y)
        {
			//int line = guiHeight - y;
			float r = .3f + (y * .003f);
			
			if(y == guiHeight - 2) {
				r = .9f;
			}
			else if(y == guiHeight - 1) {
				r = 1f;
			}
			
			if(y >= borderStart && y < borderStart + 2) {
            	r -= .1f;
				if( y == borderStart + 1) {
					r -= .1f;
				}
			}
			
			if(y >= borderStart + 2 && y <= borderEnd) {
				r = .8f;
			}
			
			if( y == borderEnd + 1) {
				r = .9f;
			}
			if(y == borderEnd + 2) {
				r = 1f;
			}
			
            Color color = new Color(r, r, r, 1);
            bgTex.SetPixel(0, y, color);
        }
        bgTex.Apply();
		
		style.normal.background = bgTex;
		
		selectedTex = new Texture2D(1,1);
		selectedTex.SetPixel(0,0, new Color(.4f, .4f, 1f, .5f));
		selectedTex.Apply();
		selectedStyle.normal.background = selectedTex;
		
		hoverTex = new Texture2D(thumbnailWidth, thumbnailHeight);
		for (int y = 0; y < hoverTex.height; y++) {
			for (int x = 0; x < hoverTex.width; x++) {
				if(y <= 3 || y >= hoverTex.height - 4 || x <= 3 || x >= hoverTex.width - 4) {
					hoverTex.SetPixel(x, y, new Color(.4f, .4f, 1f, 1f));
				}
			}
		}
		
		hoverTex.Apply();
		hoverStyle.normal.background = hoverTex;
		
		editorButtons = new ArrayList();
		editorButtons.Add(btnRotateLeft);
		editorButtons.Add(btnRotateRight);
		editorButtons.Add(btnDelete);
		
		editorPressedButtons = new ArrayList();
		editorPressedButtons.Add(btnPressedRotateLeft);
		editorPressedButtons.Add(btnPressedRotateRight);
		editorPressedButtons.Add(btnPressedDelete);
		
		menuButtons = new ArrayList();
		menuButtons.Add(btnLocked);
		menuButtons.Add(btnBuild);
		menuButtons.Add(btnSettings);
		menuButtons.Add(btnImport);
		menuButtons.Add(btnLoad);
		menuButtons.Add(btnSave);
		menuButtons.Add(btnQuit);
		menuButtons.Add (btnShortCut);
		
		menuPressedButtons = new ArrayList();
		menuPressedButtons.Add(btnPressedLocked);
		menuPressedButtons.Add(btnPressedBuild);
		menuPressedButtons.Add(btnPressedSettings);
		menuPressedButtons.Add(btnPressedImport);
		menuPressedButtons.Add(btnPressedLoad);
		menuPressedButtons.Add(btnPressedSave);
		menuPressedButtons.Add(btnPressedQuit);
		menuPressedButtons.Add (btnPressedShortCut);
		
		scrollArrowHeight = (borderStart - borderEnd) + 25;
		scrollArrowBG = new Texture2D(1, scrollArrowHeight);
		
		for (int i = 0; i < scrollArrowHeight; i++) {
			float r = .63f + (i * .002f);
			scrollArrowBG.SetPixel(0, i, new Color(r,r,r,1));
		}
		
		scrollArrowBG.Apply();
		arrowBGStyle.normal.background = scrollArrowBG;
	}

	void OnGUI() {
		loadSaveBox.drawGUI();
		loadAreaFile.drawGUI();
		if(!GameObject.FindGameObjectWithTag("MainScripts").GetComponent<SimulationScript>().isSimulationRunning) {
			int xPos = (int)((UnityEngine.Screen.width * .5) - (guiWidth * .5));
			int yPos = (UnityEngine.Screen.height - getGUIHeight());
			GUI.Label(new Rect (xPos, yPos, UnityEngine.Screen.width, 300), bgTex, style);
			
			foreach(Texture2D tex in editorButtons) {
				int index = editorButtons.IndexOf(tex);
				int btnPosX = (int)(border + scrollArrowWidth + (editorBtnsWidth + border) * editorButtons.IndexOf(tex));
				Texture2D pressedTex = (Texture2D)editorPressedButtons[editorButtons.IndexOf(tex)];
				addTopButtons(tex, pressedTex, btnPosX);
			}
			
			//Add arrows to scroll the thumbnails
			int arrowPosY = UnityEngine.Screen.height - getGUIHeight() + border + border + editorBtnsHeight - 2;
			if(GUI.Button(new Rect(UnityEngine.Screen.width - scrollArrowWidth, arrowPosY, scrollArrowWidth, scrollArrowHeight), scrollArrowBG, arrowBGStyle)) {
				//Scrollbutton right
				scrollThumbnails(-thumbnailWidth);
				
			};
			if(GUI.Button(new Rect(0, arrowPosY, scrollArrowWidth, scrollArrowHeight), scrollArrowBG, arrowBGStyle)) {
				//Scrollbutton left
				scrollThumbnails(thumbnailWidth);
			}
			
			//Draw the thumbnails
			thumbnails = categories.GetSelectedThumbnails();
			int foreachIndex = -1;
			foreach(Texture2D tex in thumbnails) {
				foreachIndex++;
				int btnPosX = (int)scrollArrowWidth + scrollIndex + (thumbnailWidth * foreachIndex);
				addMiddleButtons(tex, btnPosX);
			}
			
			//Draw the menu buttons
			foreach(Texture2D tex in menuButtons) {
				int index = menuButtons.IndexOf(tex);
				int btnPosX = (int)((UnityEngine.Screen.width - scrollArrowWidth) - ((menuBtnsWidth + border ) * (menuButtons.Count - index)));
				Texture2D pressedTex = (Texture2D)menuPressedButtons[menuButtons.IndexOf(tex)];
				addBottomButtons(tex, pressedTex, btnPosX);
			}
			
			//Add arrows to scroll the thumbnails
			//Right
			GUI.Label(new Rect(UnityEngine.Screen.width - scrollArrowWidth, arrowPosY, scrollArrowWidth, scrollArrowHeight), scrollArrowBG, arrowBGStyle);
			GUI.Label(new Rect(UnityEngine.Screen.width - scrollArrowWidth, arrowPosY + (scrollArrowHeight / 6), scrollArrowWidth, scrollArrowHeight), btnArrowRight);
			//Left
			GUI.Label(new Rect(0, arrowPosY, scrollArrowWidth, scrollArrowHeight), scrollArrowBG, arrowBGStyle);
			GUI.Label(new Rect(0, arrowPosY + (scrollArrowHeight / 6), scrollArrowWidth, scrollArrowHeight), btnArrowLeft);
			
			//Draw tabs
			if(categories.DrawTabs()) {
				selectedThumbnail = 0;
				GridScript gridScript = GameObject.FindWithTag ("MainCamera").GetComponent<GridScript> ();
				gridScript.setSelectedObject("0none");
			}
		}
		else {//Only show the bluetooth-connection information when the simulation is active
			//Show bluetoothEmpty-animation if connection is lost
			if(elapsedTimeSinceLastMsg() > 3000) {
				bthasLostConnection = true;
				if(btHadConnection) {
					if(btEmptyTimeBetween > btEmptyMaxTimeBetween) {
						btEmptyTimeBetween = 0;
						if(btEmptyShown < btEmptyMaxShown) {
							btEmptyShow = !btEmptyShow;
							if(btEmptyShow) {
								btEmptyShown++;
							}
						}
						else {
							//If the bluetoothEmpty has been shown 4 times, stop showing it
							//and restart the connection
							btHadConnection = false;
							btEmptyShown = 0;
							BluetoothConnect.closeConnectionWithoutWrite();
						}
					}
					btEmptyTimeBetween += (int)(Time.deltaTime * 1000);
					
					if(btEmptyShow) {
						int btLabelX = UnityEngine.Screen.width - (bluetoothLabelWidth * 2);
						int btLabelY = UnityEngine.Screen.height - bluetoothLabelHeight - bluetoothLabelWidth;
						GUI.Label (new Rect(btLabelX, btLabelY, bluetoothLabelWidth, bluetoothLabelHeight), bluetoothEmpty);
					}
				}
			}
			else {
				//Show bluetoothFilled-animation once if connection is made
				btHadConnection = true;
				//Only show animation if the connection has been lost since last animation
				if(bthasLostConnection) {
					//Lower the alpha by 1 every 25ms
					if(btFilledTimeBetween > btFilledMaxTimeBetween) {
						btFilledAlpha -= 0.005f;
						//If the alpha <= 0, stop showing bluetoothFilled
						if(btFilledAlpha <= 0) {
							bthasLostConnection = false;
							btFilledAlpha = 2;
						}
					}

					Color currentColor = GUI.color;
					Color colorWithAlpha = Color.white;
					colorWithAlpha.a = btFilledAlpha;
					GUI.color = colorWithAlpha;

					int btLabelX = UnityEngine.Screen.width - (bluetoothLabelWidth * 2);
					int btLabelY = UnityEngine.Screen.height - bluetoothLabelHeight - bluetoothLabelWidth;
					GUI.Label (new Rect(btLabelX, btLabelY, bluetoothLabelWidth, bluetoothLabelHeight), bluetoothFilled);

					GUI.color = currentColor;
					btFilledTimeBetween += (int)(Time.deltaTime * 1000);
				}
			}
		}
	}

	private double elapsedTimeSinceLastMsg() {
		string path = BluetoothConnect.readFromPath;
		System.DateTime lastWriteTime = System.IO.File.GetLastWriteTime(path);
		System.DateTime currentTime = System.DateTime.Now;
		System.TimeSpan difference = currentTime.Subtract(lastWriteTime);

		return difference.TotalMilliseconds;
	}

	private void addTopButtons(Texture2D texture, Texture2D pressedTex, int xPos) {
		Rect btnRect = new Rect(xPos, UnityEngine.Screen.height - getGUIHeight() + border, editorBtnsWidth, editorBtnsHeight);
		Texture2D tex = texture;
		
		int dfb = (int)(UnityEngine.Screen.height - Input.mousePosition.y) - 1;
		
		//Hover top buttons
		if(btnRect.Contains(new Vector2(Input.mousePosition.x, dfb)) && !Input.GetMouseButton(0)) {
			tex = pressedTex;
		}
		
		//Check if deleted is selected
		if(texture == btnDelete && GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GridScript>().getDeleteMode()) {
			tex = pressedTex;
		}
		
		if(GUI.Button(btnRect, tex, "")) {
			if(texture == btnRotateLeft) {
				GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GridScript>().RotateObjectLeft();
			}
			else if(texture == btnRotateRight) {
				GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GridScript>().RotateObjectRight();
			}
			else if(texture == btnDelete) {
				GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GridScript>().switchDeleteMode();
			}
		}
	}
	
	private void addMiddleButtons(Texture2D texture, int xPos) {
		Rect btnRect = new Rect(xPos, UnityEngine.Screen.height - getGUIHeight() + border + border + editorBtnsHeight, thumbnailWidth, thumbnailHeight);
		Texture2D tex = texture;
		
		int dfb = (int)(UnityEngine.Screen.height - Input.mousePosition.y) - 1;
		
		//Hover middle buttons
		if(new Rect(xPos, UnityEngine.Screen.height - getGUIHeight() + border + border + editorBtnsHeight, thumbnailWidth, thumbnailHeight).Contains(new Vector2(Input.mousePosition.x, dfb))) {
			GUI.Label(btnRect, hoverTex, hoverStyle);
		}
		
		//Change selected object
		if(selectedThumbnail == thumbnails.IndexOf(texture)) {
			GUI.Label(btnRect, selectedTex, selectedStyle);
		}
		
		//Add the button
		if(GUI.Button(btnRect, tex, "")) {
			selectedThumbnail = thumbnails.IndexOf(texture);
			GridScript gridScript = GameObject.FindWithTag ("MainCamera").GetComponent<GridScript> ();
			gridScript.setSelectedObject(texture.name);
			if(texture.name != "0none"){
				if(gridScript.objectToPlace.layer == 8){
					GameObject.FindGameObjectWithTag("MainScripts").GetComponent<OptieGUI>().setSelected ("object");
					GameObject.FindGameObjectWithTag("MainScripts").GetComponent<OptieGUI>().setSelectedGameObject (gridScript.objectToPlace);
				}
			}
			GameObject.FindGameObjectWithTag("MainScripts").GetComponent<SelectObject>().menuSelected = texture.name;
			//Switch delete mode off if a thumbnail is selected.
			if(GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GridScript>().getDeleteMode()) {
				GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GridScript>().switchDeleteMode();
			}
		}
	}
	
	private void addBottomButtons(Texture2D texture, Texture2D pressedTex, int xPos) {
		Rect btnRect = new Rect(xPos, UnityEngine.Screen.height - getGUIHeight() + border, menuBtnsWidth, menuBtnsHeight);
		Texture2D tex = texture;
		
		int dfb = (int)(UnityEngine.Screen.height - Input.mousePosition.y) - 1;
		
		if(btnRect.Contains(new Vector2(Input.mousePosition.x, dfb)) && !Input.GetMouseButton(0)) {
			tex = pressedTex;
		}
		if(GUI.Button(btnRect, tex, "")) {
			if(texture == btnBuild) {
				GameObject.FindGameObjectWithTag("MainScripts").GetComponent<SimulationScript>().SimulationToggle();
			}
			else if(texture == btnSettings) {
				GameObject.FindGameObjectWithTag ("MainScripts").GetComponent<OptieGUI> ().setSelected("car");
			}
			else if(texture == btnImport) {
				loadAreaFile.show();
			}
			else if(texture == btnLoad) {
				var confirmResult = MessageBox.Show("Do you want to save the current world before loading?","", MessageBoxButtons.YesNo);
				if (confirmResult == DialogResult.Yes)
				{
					loadSaveBox.openSaveDialog();
				}
				else
				{
					loadSaveBox.openLoadDialog();
				}
			}
			else if(texture == btnSave) {
				loadSaveBox.openSaveDialog();
			}
			else if(texture == btnQuit) {
				UnityEngine.Application.Quit();
			}
			else if(texture == btnLocked) {
				if(GameObject.FindGameObjectWithTag ("MainScripts").GetComponent<OptieGUI> ().guiLock2){
					GameObject.FindGameObjectWithTag ("MainScripts").GetComponent<OptieGUI> ().guiLock2 = false;
				}else{
					GameObject.FindGameObjectWithTag ("MainScripts").GetComponent<OptieGUI> ().guiLock2 = true;
				}
			}
			else if(texture == btnShortCut) {
				SendKeys.Send("{F1}");
			}
		}
	}
	
	private void scrollThumbnails(int offset) {
		scrollIndex += offset;
		int minScrollIndex = -(thumbnails.Count * thumbnailWidth - thumbnailWidth);
		if(scrollIndex > 0) {
			scrollIndex = 0;
		}
		else if(scrollIndex < minScrollIndex) {
			scrollIndex = minScrollIndex;
		}
	}
	
	public bool loadSaveBoxActive() {
		return loadSaveBox.getShowBox();
	}
	
	public void showLoadBox() {
		loadSaveBox.openLoadDialog();
	}
	
	public void showSaveBox() {
		loadSaveBox.openSaveDialog();
	}

	public int getGUIHeight() {
		return guiHeight - guiOffset;
	}

	public LoadAreaFile getLoadAreaFile() {
		return loadAreaFile;
	}
}
