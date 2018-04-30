using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour {
    private DebugLogComponent debugLogComponent;

    private void Start()
    {
        debugLogComponent = GetComponent<DebugLogComponent>();
    }

    public void QuitGame()
    {
        Application.Quit();
        debugLogComponent.DebugMessage("Quit", true);
    }
}
