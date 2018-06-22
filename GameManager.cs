/// <summary>
/// 番場宥輝
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// ゲームの進行管理
/// </summary>
public class GameManager :SingletonMonoBehaviour<GameManager>
{

    //Runtime Initialization
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        //This Instance!
        GameObject gameManager = new GameObject();
        gameManager.AddComponent<GameManager>();
        gameManager.name = "GameManager";
        DontDestroyOnLoad(gameManager);

        //SceneManager Instance!
        if (SceneController.Instance == null)
        {
            GameObject sceneManager = new GameObject("SceneManager");
            sceneManager.AddComponent<SceneController>();
            //Debug.Log("<color=yellow>" + typeof(SceneController) + "</color> is Create!\nDon't worry! Error Log.");
        }
        DontDestroyOnLoad(SceneController.Instance);


        //FadeManager Instance!
        if (FadeManager.Instance == null)
        {
            GameObject fadeCanvas = new GameObject("FadeManager");
            fadeCanvas.gameObject.transform.SetParent(null);

            //コンポーネント追加
            fadeCanvas.AddComponent<FadeManager>();
            //キャンバスの追加
            Canvas canvas = fadeCanvas.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 999;

            fadeCanvas.AddComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            fadeCanvas.AddComponent<GraphicRaycaster>();

            //フェードオブジェクト
            GameObject imageObject = new GameObject("Image");
            imageObject.transform.SetParent(fadeCanvas.transform, false);
            imageObject.AddComponent<Image>().color = Color.black;
            imageObject.GetComponent<RectTransform>().sizeDelta = new Vector2(2000, 2000);

            //Message
            //Debug.Log("<color=yellow>" + typeof(FadeManager) + "</color> is Create!\nDon't worry! Error Log.");
        }
        DontDestroyOnLoad(FadeManager.Instance);

        //AudioManager Instance!
        if (AudioManager.Instance == null)
        {
            GameObject audioManager = new GameObject("AudioManager");
            audioManager.AddComponent<AudioManager>();
            //Debug.Log("<color=yellow>" + typeof(AudioManager) + "</color> is Create!\nDon't worry! Error Log.");
        }
        DontDestroyOnLoad(AudioManager.Instance);
    }

    //Awake
    private void  Awake()
    {
        Application.targetFrameRate = 60;
    }

    // Use this for initialization
    void Start () {
        AudioManager.Instance.Initialize();
        FadeManager.Instance.Initialize();
        SceneController.Instance.Initialize();
    }

    //Update
    private void Update()
    {
        SceneController.Instance.SceneUpdate();
    }

    //This finalize function
    private static void Release()
    {

    }

}