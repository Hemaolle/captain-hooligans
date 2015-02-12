using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Code from Cuong Le: https://github.com/sheepbeo
public class SoundEffectManager : MonoBehaviour {
	public static SoundEffectManager instance;
	
	public AudioClip[] audios;
	public string[] audioClipKeys;
	public float[] audioClipCooldownTimes;
	public float globalCoolDownTime = 2.0f;
	
	[Range(0f,1f)]
	public float[] audioVolumes;
		
	private Dictionary<string, AudioClip> _audioClipKeys;
	private Dictionary<string, bool> _audioPlayingStatuses;
	private Dictionary<string, float> _audioTimers;
	private Dictionary<string, float> _audioVolumes;
	
	void Start () {
		if (instance == null) {
			instance = this;
		}
		
		_audioClipKeys = new Dictionary<string, AudioClip>();
		_audioPlayingStatuses = new Dictionary<string, bool>();
		_audioTimers = new Dictionary<string, float>();
		_audioVolumes = new Dictionary<string, float>();
		
		for (int i=0; i<audioClipKeys.Length; i++) {
			if (!_audioClipKeys.ContainsKey(audioClipKeys[i])) {
				_audioClipKeys.Add(audioClipKeys[i], audios[i]);
				_audioPlayingStatuses.Add(audioClipKeys[i], false);
				_audioTimers.Add(audioClipKeys[i], 0f);
				_audioVolumes.Add(audioClipKeys[i], audioVolumes[i]);
			}
		}
	}
	
	public static void playSoundEffectOnce(string key) {
		if (instance._audioClipKeys.ContainsKey(key) && instance._audioPlayingStatuses[key] == false) {
			playSoundEffect(key);
			instance._audioPlayingStatuses[key] = true;
		}
	}
	
	public static void playSoundEffectOnce(string key, Vector3 position) {
		if (instance._audioClipKeys.ContainsKey(key) && instance._audioPlayingStatuses[key] == false) {
			
			playSoundEffect(key, position);
			instance._audioPlayingStatuses[key] = true;
		}
	}
	
	public static void playSoundEffect(string key) {
		AudioSource.PlayClipAtPoint(instance._audioClipKeys[key], Vector3.zero, instance._audioVolumes[key]);
	}
	
	public static void playSoundEffect(string key, Vector3 position) {
		AudioSource.PlayClipAtPoint(instance._audioClipKeys[key], position, instance._audioVolumes[key]);
	}
	
	private static void playSoundEffect(string key, Vector3 position, float volume) {
		AudioSource.PlayClipAtPoint(instance._audioClipKeys[key], position, volume);
	}
	
	void Update() {
		for (int i=0; i<audioClipKeys.Length; i++) {
			string key = audioClipKeys[i];
			bool audioStatus = _audioPlayingStatuses[key];
			if (!audioStatus)
				continue;
			float audioTimer = _audioTimers[key];
			float audioCooldown = audioClipCooldownTimes[i];
			
			checkAndReset(ref audioStatus, ref audioTimer, ref audioCooldown);
			
			_audioPlayingStatuses[key] = audioStatus;
			_audioTimers[key] = audioTimer;
		}
		
	}
	
	private void checkAndReset(ref bool value, ref float timer, ref float resetTime) {
		if (value && timer > resetTime) {
			value = false;
			timer = 0f;
		} else if (value) {
			timer += Time.deltaTime;
		}
	}
	/**/
}