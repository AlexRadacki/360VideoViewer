using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VideoItem : MonoBehaviour {

    public string path;

    private PanoramaCanvas panoramaCanvas;

	// Use this for initialization
	void Start ()
    {
        panoramaCanvas = FindObjectOfType<PanoramaCanvas>();
    }
	
    public void StartVideo()
    {
        if (panoramaCanvas.videoPlayer.isPlaying)
            panoramaCanvas.videoPlayer.Stop();
        panoramaCanvas.videoPlayer.url = path;
        panoramaCanvas.videoPlayer.Play();
    }
}
