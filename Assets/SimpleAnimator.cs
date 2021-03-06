﻿using UnityEngine;

using UnityEngine.Events;
using System.Collections.Generic;
using System.Linq;

public class SimpleAnimator : MonoBehaviour
{
    #region Public Properties
    [System.Serializable]
    public class Anim
    {
        public string name;
        public Sprite[] frames;
        public float framesPerSec = 5;
        public bool loop = true;

        public float duration
        {
            get
            {
                return frames.Length * framesPerSec;
            }
            set
            {
                framesPerSec = value / frames.Length;
            }
        }
    }
	public bool RandomFrame = false;
    public List<Anim> animations = new List<Anim>();
	public bool justSwitched
	{
		get { return _justSwitched; }
	}

    [HideInInspector]
    public int currentFrame;

    [HideInInspector]
    public bool done
    {
        get { return currentFrame >= current.frames.Length; }
    }

    [HideInInspector]
    public bool playing
    {
        get { return _playing; }
    }

	[HideInInspector]
	public string currentAnimation
	{
		get { return current.name; }
	}

    #endregion
    //--------------------------------------------------------------------------------
    #region Private Properties
    SpriteRenderer spriteRenderer;
    Anim current;
    bool _playing;
    float secsPerFrame;
    float lastFrameTime;
    float speed = 1f;
	bool _justSwitched;

    #endregion
    //--------------------------------------------------------------------------------
    #region Editor Support
    [ContextMenu("Sort All Frames by Name")]
    void DoSort()
    {
        foreach (Anim anim in animations)
        {
            System.Array.Sort(anim.frames, (a, b) => a.name.CompareTo(b.name));
        }
        Debug.Log(gameObject.name + " animation frames have been sorted alphabetically.");
    }
    #endregion
    //--------------------------------------------------------------------------------
    #region MonoBehaviour Events
    void Start()
    {
        lastFrameTime = Time.time;
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.Log(gameObject.name + ": Couldn't find SpriteRenderer");
        }

        if (animations.Count > 0) PlayByIndex(0);
    }

    void Update()
    {
		_justSwitched = false;

        if (!_playing || Time.time < lastFrameTime + secsPerFrame / speed || spriteRenderer == null) return;
		if (RandomFrame) {
			currentFrame = Mathf.FloorToInt (Random.value * current.frames.Length);
		} else {
			currentFrame++;
		}
		_justSwitched = true;
        if (currentFrame >= current.frames.Length)
        {
            if (!current.loop)
            {
                _playing = false;
                return;
            }
            currentFrame = 0;
        }
        spriteRenderer.sprite = current.frames[currentFrame];
        lastFrameTime = Time.time;
    }

    #endregion
    //--------------------------------------------------------------------------------
    #region Public Methods
    public void Play(string name)
    {
        int index = animations.FindIndex(a => a.name == name);
        if (index < 0)
        {
            Debug.LogError(gameObject + ": No such animation: " + name);
        }
        else
        {
            PlayByIndex(index);
        }
    }

    public void SetSpeed(float s)
    {
        speed = s;
    }

    public void PlayByIndex(int index)
    {
        if (index < 0) return;
        Anim anim = animations[index];

        current = anim;

        secsPerFrame = 1f / anim.framesPerSec;
        currentFrame = -1;
        _playing = true;
        lastFrameTime = Time.time - secsPerFrame;
    }

    public void Stop()
    {
        _playing = false;
    }

    public void Resume()
    {
        _playing = true;
        lastFrameTime = Time.time;
    }

    #endregion
}