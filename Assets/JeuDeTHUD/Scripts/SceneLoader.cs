using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace JeuDeThud
{
    public class SceneLoader : MonoBehaviour
    {

        private void Start()
        {

        }

        private bool loadScene = false;

        [SerializeField]
        private int scene;

        // Updates once per frame
        void Update()
        {
            // If the new scene has started loading...
            if (loadScene == true)
            {

                // ...then pulse the transparency of the loading text to let the player know that the computer is still working.

            }
        }

        public void LoadScene()
        {
            // ...set the loadScene boolean to true to prevent loading a new scene more than once...
            loadScene = true;

            // ...and start a coroutine that will load the desired scene.
            StartCoroutine(LoadNewScene());
        }


        // The coroutine runs on its own at the same time as Update() and takes an integer indicating which scene to load.
        IEnumerator LoadNewScene()
        {

            // This line waits for 3 seconds before executing the next line in the coroutine.
            // This line is only necessary for this demo. The scenes are so simple that they load too fast to read the "Loading..." text.
            //yield return new WaitForSeconds(3);

            // Start an asynchronous operation to load the scene that was passed to the LoadNewScene coroutine.
            AsyncOperation async = SceneManager.LoadSceneAsync(scene);

            // While the asynchronous operation to load the new scene is not yet complete, continue waiting until it's done.
            while (!async.isDone)
            {
                yield return null;
            }

        }
    }
}


