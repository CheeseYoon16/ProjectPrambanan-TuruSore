using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public enum SoundType
{
    BGM,
    AMBIENCE,
    UI,
    INTERACTION,
}

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    [SerializeField] SoundList[] soundList;
    static SoundManager instance;
    public static SoundManager Instance => instance;

    private AudioSource audioSource;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject); // delete duplicates if exist
            return;
        }

        instance = this;
        DontDestroyOnLoad(this); // won't be destroyed when navigating into another scene
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }


    public void PlaySound(SoundType soundType, String name)
    {
        SoundList list = instance.soundList[(int)soundType];

        foreach (var sound in list.Sounds)
        {
            if (sound.name == name)
            {
                instance.audioSource.PlayOneShot(sound.clip, sound.volume);
                return;
            }
        }

        Debug.LogWarning($"Sound \"{name}\" not found in {soundType} category.");
    }

    [Serializable]
    public class SoundName
    {
        public string name;
        public AudioClip clip;

        [Range(0, 1)]
        public float volume = 1f;
    }

    [Serializable]
    public struct SoundList
    {
        public SoundName[] Sounds { get => sounds; }
        [HideInInspector] public string categoryName;
        [SerializeField] private SoundName[] sounds;
    }

#if UNITY_EDITOR
    private void OnEnable()
    {
        string[] names = Enum.GetNames(typeof(SoundType));
        Array.Resize(ref soundList, names.Length);
        for (int i = 0; i < soundList.Length; i++)
        {
            soundList[i].categoryName = names[i];
        }
    }

#endif
}