using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VideoItem : MonoBehaviour {

    public string path;
    public float gazeTrigger;
    public Image fillSprite;
    public Image thumbnail;
    public bool isPlaying;

    private bool isLookingAt;
    private float gazeTimer;
    private float fillAmount;

    private PanoramaCanvas panoramaCanvas;

	// Use this for initialization
	void Start ()
    {
        panoramaCanvas = FindObjectOfType<PanoramaCanvas>();
        RemoteCtrlController.Instance.videoItems.Add(this);
    }
	
    public void StartVideo()
    {
        isPlaying = true;
        panoramaCanvas.PlayMovie(path);
        CanvasManager.Instance.ToggleCanvas();
    }

    private void Update()
    {
        fillSprite.fillAmount = StaticTools.Map(gazeTimer, 0f, gazeTrigger, 0f, 1f);
        if (isLookingAt)
        {
            gazeTimer += Time.deltaTime; 
        }
        else
        {
            gazeTimer -= 0.02f;
            if (gazeTimer < 0)
            {
                gazeTimer = 0;
            }
        }
        if (!isPlaying && gazeTimer >= gazeTrigger)
        {
            StartVideo();
        }
    }

    public void PointerEnter()
    {
        isLookingAt = true;
    }
    public void PointerExit()
    {
        isLookingAt = false;
    }
}
