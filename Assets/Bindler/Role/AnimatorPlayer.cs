using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace Bindler
{
    public delegate void AnimationPlayerCallBackFun(AnimationClip clip);

    public delegate void CallBack(object o);
    [RequireComponent(typeof(Animator))]
    public class AnimatorPlayer : MonoBehaviour
    {
        [Serializable]
        public struct AniClip
        {
            [EnumName("动作剪辑")]
            public AnimationClip clip;
            [EnumName("动作融合时间")]
            public float transitionDuration;
        }
       
        [Title("动作列表")]
        public AniClip[] animationClips;
        private Dictionary<string, AniClip> animationClipMap;
        public CallBack OnHitCallBackFun;
        public CallBack OnSoundCallBackFun;
        private AnimatorPlayerable animationPlayer;
        private PlayableGraph playableGraph;

        


        private void Awake()
        {
            playableGraph = PlayableGraph.Create();
            var playalber = ScriptPlayable<AnimatorPlayerable>.Create(playableGraph);
            animationPlayer = playalber.GetBehaviour();
            animationPlayer.Initialize(playalber, playableGraph, GetComponent<Animator>());

            animationClipMap = new Dictionary<string, AniClip>();
            foreach (var item in animationClips)
            {
                if (item.clip == null) continue;
                animationClipMap[item.clip.name] = item;
            }
        }
        /// <summary>
        /// 动画完成回调
        /// </summary>
        public AnimationPlayerCallBackFun OnAnimationComplete
        {
            set
            {
                animationPlayer.OnAnimationComplete = value;
            }
        }
        /// <summary>
        /// 播放动画
        /// </summary>
        /// <param name="clip">动画剪辑</param>
        /// <param name="loop">是否循环</param>
        /// <param name="transitionDuration">动画过渡时常</param>
        public void Play(AnimationClip clip, bool loop = false, float transitionDuration = 0)
        {
            if(transitionDuration == 0 && animationClipMap.ContainsKey(clip.name))
            {
                transitionDuration = animationClipMap[clip.name].transitionDuration;
            }
            if (transitionDuration == 0) animationPlayer.Play(clip, loop);
            else animationPlayer.Crossfade(clip, loop, transitionDuration);
        }
        /// <summary>
        /// 播放动画
        /// </summary>
        /// <param name="clipName">动画剪辑名称</param>
        /// <param name="loop">是否循环</param>
        /// <param name="transitionDuration">动画过渡时常</param>
        public void Play(string clipName, bool loop = false, float transitionDuration = 0)
        {
            if(!animationClipMap.ContainsKey(clipName))
            {
                Debug.LogWarningFormat("{0} clip can not found.", clipName);
                return;
            }
            var item = animationClipMap[clipName];
            AnimationClip clip = item.clip;
            if (transitionDuration == 0) transitionDuration = item.transitionDuration;

            if (transitionDuration == 0) Play(clip, loop);
            else animationPlayer.Crossfade(clip, loop, transitionDuration);
        }

        public float GetClipLength(string clipName)
        {
            var item = animationClipMap[clipName];
            return item.clip.length;
        }
        public void Stop()
        {
            animationPlayer.Stop();
        }

        public void Pause()
        {
            animationPlayer.Pause();
        }

        public void Resume()
        {
            animationPlayer.Resume();
        }

        public void SetSpeed(double speed)
        {
            animationPlayer.SetSpeed(speed);
        }

        public void SetTime(double time)
        {
            animationPlayer.SetTime(time);
        }

        public double GetTime()
        {
            return animationPlayer.GetTime();
        }

        public bool isPlaying
        {
            get
            {
                if (animationPlayer == null) return false;
                return animationPlayer.isPlaying;
            }
        }

        public void SetDelay(double time)
        {
            animationPlayer.SetDelay(time);
        }
        private void OnDestroy()
        {
            playableGraph.Destroy();
        }

        private void OnHit(string s)
        {
            OnHitCallBackFun?.Invoke(s);
        }

        private void OnSound(string s)
        {
            OnSoundCallBackFun?.Invoke(s);
        }

        private void OnDestory()
        {
            if(animationClipMap != null) animationClipMap.Clear();
            animationClips = null;
            animationClipMap = null;
            OnHitCallBackFun = null;
            OnSoundCallBackFun = null;
            animationPlayer = null;
    }
    }
    /// <summary>
    /// 动画播放器
    /// </summary>
    public class AnimatorPlayerable : PlayableBehaviour
    {
        /// <summary>
        /// 动画播放完成回调代理
        /// </summary>
        public AnimationPlayerCallBackFun OnAnimationComplete;
        /// <summary>
        /// 到播放下一帧的剩余时间
        /// </summary>
        private float _timeToNextClip;
        private bool _isRunning;
        /// <summary>
        /// 是否需要循环播放
        /// </summary>
        private bool _loop;
        /// <summary>
        /// 变换动作的持续时间
        /// </summary>
        private float _transitionDuration = 1.0f;
        /// <summary>
        /// 变换动作经过时间
        /// </summary>
        private float _transitionTime = Mathf.Infinity;
        /// <summary>
        /// 变换动作的开始时间
        /// </summary>
        private float _transitionStart = 0.0f;
        /// <summary>
        /// 是否处于等待变换阶段
        /// </summary>
        private bool _inTransition;
        /// <summary>
        /// 延迟播放时间
        /// </summary>
        private double _delayTime;

        private Playable _mixer;
        private PlayableGraph _graph;
        private AnimationPlayableOutput _playableOutput;
        public void Initialize(Playable owner, PlayableGraph graph, Animator animator)
        {
            _isRunning = false;
            _inTransition = false;
            _graph = graph;
            owner.SetInputCount(1);
            _mixer = AnimationMixerPlayable.Create(graph, 2);
            _graph.Connect(_mixer, 0, owner, 0);
            owner.SetInputWeight(0, 1);
            _playableOutput = AnimationPlayableOutput.Create(_graph, "Animation", animator);
            _playableOutput.SetSourcePlayable(owner);
            _playableOutput.SetSourceInputPort(0);
        }
        /// <summary>
        /// 播放动画
        /// </summary>
        /// <param name="clip">动画剪辑</param>
        /// <param name="loop">是否循环</param>
        public void Play(AnimationClip clip, bool loop = false)
        {
            var animationPlayable = AnimationClipPlayable.Create(_graph, clip);
            // _mixer.DisconnectInput(0);
            MixerDisconnectAllInput();
            _mixer.ConnectInput(0, animationPlayable, 0, 1.0f);
            _loop = loop;
            _timeToNextClip = clip.length;
            animationPlayable.SetTime(0);
            _transitionTime = Mathf.Infinity;
            _graph.Play();
            _isRunning = true;
            _inTransition = false;
            _transitionStart = Time.time;
            if (_delayTime != 0) _mixer.Pause();
        }

        public void Stop()
        {
            if (!_isRunning) return;
            _graph.Stop();
        }

        public void Pause()
        {
            if (!_isRunning) return;
            _isRunning = false;
            _mixer.Pause();
        }

        public void Resume()
        {
            if (_isRunning) return;
            _isRunning = true;
            _mixer.Play();
        }

        public bool isPlaying
        {
            get{
                return _isRunning;
            }
        }

        public void SetSpeed(double speed)
        {
            _mixer.SetSpeed(speed);
        }

        public void SetTime(double time)
        {
            _mixer.SetTime(time);
        }

        public double GetTime()
        {
            return _mixer.GetTime();
        }

        public void SetDelay(double time)
        {
            _delayTime = time;
        }

        private bool checkRunning()
        {
            if (!_mixer.CanSetWeights()) return false;
            if (!_isRunning || _mixer.GetInputCount() == 0) return false;
            if (Time.time - _transitionStart < _delayTime) return false;
            if (_delayTime == 0) return true;
            _mixer.Play();
            _delayTime = 0;
            return true;
        }
        /// <summary>
        /// 过渡动画播放
        /// </summary>
        /// <param name="clip">动画剪辑</param>
        /// <param name="loop">是否循环</param>
        /// <param name="transitionDuration">动画过渡持续时间</param>
        public void Crossfade(AnimationClip clip, bool loop, float transitionDuration)
        {
            var animationPlayable = AnimationClipPlayable.Create(_graph, clip);
            _mixer.DisconnectInput(1);
            _mixer.ConnectInput(1, animationPlayable, 0, 0.0f);
            _transitionDuration = transitionDuration;
            _loop = loop;
            _timeToNextClip = clip.length;
            _isRunning = true;
            _inTransition = true;
            _transitionTime = 0;
            _transitionStart = Time.time;
            _graph.Play();
            if (_delayTime != 0) _mixer.Pause();
        }

        override public void PrepareFrame(Playable owner, FrameData info)
        {
            if (!checkRunning()) return;

            _timeToNextClip -= (float)info.deltaTime;
            if (_timeToNextClip <= 0.0f)
            {
                var currentClip = (AnimationClipPlayable)_mixer.GetInput(0);
                _timeToNextClip = currentClip.GetAnimationClip().length;
                _playableOutput.GetSourcePlayable().SetTime(0);
                if (_loop)
                {
                    _transitionTime = 10000;
                    currentClip.SetTime(0);
                }else
                {
                    _isRunning = false;
                    _graph.Stop();
                    OnAnimationComplete?.Invoke(currentClip.GetAnimationClip());
                    return;
                }
            }

            if (_transitionTime <= _transitionDuration)
            {
                _transitionTime = Time.time - _transitionStart;
                var wight = Mathf.Clamp01(_transitionTime / _transitionDuration);
                _mixer.SetInputWeight(0, 1.0f- wight);
                _mixer.SetInputWeight(1, wight);
            }
            else if (_inTransition)
            {
                var lastPlayable = _mixer.GetInput(1);
                MixerDisconnectAllInput();
                _mixer.ConnectInput(0, lastPlayable, 0, 1.0f);
                _inTransition = false;
            }
        }

        private void MixerDisconnectAllInput()
        {
            for (int i = 0; i < _mixer.GetInputCount(); i++)
            {
                _mixer.DisconnectInput(i);
            }
        }

    }
}
