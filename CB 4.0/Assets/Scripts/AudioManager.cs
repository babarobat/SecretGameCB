using UnityEngine.Audio;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour {
    public Sound[] sounds;
	// Use this for initialization

	void Awake ()
    {
		foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.outputAudioMixerGroup = s.output;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.spatialBlend = s.spatialBlend;
        }
	}
    void Start()
    {
        
        Play("Main_Theme_2");
    }
    void Update()
    {
        if (PouseMenu.GameIsPaused)
        {
            foreach (var s in sounds)
            {
                s.source.pitch = 0.5f;
            }
        }
        if (!PouseMenu.GameIsPaused)
        {
            foreach (var s in sounds)
            {
                s.source.pitch = 1f;
            }

        }
    }

    public void Play (string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        
        s.source.Play();

        
        
    }
    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Stop();
    }
    
    
}
