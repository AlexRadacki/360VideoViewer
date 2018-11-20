using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections;


public class CanvasManager : Singleton<CanvasManager> {

    public RectTransform canvasTransform;
    public Transform panoramaSphere;
    public CanvasGroup canvasGroup;
    public GridLayoutGroup gridLayout;

    private string pathToConfig;
    private bool isHidden;
    private Transform gridParent;

    // Use this for initialization
    void Start ()
    {
        OVRManager.HMDUnmounted += OnHeadsetUnmounted;
        gridParent = gridLayout.transform;
        // load config file for the canvas placement
#if UNITY_EDITOR
        pathToConfig = Application.dataPath + @"\Editor\StreamingAssets\Config\";
#elif UNITY_ANDROID && !UNITY_EDITOR
        pathToConfig = @"/storage/emulated/0/Quadrolux/Config";
#endif
        if (!Directory.Exists(pathToConfig))
            Directory.CreateDirectory(pathToConfig);

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

                panoramaSphere.position = StaticTools.Vector3FromString(configData[5], ',');
                panoramaSphere.rotation = Quaternion.Euler(StaticTools.Vector3FromString(configData[6], ','));
                panoramaSphere.localScale = StaticTools.Vector3FromString(configData[7], ',');
            }
        }
        else
        {
            Debug.LogWarning("config.txt not found in path: " + pathToConfig);
        }
    }

    private void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.Back) || Input.GetKeyDown(KeyCode.Space))
        {
            SaveConfigToFile();
        }
    }

    void OnHeadsetUnmounted()
    {
        UpdateMediaGrid(1);
        if(PanoramaCanvas.Instance.isPlaying) PanoramaCanvas.Instance.StopMovie();
    }

    public void SaveConfigToFile()
    {
        if (!File.Exists(pathToConfig + "/config.txt"))
        {
            File.Create(pathToConfig + "/config.txt");
        }

        string[] saveData = new string[8];
        saveData[0] = StaticTools.StringFromVector3(canvasTransform.position);
        saveData[1] = StaticTools.StringFromVector2(canvasTransform.sizeDelta);
        saveData[2] = StaticTools.StringFromVector3(canvasTransform.rotation.eulerAngles);
        saveData[3] = StaticTools.StringFromVector3(canvasTransform.localScale);
        saveData[4] = "---";
        saveData[5] = StaticTools.StringFromVector3(panoramaSphere.position);
        saveData[6] = StaticTools.StringFromVector3(panoramaSphere.rotation.eulerAngles);
        saveData[7] = StaticTools.StringFromVector3(panoramaSphere.localScale);

        File.WriteAllLines(pathToConfig + "/config.txt", saveData);
    }

    public void ToggleCanvas()
    {
        StartCoroutine(CanvasFader(1f));
    }

    public void UpdateMediaGrid(int amount)
    {
        GameObject[] mediaItems = new GameObject[gridParent.childCount];
        for (int i = 0; i < gridParent.childCount; i++)
        {
            mediaItems[i] = gridParent.GetChild(i).gameObject;
            mediaItems[i].SetActive(false);
        }

        switch (amount)
        {
            case 1:
                mediaItems[0].SetActive(true);
                gridLayout.childAlignment = TextAnchor.MiddleCenter;
                gridLayout.cellSize = new Vector2(1680, 945);
                mediaItems[0].GetComponent<VideoItem>().thumbnail.enabled = false;
                break;
            case 2:
                mediaItems[0].SetActive(true);
                mediaItems[0].GetComponent<VideoItem>().thumbnail.enabled = true;
                mediaItems[1].SetActive(true);
                gridLayout.childAlignment = TextAnchor.MiddleCenter;
                gridLayout.cellSize = new Vector2(840, 472.5f);
                break;
            case 3:
                mediaItems[0].SetActive(true);
                mediaItems[1].SetActive(true);
                mediaItems[2].SetActive(true);
                gridLayout.childAlignment = TextAnchor.UpperLeft;
                gridLayout.cellSize = new Vector2(840, 472.5f);
                break;
            case 4:
                mediaItems[0].SetActive(true);
                mediaItems[1].SetActive(true);
                mediaItems[2].SetActive(true);
                mediaItems[3].SetActive(true);
                gridLayout.childAlignment = TextAnchor.UpperLeft;
                gridLayout.cellSize = new Vector2(840, 472.5f);
                break;
            default:
                break;
        }
    }

    IEnumerator CanvasFader(float duration)
    {
        isHidden = !isHidden;
        float t = 0;
        if (!isHidden)
        {
            yield return new WaitForSeconds(1f);
        }
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
