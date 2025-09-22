using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Net.NetworkInformation;

public class FileDataHandle 
{
    private string dataDirPath = "";
    private string[] dataFileName;
    private string[] dataFileName_preview;

    public FileDataHandle(string _dataDirPath, string[] _dataFileName, string[] _dataFileName_preview)
    {
        dataDirPath = _dataDirPath;
        dataFileName = _dataFileName;
        dataFileName_preview = _dataFileName_preview;
    }

    public void Save(GameData _data, GameData_Preview _data_preview, int index = 0)
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName[index]);
        string fullPath_preview = Path.Combine(dataDirPath, dataFileName_preview[index]);

        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            string dataToStore = JsonUtility.ToJson(_data, true);

            using(FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using(StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
        }
        catch(Exception e)
        {
            Debug.LogError(fullPath + "\n" + e);
        }

        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath_preview));

            string dataToStore = JsonUtility.ToJson(_data_preview, true);

            using (FileStream stream = new FileStream(fullPath_preview, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError(fullPath_preview + "\n" + e);
        }
    }

    public GameData Load(int index = 0)
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName[index]);
        GameData loadData = null;

        if (File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";

                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                loadData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch (Exception e)
            {
                Debug.LogError(fullPath+ "\n" + e);
            }
        }
        else
        {
            Debug.Log("Not Found Save File");

            return null;
        }

        return loadData;
    }

    public GameData_Preview LoadPreviewGameData(int index = 0)
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName_preview[index]);
        GameData_Preview loadData = null;

        if (File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";

                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                loadData = JsonUtility.FromJson<GameData_Preview>(dataToLoad);
            }
            catch (Exception e)
            {
                Debug.LogError(fullPath + "\n" + e);
            }
        }
        else
        {
            return null;
        }

        return loadData;
    }
}