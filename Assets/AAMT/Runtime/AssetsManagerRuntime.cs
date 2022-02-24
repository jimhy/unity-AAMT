using System;
using System.Collections;
using UnityEngine;

namespace AAMT
{
    internal class AssetsManagerRuntime : MonoBehaviour
    {
        public Action OnUpdate;
        internal static AssetsManagerRuntime Instance { get; private set; }

        private AssetsManagerRuntime()
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