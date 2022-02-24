using AAMT;
using UnityEngine;

namespace GameLogic
{
    public class Main : MonoBehaviour
    {
        public GameObject loadButton;
        public GameObject loadButtonBatch;
        public UILabel label;

        private string[] pathList = new string[]
        {
            "Roles/Prefabs/lz_02_lv1_hd.prefab",
            "Roles/Prefabs/lz_02_lv1_low.prefab",
            "Roles/Prefabs/lz_02_lv2_hd.prefab",
            "Roles/Prefabs/lz_02_lv2_low.prefab",
            "Roles/Prefabs/lz_02_lv3_hd.prefab",
            "Roles/Prefabs/lz_02_lv3_low.prefab",
            "Roles/Prefabs/lz_03_lv1_hd.prefab",
            "Roles/Prefabs/lz_03_lv1_low.prefab",
            "Roles/Prefabs/lz_03_lv2_hd.prefab",
            "Roles/Prefabs/lz_03_lv2_low.prefab",
            "Roles/Prefabs/lz_03_lv3_hd.prefab",
            "Roles/Prefabs/lz_03_lv3_low.prefab",
            "Roles/Prefabs/lz_04_lv1_hd.prefab",
            "Roles/Prefabs/lz_04_lv1_low.prefab",
            "Roles/Prefabs/lz_04_lv2_hd.prefab",
            "Roles/Prefabs/lz_04_lv2_low.prefab",
            "Roles/Prefabs/lz_04_lv3_hd.prefab",
            "Roles/Prefabs/lz_04_lv3_low.prefab"
        };

        void Start()
        {
            if (loadButton != null) UIEventListener.Get(loadButton).onClick = OnLoad;
            if (loadButtonBatch != null) UIEventListener.Get(loadButtonBatch).onClick = OnLoadBatch;
        }

        private float _lastTime;

        private void OnLoad(GameObject go)
        {
            _lastTime = Time.time;
            AssetsManager.Instance.LoadAssets(pathList, OnLoadComplete);
        }

        private void OnLoadBatch(GameObject go)
        {
            _lastTime = Time.time;
            AssetsManager.Instance.LoadAssetsBatch(pathList, OnLoadComplete);
        }

        private void OnLoadComplete()
        {
            var time = Time.time - _lastTime;
            var str = $"used time:{time} s";
            label.text = str;
            var go = AssetsManager.Instance.GetAssets<GameObject>(pathList[0]);
            Instantiate(go);
        }


        // Update is called once per frame
        void Update()
        {
        }
    }
}