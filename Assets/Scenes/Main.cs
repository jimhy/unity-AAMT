using System.Threading;
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
            "roles/prefabs/lz_02_lv1_hd.prefab",
            "roles/prefabs/lz_02_lv1_low.prefab",
            "roles/prefabs/lz_02_lv2_hd.prefab",
            "roles/prefabs/lz_02_lv2_low.prefab",
            "roles/prefabs/lz_02_lv3_hd.prefab",
            "roles/prefabs/lz_02_lv3_low.prefab",
            "roles/prefabs/lz_03_lv1_hd.prefab",
            "roles/prefabs/lz_03_lv1_low.prefab",
            "roles/prefabs/lz_03_lv2_hd.prefab",
            "roles/prefabs/lz_03_lv2_low.prefab",
            "roles/prefabs/lz_03_lv3_hd.prefab",
            "roles/prefabs/lz_03_lv3_low.prefab",
            "roles/prefabs/lz_04_lv1_hd.prefab",
            "roles/prefabs/lz_04_lv1_low.prefab",
            "roles/prefabs/lz_04_lv2_hd.prefab",
            "roles/prefabs/lz_04_lv2_low.prefab",
            "roles/prefabs/lz_04_lv3_hd.prefab",
            "roles/prefabs/lz_04_lv3_low.prefab",
            "roles/prefabs/mh_01_lv1_hd.prefab",
            "roles/prefabs/mh_01_lv1_low.prefab",
            "roles/prefabs/mh_01_lv2_hd.prefab",
            "roles/prefabs/mh_01_lv2_low.prefab",
            "roles/prefabs/mh_01_lv3_hd.prefab",
            "roles/prefabs/mh_01_lv3_low.prefab",
            "roles/prefabs/mh_02_lv1_hd.prefab",
            "roles/prefabs/mh_02_lv1_low.prefab",
            "roles/prefabs/mh_02_lv2_hd.prefab",
            "roles/prefabs/mh_02_lv2_low.prefab",
            "roles/prefabs/mh_02_lv3_hd.prefab",
            "roles/prefabs/mh_02_lv3_low.prefab",
            "roles/prefabs/mh_03_lv1_hd.prefab",
            "roles/prefabs/mh_03_lv1_low.prefab",
            "roles/prefabs/mh_03_lv2_hd.prefab",
            "roles/prefabs/mh_03_lv2_low.prefab",
            "roles/prefabs/mh_03_lv3_hd.prefab",
            "roles/prefabs/mh_03_lv3_low.prefab",
            "roles/prefabs/mh_04_lv1_hd.prefab",
            "roles/prefabs/mh_04_lv1_low.prefab",
            "roles/prefabs/mh_04_lv2_hd.prefab",
            "roles/prefabs/mh_04_lv2_low.prefab",
            "roles/prefabs/mh_04_lv3_hd.prefab",
            "roles/prefabs/mh_04_lv3_low.prefab",
            "roles/prefabs/mh_05_lv1_hd.prefab",
            "roles/prefabs/mh_05_lv1_low.prefab",
            // "roles/prefabs/mh_05_lv2_hd.prefab",
            // "roles/prefabs/mh_05_lv2_low.prefab",
            // "roles/prefabs/mh_05_lv3_hd.prefab",
            // "roles/prefabs/mh_05_lv3_low.prefab",
            // "roles/prefabs/mh_06_lv1_hd.prefab",
            // "roles/prefabs/mh_06_lv1_low.prefab",
            // "roles/prefabs/mh_06_lv2_hd.prefab",
            // "roles/prefabs/mh_06_lv2_low.prefab",
            // "roles/prefabs/mh_06_lv3_hd.prefab",
            // "roles/prefabs/mh_06_lv3_low.prefab",
            // "roles/prefabs/mh_07_lv1_hd.prefab",
            // "roles/prefabs/mh_07_lv1_low.prefab",
            // "roles/prefabs/mh_07_lv2_hd.prefab",
            // "roles/prefabs/mh_07_lv2_low.prefab",
            // "roles/prefabs/mh_07_lv3_hd.prefab",
            // "roles/prefabs/mh_07_lv3_low.prefab",
            // "roles/prefabs/mz_01_lv1_hd.prefab",
            // "roles/prefabs/mz_01_lv1_low.prefab",
            // "roles/prefabs/mz_01_lv2_hd.prefab",
            // "roles/prefabs/mz_01_lv2_low.prefab",
            // "roles/prefabs/mz_01_lv3_hd.prefab",
            // "roles/prefabs/mz_01_lv3_low.prefab",
            // "roles/prefabs/mz_02_lv1_hd.prefab",
            // "roles/prefabs/mz_02_lv1_low.prefab",
            // "roles/prefabs/mz_02_lv2_hd.prefab",
            // "roles/prefabs/mz_02_lv2_low.prefab",
            // "roles/prefabs/mz_02_lv3_hd.prefab",
            // "roles/prefabs/mz_02_lv3_low.prefab",
            // "roles/prefabs/mz_03_bs_lv1_low.prefab",
            // "roles/prefabs/mz_03_bs_lv2_low.prefab",
            // "roles/prefabs/mz_03_bs_lv3_low.prefab",
            // "roles/prefabs/mz_03_lv1_hd.prefab",
            // "roles/prefabs/mz_03_lv1_low.prefab",
            // "roles/prefabs/mz_03_lv2_hd.prefab",
            // "roles/prefabs/mz_03_lv2_low.prefab",
            // "roles/prefabs/mz_03_lv3_hd.prefab",
            // "roles/prefabs/mz_03_lv3_low.prefab",
            // "roles/prefabs/mz_04_lv1_hd.prefab",
            // "roles/prefabs/mz_04_lv1_low.prefab",
            // "roles/prefabs/mz_04_lv2_hd.prefab",
            // "roles/prefabs/mz_04_lv2_low.prefab",
            // "roles/prefabs/mz_04_lv3_hd.prefab",
            // "roles/prefabs/mz_04_lv3_low.prefab",
            // "roles/prefabs/mz_05_lv1_hd.prefab",
            // "roles/prefabs/mz_05_lv1_low.prefab",
            // "roles/prefabs/mz_05_lv2_hd.prefab",
            // "roles/prefabs/mz_05_lv2_low.prefab",
            // "roles/prefabs/mz_05_lv3_hd.prefab",
            // "roles/prefabs/mz_05_lv3_low.prefab",
            // "roles/prefabs/npc_shangren.prefab",
            // "roles/prefabs/npc_shangren_che.prefab",
            // "roles/prefabs/npc_xiaozhu.prefab",
            // "roles/prefabs/qs_jnn_hd.prefab",
            // "roles/prefabs/qs_jnn_low.prefab",
            // "roles/prefabs/qs_jntn_hd.prefab",
            // "roles/prefabs/qs_jntn_low.prefab",
            // "roles/prefabs/qs_ss_hd.prefab",
            // "roles/prefabs/qs_ss_low.prefab",
            // "roles/prefabs/qs_xsm_hd.prefab",
            // "roles/prefabs/qs_xsm_low.prefab",
            // "roles/prefabs/rl_01_lv1_hd.prefab",
            // "roles/prefabs/rl_01_lv1_low.prefab",
            // "roles/prefabs/rl_01_lv2_hd.prefab",
            // "roles/prefabs/rl_01_lv2_low.prefab",
            // "roles/prefabs/rl_01_lv3_hd.prefab",
            // "roles/prefabs/rl_01_lv3_low.prefab",
            // "roles/prefabs/rl_02_lv1_hd.prefab",
            // "roles/prefabs/rl_02_lv1_low.prefab",
            // "roles/prefabs/rl_02_lv2_hd.prefab",
            // "roles/prefabs/rl_02_lv2_low.prefab",
            // "roles/prefabs/rl_02_lv3_hd.prefab",
            // "roles/prefabs/rl_02_lv3_low.prefab",
            // "roles/prefabs/rl_03_lv1_hd.prefab",
            // "roles/prefabs/rl_03_lv1_low.prefab",
            // "roles/prefabs/rl_03_lv2_hd.prefab",
            // "roles/prefabs/rl_03_lv2_low.prefab",
            // "roles/prefabs/rl_03_lv3_hd.prefab",
            // "roles/prefabs/rl_03_lv3_low.prefab",
            // "roles/prefabs/rl_04_lv1_hd.prefab",
            // "roles/prefabs/rl_04_lv1_low.prefab",
            // "roles/prefabs/rl_04_lv2_hd.prefab",
            // "roles/prefabs/rl_04_lv2_low.prefab",
            // "roles/prefabs/rl_04_lv3_hd.prefab",
            // "roles/prefabs/rl_04_lv3_low.prefab",
            // "roles/prefabs/rl_05_bs_lv1_low.prefab",
            // "roles/prefabs/rl_05_bs_lv2_low.prefab",
            // "roles/prefabs/rl_05_bs_lv3_low.prefab",
            // "roles/prefabs/rl_05_lv1_hd.prefab",
            // "roles/prefabs/rl_05_lv1_low.prefab",
            // "roles/prefabs/rl_05_lv1_zhw.prefab",
            // "roles/prefabs/rl_05_lv2_hd.prefab",
            // "roles/prefabs/rl_05_lv2_low.prefab",
            // "roles/prefabs/rl_05_lv2_zhw.prefab",
            // "roles/prefabs/rl_05_lv3_hd.prefab",
            // "roles/prefabs/rl_05_lv3_low.prefab",
            // "roles/prefabs/rl_05_lv3_zhw.prefab",
            // "roles/prefabs/rl_06_lv1_hd.prefab",
            // "roles/prefabs/rl_06_lv1_low.prefab",
            // "roles/prefabs/rl_06_lv2_hd.prefab",
            // "roles/prefabs/rl_06_lv2_low.prefab",
            // "roles/prefabs/rl_06_lv3_hd.prefab",
            // "roles/prefabs/rl_06_lv3_low.prefab",
            // "roles/prefabs/rsww_01_lv1_hd.prefab",
            // "roles/prefabs/rsww_01_lv1_low.prefab",
            // "roles/prefabs/rsww_01_lv2_hd.prefab",
            // "roles/prefabs/rsww_01_lv2_low.prefab",
            // "roles/prefabs/rsww_01_lv3_hd.prefab",
            // "roles/prefabs/rsww_01_lv3_low.prefab",
            // "roles/prefabs/sx_01_lv1_hd.prefab",
            // "roles/prefabs/sx_01_lv1_low.prefab",
            // "roles/prefabs/sx_01_lv2_hd.prefab",
            // "roles/prefabs/sx_01_lv2_low.prefab",
            // "roles/prefabs/sx_01_lv3_hd.prefab",
            // "roles/prefabs/sx_01_lv3_low.prefab",
            // "roles/prefabs/sx_02_lv1_hd.prefab",
            // "roles/prefabs/sx_02_lv1_low.prefab",
            // "roles/prefabs/sx_02_lv2_hd.prefab",
            // "roles/prefabs/sx_02_lv2_low.prefab",
            // "roles/prefabs/sx_02_lv3_hd.prefab",
            // "roles/prefabs/sx_02_lv3_low.prefab",
            // "roles/prefabs/sx_03_lv1_hd.prefab",
            // "roles/prefabs/sx_03_lv1_low.prefab",
            // "roles/prefabs/sx_03_lv2_hd.prefab",
            // "roles/prefabs/sx_03_lv2_low.prefab",
            // "roles/prefabs/sx_03_lv3_hd.prefab",
            // "roles/prefabs/sx_03_lv3_low.prefab",
            // "roles/prefabs/sx_04_lv1_hd.prefab",
            // "roles/prefabs/sx_04_lv1_low.prefab",
            // "roles/prefabs/sx_04_lv2_hd.prefab",
            // "roles/prefabs/sx_04_lv2_low.prefab",
            // "roles/prefabs/sx_04_lv3_hd.prefab",
            // "roles/prefabs/sx_04_lv3_low.prefab",
            // "roles/prefabs/sz_01_lv1_hd.prefab",
            // "roles/prefabs/sz_01_lv1_low.prefab",
            // "roles/prefabs/sz_01_lv1_zhw.prefab",
            // "roles/prefabs/sz_01_lv2_hd.prefab",
            // "roles/prefabs/sz_01_lv2_low.prefab",
            // "roles/prefabs/sz_01_lv2_zhw.prefab",
            // "roles/prefabs/sz_01_lv3_hd.prefab",
            // "roles/prefabs/sz_01_lv3_low.prefab",
            // "roles/prefabs/sz_01_lv3_zhw.prefab",
            // "roles/prefabs/sz_02_lv1_hd.prefab",
            // "roles/prefabs/sz_02_lv1_low.prefab",
            // "roles/prefabs/sz_02_lv2_hd.prefab",
            // "roles/prefabs/sz_02_lv2_low.prefab",
            // "roles/prefabs/sz_02_lv3_hd.prefab",
            // "roles/prefabs/sz_02_lv3_low.prefab",
            // "roles/prefabs/sz_03_lv1_hd.prefab",
            // "roles/prefabs/sz_03_lv1_low.prefab",
            // "roles/prefabs/sz_03_lv2_hd.prefab",
            // "roles/prefabs/sz_03_lv2_low.prefab",
            // "roles/prefabs/sz_03_lv3_hd.prefab",
            // "roles/prefabs/sz_03_lv3_low.prefab",
            // "roles/prefabs/sz_04_lv1_hd.prefab",
            // "roles/prefabs/sz_04_lv1_low.prefab",
            // "roles/prefabs/sz_04_lv1_zhw.prefab",
            // "roles/prefabs/sz_04_lv2_hd.prefab",
            // "roles/prefabs/sz_04_lv2_low.prefab",
            // "roles/prefabs/sz_04_lv2_zhw.prefab",
            // "roles/prefabs/sz_04_lv3_hd.prefab",
            // "roles/prefabs/sz_04_lv3_low.prefab",
            // "roles/prefabs/sz_04_lv3_zhw.prefab",
            // "roles/prefabs/sz_05_lv1_hd.prefab",
            // "roles/prefabs/sz_05_lv1_low.prefab",
            // "roles/prefabs/sz_05_lv2_hd.prefab",
            // "roles/prefabs/sz_05_lv2_low.prefab",
            // "roles/prefabs/sz_05_lv3_hd.prefab",
            // "roles/prefabs/sz_05_lv3_low.prefab",
            // "roles/prefabs/sz_05_lv3_zhw.prefab",
            // "roles/prefabs/sz_06_lv1_hd.prefab",
            // "roles/prefabs/sz_06_lv1_low.prefab",
            // "roles/prefabs/sz_06_lv2_hd.prefab",
            // "roles/prefabs/sz_06_lv2_low.prefab",
            // "roles/prefabs/sz_06_lv3_hd.prefab",
            // "roles/prefabs/sz_06_lv3_low.prefab",
            // "roles/prefabs/sz_07_lv1_hd.prefab",
            // "roles/prefabs/sz_07_lv1_low.prefab",
            // "roles/prefabs/sz_07_lv2_hd.prefab",
            // "roles/prefabs/sz_07_lv2_low.prefab",
            // "roles/prefabs/sz_07_lv3_hd.prefab",
            // "roles/prefabs/sz_07_lv3_low.prefab",
            // "roles/prefabs/wy_01_lv1_hd.prefab",
            // "roles/prefabs/wy_01_lv1_low.prefab",
            // "roles/prefabs/wy_01_lv2_hd.prefab",
            // "roles/prefabs/wy_01_lv2_low.prefab",
            // "roles/prefabs/wy_01_lv3_hd.prefab",
            // "roles/prefabs/wy_01_lv3_low.prefab",
            // "roles/prefabs/wy_02_lv1_hd.prefab",
            // "roles/prefabs/wy_02_lv1_low.prefab",
            // "roles/prefabs/wy_02_lv2_hd.prefab",
            // "roles/prefabs/wy_02_lv2_low.prefab",
            // "roles/prefabs/wy_02_lv3_hd.prefab",
            // "roles/prefabs/wy_02_lv3_low.prefab",
            // "roles/prefabs/wy_03_lv1_hd.prefab",
            // "roles/prefabs/wy_03_lv1_low.prefab",
            // "roles/prefabs/wy_03_lv2_hd.prefab",
            // "roles/prefabs/wy_03_lv2_low.prefab",
            // "roles/prefabs/wy_03_lv3_hd.prefab",
            // "roles/prefabs/wy_03_lv3_low.prefab",
            // "roles/prefabs/xz_01_lv1_hd.prefab",
            // "roles/prefabs/xz_01_lv1_low.prefab",
            // "roles/prefabs/xz_01_lv2_hd.prefab",
            // "roles/prefabs/xz_01_lv2_low.prefab",
            // "roles/prefabs/xz_01_lv3_hd.prefab",
            // "roles/prefabs/xz_01_lv3_low.prefab",
            // "roles/prefabs/xz_02_bs_lv1_hd.prefab",
            // "roles/prefabs/xz_02_bs_lv1_low.prefab",
            // "roles/prefabs/xz_02_bs_lv2_hd.prefab",
            // "roles/prefabs/xz_02_bs_lv2_low.prefab",
            // "roles/prefabs/xz_02_bs_lv3_hd.prefab",
            // "roles/prefabs/xz_02_bs_lv3_low.prefab",
            // "roles/prefabs/xz_02_lv1_hd.prefab",
            // "roles/prefabs/xz_02_lv1_low.prefab",
            // "roles/prefabs/xz_02_lv2_hd.prefab",
            // "roles/prefabs/xz_02_lv2_low.prefab",
            // "roles/prefabs/xz_02_lv3_hd.prefab",
            // "roles/prefabs/xz_02_lv3_low.prefab",
            // "roles/prefabs/xz_03_lv1_hd.prefab",
            // "roles/prefabs/xz_03_lv1_low.prefab",
            // "roles/prefabs/xz_03_lv2_hd.prefab",
            // "roles/prefabs/xz_03_lv2_low.prefab",
            // "roles/prefabs/xz_03_lv3_hd.prefab",
            // "roles/prefabs/xz_03_lv3_low.prefab",
            // "roles/prefabs/xz_04_lv1_hd.prefab",
            // "roles/prefabs/xz_04_lv1_low.prefab",
            // "roles/prefabs/xz_04_lv2_hd.prefab",
            // "roles/prefabs/xz_04_lv2_low.prefab",
            // "roles/prefabs/xz_04_lv3_hd.prefab",
            // "roles/prefabs/xz_04_lv3_low.prefab",
            // "roles/prefabs/xz_05_lv1_hd.prefab",
            // "roles/prefabs/xz_05_lv1_low.prefab",
            // "roles/prefabs/xz_05_lv2_hd.prefab",
            // "roles/prefabs/xz_05_lv2_low.prefab",
            // "roles/prefabs/xz_05_lv3_hd.prefab",
            // "roles/prefabs/xz_05_lv3_low.prefab",
            // "roles/prefabs/xz_06_lv1_hd.prefab",
            // "roles/prefabs/xz_06_lv1_low.prefab",
            // "roles/prefabs/xz_06_lv2_hd.prefab",
            // "roles/prefabs/xz_06_lv2_low.prefab",
            // "roles/prefabs/xz_06_lv3_hd.prefab",
            // "roles/prefabs/xz_06_lv3_low.prefab",
            // "roles/prefabs/ym_01_lv1_hd.prefab",
            // "roles/prefabs/ym_01_lv1_low.prefab",
            // "roles/prefabs/ym_01_lv2_hd.prefab",
            // "roles/prefabs/ym_01_lv2_low.prefab",
            // "roles/prefabs/ym_01_lv3_hd.prefab",
            // "roles/prefabs/ym_01_lv3_low.prefab",
            // "roles/prefabs/ym_02_lv1_hd.prefab",
            // "roles/prefabs/ym_02_lv1_low.prefab",
            // "roles/prefabs/ym_02_lv2_hd.prefab",
            // "roles/prefabs/ym_02_lv2_low.prefab",
            // "roles/prefabs/ym_02_lv3_hd.prefab",
            // "roles/prefabs/ym_02_lv3_low.prefab",
            // "roles/prefabs/ym_03_lv1_hd.prefab",
            // "roles/prefabs/ym_03_lv1_low.prefab",
            // "roles/prefabs/ym_03_lv2_hd.prefab",
            // "roles/prefabs/ym_03_lv2_low.prefab",
            // "roles/prefabs/ym_03_lv3_hd.prefab",
            // "roles/prefabs/ym_03_lv3_low.prefab",
            // "roles/prefabs/ym_04_lv1_hd.prefab",
            // "roles/prefabs/ym_04_lv1_low.prefab",
            // "roles/prefabs/ym_04_lv2_hd.prefab",
            // "roles/prefabs/ym_04_lv2_low.prefab",
            // "roles/prefabs/ym_04_lv3_hd.prefab",
            // "roles/prefabs/ym_04_lv3_low.prefab",
            // "roles/prefabs/yz_01_lv1_hd.prefab",
            // "roles/prefabs/yz_01_lv1_low.prefab",
            // "roles/prefabs/yz_01_lv2_hd.prefab",
            // "roles/prefabs/yz_01_lv2_low.prefab",
            // "roles/prefabs/yz_01_lv3_hd.prefab",
            // "roles/prefabs/yz_01_lv3_low.prefab",
            // "roles/prefabs/yz_02_lv1_hd.prefab",
            // "roles/prefabs/yz_02_lv1_low.prefab",
            // "roles/prefabs/yz_02_lv2_hd.prefab",
            // "roles/prefabs/yz_02_lv2_low.prefab",
            // "roles/prefabs/yz_02_lv3_hd.prefab",
            // "roles/prefabs/yz_02_lv3_low.prefab",
            // "roles/prefabs/yz_03_lv1_hd.prefab",
            // "roles/prefabs/yz_03_lv1_low.prefab",
            // "roles/prefabs/yz_03_lv2_hd.prefab",
            // "roles/prefabs/yz_03_lv2_low.prefab",
            // "roles/prefabs/yz_03_lv3_hd.prefab",
            // "roles/prefabs/yz_03_lv3_low.prefab",
            // "roles/prefabs/yz_04_lv1_hd.prefab",
            // "roles/prefabs/yz_04_lv1_low.prefab",
            // "roles/prefabs/yz_04_lv1_zhw.prefab",
            // "roles/prefabs/yz_04_lv2_hd.prefab",
            // "roles/prefabs/yz_04_lv2_low.prefab",
            // "roles/prefabs/yz_04_lv2_zhw.prefab",
            // "roles/prefabs/yz_04_lv3_hd.prefab",
            // "roles/prefabs/yz_04_lv3_low.prefab",
            // "roles/prefabs/yz_04_lv3_zhw.prefab",
            // "roles/prefabs/yz_05_lv1_hd.prefab",
            // "roles/prefabs/yz_05_lv1_low.prefab",
            // "roles/prefabs/yz_05_lv2_hd.prefab",
            // "roles/prefabs/yz_05_lv2_low.prefab",
            // "roles/prefabs/yz_05_lv3_hd.prefab",
            // "roles/prefabs/yz_05_lv3_low.prefab",
            // "roles/prefabs/yz_06_lv1_hd.prefab",
            // "roles/prefabs/yz_06_lv1_low.prefab",
            // "roles/prefabs/yz_06_lv2_hd.prefab",
            // "roles/prefabs/yz_06_lv2_low.prefab",
            // "roles/prefabs/yz_06_lv3_hd.prefab",
            // "roles/prefabs/yz_06_lv3_low.prefab",
            // "roles/prefabs/yz_07_lv1_hd.prefab",
            // "roles/prefabs/yz_07_lv1_low.prefab",
            // "roles/prefabs/yz_07_lv2_hd.prefab",
            // "roles/prefabs/yz_07_lv2_low.prefab",
            // "roles/prefabs/yz_07_lv3_hd.prefab",
            // "roles/prefabs/yz_07_lv3_low.prefab",
            // "roles/prefabs/yz_08_lv1_hd.prefab",
            // "roles/prefabs/yz_08_lv1_low.prefab",
            // "roles/prefabs/yz_08_lv2_hd.prefab",
            // "roles/prefabs/yz_08_lv2_low.prefab",
            // "roles/prefabs/yz_08_lv3_hd.prefab",
            // "roles/prefabs/yz_08_lv3_low.prefab"
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
        }

        private void OnLoad(GameObject go)
        {
            loadingImg.SetActive(true);
            var handler = AssetsManager.LoadAssets(pathList);
            handler.onComplete = OnLoadComplete;
            handler.onProgress = loaderHandler => { slider.value = loaderHandler.progress; };
        }

        private int index = 0;

        private void OnRelease()
        {
            var paths = new string[] {"Test/P3/P3.prefab", "Test/P1/P1.prefab"};
            if (index >= paths.Length) return;
            var path = paths[index++];
            AssetsManager.Release(path);
        }

        private void OnLoad1()
        {
            loadingImg.SetActive(true);
            string uiPath = "";
            // AssetsManager.GetAssets<GameObject>(pathList, obj =>
            // {
            //     loadingImg.SetActive(false);
            //     Debug.LogFormat("complete->{0}", obj.GetInstanceID());
            //     if (obj != null)
            //     {
            //         var go = Instantiate(obj, RandomPosition(), Quaternion.identity, roleLayer);
            //         Debug.LogFormat("instance id={0}", go.GetInstanceID());
            //     }
            // });
            uiPath = "UI/UserInfo/sc_bg.png";

            AssetsManager.GetAssets<Texture2D>(uiPath,
                tx =>
                {
                    loadingImg.SetActive(false);
                    rawImage.texture = tx;
                });

            uiPath = "ui/userinfo/userinfopanel.png";
            AssetsManager.GetAssets<AAMTSpriteAtlas>(uiPath, atlas =>
            {
                loadingImg.SetActive(false);

                foreach (var sp in atlas.GetSprites())
                {
                    Debug.Log(sp.name);
                }
            });
            uiPath = "ui/userinfo/userinfopanel.png?dw_pic_08";
            AssetsManager.GetAssets<Sprite>(uiPath, sp => { image.sprite = sp; });
            // uiPath = "Test/P1/P1.prefab";
            // AssetsManager.GetAssets<GameObject>(uiPath, obj =>
            // {
            //     if (obj != null)
            //     {
            //         var go = Instantiate(obj, Vector3.zero, Quaternion.identity, uiRoot);
            //         go.SetActive(true);
            //     }
            // });
            // uiPath = "Test/P3/P3.prefab";
            // AssetsManager.GetAssets<GameObject>(uiPath, obj =>
            // {
            //     if (obj != null)
            //     {
            //         var p3 = Instantiate(obj, Vector3.zero, Quaternion.identity, uiRoot);
            //         p3.transform.localPosition = new Vector3(0, 200);
            //     }
            // });
        }

        private void OnLoadComplete(LoaderHandler handler)
        {
            foreach (var path in pathList)
            {
                Debug.LogFormat("Start to get assets:{0}", path);
                AssetsManager.GetAssets<GameObject>(path, go =>
                {
                    Debug.LogFormat("GetAssets:{0}", go.name);
                    if (go != null) Instantiate(go, RandomPosition(), Quaternion.identity, roleLayer);
                });
            }

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
}