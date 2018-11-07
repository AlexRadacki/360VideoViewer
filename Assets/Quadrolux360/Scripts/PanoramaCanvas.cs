using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class PanoramaCanvas : Singleton<PanoramaCanvas> {

    public VideoPlayer videoPlayer;
    public Material mat;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PlayMovie(string url)
    {
        videoPlayer.url = url;
        videoPlayer.Play();
    }
}
