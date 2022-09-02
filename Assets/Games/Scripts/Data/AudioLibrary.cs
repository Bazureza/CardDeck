using System.Collections.Generic;
using UnityEngine;

namespace GuraGames.Data
{
    public class AudioLibrary : MonoBehaviour
    {
        [SerializeField] private List<ClipData> clips = new List<ClipData>();
        private Dictionary<string, ClipData> dictionaries_clip = new Dictionary<string, ClipData>();

        public void InitDictionaries()
        {
            dictionaries_clip.Clear();
            foreach (ClipData clip in clips)
            {
                dictionaries_clip.Add(clip.clip_id, clip);
            }
        }

        public AudioClip GetClip(string clip_id)
        {
            return dictionaries_clip.ContainsKey(clip_id) ? dictionaries_clip[clip_id].clip : null;
        }

        [System.Serializable]
        public class ClipData
        {
            public string clip_id;
            public AudioClip clip;
        }
    }
}