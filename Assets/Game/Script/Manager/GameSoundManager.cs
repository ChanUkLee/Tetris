using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameSoundManager : Singleton<GameSoundManager> {

	public int _maxEffectCount = 50; 

	private const float _minDistnace = 10.0f;
	private const float _maxDistance = 30.0f;

	private AudioSource _bgmSource = null;
	private AudioSource[] _effectSource = null;

	private bool _bgmOn = true;
	public bool BGMOn {
		get {
			return this._bgmOn;
		}
	}

	private bool _effectOn = true;
	public bool EffectOn {
		get {
			return this._effectOn;
		}
	}

	private float _bgmVolume = 1f;
	public float BGMVolume {
		get {
			return this._bgmVolume;
		}
	}

	private float _effectVolume = 1f;
	public float EffectVolume {
		get {
			return this._effectVolume;
		}
	}

	private void Initialize () {
		this._bgmSource = this.gameObject.AddComponent<AudioSource> ();

		this._effectSource = new AudioSource[this._maxEffectCount];
		for (int i = 0; i < this._effectSource.Length; i++) {
			GameObject instant = new GameObject ("EffectSource");
			instant.transform.SetParent (this.gameObject.transform);
			this._effectSource[i] = instant.AddComponent <AudioSource> ();
			this._effectSource[i].playOnAwake = false;
		}
	}

	public void SetBGM(bool enable) {
		this._bgmOn = enable;

		if (this._bgmOn == true) {
			if (this._bgmSource.clip != null) {
				this._bgmSource.Play ();
			}
		} else {
			if (this._bgmSource.isPlaying == true) {
				this._bgmSource.Stop ();
			}
		}
	}

	public void SetEffect(bool enable) {
		this._effectOn = enable;

		if (this._bgmOn == false ) {
			for (int i = 0; i < this._effectSource.Length; i++) {
				if (this._effectSource[i].isPlaying == true) {
					this._effectSource[i].Stop ();
				}
			}
		}
	}

	public void SetBGMVolume(float volume) {
		this._bgmVolume = volume;

		if (this._bgmOn == true && this._bgmSource.isPlaying == true) {
			this._bgmSource.volume = this._bgmVolume;
		}
	}

	public void SetEffectVolume(float volume) {
		this._effectVolume = volume;

		for (int i = 0; i < this._effectSource.Length; i++) {
			if (this._effectSource[i].isPlaying == true) {
				this._effectSource[i].volume = this._effectVolume;
			}
		}
	}

	public void PlayBGM(string clipName, bool loop) {
		AudioClip clip = GameResourceManager.Instance.GetAudioClip (clipName);
		if (clip != null) {
			if (this._bgmSource.clip != null && this._bgmSource.clip.name.Equals (clip.name) == true) {
				if (this._bgmSource.isPlaying == true) {
				} else {
					this._bgmSource.volume = this._bgmVolume;
					this._bgmSource.Play ();
				}
				return;
			}
			
			this._bgmSource.Stop ();
			this._bgmSource.clip = clip;
			//this._bgmSource.minDistance = _minDistnace;
			//this._bgmSource.maxDistance = _maxDistance;
			this._bgmSource.volume = this._bgmVolume;
			this._bgmSource.loop = loop;
			this._bgmSource.playOnAwake = false;
			
			if (this._bgmOn == true) {
				this._bgmSource.Play ();
			}
		}
	}

	public void StopBGM() {
		this._bgmSource.Stop ();
	}

	public void PlayEffect(string clipName, bool loop) {
		AudioClip clip = GameResourceManager.Instance.GetAudioClip (clipName);
		if (clip != null) {
			for (int i = 0; i < this._effectSource.Length; i++) {
				if (this._effectSource[i].isPlaying == false) {
					this._effectSource[i].clip = clip;
					//this._effectSource[i].minDistance = _minDistnace;
					//this._effectSource[i].maxDistance = _maxDistance;
					this._effectSource[i].volume = this._effectVolume;
					this._effectSource[i].loop = loop;
					
					if (this._effectOn == true) {
						this._effectSource[i].Play ();
					}
					return;
				}
			}

			GameDebug.Log ("effect audio source is full");
		}
	}

	public void StopEffect(string clipName) {
		for (int i = 0; i < this._effectSource.Length; i++) {
			if (this._effectSource[i].isPlaying == true && this._effectSource[i].clip.name.Equals(clipName) == true) {
				this._effectSource[i].Stop ();
			}
		}
	}

	public void StopEffect() {
		for (int i = 0; i < this._effectSource.Length; i++) {
			if (this._effectSource[i].isPlaying == true) {
				this._effectSource[i].Stop ();
			}
		}
	}
}