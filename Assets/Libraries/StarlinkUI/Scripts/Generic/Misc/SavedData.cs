/*using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System;
using TNet;

/// <summary>
/// Simple generic save game functionality. Kind of like using PlayerPrefs, but everything gets saved into one file.
/// Use this class to save data that you want to be stored on the server.
/// </summary>

public class SavedData
{
	bool mLoaded = false;
	Dictionary<string, object> mData = new Dictionary<string, object>();

	/// <summary>
	/// Name of the file to save into.
	/// </summary>

	public string fileName = null;

	/// <summary>
	/// Set the value within the save.
	/// </summary>

	public void Set (string key, object val)
	{
		if (!mLoaded) Load();
		mData[key] = val;
	}

	/// <summary>
	/// Retrieve the value from within the save.
	/// </summary>

	public T Get<T> (string key)
	{
		if (!mLoaded) Load();
		if (mData.ContainsKey(key)) return (T)mData[key];
		return default(T);
	}

	/// <summary>
	/// Retrieve the value from within the save.
	/// </summary>

	public bool Get<T> (string key, ref T val)
	{
		if (!mLoaded) Load();

		if (mData.ContainsKey(key))
		{
			val = (T)mData[key];
			return true;
		}
		return false;
	}

	/// <summary>
	/// Clear the saved data.
	/// </summary>

	public void Clear () { mData.Clear(); mLoaded = true; }

	/// <summary>
	/// Save the data into the specified filename.
	/// </summary>

	public bool Save (string fn)
	{
		fileName = fn;
		return Save();
	}

	/// <summary>
	/// Save everything into the file.
	/// </summary>

	public bool Save ()
	{
		if (string.IsNullOrEmpty(fileName)) return false;
#if UNITY_WEBPLAYER
		return false;
#else
		if (mData.Count == 0) return false;

		string path = Application.persistentDataPath + "/" + fileName;
		FileStream file = null;

		try
		{
			file = File.Create(path);
		}
		catch (System.Exception ex)
		{
			Debug.Log(ex.Message);
			return false;
		}

		Save(file);
		file.Close();
		return true;
#endif
	}

	/// <summary>
	/// Convenience function that converts the saved data to a new memory stream.
	/// </summary>

	public MemoryStream ToStream ()
	{
		MemoryStream stream = new MemoryStream();
		Save(stream);
		return stream;
	}

	/// <summary>
	/// Convenience function -- convert the list to a byte array.
	/// </summary>

	public byte[] ToArray ()
	{
		MemoryStream ms = ToStream();
		byte[] arr = ms.ToArray();
		ms.Dispose();
		return arr;
	}

	/// <summary>
	/// Save everything into the specified stream.
	/// </summary>

	public void Save (Stream stream) { Save(new BinaryWriter(stream)); }

	/// <summary>
	/// Save everything using the specified binary writer.
	/// </summary>

	public void Save (BinaryWriter bw)
	{
		UnityTools.WriteInt(bw, mData.Count);

		foreach (KeyValuePair<string, object> pair in mData)
		{
			bw.Write(pair.Key);
			UnityTools.WriteObject(bw, pair.Value);
		}
	}

	/// <summary>
	/// Load the data from the specified filename.
	/// </summary>

	public bool Load (string fn)
	{
		fileName = fn;
		return Load();
	}

	/// <summary>
	/// Load everything from the file.
	/// </summary>

	public bool Load ()
	{
		Clear();
		mLoaded = true;
		if (string.IsNullOrEmpty(fileName)) return false;
#if UNITY_WEBPLAYER
		return false;
#else
		string path = Application.persistentDataPath + "/" + fileName;
		if (!File.Exists(path)) return false;
		FileStream file = null;

		try
		{
			file = File.OpenRead(path);
		}
		catch (System.Exception ex)
		{
			Debug.Log(ex.Message);
			return false;
		}

		Load(file);
		file.Close();
		return true;
#endif
	}

	/// <summary>
	/// Load everything from the specified stream.
	/// </summary>

	public void Load (Stream stream) { Load(new BinaryReader(stream)); }

	/// <summary>
	/// Load everything from the specified binary reader.
	/// </summary>

	public void Load (BinaryReader reader)
	{
		mLoaded = true;
		mData.Clear();
		int count = UnityTools.ReadInt(reader);

		for (int i = 0; i < count; ++i)
		{
			string key = reader.ReadString();
			mData[key] = UnityTools.ReadObject(reader);
		}
	}
}
*/