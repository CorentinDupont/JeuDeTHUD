using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JeuDeThud.Camera
{
    public class CanvasOverlayFix : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {
            GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}

