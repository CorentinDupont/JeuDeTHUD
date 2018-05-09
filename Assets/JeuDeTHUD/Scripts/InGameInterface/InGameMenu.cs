using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameMenu : MonoBehaviour {
    private void Start()
    {
        foreach (Transform boardBoxTransform in this.gameObject.transform)
        {
            GameObject theBoardBox = boardBoxTransform.gameObject;
        }
    }
}
