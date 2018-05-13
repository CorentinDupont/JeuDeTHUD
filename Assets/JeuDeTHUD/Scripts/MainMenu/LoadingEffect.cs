using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(RectTransform))]
public class LoadingEffect : MonoBehaviour {

    private RectTransform RectTransform { get { return GetComponent<RectTransform>(); } }
    private Image Image { get { return GetComponent<Image>(); } }
    private Color disableColor = new Color(1, 1, 1, 0);
    private Color normalColor;

    public float rotationSpeed;
    public bool isLoading;

    private void Start()
    {
        normalColor = Image.color;
    }
    // Update is called once per frame
    void Update () {
        if (isLoading)
        {
            if (Image.color != normalColor)
            {
                Image.color = normalColor;
            }
            RectTransform.Rotate(-transform.forward, rotationSpeed * Time.deltaTime);
        }
        else
        {
            if(Image.color != disableColor)
            {
                Image.color = disableColor;
            }
        }
        
	}
}
