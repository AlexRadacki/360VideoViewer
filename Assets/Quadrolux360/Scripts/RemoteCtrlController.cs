using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RemoteCtrlController : MonoBehaviour {

    public string uiMovieItemLayer;

    private int layerIndex;

	// Use this for initialization
	void Start ()
    {
        layerIndex = LayerMask.NameToLayer(uiMovieItemLayer);
    }
	
	// Update is called once per frame
	void Update () {
        RaycastWorldUI();
    }

    void RaycastWorldUI()
    {
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger) || Input.GetButtonDown("Jump"))
        {
            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, layerIndex))
            {
                hit.transform.GetComponent<VideoItem>().StartVideo();
            }
        }
    }
}
