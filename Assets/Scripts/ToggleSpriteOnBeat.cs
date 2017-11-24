using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleSpriteOnBeat : AudioEventListener {

	SpriteRenderer sprite;

	// Use this for initialization
	void Start () {
		sprite = GetComponent<SpriteRenderer> ();
		sprite.enabled = false;
	}

	bool showing = false;
	float showTime;
	public float TimeToShow = 1f;
	public override void Trigger (string key) {
		sprite.enabled = true;
		showing = true;
		showTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		if (showing) {
			if (Time.time >= showTime + TimeToShow) {
				showing = false;
				sprite.enabled = false;
			}
		}
	}
}
