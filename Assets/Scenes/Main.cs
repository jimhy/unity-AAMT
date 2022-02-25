using System;
using System.Collections.Generic;
using AAMT;
using UnityEngine;

namespace GameLogic
{
    public class Main : MonoBehaviour
    {
        public GameObject loadButton;
        public GameObject loadButtonBatch;
        public UILabel label;
        public Transform roleLayer;

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
            "roles/prefabs/mh_05_lv2_hd.prefab",
            "roles/prefabs/mh_05_lv2_low.prefab",
            "roles/prefabs/mh_05_lv3_hd.prefab",
            "roles/prefabs/mh_05_lv3_low.prefab",
            "roles/prefabs/mh_06_lv1_hd.prefab",
            "roles/prefabs/mh_06_lv1_low.prefab",
            "roles/prefabs/mh_06_lv2_hd.prefab",
            "roles/prefabs/mh_06_lv2_low.prefab",
            "roles/prefabs/mh_06_lv3_hd.prefab",
            "roles/prefabs/mh_06_lv3_low.prefab",
            "roles/prefabs/mh_07_lv1_hd.prefab",
            "roles/prefabs/mh_07_lv1_low.prefab",
            "roles/prefabs/mh_07_lv2_hd.prefab",
            "roles/prefabs/mh_07_lv2_low.prefab",
            "roles/prefabs/mh_07_lv3_hd.prefab",
            "roles/prefabs/mh_07_lv3_low.prefab",
            "roles/prefabs/mz_01_lv1_hd.prefab",
            "roles/prefabs/mz_01_lv1_low.prefab",
            "roles/prefabs/mz_01_lv2_hd.prefab",
            "roles/prefabs/mz_01_lv2_low.prefab",
            "roles/prefabs/mz_01_lv3_hd.prefab",
            "roles/prefabs/mz_01_lv3_low.prefab",
            "roles/prefabs/mz_02_lv1_hd.prefab",
            "roles/prefabs/mz_02_lv1_low.prefab",
            "roles/prefabs/mz_02_lv2_hd.prefab",
            "roles/prefabs/mz_02_lv2_low.prefab",
            "roles/prefabs/mz_02_lv3_hd.prefab",
            "roles/prefabs/mz_02_lv3_low.prefab",
            "roles/prefabs/mz_03_bs_lv1_low.prefab",
            "roles/prefabs/mz_03_bs_lv2_low.prefab",
            "roles/prefabs/mz_03_bs_lv3_low.prefab",
            "roles/prefabs/mz_03_lv1_hd.prefab",
            "roles/prefabs/mz_03_lv1_low.prefab",
            "roles/prefabs/mz_03_lv2_hd.prefab",
            "roles/prefabs/mz_03_lv2_low.prefab",
            "roles/prefabs/mz_03_lv3_hd.prefab",
            "roles/prefabs/mz_03_lv3_low.prefab",
            "roles/prefabs/mz_04_lv1_hd.prefab",
            "roles/prefabs/mz_04_lv1_low.prefab",
            "roles/prefabs/mz_04_lv2_hd.prefab",
            "roles/prefabs/mz_04_lv2_low.prefab",
            "roles/prefabs/mz_04_lv3_hd.prefab",
            "roles/prefabs/mz_04_lv3_low.prefab",
            "roles/prefabs/mz_05_lv1_hd.prefab",
            "roles/prefabs/mz_05_lv1_low.prefab",
            "roles/prefabs/mz_05_lv2_hd.prefab",
            "roles/prefabs/mz_05_lv2_low.prefab",
            "roles/prefabs/mz_05_lv3_hd.prefab",
            "roles/prefabs/mz_05_lv3_low.prefab",
            "roles/prefabs/npc_shangren.prefab",
            "roles/prefabs/npc_shangren_che.prefab",
            "roles/prefabs/npc_xiaozhu.prefab",
            "roles/prefabs/qs_jnn_hd.prefab",
            "roles/prefabs/qs_jnn_low.prefab",
            "roles/prefabs/qs_jntn_hd.prefab",
            "roles/prefabs/qs_jntn_low.prefab",
            "roles/prefabs/qs_ss_hd.prefab",
            "roles/prefabs/qs_ss_low.prefab",
            "roles/prefabs/qs_xsm_hd.prefab",
            "roles/prefabs/qs_xsm_low.prefab",
            "roles/prefabs/rl_01_lv1_hd.prefab",
            "roles/prefabs/rl_01_lv1_low.prefab",
            "roles/prefabs/rl_01_lv2_hd.prefab",
            "roles/prefabs/rl_01_lv2_low.prefab",
            "roles/prefabs/rl_01_lv3_hd.prefab",
            "roles/prefabs/rl_01_lv3_low.prefab",
            "roles/prefabs/rl_02_lv1_hd.prefab",
            "roles/prefabs/rl_02_lv1_low.prefab",
            "roles/prefabs/rl_02_lv2_hd.prefab",
            "roles/prefabs/rl_02_lv2_low.prefab",
            "roles/prefabs/rl_02_lv3_hd.prefab",
            "roles/prefabs/rl_02_lv3_low.prefab",
            "roles/prefabs/rl_03_lv1_hd.prefab",
            "roles/prefabs/rl_03_lv1_low.prefab",
            "roles/prefabs/rl_03_lv2_hd.prefab",
            "roles/prefabs/rl_03_lv2_low.prefab",
            "roles/prefabs/rl_03_lv3_hd.prefab",
            "roles/prefabs/rl_03_lv3_low.prefab",
            "roles/prefabs/rl_04_lv1_hd.prefab",
            "roles/prefabs/rl_04_lv1_low.prefab",
            "roles/prefabs/rl_04_lv2_hd.prefab",
            "roles/prefabs/rl_04_lv2_low.prefab",
            "roles/prefabs/rl_04_lv3_hd.prefab",
            "roles/prefabs/rl_04_lv3_low.prefab",
            "roles/prefabs/rl_05_bs_lv1_low.prefab",
            "roles/prefabs/rl_05_bs_lv2_low.prefab",
            "roles/prefabs/rl_05_bs_lv3_low.prefab",
            "roles/prefabs/rl_05_lv1_hd.prefab",
            "roles/prefabs/rl_05_lv1_low.prefab",
            "roles/prefabs/rl_05_lv1_zhw.prefab",
            "roles/prefabs/rl_05_lv2_hd.prefab",
            "roles/prefabs/rl_05_lv2_low.prefab",
            "roles/prefabs/rl_05_lv2_zhw.prefab",
            "roles/prefabs/rl_05_lv3_hd.prefab",
            "roles/prefabs/rl_05_lv3_low.prefab",
            "roles/prefabs/rl_05_lv3_zhw.prefab",
            "roles/prefabs/rl_06_lv1_hd.prefab",
            "roles/prefabs/rl_06_lv1_low.prefab",
            "roles/prefabs/rl_06_lv2_hd.prefab",
            "roles/prefabs/rl_06_lv2_low.prefab",
            "roles/prefabs/rl_06_lv3_hd.prefab",
            "roles/prefabs/rl_06_lv3_low.prefab",
            "roles/prefabs/rsww_01_lv1_hd.prefab",
            "roles/prefabs/rsww_01_lv1_low.prefab",
            "roles/prefabs/rsww_01_lv2_hd.prefab",
            "roles/prefabs/rsww_01_lv2_low.prefab",
            "roles/prefabs/rsww_01_lv3_hd.prefab",
            "roles/prefabs/rsww_01_lv3_low.prefab",
            "roles/prefabs/sx_01_lv1_hd.prefab",
            "roles/prefabs/sx_01_lv1_low.prefab",
            "roles/prefabs/sx_01_lv2_hd.prefab",
            "roles/prefabs/sx_01_lv2_low.prefab",
            "roles/prefabs/sx_01_lv3_hd.prefab",
            "roles/prefabs/sx_01_lv3_low.prefab",
            "roles/prefabs/sx_02_lv1_hd.prefab",
            "roles/prefabs/sx_02_lv1_low.prefab",
            "roles/prefabs/sx_02_lv2_hd.prefab",
            "roles/prefabs/sx_02_lv2_low.prefab",
            "roles/prefabs/sx_02_lv3_hd.prefab",
            "roles/prefabs/sx_02_lv3_low.prefab",
            "roles/prefabs/sx_03_lv1_hd.prefab",
            "roles/prefabs/sx_03_lv1_low.prefab",
            "roles/prefabs/sx_03_lv2_hd.prefab",
            "roles/prefabs/sx_03_lv2_low.prefab",
            "roles/prefabs/sx_03_lv3_hd.prefab",
            "roles/prefabs/sx_03_lv3_low.prefab",
            "roles/prefabs/sx_04_lv1_hd.prefab",
            "roles/prefabs/sx_04_lv1_low.prefab",
            "roles/prefabs/sx_04_lv2_hd.prefab",
            "roles/prefabs/sx_04_lv2_low.prefab",
            "roles/prefabs/sx_04_lv3_hd.prefab",
            "roles/prefabs/sx_04_lv3_low.prefab",
            "roles/prefabs/sz_01_lv1_hd.prefab",
            "roles/prefabs/sz_01_lv1_low.prefab",
            "roles/prefabs/sz_01_lv1_zhw.prefab",
            "roles/prefabs/sz_01_lv2_hd.prefab",
            "roles/prefabs/sz_01_lv2_low.prefab",
            "roles/prefabs/sz_01_lv2_zhw.prefab",
            "roles/prefabs/sz_01_lv3_hd.prefab",
            "roles/prefabs/sz_01_lv3_low.prefab",
            "roles/prefabs/sz_01_lv3_zhw.prefab",
            "roles/prefabs/sz_02_lv1_hd.prefab",
            "roles/prefabs/sz_02_lv1_low.prefab",
            "roles/prefabs/sz_02_lv2_hd.prefab",
            "roles/prefabs/sz_02_lv2_low.prefab",
            "roles/prefabs/sz_02_lv3_hd.prefab",
            "roles/prefabs/sz_02_lv3_low.prefab",
            "roles/prefabs/sz_03_lv1_hd.prefab",
            "roles/prefabs/sz_03_lv1_low.prefab",
            "roles/prefabs/sz_03_lv2_hd.prefab",
            "roles/prefabs/sz_03_lv2_low.prefab",
            "roles/prefabs/sz_03_lv3_hd.prefab",
            "roles/prefabs/sz_03_lv3_low.prefab",
            "roles/prefabs/sz_04_lv1_hd.prefab",
            "roles/prefabs/sz_04_lv1_low.prefab",
            "roles/prefabs/sz_04_lv1_zhw.prefab",
            "roles/prefabs/sz_04_lv2_hd.prefab",
            "roles/prefabs/sz_04_lv2_low.prefab",
            "roles/prefabs/sz_04_lv2_zhw.prefab",
            "roles/prefabs/sz_04_lv3_hd.prefab",
            "roles/prefabs/sz_04_lv3_low.prefab",
            "roles/prefabs/sz_04_lv3_zhw.prefab",
            "roles/prefabs/sz_05_lv1_hd.prefab",
            "roles/prefabs/sz_05_lv1_low.prefab",
            "roles/prefabs/sz_05_lv2_hd.prefab",
            "roles/prefabs/sz_05_lv2_low.prefab",
            "roles/prefabs/sz_05_lv3_hd.prefab",
            "roles/prefabs/sz_05_lv3_low.prefab",
            "roles/prefabs/sz_05_lv3_zhw.prefab",
            "roles/prefabs/sz_06_lv1_hd.prefab",
            "roles/prefabs/sz_06_lv1_low.prefab",
            "roles/prefabs/sz_06_lv2_hd.prefab",
            "roles/prefabs/sz_06_lv2_low.prefab",
            "roles/prefabs/sz_06_lv3_hd.prefab",
            "roles/prefabs/sz_06_lv3_low.prefab",
            "roles/prefabs/sz_07_lv1_hd.prefab",
            "roles/prefabs/sz_07_lv1_low.prefab",
            "roles/prefabs/sz_07_lv2_hd.prefab",
            "roles/prefabs/sz_07_lv2_low.prefab",
            "roles/prefabs/sz_07_lv3_hd.prefab",
            "roles/prefabs/sz_07_lv3_low.prefab",
            "roles/prefabs/wy_01_lv1_hd.prefab",
            "roles/prefabs/wy_01_lv1_low.prefab",
            "roles/prefabs/wy_01_lv2_hd.prefab",
            "roles/prefabs/wy_01_lv2_low.prefab",
            "roles/prefabs/wy_01_lv3_hd.prefab",
            "roles/prefabs/wy_01_lv3_low.prefab",
            "roles/prefabs/wy_02_lv1_hd.prefab",
            "roles/prefabs/wy_02_lv1_low.prefab",
            "roles/prefabs/wy_02_lv2_hd.prefab",
            "roles/prefabs/wy_02_lv2_low.prefab",
            "roles/prefabs/wy_02_lv3_hd.prefab",
            "roles/prefabs/wy_02_lv3_low.prefab",
            "roles/prefabs/wy_03_lv1_hd.prefab",
            "roles/prefabs/wy_03_lv1_low.prefab",
            "roles/prefabs/wy_03_lv2_hd.prefab",
            "roles/prefabs/wy_03_lv2_low.prefab",
            "roles/prefabs/wy_03_lv3_hd.prefab",
            "roles/prefabs/wy_03_lv3_low.prefab",
            "roles/prefabs/xz_01_lv1_hd.prefab",
            "roles/prefabs/xz_01_lv1_low.prefab",
            "roles/prefabs/xz_01_lv2_hd.prefab",
            "roles/prefabs/xz_01_lv2_low.prefab",
            "roles/prefabs/xz_01_lv3_hd.prefab",
            "roles/prefabs/xz_01_lv3_low.prefab",
            "roles/prefabs/xz_02_bs_lv1_hd.prefab",
            "roles/prefabs/xz_02_bs_lv1_low.prefab",
            "roles/prefabs/xz_02_bs_lv2_hd.prefab",
            "roles/prefabs/xz_02_bs_lv2_low.prefab",
            "roles/prefabs/xz_02_bs_lv3_hd.prefab",
            "roles/prefabs/xz_02_bs_lv3_low.prefab",
            "roles/prefabs/xz_02_lv1_hd.prefab",
            "roles/prefabs/xz_02_lv1_low.prefab",
            "roles/prefabs/xz_02_lv2_hd.prefab",
            "roles/prefabs/xz_02_lv2_low.prefab",
            "roles/prefabs/xz_02_lv3_hd.prefab",
            "roles/prefabs/xz_02_lv3_low.prefab",
            "roles/prefabs/xz_03_lv1_hd.prefab",
            "roles/prefabs/xz_03_lv1_low.prefab",
            "roles/prefabs/xz_03_lv2_hd.prefab",
            "roles/prefabs/xz_03_lv2_low.prefab",
            "roles/prefabs/xz_03_lv3_hd.prefab",
            "roles/prefabs/xz_03_lv3_low.prefab",
            "roles/prefabs/xz_04_lv1_hd.prefab",
            "roles/prefabs/xz_04_lv1_low.prefab",
            "roles/prefabs/xz_04_lv2_hd.prefab",
            "roles/prefabs/xz_04_lv2_low.prefab",
            "roles/prefabs/xz_04_lv3_hd.prefab",
            "roles/prefabs/xz_04_lv3_low.prefab",
            "roles/prefabs/xz_05_lv1_hd.prefab",
            "roles/prefabs/xz_05_lv1_low.prefab",
            "roles/prefabs/xz_05_lv2_hd.prefab",
            "roles/prefabs/xz_05_lv2_low.prefab",
            "roles/prefabs/xz_05_lv3_hd.prefab",
            "roles/prefabs/xz_05_lv3_low.prefab",
            "roles/prefabs/xz_06_lv1_hd.prefab",
            "roles/prefabs/xz_06_lv1_low.prefab",
            "roles/prefabs/xz_06_lv2_hd.prefab",
            "roles/prefabs/xz_06_lv2_low.prefab",
            "roles/prefabs/xz_06_lv3_hd.prefab",
            "roles/prefabs/xz_06_lv3_low.prefab",
            "roles/prefabs/ym_01_lv1_hd.prefab",
            "roles/prefabs/ym_01_lv1_low.prefab",
            "roles/prefabs/ym_01_lv2_hd.prefab",
            "roles/prefabs/ym_01_lv2_low.prefab",
            "roles/prefabs/ym_01_lv3_hd.prefab",
            "roles/prefabs/ym_01_lv3_low.prefab",
            "roles/prefabs/ym_02_lv1_hd.prefab",
            "roles/prefabs/ym_02_lv1_low.prefab",
            "roles/prefabs/ym_02_lv2_hd.prefab",
            "roles/prefabs/ym_02_lv2_low.prefab",
            "roles/prefabs/ym_02_lv3_hd.prefab",
            "roles/prefabs/ym_02_lv3_low.prefab",
            "roles/prefabs/ym_03_lv1_hd.prefab",
            "roles/prefabs/ym_03_lv1_low.prefab",
            "roles/prefabs/ym_03_lv2_hd.prefab",
            "roles/prefabs/ym_03_lv2_low.prefab",
            "roles/prefabs/ym_03_lv3_hd.prefab",
            "roles/prefabs/ym_03_lv3_low.prefab",
            "roles/prefabs/ym_04_lv1_hd.prefab",
            "roles/prefabs/ym_04_lv1_low.prefab",
            "roles/prefabs/ym_04_lv2_hd.prefab",
            "roles/prefabs/ym_04_lv2_low.prefab",
            "roles/prefabs/ym_04_lv3_hd.prefab",
            "roles/prefabs/ym_04_lv3_low.prefab",
            "roles/prefabs/yz_01_lv1_hd.prefab",
            "roles/prefabs/yz_01_lv1_low.prefab",
            "roles/prefabs/yz_01_lv2_hd.prefab",
            "roles/prefabs/yz_01_lv2_low.prefab",
            "roles/prefabs/yz_01_lv3_hd.prefab",
            "roles/prefabs/yz_01_lv3_low.prefab",
            "roles/prefabs/yz_02_lv1_hd.prefab",
            "roles/prefabs/yz_02_lv1_low.prefab",
            "roles/prefabs/yz_02_lv2_hd.prefab",
            "roles/prefabs/yz_02_lv2_low.prefab",
            "roles/prefabs/yz_02_lv3_hd.prefab",
            "roles/prefabs/yz_02_lv3_low.prefab",
            "roles/prefabs/yz_03_lv1_hd.prefab",
            "roles/prefabs/yz_03_lv1_low.prefab",
            "roles/prefabs/yz_03_lv2_hd.prefab",
            "roles/prefabs/yz_03_lv2_low.prefab",
            "roles/prefabs/yz_03_lv3_hd.prefab",
            "roles/prefabs/yz_03_lv3_low.prefab",
            "roles/prefabs/yz_04_lv1_hd.prefab",
            "roles/prefabs/yz_04_lv1_low.prefab",
            "roles/prefabs/yz_04_lv1_zhw.prefab",
            "roles/prefabs/yz_04_lv2_hd.prefab",
            "roles/prefabs/yz_04_lv2_low.prefab",
            "roles/prefabs/yz_04_lv2_zhw.prefab",
            "roles/prefabs/yz_04_lv3_hd.prefab",
            "roles/prefabs/yz_04_lv3_low.prefab",
            "roles/prefabs/yz_04_lv3_zhw.prefab",
            "roles/prefabs/yz_05_lv1_hd.prefab",
            "roles/prefabs/yz_05_lv1_low.prefab",
            "roles/prefabs/yz_05_lv2_hd.prefab",
            "roles/prefabs/yz_05_lv2_low.prefab",
            "roles/prefabs/yz_05_lv3_hd.prefab",
            "roles/prefabs/yz_05_lv3_low.prefab",
            "roles/prefabs/yz_06_lv1_hd.prefab",
            "roles/prefabs/yz_06_lv1_low.prefab",
            "roles/prefabs/yz_06_lv2_hd.prefab",
            "roles/prefabs/yz_06_lv2_low.prefab",
            "roles/prefabs/yz_06_lv3_hd.prefab",
            "roles/prefabs/yz_06_lv3_low.prefab",
            "roles/prefabs/yz_07_lv1_hd.prefab",
            "roles/prefabs/yz_07_lv1_low.prefab",
            "roles/prefabs/yz_07_lv2_hd.prefab",
            "roles/prefabs/yz_07_lv2_low.prefab",
            "roles/prefabs/yz_07_lv3_hd.prefab",
            "roles/prefabs/yz_07_lv3_low.prefab",
            "roles/prefabs/yz_08_lv1_hd.prefab",
            "roles/prefabs/yz_08_lv1_low.prefab",
            "roles/prefabs/yz_08_lv2_hd.prefab",
            "roles/prefabs/yz_08_lv2_low.prefab",
            "roles/prefabs/yz_08_lv3_hd.prefab",
            "roles/prefabs/yz_08_lv3_low.prefab"
        };

        private void Awake()
        {
            QualitySettings.vSyncCount = 1;
            Application.targetFrameRate = 30;
            QualitySettings.vSyncCount = 0;
        }

        private System.Diagnostics.Stopwatch stopwatch;

        void Start()
        {
            stopwatch = new System.Diagnostics.Stopwatch();

            if (loadButton != null) UIEventListener.Get(loadButton).onClick = OnLoad;
            if (loadButtonBatch != null) UIEventListener.Get(loadButtonBatch).onClick = OnLoadBatch;
        }


        private void OnLoad(GameObject go)
        {
            stopwatch.Start();
            AssetsManager.Instance.LoadAssets(pathList, OnLoadComplete1);
        }

        private void OnLoadBatch(GameObject go)
        {
            stopwatch.Start();
            PlayerPrefs.SetString("SetTime", DateTime.Now.ToShortTimeString());
            AssetsManager.Instance.LoadAssetsBatch(pathList, OnLoadComplete2);
        }

        private void OnLoadComplete1()
        {
            foreach (var path in pathList)
            {
                var obj = AssetsManager.Instance.GetAssets<GameObject>(path);
                var go = GameObject.Instantiate(obj);
                go.transform.position = RandomPosition();
                go.transform.parent = roleLayer;
            }
            SetUseTimeView();
        }

        private void OnLoadComplete2()
        {
            foreach (var path in pathList)
            {
                var go = AssetsManager.Instance.GetAssets<GameObject>(path);
                Instantiate(go, RandomPosition(), Quaternion.identity, roleLayer);
            }
            SetUseTimeView();
        }

        private void SetUseTimeView()
        {
            stopwatch.Stop();
            var str = $"used time:{stopwatch.Elapsed.TotalSeconds} s";
            label.text = str;
        }

        private Vector3 RandomPosition()
        {
            var x = UnityEngine.Random.Range(-25f, 25f);
            var z = UnityEngine.Random.Range(-25f, 25f);
            return new Vector3(x, 0, z);
        }

        // Update is called once per frame
        void Update()
        {
        }

        private void OnGUI()
        {
            DrawFps.Instance.OnGUI();
        }
    }
}