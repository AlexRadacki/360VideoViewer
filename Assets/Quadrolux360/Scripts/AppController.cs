using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class AppController : Singleton<AppController> {

    public RectTransform canvasTransform;

    private string pathToConfig;

	// Use this for initialization
	void Start ()
    {
        System.Globalization.CultureInfo.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;

        // load config file for the canvas placement
#if UNITY_EDITOR
        pathToConfig = Application.streamingAssetsPath;
#elif UNITY_ANDROID
        pathToConfig = Application.persistentDataPath;
#endif

        if (File.Exists(pathToConfig + "/config.txt"))
        {
            string[] configData = File.ReadAllLines(pathToConfig + "/config.txt");
            if (configData.Length > 0)
            {
                // apply values
                // position

                canvasTransform.position = Vector3FromString(configData[0], ',');
                canvasTransform.sizeDelta = Vector2FromString(configData[1], ',');
                canvasTransform.rotation = Quaternion.Euler(Vector3FromString(configData[2], ','));
                canvasTransform.localScale = Vector3FromString(configData[3], ',');
            }
        }
        else
        {
            Debug.LogWarning("config.txt not found in path: " + pathToConfig);
        }
    }

    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            SaveConfigToFile();
        }
    }

    void SaveConfigToFile()
    {
        if (!File.Exists(pathToConfig + "/config.txt"))
        {
            File.Create(pathToConfig + "/config.txt");
        }

        string[] saveData = new string[6];
        saveData[0] = StringFromVector3(canvasTransform.position);
        saveData[1] = StringFromVector3(canvasTransform.sizeDelta);
        saveData[2] = StringFromVector3(canvasTransform.rotation.eulerAngles);
        saveData[3] = StringFromVector3(canvasTransform.localScale);

        File.WriteAllLines(pathToConfig + "/config.txt", saveData);
    }


    Vector2 Vector2FromString(string input, char seperator)
    {
        string[] vectorData = input.Split(seperator);
        return new Vector2(float.Parse(vectorData[0]), float.Parse(vectorData[1]));
    }

    Vector3 Vector3FromString(string input, char seperator)
    {
        string[] vectorData = input.Split(seperator);
        return new Vector3(float.Parse(vectorData[0]), float.Parse(vectorData[1]), float.Parse(vectorData[2]));
    }

    string StringFromVector2(Vector2 input)
    {
        string output = input.x + "," + input.y;
        return output;
    }

    string StringFromVector3(Vector3 input)
    {
        string output = input.x + "," + input.y + "," + input.z;
        return output;
    }
}
