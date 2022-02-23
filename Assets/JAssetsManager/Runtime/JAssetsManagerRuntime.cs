using System;
using System.Collections;
using UnityEngine;

namespace JAssetsManager
{
    internal class JAssetsManagerRuntime : MonoBehaviour
    {
        public Action OnUpdate;
        internal static JAssetsManagerRuntime Instance { get; private set; }

        private JAssetsManagerRuntime()
        {
        }

        private void Awake()
        {
            Instance = this;
        }

        private void Update()
        {
            if (OnUpdate != null) OnUpdate();
        }

        public void Coroutine(IEnumerator routine)
        {
            StartCoroutine(routine);
        }
    }
}