using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SquiggleController : BeatListenerBlank {
	[System.Serializable]
	public class Animation {
		public string Name;
		public Sprite[] Frames;
		public int Length = 10;
		public float fps = 30f;
	}

	public List<Animation> Animations = new List<Animation>();
	private float frameChangeTimer = 0f;
	private bool playing = false;
	private int currentAnimationIndex = -1;
	private int[] frameList;
	private int frameIndex;
	private SpriteRenderer sprite;

	void PlayAnimation(string name) {
		for (int i = 0; i < Animations.Count; i++) {
			if (Animations [i].Name == name) {
				frameChangeTimer = 0f;
				currentAnimationIndex = i;
				playing = true;

				// Generate a frame list.
				frameList = new int[Animations[i].Length];
				int f = 0;
				for (int j = 0; j < frameList.Length; j++) {
					frameList [j] = f;
					f++;
					if (f > Animations[i].Frames.Length - 1) {
						f = 0;
					}
				}
				frameList = frameList.OrderBy (x => Random.value).ToArray ();
				frameIndex = 0;
				return;
			}
		}
	}

	public override void Trigger (string key) {
		PlayAnimation (key);
	}

	// Use this for initialization
	void Start () {
		sprite = GetComponent<SpriteRenderer> ();
		sprite.enabled = false;
	}

	bool SetFrame() {
		if (frameIndex < frameList.Length) {
			if (!sprite.enabled) {
				sprite.enabled = true;
			}
			sprite.sprite = Animations [currentAnimationIndex].Frames [frameList[frameIndex]];
			return true;
		} else {
			if (sprite.enabled) {
				sprite.enabled = false;
			}
			return false;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (playing) {
			frameChangeTimer += Time.deltaTime;
			float frameTime = 1f / Animations [currentAnimationIndex].fps;
			while (frameChangeTimer >= frameTime) {
				frameChangeTimer -= frameTime;
				frameIndex++;
				playing = SetFrame ();
			}
		}
	}
}
