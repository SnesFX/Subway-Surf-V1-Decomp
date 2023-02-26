using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class OnlineSettings
{
	private const string URL = "/onlinesettings.php?android";

	private const string FILE_SECRET = "pdvshbhkndf92k19zvbckawd92fjk";

	private const int FILEFORMAT_VERSION = 1;

	private static readonly char[] LINEBREAK_CHARS = new char[2] { '\n', '\r' };

	private static OnlineSettings _instance;

	private Dictionary<string, string> _settings = new Dictionary<string, string>();

	private bool _isDownloading;

	private GameObject _downloadGameObject;

	public bool isInstanced
	{
		get
		{
			return _instance != null;
		}
	}

	public static OnlineSettings instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new OnlineSettings();
			}
			return _instance;
		}
	}

	public bool isDownloading
	{
		get
		{
			return _isDownloading;
		}
	}

	private OnlineSettings()
	{
		LoadFromFile();
	}

	public void DownloadNow()
	{
		if (!_isDownloading)
		{
			if (_downloadGameObject != null)
			{
				UnityEngine.Object.Destroy(_downloadGameObject);
			}
			_downloadGameObject = new GameObject("OnlineSettingsDownloader");
			UnityEngine.Object.DontDestroyOnLoad(_downloadGameObject);
		}
	}

	public string GetValue(string key, string defaultValue)
	{
		string value;
		if (_settings.TryGetValue(key, out value))
		{
			return value;
		}
		return defaultValue;
	}

	public bool TryGetValue(string key, out string valueString)
	{
		return _settings.TryGetValue(key, out valueString);
	}

	public bool HasValue(string key)
	{
		return _settings.ContainsKey(key);
	}

	private void LoadFromFile()
	{
		try
		{
			string saveDataPath = GetSaveDataPath();
			byte[] buffer = FileUtil.Load(saveDataPath, "pdvshbhkndf92k19zvbckawd92fjk");
			MemoryStream memoryStream = new MemoryStream(buffer);
			BinaryReader binaryReader = new BinaryReader(memoryStream);
			int num = binaryReader.ReadInt32();
			_settings = FileUtil.ReadStringStringDictionary(binaryReader);
			memoryStream.Close();
		}
		catch (FileNotFoundException)
		{
		}
		catch (Exception)
		{
		}
	}

	private void SaveToFile()
	{
		try
		{
			MemoryStream memoryStream = new MemoryStream(4096);
			BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
			binaryWriter.Write(1);
			FileUtil.WriteStringStringDictionary(binaryWriter, _settings);
			string saveDataPath = GetSaveDataPath();
			FileUtil.Save(saveDataPath, "pdvshbhkndf92k19zvbckawd92fjk", memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
			memoryStream.Close();
		}
		catch (Exception)
		{
		}
	}

	private static string GetSaveDataPath()
	{
		return Application.persistentDataPath + "/onlinesettings";
	}
}
