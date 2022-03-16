using System;

namespace AAMT
{
    
    public class AsyncHandler
    {
        internal int currentCount;
        internal int totalCount;
        public long currentBytes { get; internal set; }
        public long totalBytes { get; internal set; }
        public object customData { get; set; }
        public Action<AsyncHandler> onComplete;
        public Action<AsyncHandler> onProgress;

        public float progress => totalCount == 0 ? 0 : (float) currentCount / (float) totalCount;

        internal void OnProgress()
        {
            onProgress?.Invoke(this);
        }

        internal void OnComplete()
        {
            onComplete?.Invoke(this);
        }
    }
}