using System.Collections;
using System.Collections.Generic;
using AAMT;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoaderScene : MonoBehaviour
{
    public Button loaderButton;

    void Start()
    {
        loaderButton.onClick.AddListener(onLoadScene);
    }

    private void onLoadScene()
    {
        var path = "scenes/samplescene.unity";
        var handler = AssetsManager.LoadAssets(path);
        handler.onComplete = loaderHandler =>
        {
            SceneManager.LoadSceneAsync("SampleScene");
        };
    }

    // Update is called once per frame
    void Update()
    {
    }
}