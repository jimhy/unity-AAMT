using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using AAMT;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LoadStreamAssets : MonoBehaviour
{
    public Button button;
    public TextMeshProUGUI label;

    void Start()
    {
        Application.logMessageReceived += OnMessage;
        button.onClick.AddListener(onClick);
    }

    private void OnMessage(string condition, string stacktrace, LogType type)
    {
        if (label != null)
        {
            label.text += $"{condition}\n";
        }
    }

    private void onClick()
    {
        var mgr = new BundleDowndloadManager();
        Debug.Log("mgr start1...");
        mgr.Start();
        Debug.Log("mgr start2...");
    }
    private void onClick1()
    {
        var path = $"{Application.persistentDataPath}/{AAMTDefine.AAMT_ASSET_VERSION}";
        try
        {
            if (File.Exists(path))
                Debug.LogFormat("found assets:{0}", path);
            else
                Debug.LogFormat("can not found assets:{0}", path);
        }
        catch (Exception e)
        {
            label.text += e;
            throw;
        }

        StartCoroutine(Load1(path));
    }

    private IEnumerator Load1(string path)
    {
        path = $"file:///{path}";
        Debug.LogFormat("StartCoroutine to load:{0}", path);
        var request = UnityWebRequest.Get(path);
        var h = request.SendWebRequest();
        yield return h;
        if (h.isDone)
        {
            Debug.LogFormat("data len={0}", request.downloadHandler.data.Length);
        }
        else
        {
            Debug.Log("StartCoroutine can not load...");
        }
    }

    void Update()
    {
    }
}