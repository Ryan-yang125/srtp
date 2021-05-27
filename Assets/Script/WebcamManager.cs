using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;
using UnityEngine;

public class WebcamManager : MonoBehaviour
{
    private WebCamTexture webCamTexture;
    private bool isPlay = false;
    private Color32[] frameData;
    public GameObject player;
    public GameObject[] emotionsUI;
    public GameObject webTexturePoint0;
    public GameObject webTexturePoint1;
    public Camera camera;
    public float x0, y0, x1, y1;

    // Start is called before the first frame update
    void Start()
    {

#if UNITY_STANDALONE_OSX
    Debug.Log("Stand Alone OSX");
        for (int i = 0; i < WebCamTexture.devices.Length; i++)
        {
            if (WebCamTexture.devices[i].isFrontFacing)
            {

                Debug.Log(WebCamTexture.devices[i].name);
                webCamTexture = new WebCamTexture(WebCamTexture.devices[i].name, 600, 360, 30);
                webCamTexture.Play();
                isPlay = true;
                break;
            }
        }
#endif

#if UNITY_STANDALONE_WIN
        Debug.Log("Stand Alone Windows");
        for (int i = 1; i < WebCamTexture.devices.Length; i++)
        {
            if (WebCamTexture.devices[i].isFrontFacing)
            {

                Debug.Log(WebCamTexture.devices[i].name);
                webCamTexture = new WebCamTexture(WebCamTexture.devices[i].name, 600, 360, 30);
                webCamTexture.Play();
                isPlay = true;
                break;
            }
        }
#endif

        Debug.Log(webTexturePoint0.transform.position.x);
        Debug.Log(webTexturePoint0.transform.position.y);
        Debug.Log(webTexturePoint1.transform.position.x);
        Debug.Log(webTexturePoint1.transform.position.y);
        // Get Texture Display Position
        x0 = webTexturePoint0.transform.position.x;
        y0 = Screen.height - webTexturePoint0.transform.position.y;
        x1 = webTexturePoint1.transform.position.x - x0;
        y1 = Screen.height - webTexturePoint1.transform.position.y - y0;
    }

    // Update is called once per frame
    void Update()
    {
        frameData = new Color32[webCamTexture.height * webCamTexture.width];
        webCamTexture.GetPixels32(frameData);
        Debug.Log(frameData.Length);

        //JSON
        //Frame frame = new Frame();
        //for (int i = 0; i < frameData.Length; i++)
        //{
        //   frame.pixels.Add(frameData[i]);
        //}
        //string json = JsonUtility.ToJson(frame);

        //PNG
        Texture2D t = new Texture2D(webCamTexture.width, webCamTexture.height);
        t.SetPixels32(frameData);
        t.Apply();

        byte[] img = t.EncodeToPNG();
        string filePath = Application.dataPath + "/test.png";
        Debug.Log(filePath);
        File.WriteAllBytes(filePath, img);

        // Reading PYTHON Json
        if (File.Exists(Application.dataPath + "/test.json"))
        {

            string jsonStr = File.ReadAllText(Application.dataPath + "/test.json");
            List<Emotion> jsonEmotionDatas = new List<Emotion>();
            if (jsonStr != null && jsonStr != "") jsonEmotionDatas = JsonUtility.FromJson<EmotionJson>(jsonStr).datas;

            // Draw UI Graph
            for (int i = 0; i < jsonEmotionDatas.Count; i++)
            {
                GameObject emotionSlider = emotionsUI[i];
                // Set Emotion Name
                Text text = emotionSlider.GetComponentInChildren<Text>();
                text.text = jsonEmotionDatas[i].name;

                // Set Emotion Ratio value
                Slider slider = emotionSlider.GetComponentInChildren<Slider>();
                slider.value = jsonEmotionDatas[i].value;
                //Debug.Log(text.text + ":" + slider.value.ToString());

                PlayerController playerController = player.GetComponent<PlayerController>();

                if (text.text == "happy" && slider.value > 0.5f)
                {
                    // playerController.Movement(slider.value);
                }
                //Debug.Log(slider.value);
                if (text.text == "angry" && slider.value > 0.2f)
                {
                    // playerController.Movement(1.0f);
                    // playerController.Jump();
                }
            }
        }
        //PlayerController playerController = player.GetComponent<PlayerController>();
        // TODO playerController
    }

    void OnGUI()
    {
        if (isPlay)
        {
            GUI.DrawTexture(new Rect(x0, y0, x1, y1), webCamTexture, ScaleMode.ScaleToFit);
        }
    }
}
