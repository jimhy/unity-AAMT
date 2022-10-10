using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AAMT;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace GameLogic
{
    public class Main : MonoBehaviour
    {
        public Button loadButton;
        public Button releaseButton;
        public Transform roleLayer;
        public GameObject loadingImg;
        public Image image;
        public Slider slider;
        public RawImage rawImage;
        public Transform uiRoot;

        #region 配置

        private string[] pathList = new string[]
        {
            "res/roles/prefabs/lz_02_lv1_hd.prefab",
            "res/roles/prefabs/lz_02_lv1_low.prefab",
            "res/roles/prefabs/lz_02_lv2_hd.prefab",
            "res/roles/prefabs/lz_02_lv2_low.prefab",
            "res/roles/prefabs/lz_02_lv3_hd.prefab",
            "res/roles/prefabs/lz_02_lv3_low.prefab",
            "res/roles/prefabs/lz_03_lv1_hd.prefab",
            "res/roles/prefabs/lz_03_lv1_low.prefab",
            "res/roles/prefabs/lz_03_lv2_hd.prefab",
            "res/roles/prefabs/lz_03_lv2_low.prefab",
            "res/roles/prefabs/lz_03_lv3_hd.prefab",
            "res/roles/prefabs/lz_03_lv3_low.prefab",
            "res/roles/prefabs/lz_04_lv1_hd.prefab",
            "res/roles/prefabs/lz_04_lv1_low.prefab",
            "res/roles/prefabs/lz_04_lv2_hd.prefab",
            "res/roles/prefabs/lz_04_lv2_low.prefab",
            "res/roles/prefabs/lz_04_lv3_hd.prefab",
            "res/roles/prefabs/lz_04_lv3_low.prefab",
            "res/roles/prefabs/mh_01_lv1_hd.prefab",
            "res/roles/prefabs/mh_01_lv1_low.prefab",
            "res/roles/prefabs/mh_01_lv2_hd.prefab",
            "res/roles/prefabs/mh_01_lv2_low.prefab",
            "res/roles/prefabs/mh_01_lv3_hd.prefab",
            "res/roles/prefabs/mh_01_lv3_low.prefab",
            "res/roles/prefabs/mh_02_lv1_hd.prefab",
            "res/roles/prefabs/mh_02_lv1_low.prefab",
            "res/roles/prefabs/mh_02_lv2_hd.prefab",
            "res/roles/prefabs/mh_02_lv2_low.prefab",
            "res/roles/prefabs/mh_02_lv3_hd.prefab",
            "res/roles/prefabs/mh_02_lv3_low.prefab",
            "res/roles/prefabs/mh_03_lv1_hd.prefab",
            "res/roles/prefabs/mh_03_lv1_low.prefab",
            "res/roles/prefabs/mh_03_lv2_hd.prefab",
            "res/roles/prefabs/mh_03_lv2_low.prefab",
            "res/roles/prefabs/mh_03_lv3_hd.prefab",
            "res/roles/prefabs/mh_03_lv3_low.prefab",
            "res/roles/prefabs/mh_04_lv1_hd.prefab",
            "res/roles/prefabs/mh_04_lv1_low.prefab",
            "res/roles/prefabs/mh_04_lv2_hd.prefab",
            "res/roles/prefabs/mh_04_lv2_low.prefab",
            "res/roles/prefabs/mh_04_lv3_hd.prefab",
            "res/roles/prefabs/mh_04_lv3_low.prefab",
            "res/roles/prefabs/mh_05_lv1_hd.prefab",
            "res/roles/prefabs/mh_05_lv1_low.prefab",
        };

        #endregion

        private void Awake()
        {
            QualitySettings.vSyncCount = 1;
            Application.targetFrameRate = 30;
            QualitySettings.vSyncCount = 0;
        }


        void Start()
        {
            slider.value = 0;
            loadingImg.SetActive(false);
            if (loadButton != null) loadButton.onClick.AddListener(OnLoad1);
            if (releaseButton != null) releaseButton.onClick.AddListener(OnRelease);
            // AAMTManager.MoveBundles();
        }

        private void OnLoad()
        {
            loadingImg.SetActive(true);
            var handler = AAMTManager.LoadAssetsAsync(pathList);
            handler.onComplete = OnLoadComplete;
            handler.onProgress = loaderHandler =>
            {
                slider.value = loaderHandler.progress;
                Debug.Log($"progress:{loaderHandler.progress}");
            };
        }

        private int index = 0;

        private void OnRelease()
        {
            var paths = new string[] {"AAMT/Sample/res/Test/P3/P3.prefab", "AAMT/Sample/res/Test/P1/P1.prefab"};
            if (index >= paths.Length) return;
            var path = paths[0];
            AAMTManager.Release(path);
        }

        private void OnLoad1()
        {
            loadingImg.SetActive(true);
            string uiPath = "";
            // AAMTManager.GetAssets<GameObject>(pathList, obj =>
            // {
            //     loadingImg.SetActive(false);
            //     Debug.LogFormat("complete->{0}", obj.GetInstanceID());
            //     if (obj != null)
            //     {
            //         var go = Instantiate(obj, RandomPosition(), Quaternion.identity, roleLayer);
            //         Debug.LogFormat("instance id={0}", go.GetInstanceID());
            //     }
            // });
            // uiPath = "res/UI/UserInfo/sc_bg.png";
            //
            // AAMTManager.GetAssets<Texture2D>(uiPath,
            //     tx =>
            //     {
            //         loadingImg.SetActive(false);
            //         rawImage.texture = tx;
            //     });
            //
            // uiPath = "res/ui/userinfo/userinfopanel.png";
            // AAMTManager.GetAssets<AAMTSpriteAtlas>(uiPath, atlas =>
            // {
            //     loadingImg.SetActive(false);
            //
            //     foreach (var sp in atlas.GetSprites())
            //     {
            //         Debug.Log(sp.name);
            //     }
            // });
            // uiPath = "res/ui/userinfo/userinfopanel.png?dw_pic_08";
            // AAMTManager.GetAssets<Sprite>(uiPath, sp =>
            // {
            //     image.sprite = sp;
            // });
            uiPath = "AAMT/Sample/res/Test/P1/P1.prefab";
            AAMTManager.GetAssetsAsync<GameObject>(uiPath, obj =>
            {
                var go = Instantiate(obj, Vector3.zero, Quaternion.identity, uiRoot);
                go.SetActive(true);
                go.transform.localPosition = Vector3.zero;
            });
            uiPath = "AAMT/Sample/res/Test/P3/P3.prefab";
            AAMTManager.GetAssetsAsync<GameObject>(uiPath, obj =>
            {
                var p3 = Instantiate(obj, Vector3.zero, Quaternion.identity, uiRoot);
                p3.transform.localPosition = new Vector3(0, 200);
            });
        }

        private void OnLoadComplete(AsyncHandler obj)
        {
            Debug.LogFormat("Start to get assets:{0}", pathList);
            AAMTManager.GetAssetsAsync<GameObject>(pathList, go =>
            {
                Debug.LogFormat("GetAssets:{0}", go.name);
                if (go) Instantiate(go, RandomPosition(), Quaternion.identity, roleLayer);
            });

            loadingImg.SetActive(false);
        }

        private Vector3 RandomPosition()
        {
            var x = UnityEngine.Random.Range(-25f, 25f);
            var z = UnityEngine.Random.Range(-25f, 25f);
            return new Vector3(x, 0, z);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
        }

        private void OnGUI()
        {
            DrawFps.Instance.OnGUI();
        }
    }

    internal class LoaderHandler
    {
    }
}