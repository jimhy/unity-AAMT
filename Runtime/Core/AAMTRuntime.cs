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

        public void CallOnNextFrame(Action<object> cb, uint delayFrames = 1, object data = null)
        {
            StartCoroutine(OnCall(cb, delayFrames, data));
        }

        private IEnumerator OnCall(Action<object> cb, uint delayFrames, object data = null)
        {
            for (int i = 0; i < delayFrames; i++)
            {
                yield return 0;
            }

            cb.Invoke(data);
        }
    }
}