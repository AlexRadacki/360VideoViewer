﻿using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MediaLoader : Singleton<MediaLoader> {

    public string imagePath;
    public string videoPath;

    public Transform contentParent;
    public GameObject movieItemPrefab;

    public Texture2D bgTex;

    // Use this for initialization
    void Awake ()
    {
#if UNITY_EDITOR
        imagePath = Application.dataPath + @"\Editor\StreamingAssets\Image\";
        videoPath = Application.dataPath + @"\Editor\StreamingAssets\Video\";
#elif UNITY_ANDROID && !UNITY_EDITOR
      imagePath = "/storage/emulated/0/Quadrolux/Image/";
      videoPath = "/storage/emulated/0/Quadrolux/Video/";
#endif
        if (!Directory.Exists(imagePath))
            Directory.CreateDirectory(imagePath);
        if (!Directory.Exists(videoPath))
            Directory.CreateDirectory(videoPath);
    }

    public void LoadImage(string name, Material target)
    {
        if (name.Length == 0)
        {
            return;
        }
        Texture2D tex = new Texture2D(2, 2);
        tex.LoadImage(File.ReadAllBytes(name));
        tex.Apply();
        target.mainTexture = tex;
        bgTex = tex;
    }

    public void CreateVideoItem(string path)
    {
        GameObject newMovieItem = Instantiate(movieItemPrefab, contentParent);
        VideoItem videoItem = newMovieItem.GetComponent<VideoItem>();
        videoItem.path = path;

        string thumbnailPath = path.Substring(0, path.Length - 3);
        thumbnailPath += "jpg";
        if (File.Exists(thumbnailPath))
        {
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(File.ReadAllBytes(thumbnailPath));
            tex.Apply();
            videoItem.thumbnail.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
        }
        else
        {
            Debug.Log("no thumbnail found at: " + thumbnailPath);
        }
    }
}
