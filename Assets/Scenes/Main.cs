using System.Collections;
using System.Collections.Generic;
using JAssetsManager;
using UnityEngine;

namespace GameLogic
{
    public class Main : MonoBehaviour
    {
        public GameObject loadButton;
        private string path = "roles/prefabs";

        void Start()
        {
            if (loadButton != null) UIEventListener.Get(loadButton).onClick = onLoad;
        }

        private void onLoad(GameObject go)
        {
            AssetsManager.Instance.LoadAssets(path, OnLoadComplete);
        }

        private void OnLoadComplete()
        {
            var go = AssetsManager.Instance.GetAssets<GameObject>(path,"LZ_02_lv1_hd.prefab");
            Instantiate(go);
        }


        // Update is called once per frame
        void Update()
        {
        }
    }
}