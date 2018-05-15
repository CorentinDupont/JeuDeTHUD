using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JeuDeThud.Sound
{
    [RequireComponent(typeof(Button))]
    public class ClicSound : MonoBehaviour
    {

        public AudioClip sound;
        public AudioSource audioSource;

        private Button button { get { return GetComponent<Button>(); } }



        // Use this for initialization
        void Start()
        {
            gameObject.AddComponent<AudioSource>();
            audioSource.clip = sound;
            audioSource.playOnAwake = false;

            button.onClick.AddListener(() => PlaySound());
        }

        public void PlaySound()
        {
            audioSource.PlayOneShot(sound);
        }
    }
}


