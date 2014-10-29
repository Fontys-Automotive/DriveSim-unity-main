using UnityEngine;
using System.Collections;
using System.IO;

public class Categories : Object
{
	
	private ArrayList categories;
	private int selectedIndex = 0;
	private ArrayList selectedThumbnails;
	
	//bibeko50 textures
	public Texture2D bibeko50s;
	public Texture2D bibeko50c;
	public Texture2D bibeko50t;
	public Texture2D bibeko50i;
	
	//environment textures
	public Texture2D envTree1;
	
	private int xPosPerButton = 40;
	
	//Constructor
	public Categories ()
	{
		categories = new ArrayList ();
		
		string imagesFolder = Application.dataPath + "/Resources/guiImages";
		string[] files = Directory.GetDirectories (imagesFolder);
		
		foreach (string s in files) {
			string name = s.Substring (s.LastIndexOf ("\\") + 1);
			ArrayList thumbnails = new ArrayList ();
			
			string[] imagePaths = Directory.GetFiles (s);
			foreach (string s2 in imagePaths) {
				if (s2.EndsWith (".png")) {
					Texture2D tex = new Texture2D (64, 64);
					tex.LoadImage (File.ReadAllBytes (s2));
					tex.name = s2.Substring (s2.LastIndexOf ("\\") + 1).Replace (".png", "");
					thumbnails.Add (tex);
					//Resources.Load("glass", typeof(Texture2D));
				}
			}
			
			addNextCategory (name, thumbnails);
		}
		
		selectedThumbnails = ((Category)categories [0]).GetThumbnails ();
		
//		//Add roads
//		ArrayList bibeko50 = new ArrayList();
//		bibeko50.Add(bibeko50s);
//		bibeko50.Add(bibeko50c);
//		bibeko50.Add(bibeko50t);
//		bibeko50.Add(bibeko50i);
//		addNextCategory("Roads", bibeko50);
//		selectedThumbnails = bibeko50;
//		
//		//Add buildings
//		ArrayList buildings = new ArrayList();
//		addNextCategory("Buildings", buildings);
//		
//		//Add environment
//		ArrayList environment = new ArrayList();
//		environment.Add(envTree1);
//		addNextCategory("Environment", environment);
	}

	/**
	 * Adds a new {@code Category} to the UI.
	 * @Param name - The name of the category
	 * @Param thumbnails - the thumbnails which should be shown within the category
	 */
	private void addNextCategory (string name, ArrayList thumbnails)
	{
		categories.Add (new Category (name, thumbnails, xPosPerButton));
		xPosPerButton += (int)20 + (name.Length * 10);
	}

	/**
	 * Returns the thumbnails of the selected category
	 */
	public ArrayList GetSelectedThumbnails ()
	{
		return selectedThumbnails;
	}

	/**
	 * Draw the tabs on the UI. Each category has it's own tab.
	 */
	public bool DrawTabs ()
	{
		bool selectedChanged = false;
		int currentIndex = -1;
		foreach (Category c in categories) {
			currentIndex++;
			
			if (currentIndex != selectedIndex) {
				if (c.DrawButton ()) {
					selectedIndex = currentIndex;
					selectedThumbnails = c.GetThumbnails ();
					selectedChanged = true;
				}
			} else {
				c.DrawSelectedButton ();
			}
		}
		return selectedChanged;
	}
}
