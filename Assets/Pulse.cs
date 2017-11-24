using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class makes a unity GameObject pulse in time with music.
// Any behaviour that you want to sync to music has to extend AudioEventListener.
// Check out BeatListenerBlank to see how to make your own AudioEventListener.
public class Pulse : AudioEventListener {
	// The scale that the object starts at (and should return to after it expands)
	Vector3 startScale;
	// By how much do we want to increase the scale on the beat?
	// i.e. if this was 2, the object would double in size.
	// Because this is a 'public' field, we can set it in the editor.
	public float Magnitude = 1.25f;
	// How quickly do we want the object to return to its original size?
	// If this was 0, it would not move at all.
	// If this was 1, it would move return instantly.
	// Because it's set to 0.1, what happens is the scale moves 1 tenth of the distance to its original, every frame.
		// So we get a nice smooth effect.
	public float Return = 0.1f;

	// Start() is called when the object is created.
	void Start () {
		// Storing the original object scale in this variable so we know what to return to.
		startScale = transform.localScale;
	}

	// This happens on the beat.
	// It just multiplies the scale by the magnitude.
	public override void Trigger (string key) {
		transform.localScale = startScale * Magnitude;
	}

	// Update() is called once a frame.
	// Here, we just move the object's scale back to the original value.
	void Update () {
		transform.localScale = Vector3.Lerp (transform.localScale, startScale, Return);
	}
}
