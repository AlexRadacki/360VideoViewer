using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class PanoramaCanvas : Singleton<PanoramaCanvas> {

    public VideoPlayer videoPlayer;
    public Material mat;
    public Material innerMat;

	// Use this for initialization
	void Start () {
        videoPlayer.loopPointReached += OnVideoFinished;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator CanvasBlenderBlack(float duration)
    {
        float t = 0;

        while (t < duration)
        {
            t += Time.deltaTime;
            innerMat.color = Color.Lerp(Color.clear, Color.black, t / duration);
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator CanvasBlenderClear(float duration)
    {
        float t = 0;
        t = 0;
        while (t < duration)
        {
            t += Time.deltaTime;
            innerMat.color = Color.Lerp(Color.black, Color.clear, t / duration);
            yield return new WaitForEndOfFrame();
        }
    }

    public void PlayMovie(string url)
    {
        StartCoroutine(BlendAndPlay(url));
    }

    IEnumerator BlendAndPlay(string url)
    {
        StartCoroutine(CanvasBlenderBlack(1f));
        yield return new WaitForSeconds(1f);
        videoPlayer.url = url;
        videoPlayer.Play();
        while (!videoPlayer.isPrepared)
        {
            yield return new WaitForEndOfFrame();
        }
        StartCoroutine(CanvasBlenderClear(1f));
    }

    IEnumerator BackToMain()
    {
        StartCoroutine(CanvasBlenderBlack(1f));
        yield return new WaitForSeconds(1f);

        // switch texture to main
        //mat.mainTexture = MediaLoader.Instance.bgTex;
        videoPlayer.Stop();
        yield return new WaitForEndOfFrame();

        StartCoroutine(CanvasBlenderClear(1f));
    }

    void OnVideoFinished(VideoPlayer source)
    {
        CanvasManager.Instance.ToggleCanvas();
        foreach (var item in FindObjectsOfType<VideoItem>())
        {
            item.isPlaying = false;
        }
        StartCoroutine(BackToMain());
    }
}
