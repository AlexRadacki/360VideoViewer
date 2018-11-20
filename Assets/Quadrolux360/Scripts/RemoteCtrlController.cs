using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RemoteCtrlController : Singleton<RemoteCtrlController> {

    public string uiMovieItemLayer;
    public CanvasGroup controllerMainMenu;
    public CanvasGroup moveMenu;
    public CanvasGroup moveModeImage;

    public Button[] mainMenuButtonsLeft;
    public Button[] mainMenuButtonsRight;
    public Button[] moveMenuButtonsLeft;
    public Button[] moveMenuButtonsRight;

    public int currentMode;
    public AudioMixer mixer;

    public List<VideoItem> videoItems = new List<VideoItem>();

    private int volume = 0;
    private int layerIndex;
    private float operatorMenuTimer;

    private bool rightButtonSide;
    private bool upperButton;
    private int buttonIndex;

    private Button currentButton;
    private Transform transformToMove;
    private MeshRenderer meshRenderer;

    // Use this for initialization
    void Start ()
    {
        layerIndex = LayerMask.NameToLayer(uiMovieItemLayer);
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.enabled = false;
    }
	
	// Update is called once per frame
	void Update () {
        //RaycastWorldUI();

        if (currentMode == 0 && operatorMenuTimer < 2f && (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger) || Input.GetKey(KeyCode.Backspace)))
        {
            operatorMenuTimer += Time.deltaTime;
        }
        else
        {
            operatorMenuTimer = 0;
        }

        if (operatorMenuTimer > 2f)
        {
            currentMode = 1;
            meshRenderer.enabled = true;
            Debug.Log("enter menu");
            ToggleMenu(controllerMainMenu, true);
            SelectButton(mainMenuButtonsLeft[0]);
        }

        if (currentMode > 0)
        {
            if (((OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad).x < -0.5f && OVRInput.GetDown(OVRInput.Button.PrimaryTouchpad)) || Input.GetKeyDown(KeyCode.LeftArrow)))
            {
                EventSystem.current.SetSelectedGameObject(null);
                rightButtonSide = false;
                switch (currentMode)
                {
                    case 1:
                        SelectButton(mainMenuButtonsLeft[0]);
                        break;
                    case 2:
                        if (currentButton == moveMenuButtonsRight[0])
                            SelectButton(moveMenuButtonsLeft[0]);
                        else
                            SelectButton(moveMenuButtonsLeft[1]);
                        break;
                    default:
                        break;
                }
            }

            if (((OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad).x > 0.5f && OVRInput.GetDown(OVRInput.Button.PrimaryTouchpad)) || Input.GetKeyDown(KeyCode.RightArrow)))
            {
                EventSystem.current.SetSelectedGameObject(null);
                rightButtonSide = true;
                switch (currentMode)
                {
                    case 1:
                        SelectButton(mainMenuButtonsRight[0]);
                        break;
                    case 2:
                        if (currentButton == moveMenuButtonsLeft[0])
                            SelectButton(moveMenuButtonsRight[0]);
                        else
                            SelectButton(moveMenuButtonsRight[1]);
                        break;
                    default:
                        break;
                }
            }

            if (OVRInput.GetDown(OVRInput.Button.Back) || Input.GetKeyDown(KeyCode.Backspace))
            {
                ExitMenu();
            }

            if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger) || Input.GetKeyDown(KeyCode.R))
            {
                currentButton.onClick.Invoke();
            }

            if (currentMode == 2 && ((OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad).y > 0.5f && OVRInput.GetDown(OVRInput.Button.PrimaryTouchpad)) || Input.GetKeyDown(KeyCode.UpArrow)))
            {
                EventSystem.current.SetSelectedGameObject(null);
                if (rightButtonSide)
                {
                    SelectButton(moveMenuButtonsRight[0]);
                }
                else
                {
                    SelectButton(moveMenuButtonsLeft[0]);
                }
            }

            if (currentMode == 2 && ((OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad).y < -0.5f && OVRInput.GetDown(OVRInput.Button.PrimaryTouchpad)) || Input.GetKeyDown(KeyCode.DownArrow)))
            {
                EventSystem.current.SetSelectedGameObject(null);
                if (rightButtonSide)
                {
                    SelectButton(moveMenuButtonsRight[1]);
                }
                else
                {
                    SelectButton(moveMenuButtonsLeft[1]);
                }
            }

            if ((currentMode == 3 && transformToMove != null))
            {
                if (((OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad).y < -0.5f && OVRInput.GetDown(OVRInput.Button.PrimaryTouchpad)) || Input.GetKeyDown(KeyCode.DownArrow)))
                {
                    if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger) || Input.GetKey(KeyCode.RightShift))
                        transformToMove.position -= new Vector3(0, 0.1f, 0);
                    else
                        transformToMove.position -= new Vector3(0, 0, 0.1f);
                }
                if (((OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad).y > 0.5f && OVRInput.GetDown(OVRInput.Button.PrimaryTouchpad)) || Input.GetKeyDown(KeyCode.UpArrow)))
                {
                    if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger) || Input.GetKey(KeyCode.RightShift))
                        transformToMove.position += new Vector3(0, 0.1f, 0);
                    else
                        transformToMove.position += new Vector3(0, 0, 0.1f);
                }
                if (((OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad).x < -0.5f && OVRInput.GetDown(OVRInput.Button.PrimaryTouchpad)) || Input.GetKeyDown(KeyCode.LeftArrow)))
                {
                    if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger) || Input.GetKey(KeyCode.RightShift))
                        transformToMove.Rotate(transformToMove.up, -1f);
                    else
                        transformToMove.position -= new Vector3(0.1f, 0, 0);
                }
                if (((OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad).x > 0.5f && OVRInput.GetDown(OVRInput.Button.PrimaryTouchpad)) || Input.GetKeyDown(KeyCode.RightArrow)))
                {
                    if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger) || Input.GetKey(KeyCode.RightShift))
                        transformToMove.Rotate(transformToMove.up, 1f);
                    else
                    transformToMove.position += new Vector3(0.1f, 0, 0);
                }
            }
        }
        else
        {
            if ((OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad).x < -0.5f && OVRInput.GetDown(OVRInput.Button.PrimaryTouchpad)) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (!PanoramaCanvas.Instance.isPlaying)
                {
                    videoItems[0].StartVideo();
                }

            }
            if ((OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad).x > 0.5f && OVRInput.GetDown(OVRInput.Button.PrimaryTouchpad)) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (!PanoramaCanvas.Instance.isPlaying)
                {
                    videoItems[1].StartVideo();
                }
            }

            if ((OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad).y > 0.5f && OVRInput.GetDown(OVRInput.Button.PrimaryTouchpad)) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                //Volume up
                if(volume < 0) volume += 10;
                mixer.SetFloat("Volume", volume);
            }
            if ((OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad).y < -0.5f && OVRInput.GetDown(OVRInput.Button.PrimaryTouchpad)) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                //Volume down
                if (volume > -80) volume -= 10;
                mixer.SetFloat("Volume", volume);
            }
        }

        if (PanoramaCanvas.Instance.isPlaying)
        {
            if (OVRInput.GetDown(OVRInput.Button.Back) || Input.GetKeyDown(KeyCode.Backspace))
            {
                PanoramaCanvas.Instance.StopMovie();
            }
        }
    }

    void RaycastWorldUI()
    {
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger) || Input.GetButtonDown("Jump"))
        {
            //print("RaycastWorldUI");

            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, layerIndex))
            {
                print(hit.transform.name);
                hit.transform.GetComponent<VideoItem>().StartVideo();
            }
        }
    }

    void ToggleMenu(CanvasGroup canvasGroup, bool show)
    {
        if (show)
        {
            //print("toggle on: " + canvasGroup.transform.name);
            canvasGroup.blocksRaycasts = true;
            canvasGroup.interactable = true;
            canvasGroup.alpha = 1f;
        }
        else
        {
            //print("toggle off: " + canvasGroup.transform.name);
            canvasGroup.blocksRaycasts = false;
            //canvasGroup.interactable = false;
            canvasGroup.alpha = 0f;
        }
    }

    public void ExitMenu()
    {
        //print("ExitMenu: " + currentMode);
        EventSystem.current.SetSelectedGameObject(null);
        switch (currentMode)
        {
            case 1:
                currentMode = 0;
                rightButtonSide = false;
                ToggleMenu(controllerMainMenu, false);
                meshRenderer.enabled = false;
                break;
            case 2:
                currentMode = 1;
                ToggleMenu(controllerMainMenu, true);
                ToggleMenu(moveMenu, false);
                rightButtonSide = true;
                SelectButton(mainMenuButtonsRight[0]);
                break;
            case 3:
                currentMode = 2;
                ToggleMenu(moveMenu, true);
                ToggleMenu(moveModeImage, false);
                rightButtonSide = true;
                SelectButton(moveMenuButtonsRight[0]);
                break;
            default:
                break;
        }
    }

    public void GoToMoveMenu()
    {
        currentMode = 2;
        ToggleMenu(controllerMainMenu, false);
        ToggleMenu(moveMenu, true);
        rightButtonSide = false;
        SelectButton(moveMenuButtonsLeft[0]);
    }

    public void EnableMoveMode(Transform target)
    {
        currentMode = 3;
        ToggleMenu(moveMenu, false);
        ToggleMenu(moveModeImage, true);
        transformToMove = target;
    }

    public void SaveTransforms()
    {
        CanvasManager.Instance.SaveConfigToFile();
    } 

    void SelectButton(Button btn)
    {
        foreach (var item in FindObjectsOfType<Text>())
        {
            item.color = Color.white;
        }
        btn.Select();
        btn.transform.GetChild(0).GetComponent<Text>().color = Color.yellow;
        currentButton = btn;
    }
}
