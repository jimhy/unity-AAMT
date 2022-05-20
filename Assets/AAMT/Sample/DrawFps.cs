using GameLogic;
using System;
using UnityEngine;

namespace GameLogic
{
	public class DrawFps
	{
		public bool isDraw = true;
		GUIStyle style = new GUIStyle();
		public static DrawFps Instance = new DrawFps();
		private DrawFps ()
		{
			style.fontSize = 20;
		}

		public void OnGUI()
		{
			Update();
			if (isDraw)
			{
				if (_mLastFps > 25)
				{
					style.normal.textColor = new Color(0, 1, 0);
				}
				else if (_mLastFps > 20)
				{
					style.normal.textColor = new Color(1, 1, 0);
				}
				else
				{
					style.normal.textColor = new Color(1.0f, 0, 0);
				}

				GUI.Label(new Rect(5, 5, 200, 100), "fps: " + _mLastFps, style);
			}
		}

		private long _mFrameCount = 0;
		private long _mLastFrameTime = 0;
		private static long _mLastFps = 0;

		private void Update()
		{
			if (true)
            {
                _mFrameCount++;
				long nCurTime = TickToMilliSec(System.DateTime.Now.Ticks);
				if (_mLastFrameTime == 0)
				{
					_mLastFrameTime = TickToMilliSec(System.DateTime.Now.Ticks);
				}

				if ((nCurTime - _mLastFrameTime) >= 1000)
				{
					long fps = (long)(_mFrameCount * 1.0f / ((nCurTime - _mLastFrameTime) / 1000.0f));

					_mLastFps = fps;

					_mFrameCount = 0;

					_mLastFrameTime = nCurTime;
				}
			}

		}
		public static long TickToMilliSec(long tick)
		{
			return tick / (10 * 1000);
		}
	}
}

