using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DeviceCamera : MonoBehaviour
{
    private bool cameraAvailable;
    private WebCamTexture backCamera;
    private Texture defaultBackground;
    private Texture capturedTexture;
    Texture2D pipTexture;

    [SerializeField]
    GameObject CameraCanvas;
    [SerializeField]
    GameObject CameraPanel;
    [SerializeField]
    GameObject CostumePanel;
    [SerializeField]
    RawImage background;
    [SerializeField]
    RawImage picInPic;
    [SerializeField]
    RawImage suitFace;
    [SerializeField]
    AspectRatioFitter fit;
    [SerializeField]
    TextMeshProUGUI DebugText;
    [SerializeField]
    RectTransform FramingWindow;

    float captureSquareLength = 200f;

    // Start is called before the first frame update
    void Start()
    {
        defaultBackground = background.texture;
        WebCamDevice[] devices = WebCamTexture.devices;

        if (devices.Length == 0)
        {
            Debug.Log("no camera available");
            cameraAvailable = false;
            return;
        }

        for (int x = 0; x < devices.Length; x++)
        {
            if (!devices[x].isFrontFacing)
            {
                backCamera = new WebCamTexture(devices[x].name, Screen.width, Screen.height);
            }
        }
        if (backCamera == null)
        {
            Debug.Log("no back camera available");
            return;
        }

        backCamera.Play();
        background.texture = backCamera;

        cameraAvailable = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!cameraAvailable)
            return;

        float ratio = (float)backCamera.width / (float)backCamera.height;

        background.rectTransform.sizeDelta = new Vector2((float)backCamera.width, (float)backCamera.height);
        FramingWindow.sizeDelta = new Vector2(captureSquareLength, captureSquareLength);

        fit.aspectRatio = ratio;

        float scaleY = backCamera.videoVerticallyMirrored ? -1f : 1f;
        background.rectTransform.localScale = new Vector3(1f, scaleY, 1f);

        int orient = -backCamera.videoRotationAngle;
        background.rectTransform.localEulerAngles = new Vector3(0, 0, orient);

        DebugText.text = "w:" + backCamera.width + ", h:" + backCamera.height + ", r:" + ratio + ", s:" + scaleY + ", o:" + orient;
    }

    public void CaptureImage()
    {
        if (!cameraAvailable)
            return;

        float scaleY = backCamera.videoVerticallyMirrored ? -1f : 1f;
        picInPic.rectTransform.localScale = new Vector3(1f, scaleY, 1f);
        suitFace.rectTransform.localScale = new Vector3(1f, scaleY, 1f);

        int orient = -backCamera.videoRotationAngle;
        picInPic.rectTransform.localEulerAngles = new Vector3(0, 0, orient);
        suitFace.rectTransform.localEulerAngles = new Vector3(0, 0, orient);

        Color[] c = backCamera.GetPixels((int)(backCamera.width / 2f - captureSquareLength / 2f), (int)(backCamera.height / 2f - captureSquareLength / 2f), (int)captureSquareLength, (int)captureSquareLength);
        pipTexture = new Texture2D((int)captureSquareLength, (int)captureSquareLength);
        pipTexture.SetPixels(c);
        pipTexture.Apply();
        picInPic.texture = pipTexture;
        suitFace.texture = pipTexture;
    }

    public void ShowCamera()
    {
        CameraCanvas.SetActive(true);
        CameraCanvas.SetActive(true);
        CostumePanel.SetActive(false);
    }

    public void ShowCostume()
    {
        CameraCanvas.SetActive(false);
        CameraCanvas.SetActive(false);
        CostumePanel.SetActive(true);
    }
}
