using System.Collections;
using System.Collections.Generic;
using AAMT;
using UnityEngine;
using UnityEngine.UI;

public class LoadAssetsFromRemote : MonoBehaviour
{
    public Button MoveButton;
    public Button UpdateButton;
    public Button DeleteCacheButton;

    void Start()
    {
        MoveButton.onClick.AddListener(StartMoveAssets);
        UpdateButton.onClick.AddListener(StartUpdateAssets);
        DeleteCacheButton.onClick.AddListener(DeleteCache);
    }

    private void DeleteCache()
    {
        AAMTDefine.SetMoveFilesToPersistentComplete(false);
    }

    private void StartMoveAssets()
    {
        var handler = AAMTManager.MoveBundles();
        handler.onComplete = OnMoveComplete;
        handler.onProgress = OnMoveAssetsProgress;
    }

    private void OnMoveAssetsProgress(AsyncHandler handler)
    {
        Debug.Log($"OnMoveAssetsProgress-->{handler.progress}");
    }

    private void OnMoveComplete(AsyncHandler obj)
    {
        Debug.Log("OnMoveComplete");
    }

    private void StartUpdateAssets()
    {
        var handler = AAMTManager.UpdateAssets();
        handler.onComplete = OnDownLoadComplete;
        handler.onProgress = OnUpdateAssetsProgress;
    }

    private void OnDownLoadComplete(AsyncHandler obj)
    {
        Debug.Log("OnDownLoadComplete");
    }

    private void OnUpdateAssetsProgress(AsyncHandler handler)
    {
        Debug.Log($"OnUpdateAssetsProgress-->{handler.progress}");
    }
}