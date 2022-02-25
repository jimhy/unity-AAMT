using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Playables;
using Bindler;
using static SimpleAnimationPlayable;

[ExecuteInEditMode]
[RequireComponent(typeof(Animator))]
public partial class SimpleAnimation: MonoBehaviour
{
    private void Start()
    {
    }


    public interface State
    {
        bool enabled { get; set; }
        bool isValid { get; }
        float time { get; set; }
        float normalizedTime { get; set; }
        float speed { get; set; }
        string name { get; set; }
        float weight { get; set; }
        float length { get; }
        AnimationClip clip { get; }
        WrapMode wrapMode { get; set; }

    }
    public Animator animator
    {
        get
        {
            if (m_Animator == null)
            {
                m_Animator = GetComponent<Animator>();
            }
            return m_Animator;
        }
    }

    public bool animatePhysics
    {
        get { return m_AnimatePhysics; }
        set { m_AnimatePhysics = value; animator.updateMode = m_AnimatePhysics ? AnimatorUpdateMode.AnimatePhysics : AnimatorUpdateMode.Normal; }
    }

    public AnimatorCullingMode cullingMode
    {
        get { return animator.cullingMode; }
        set { m_CullingMode = value;  animator.cullingMode = m_CullingMode; }
    }

    public bool isPlaying {
        get {
            if(m_Playable != null)
                return m_Playable.IsPlaying();
            return false;
        }
    }

    public bool playAutomatically
    {
        get { return m_PlayAutomatically; }
        set { m_PlayAutomatically = value; }
    }

    public AnimationClip clip
    {
        get { return m_Clip; }
        set
        {
            LegacyClipCheck(value);
            m_Clip = value;
        }  
    }

    public WrapMode wrapMode
    {
        get { return m_WrapMode; }
        set { m_WrapMode = value; }
    }

    public void AddClip(AnimationClip clip, string newName)
    {
        LegacyClipCheck(clip);
        AddState(clip, newName);
    }

    public void Blend(string stateName, float targetWeight, float fadeLength)
    {
        m_Animator.enabled = true;
        Kick();
        m_Playable.Blend(stateName, targetWeight,  fadeLength);
    }

    public void CrossFade(string stateName, float fadeLength)
    {
        m_Animator.enabled = true;
        Kick();
        m_Playable.Crossfade(stateName, fadeLength);
    }

    public void CrossFadeQueued(string stateName, float fadeLength, QueueMode queueMode)
    {
        m_Animator.enabled = true;
        Kick();
        m_Playable.CrossfadeQueued(stateName, fadeLength, queueMode);
    }

    public int GetClipCount()
    {
        return m_Playable.GetClipCount();
    }

    public bool IsPlaying(string stateName)
    {
        return m_Playable.IsPlaying(stateName);
    }

    public void Stop()
    {
        m_Playable.StopAll();
    }

    public void Stop(string stateName)
    {
        m_Playable.Stop(stateName);
    }

    public void Sample()
    {
        m_Graph.Evaluate();
    }

    public bool Play()
    {
        m_Animator.enabled = true;
        Kick();
        if (m_Clip != null && m_PlayAutomatically)
        {
            m_Playable.Play(kDefaultStateName);
        }
        return false;
    }

    public void AddState(AnimationClip clip, string name)
    {
        LegacyClipCheck(clip);
        Kick();
        if (m_Playable.AddClip(clip, name))
        {
            RebuildStates();
        }
    }

    public void RemoveState(string name)
    {
        if (m_Playable.RemoveClip(name))
        {
            RebuildStates();
        }
    }

    bool mPlay(string stateName)
    {
        if(m_Animator != null)
        m_Animator.enabled = true;
        Kick();
        return m_Playable.Play(stateName);
    }
    
    public void Play(string actionName,Action<string> onComplete = null,Action<string> onHit = null)
    {
        if (!isPlaying)
        {
            mPlay(actionName);
            //return;
        }
        if (!durationDic.ContainsKey(actionName))
        {
            Debug.LogErrorFormat("没有此动作 : {0},请检查预制体 {1}", actionName, gameObject.name);
            return;
        }

        float time = durationDic[actionName];
        m_Playable.onAnimationComplete = onComplete;
        onHitCallBackFunc = onHit;
        if (time > 0)
        {
            m_Playable.Crossfade(actionName, time);
        }
        else
        {
            m_Playable.Play(actionName);
        }
    }

    public void PlayQueued(string stateName, QueueMode queueMode)
    {
        m_Animator.enabled = true;
        Kick();
        m_Playable.PlayQueued(stateName, queueMode);
    }

    public void RemoveClip(AnimationClip clip)
    {
        if (clip == null)
            throw new System.NullReferenceException("clip");

        if ( m_Playable.RemoveClip(clip) )
        {
            RebuildStates();
        }
       
    }

    /// <summary>
    /// 是否拥有该动画
    /// </summary>
    public bool HasAction(string actionName)
    {
        if (!durationDic.ContainsKey(actionName))
        {
            return false;
        }
        return true;
    }

    public void Rewind()
    {
        Kick();
        m_Playable.Rewind();
    }

    public void Rewind(string stateName)
    {
        Kick();
        m_Playable.Rewind(stateName);
    }

    public State GetState(string stateName)
    {
        SimpleAnimationPlayable.IState state = m_Playable.GetState(stateName);
        if (state == null)
            return null;

        return new StateImpl(state, this);
    }

    public IEnumerable<State> GetStates()
    {
        return new StateEnumerable(this);
    }

    public State this[string name]
    {
        get { return GetState(name); }
    }

}
