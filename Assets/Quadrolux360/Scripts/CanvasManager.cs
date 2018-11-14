using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections;


public class CanvasManager : Singleton<CanvasManager> {

    public RectTransform canvasTransform;
    public CanvasGroup canvasGroup;

    private string pathToConfig;
    private bool isHidden;

	// Use this for initialization
	void Start ()
    {

        // load config file for the canvas placement
#if UNITY_EDITOR
        pathToConfig = Application.streamingAssetsPath;
#elif UNITY_ANDROID && !UNITY_EDITOR
        pathToConfig = Application.persistentDataPath;
#endif

        if (File.Exists(pathToConfig + "/config.txt"))
        {
            string[] configData = File.ReadAllLines(pathToConfig + "/config.txt");
            if (configData.Length > 0)
            {
                // apply values
                // position

                canvasTransform.position = StaticTools.Vector3FromString(configData[0], ',');
                canvasTransform.sizeDelta = StaticTools.Vector2FromString(configData[1], ',');
                canvasTransform.rotation = Quaternion.Euler(StaticTools.Vector3FromString(configData[2], ','));
                canvasTransform.localScale = StaticTools.Vector3FromString(configData[3], ',');
            }
        }
        else
        {
            Debug.LogWarning("config.txt not found in path: " + pathToConfig);
        }
    }

    private void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.Back))
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

        string[] saveData = new string[4];
        saveData[0] = StaticTools.StringFromVector3(canvasTransform.position);
        saveData[1] = StaticTools.StringFromVector2(canvasTransform.sizeDelta);
        saveData[2] = StaticTools.StringFromVector3(canvasTransform.rotation.eulerAngles);
        saveData[3] = StaticTools.StringFromVector3(canvasTransform.localScale);
        File.WriteAllLines(pathToConfig + "/config.txt", saveData);
    }

    public void ToggleCanvas()
    {
        StartCoroutine(CanvasFader(0.5f));
    }

    IEnumerator CanvasFader(float duration)
    {
        isHidden = !isHidden;
        float t = 0;
        
        while (t < duration)
        {
            t += Time.deltaTime;
            if (isHidden)
            {
                canvasGroup.alpha = Mathf.Lerp(1f, 0f, t / duration);
            }
            else
            {
                canvasGroup.alpha = Mathf.Lerp(0f, 1f, t / duration);
            }

            yield return new WaitForEndOfFrame();
        }

        if (isHidden)
        {
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
        else
        {
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }
    }
}
