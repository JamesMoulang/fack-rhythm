using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This is a class definition.
// : AudioEventListener means that SpriteLoopBeatListener is, like, an evolution from AudioEventListener.
// AudioEventListener is a class I already wrote that just listens to a beat. So, if you copy paste this code
// into your file you'll see it changes in the inspector too. Once you save.
public class SpriteLoopBeatListener : AudioEventListener {
	// This happens on the beat.
	public override void Trigger (string key) {
		
	}
}
