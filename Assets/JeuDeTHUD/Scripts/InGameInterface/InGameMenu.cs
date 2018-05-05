using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameMenu : MonoBehaviour {

	public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
