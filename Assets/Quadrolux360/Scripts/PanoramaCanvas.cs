using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class PanoramaCanvas : Singleton<PanoramaCanvas> {

    public VideoPlayer videoPlayer;
    public Material mat;
    public Material innerMat;
    public bool isPlaying;

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

    public void StopMovie()
    {
        CanvasManager.Instance.ToggleCanvas();
        foreach (var item in FindObjectsOfType<VideoItem>())
        {
            item.isPlaying = false;
        }
        StartCoroutine(BackToMain());
    }

    public void StopMovieAndGoBackToStart()
    {
        CanvasManager.Instance.ToggleCanvas();
        foreach (var item in FindObjectsOfType<VideoItem>())
        {
            item.isPlaying = false;
        }
        videoPlayer.Stop();
        isPlaying = false;
    }

    IEnumerator BlendAndPlay(string url)
    {
        StartCoroutine(CanvasBlenderBlack(1f));
        yield return new WaitForSeconds(1f);
        videoPlayer.url = url;
        Debug.Log("start video");
        videoPlayer.Play();
        isPlaying = true;
        float t = 0;
        while (!videoPlayer.isPrepared)
        {
            t += Time.deltaTime;
            if (t > 2f)
            {
                Debug.Log("preparation takes over 2s, resetting...");
                videoPlayer.Stop();
                yield return new WaitForEndOfFrame();
                yield return new WaitForEndOfFrame();
                yield return new WaitForEndOfFrame();
                t = 0;
                videoPlayer.Play();
            }
            yield return new WaitForEndOfFrame();
        }
        Debug.Log("video prepared");
        StartCoroutine(CanvasBlenderClear(1f));
        yield return new WaitForSeconds(0.2f);
        while (!videoPlayer.isPlaying)
        {
            Debug.Log("video not playing");
            videoPlayer.Stop();
            yield return new WaitForEndOfFrame();
            Debug.Log("restart video");
            videoPlayer.Play();
            while (!videoPlayer.isPrepared)
            {
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForSeconds(0.2f);
        }
        Debug.Log("video started successfully");
    }

    IEnumerator BackToMain()
    {
        StartCoroutine(CanvasBlenderBlack(1f));
        yield return new WaitForSeconds(1f);

        // switch texture to main
        //mat.mainTexture = MediaLoader.Instance.bgTex;
        videoPlayer.Stop();
        isPlaying = false;

        CanvasManager.Instance.UpdateMediaGrid(2);
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
