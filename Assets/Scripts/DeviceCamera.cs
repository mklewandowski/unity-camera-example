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

    [SerializeField]
    RawImage background;
    [SerializeField]
    RawImage picInPic;
    [SerializeField]
    AspectRatioFitter fit;
    [SerializeField]
    TextMeshProUGUI DebugText;

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

        fit.aspectRatio = ratio;

        float scaleY = backCamera.videoVerticallyMirrored ? -1f : 1f;
        background.rectTransform.localScale = new Vector3(1f, scaleY, 1f);

        int orient = -backCamera.videoRotationAngle;
        background.rectTransform.localEulerAngles = new Vector3(0, 0, orient);

        DebugText.text = "w:" + backCamera.width + ", h:" + backCamera.height + ", r:" + ratio + ", s:" + scaleY + ", o:" + orient;
    }

    Texture2D pipTexture;
    public void CaptureImage()
    {
        if (!cameraAvailable)
            return;

        float scaleY = backCamera.videoVerticallyMirrored ? -1f : 1f;
        picInPic.rectTransform.localScale = new Vector3(1f, scaleY, 1f);

        int orient = -backCamera.videoRotationAngle;
        picInPic.rectTransform.localEulerAngles = new Vector3(0, 0, orient);

        Color[] c = backCamera.GetPixels((int)(backCamera.width / 2f - 100), (int)(backCamera.height / 2f - 100), 200, 200);
        pipTexture = new Texture2D(200, 200);
        pipTexture.SetPixels(c);
        pipTexture.Apply();
        picInPic.texture = pipTexture;
    }
}
