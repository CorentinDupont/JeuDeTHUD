using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using JeuDeThud.Util;

namespace JeuDeThud.Battle.UI
{
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(DebugLogComponent))]

    public class TurnBanner : MonoBehaviour
    {

        RectTransform RectTransform { get { return GetComponent<RectTransform>(); } }
        DebugLogComponent DebugLog { get { return GetComponent<DebugLogComponent>(); } }

        private void OnEnable()
        {
            //Initialize image position
            RectTransform.anchoredPosition = new Vector3(-RectTransform.rect.width, 0, 0);
            DebugLog.DebugMessage("Enable Banner !", true);
            //Launch Animation
            StartCoroutine(TransitionRightToCenterToLeft(RectTransform.anchoredPosition, Time.time));
        }

        private void Update()
        {

        }

        IEnumerator TransitionRightToCenterToLeft(Vector3 startPosition, float startTime)
        {

            //while x position is not approximativaly 0
            while (!(RectTransform.anchoredPosition.x < 1 && RectTransform.anchoredPosition.x > -1))
            {
                RectTransform.anchoredPosition = Vector3.Lerp(startPosition, new Vector3(0, 0, 0), (Time.time - startTime) / 0.5f);
                yield return null;
            }


            //Wait at center for 1 second
            yield return new WaitForSeconds(1);

            //Update paramaters before new translation
            startPosition = RectTransform.anchoredPosition;
            startTime = Time.time;

            //while x position is not approximativaly the image width
            while (!(RectTransform.anchoredPosition.x < RectTransform.rect.width + 1 && RectTransform.anchoredPosition.x > RectTransform.rect.width - 1))
            {
                RectTransform.anchoredPosition = Vector3.Lerp(startPosition, new Vector3(RectTransform.rect.width, 0, 0), (Time.time - startTime) / 0.5f);
                yield return null;
            }

            gameObject.SetActive(false);
        }

        IEnumerator WaitForSeconds(float second)
        {
            yield return new WaitForSeconds(second);
        }
    }
}

