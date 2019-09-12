using UnityEngine.Audio;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    
    public Audio[] audios;
    public static AudioManager cur;
    private Scene scene;
    void Awake()
    {
        if (cur == null)
        {
            cur = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
            DontDestroyOnLoad(gameObject);
        foreach (Audio s in audios)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.pitch = s.pitch;
            s.source.volume = s.volume;
            s.source.loop = s.loop;
            s.source.mute = s.mute;
            s.source.outputAudioMixerGroup = s.mixer;
        }
    }
    private void Start()
    {
        Play("BGM1");
        Play("BGM2");
        Play("BGM3");
        Play("BGM4");
        Play("PlayerRun");
    }

    //    void Start()
    //{
    //    Debug.Log("Start");
    //    Play("BGM1");
    //    Play("BGM2");
    //    Play("BGM3");
    //    Play("BGM4");
    //    Play("PlayerRun");
    //}
    public void Mute(string name)
    {

        Audio s = Array.Find(audios, sound => sound.name == name);
        if (s == null)
        {
            //Debug.Log(name + "没找到啊");
            //return;
        }
        if (!s.source.mute)

            s.source.mute = true;
    }

    public void UnMute(string name)
    {

        Audio s = Array.Find(audios, sound => sound.name == name);
        if (s == null)
        {
            //Debug.Log(name + "没找到啊");
            //return;
        }
        if (s.source.mute)
            s.source.mute = false;
    }

    public void Play(string name)
    {

        Audio s = Array.Find(audios, sound => sound.name == name);
        if (s == null)
        {
            //Debug.Log(name + "没找到啊");
            //return;
        }
        //Debug.Log(name + "找到了");
        s.source.Play();
        
    }
    public void Stop(string name)
    {

        Audio s = Array.Find(audios, sound => sound.name == name);
        if (s == null)
        {
            //Debug.Log(name + "没找到啊");
            //return;
        }
        //Debug.Log(name + "找到了");
        s.source.Stop();

    }

}
