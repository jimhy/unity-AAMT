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

        public void CallOnNextFrame(Action cb, uint delayFrames = 1)
        {
            StartCoroutine(OnCall(cb, delayFrames));
        }

        private IEnumerator OnCall(Action cb, uint delayFrames)
        {
            for (int i = 0; i < delayFrames; i++)
            {
                yield return 0;
            }

            cb.Invoke();
        }
    }
}