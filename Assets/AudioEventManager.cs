using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.IO;
using System;

// You probably don't need to look at this class.

class Beat {
	public float start;
	public float end;
	public float length;
	public string key;

	public Beat(float start, string key) {
		this.start = start;
		this.key = key;
	}

	public void SetEnd(float end) {
		this.end = end;
	}

	public void DebugBeat() {
		Debug.Log("Beat " + key + " at delta " + start + " with length " + (end-start));
	}
}

public abstract class AudioEventListener : MonoBehaviour {
	public abstract void Trigger (string key);
	public string[] keys;
	AudioEventManager audio;
	bool active = true;

	void Subscribe() {
		audio = GameObject.Find ("Music").GetComponent<AudioEventManager> ();
		this.keys = keys;
		for (var i = 0; i < keys.Length; i++) {
			audio.Subscribe (keys[i], this);
		}
	}

	public float GetPlayTime() {
		return audio.GetPlayTime();
	}

	public float GetPlayTimeRatio() {
		return audio.GetPlayTimeRatio ();
	}

	public void _Trigger(string key) {
		if (active) {
			Trigger (key);
		}
	}

	public void Unsubscribe() {
		active = false;
//		audio.Unsubscribe (keys, this);
	}

	void Awake() {
		Subscribe ();
	}
}

public class Track {
	public string key;
	public float offset;
}

public class AudioEventManager : MonoBehaviour {
	AudioSource music;
	List<Beat> beats = new List<Beat>();
	private float delta = 0f;
	public string[] Tracks;
	public float[] Offsets;
	int beatIndex = 0;
	float flashTimer = 0f;
	private Vector3 startScale;
	public float BPM = 120;
	public float PPQ = 96;
	public float StartTime = 0f;

	private Dictionary<string, List<AudioEventListener>> listeners = new Dictionary<string, List<AudioEventListener>> ();

	// Use this for initialization
	void Start () {
		for (var i = 0; i < Tracks.Length; i++) {
			Load (Tracks [i], Offsets[i]);
			delta = 0f;
		}
		beats.Sort ((Beat x, Beat y) => x.start.CompareTo (y.start));
		string output = "";
		beats.ForEach((Beat beat) => output += beat.start + ": " + beat.key + "\n");
		Debug.Log (output);
		music.time = StartTime;
	}

	public void StartMusic() {
		if (!music.isPlaying) {
			music.Play ();
		}
	}

	public float GetPlayTime() {
		return music.time;
	}

	public float GetPlayTimeRatio() {
		return music.time / music.clip.length;
	}
	
	// Update is called once per frame
	void Update () {
//		Debug.Log ("music time is " + (music.time * 1000f));
		float t = music.time * 1000f;
		while (t > beats [beatIndex].start) {
			if (listeners.ContainsKey (beats [beatIndex].key)) {
				List<AudioEventListener> keyListeners = listeners [beats[beatIndex].key];
				for (var i = 0; i < keyListeners.Count; i++) {
					keyListeners [i]._Trigger (beats[beatIndex].key);
				}
			}
			beatIndex++;
		}
	}

	void HandleLine(string line, string key, float offset) {
		if (line.IndexOf ("delta") > -1) {
			line = line.Replace ("delta: ", "");
			delta += float.Parse (line);
		} else if (line.IndexOf ("Note") > -1) {
			if (line.IndexOf ("On") > -1) {
				// Create a beat with length 0
				beats.Add (new Beat ((60000 / (BPM * PPQ)) * delta + offset, key));
			} else if (line.IndexOf ("Off") > -1) {
				// Edit the length of the last beat.
				beats[beats.Count -1].SetEnd((60000 / (BPM * PPQ)) * delta + offset);
			}
		}
	}

	private void Load(string fileName, float offset) {
		string output = "";

		TextAsset text = Resources.Load(fileName) as TextAsset;
		string[] linesFromfile = text.text.Split("\n"[0]);
		foreach (string line in linesFromfile) {
			if (line != null) {
				output += line + "\n";
			}
		}
		//Start playing.
		music = GetComponent<AudioSource>();

		string[] lines = output.Split('\n');

		for (var l = 0; l < lines.Length; l++) {
			// Handle the line.
			string line = lines[l];
			HandleLine (line, fileName, offset);
		}
	}

	public void Subscribe(string key, AudioEventListener listener) {
		if (listeners.ContainsKey (key)) {
			listeners [key].Add (listener);
		} else {
			listeners [key] = new List<AudioEventListener> ();
			listeners [key].Add (listener);
		}
	}

	public void Unsubscribe(string[] keys, AudioEventListener listener) {
		for (var i = 0; i < keys.Length; i++) {
			string key = keys [i];
			if (listeners.ContainsKey (key)) {
				listeners [key].Remove (listener);
			}
		}
	}
}
