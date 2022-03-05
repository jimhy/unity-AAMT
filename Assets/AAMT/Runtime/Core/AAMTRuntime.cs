using System;
using System.Collections;
using UnityEngine;

namespace AAMT
{
    internal class AAMTRuntime : MonoBehaviour
    {
        public Action OnUpdate;
        internal static AAMTRuntime Instance { get; private set; }

        private AAMTRuntime()
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
    }
}