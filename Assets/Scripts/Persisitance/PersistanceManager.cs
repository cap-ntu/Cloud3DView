using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public static class PersistanceManager {

	private static readonly string LOCAL_MODEL_DATA_FOLDER_NAME = "Local Json";
	public static string localModelDataFolderPath {
		get {
			string persistentDataPath = Application.persistentDataPath;
			if (Application.platform == RuntimePlatform.WindowsEditor ||
			    Application.platform == RuntimePlatform.WindowsPlayer ||
			    Application.platform == RuntimePlatform.WindowsWebPlayer) {
				persistentDataPath = persistentDataPath.Replace('/', '\\');
			}
			return Path.Combine(persistentDataPath, LOCAL_MODEL_DATA_FOLDER_NAME);
			//return Application.persistentDataPath + Path.DirectorySeparatorChar + LOCAL_MODEL_DATA_FOLDER_NAME;
		}
	}

	public static void StoreLocalModelData (string filename, string dataContent, string modelPath) {
		Debug.Log(Application.persistentDataPath);
		if (!File.Exists(localModelDataFolderPath)) {
			Directory.CreateDirectory(localModelDataFolderPath);	
		}
		
		string containerFolderPath = Path.Combine(localModelDataFolderPath, filename);
		Directory.CreateDirectory(containerFolderPath);

		//string jsonFilePath = containerFolderPath + Path.DirectorySeparatorChar + filename + ".json";
		string jsonFilePath = Path.Combine(containerFolderPath, filename + ".json");
		File.WriteAllText(jsonFilePath, dataContent);

		var arr = modelPath.Split('.');
		string destinationModelFolder = Path.Combine(containerFolderPath, "Model");
		if (!Directory.Exists(destinationModelFolder)) {
			Debug.Log("Create " + destinationModelFolder);
			Directory.CreateDirectory(destinationModelFolder);
		}

		string destinationFilePath = Path.Combine(destinationModelFolder, filename + "." + arr[arr.Length-1]);

		if (!modelPath.Equals(destinationFilePath)) {
			CopyFile(modelPath, destinationFilePath);
		}
	}

	public static List<string> GetLocalModelData () {
		List<string> dataList = new List<string>();
		if (!Directory.Exists(localModelDataFolderPath)) return dataList;

		DirectoryInfo localModelDataFolder = new DirectoryInfo (localModelDataFolderPath);
		foreach (DirectoryInfo subFolder in localModelDataFolder.GetDirectories()) {
			FileInfo[] files = subFolder.GetFiles("*.json");
			if (files == null || files.Length != 1) {
				Debug.LogError("Incorrect json file reading in " + subFolder.FullName);
			}

			FileInfo jsonFile = files[0];
			string jsonString = File.ReadAllText(jsonFile.FullName);
			dataList.Add(jsonString);
		}

		return dataList;
	}

	public static string GetLocalServerModelPath (string serverName) {
		string path = Path.Combine(localModelDataFolderPath, serverName);
		path = Path.Combine(path, "Model");

		if (Directory.Exists(path)) {
			var directory = new DirectoryInfo(path);
			foreach (var file in directory.GetFiles()) {
				if (file.Name.Contains(".obj") ||
				    file.Name.Contains(".OBJ") ||
				    file.Name.Contains(".fbx") ||
				    file.Name.Contains(".FBX")) {
					path = Path.Combine(path, file.Name);
				}
			}
		}
		else {
			//Debug.LogError("Model folder not found: " + path);
		}

		return path;
	}

	private static void CopyFile (string filePath, string destinationFilePath) {
		if (!File.Exists(destinationFilePath)) {
			var file =File.Create(destinationFilePath);
			file.Close();
		}

		File.Copy(filePath, destinationFilePath, true);
	}

	private static void CopyFileContainerFolder (string filePath, string destinationFolder) {
		if (!Directory.Exists(destinationFolder)) {
			Directory.CreateDirectory(destinationFolder);
		}

		FileInfo fileInfo = new FileInfo(filePath);
		DirectoryInfo folderInfo = fileInfo.Directory;

		DirectoryCopy(folderInfo.FullName, destinationFolder, true);
	}

	private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
	{
		// Get the subdirectories for the specified directory.
		DirectoryInfo dir = new DirectoryInfo(sourceDirName);
		DirectoryInfo[] dirs = dir.GetDirectories();
		
		if (!dir.Exists)
		{
			throw new DirectoryNotFoundException(
				"Source directory does not exist or could not be found: "
				+ sourceDirName);
		}
		
		// If the destination directory doesn't exist, create it. 
		if (!Directory.Exists(destDirName))
		{
			Directory.CreateDirectory(destDirName);
		}
		
		// Get the files in the directory and copy them to the new location.
		FileInfo[] files = dir.GetFiles();
		foreach (FileInfo file in files)
		{
			string temppath = Path.Combine(destDirName, file.Name);
			file.CopyTo(temppath, false);
		}
		
		// If copying subdirectories, copy them and their contents to new location. 
		if (copySubDirs)
		{
			foreach (DirectoryInfo subdir in dirs)
			{
				string temppath = Path.Combine(destDirName, subdir.Name);
				DirectoryCopy(subdir.FullName, temppath, copySubDirs);
			}
		}
	}
}
