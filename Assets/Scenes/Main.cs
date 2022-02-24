using AAMT;
using UnityEngine;

namespace GameLogic
{
    public class Main : MonoBehaviour
    {
        public GameObject loadButton;
        private string path = "Roles/Prefabs/LZ_03_lv3_low.prefab";

        void Start()
        {
            if (loadButton != null) UIEventListener.Get(loadButton).onClick = onLoad;
        }

        private void onLoad(GameObject go)
        {
            AssetsManager.Instance.LoadAssets(new[] {path}, OnLoadComplete);
        }

        private void OnLoadComplete()
        {
            var go = AssetsManager.Instance.GetAssets<GameObject>(path);
            Instantiate(go);
        }


        // Update is called once per frame
        void Update()
        {
        }
    }
}