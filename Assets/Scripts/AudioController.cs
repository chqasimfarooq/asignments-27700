using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable] // Change to Serializable to allow this class to be serialized in the inspector.
public class Sound
{
    public string name;
    public AudioClip clip; // Corrected AudioClip spelling
    public bool loop;
    private AudioSource Source;

    public void SetSource(AudioSource source)
    {
        Source = source;
        Source.clip = clip;
    }

    public void Play()
    {
        Source.loop = loop;
        Source.Play();
    }

    public void Stop()
    {
        Source.Stop();
    }
}

public class AudioController : MonoBehaviour
{
    public static AudioController audioControler;
    [SerializeField]
    private Sound[] sounds;

    private void Awake()
    {
        if (audioControler == null)
        {
            DontDestroyOnLoad(gameObject);
            audioControler = this;
        }
        else if (audioControler != this)
        {
            Destroy(gameObject);
        }

        for (int i = 0; i < sounds.Length; i++)
        {
            GameObject go = new GameObject("sound_" + i + "_" + sounds[i].name);
            go.transform.SetParent(this.transform);
            sounds[i].SetSource(go.AddComponent<AudioSource>());
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Additional setup if needed
    }

    // Update is called once per frame
    void Update()
    {
        // Additional logic if needed
    }
    public void PlaySound(string name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name == name)
            {
                sounds[i].Play();
                return;
            }
        }
    }

    public void PlayBGMSound(string name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name == name)
            {
                sounds[i].Play();
                return;
            }
        }
    }

    public void StopSound(string name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name == name)
            {
                sounds[i].Stop();
                return;
            }
        }
    }

    public void StopAllSound()
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            sounds[i].Stop();
            return;
        }
    }
}
