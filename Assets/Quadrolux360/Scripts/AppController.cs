using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class AppController : Singleton<AppController> {

    void Awake()
    {
        System.Globalization.CultureInfo.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
    }

    // Use this for initialization
    void Start ()
    {
        LoadBackground();
        LoadVideos();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void LoadBackground()
    {
        MediaLoader.Instance.LoadImage(GetBgName(), PanoramaCanvas.Instance.mat);
    }

    void LoadVideos()
    {
        string[] files = Directory.GetFiles(MediaLoader.Instance.videoPath);
        List<string> validFiles = new List<string>();

        for (int i = 0; i < files.Length; i++)
        {
            if (files[i].EndsWith("mov") || files[i].EndsWith("avi") || files[i].EndsWith("mp4"))
            {
                validFiles.Add(files[i]);
            }
        }

        foreach (var item in validFiles)
        {
            MediaLoader.Instance.CreateVideoItem(item);
        }
    }

    string GetBgName()
    {
        if (Directory.GetFiles(MediaLoader.Instance.imagePath).Length == 0)
        {
            Debug.LogWarning("no bg image found in: "+ MediaLoader.Instance.imagePath);
            return "";
        }
        return Directory.GetFiles(MediaLoader.Instance.imagePath)[0];
    }
}
