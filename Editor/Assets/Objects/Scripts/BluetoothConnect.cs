using UnityEngine;
using System.Collections;
using System.IO;
using System;

public class BluetoothConnect : MonoBehaviour {

	private static string jarFile;
	public static string readFromPath = "C:/Temp/BTreceive.txt";
	private static string writeToPath = "C:/Temp/BTsend.txt";
	private System.DateTime prevReadDate = new System.DateTime();
	private static bool connectionStarted = false;
	private static int writeAttempts = 0;

	private static readonly string cmd_EXIT = "EXIT";

	// Use this for initialization
	void Start () {
		jarFile = Application.dataPath + "/BluetoothApp_Liability.jar";
	}
	
	// Update is called once per frame
	void Update () {
		readFromFile();
	}

	/**
	 * Reads commando's given by the connected device
	 */
	private string readFromFile() {
		string returnVal = "";
		if(File.Exists(readFromPath) && connectionStarted) {
			if(File.GetLastWriteTime(readFromPath).CompareTo(prevReadDate) != 0) {
				try {
					prevReadDate = File.GetLastWriteTime(readFromPath);
					byte[] bytes = File.ReadAllBytes(readFromPath);
					returnVal = System.Text.Encoding.UTF8.GetString(bytes);
//					UnityEngine.Debug.Log("Read[" + returnVal + "]");
				}
				catch(IOException ex) {
				}
			}
		}
		return returnVal;
	}

	/**
	 * Writes commando's to the connected device
	 * @Param write - The {@code string} to be written
	 */
	public static void writeToFile(string write) {
		if(connectionStarted) {
			try {
				File.WriteAllText(writeToPath, write);
				writeAttempts = 0;
			}
			catch(IOException ex) {
				writeAttempts ++;
				if(writeAttempts < 10) {
					writeToFile(write);
				}
			}
		}
	}

	/**
	 * Returns the {@code boolean} connectionStarted
	 */
	public static bool getConnectionStarted() {
		return connectionStarted;
	}

	/**
	 * Opens the plugin which starts a connection to a device.
	 */
	public static void startConnectionToDevice() {
		if(!connectionStarted) {
			Application.OpenURL(jarFile);
			connectionStarted = true;
		}
	}

	/**
	 * Writes the exit command, which closes the plugin that maintains the
	 * bluetooth connection.
	 */
	public static void closeConnection() {
		if(connectionStarted) {
			writeToFile (cmd_EXIT);
			connectionStarted = false;
		}
	}

	public static void closeConnectionWithoutWrite() {
		connectionStarted = false;
	}

	/**
	 * Event triggered on application closure.
	 */
	void OnApplicationQuit() {
		closeConnection();
	}
}












