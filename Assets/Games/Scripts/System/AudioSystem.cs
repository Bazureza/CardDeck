using GuraGames.Data;
using System.Collections;
using System.Collections.Generic;
using TomGustin.GameDesignPattern;
using UnityEngine;

namespace GuraGames.GameSystem
{
    public class AudioSystem : Singleton<AudioSystem>
    {
        [SerializeField] private AudioLibrary audioLibrary;
        public static float BGMVolume
        {
            get { return bgm_volume; }
            set
            {
                bgm_volume = Mathf.Clamp01(value);
                if (Instance && Instance.bgm_source) Instance.bgm_source.volume = bgm_volume;
            }
        }
        public static float SFXVolume
        {
            get { return sfx_volume; }
            set
            {
                sfx_volume = Mathf.Clamp01(value);
                if (Instance && Instance.sfx_source) Instance.sfx_source.volume = sfx_volume;
            }
        }

        private static float bgm_volume;
        private static float sfx_volume;

        private AudioSource bgm_source;
        private AudioSource sfx_source;

        private void Awake()
        {
            OnInitialize();

            bgm_source = gameObject.AddComponent<AudioSource>();
            sfx_source = gameObject.AddComponent<AudioSource>();

            InitSetting();
        }

        private void InitSetting()
        {
            bgm_source.loop = true;
            sfx_source.loop = false;

            BGMVolume = 1f;
            SFXVolume = 1f;
        }

        public static void PlayBGM(string bgm_code)
        {
            if (!Instance) return;

            Instance.PlayBGM_Internal(bgm_code);
        }

        public static void PlaySFX(string sfx_code)
        {
            if (!Instance) return;

            Instance.PlaySFX_Internal(sfx_code);
        }

        private void PlayBGM_Internal(string bgm_code)
        {
            AudioClip clip = audioLibrary.GetClip(bgm_code);
            bgm_source.volume = bgm_volume;
            bgm_source.clip = clip;
            bgm_source.Play();
        }

        private void PlaySFX_Internal(string sfx_code)
        {
            AudioClip clip = audioLibrary.GetClip(sfx_code);
            sfx_source.volume = sfx_volume;
            sfx_source.PlayOneShot(clip);
        }
    }
}