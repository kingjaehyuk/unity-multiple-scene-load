using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Jaehyuk.SceneManagement
{
    public class SceneManager : MonoBehaviour
    {
        public static string[] nextSceneNames;

        private AsyncOperation[] op;

        [SerializeField]
        private Image progressBar = null;

        [SerializeField]
        private float totalProgress = 0f;

        public static void LoadScenes(string[] sceneNames)
        {
            nextSceneNames = sceneNames;
            UnityEngine.SceneManagement.SceneManager.LoadScene("Loading");
        }

        private void Awake()
        {
            progressBar.fillAmount = 0f;

            op = new AsyncOperation[nextSceneNames.Length];

            for (int i = 0; i < nextSceneNames.Length; i++)
            {
                StartCoroutine(LoadScene(nextSceneNames[i], i));
            }
        }

        private void UnloadScene()
        {
            UnityEngine.SceneManagement.SceneManager.SetActiveScene(UnityEngine.SceneManagement.SceneManager.GetSceneAt(1));
            AsyncOperation op = UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(UnityEngine.SceneManagement.SceneManager.GetSceneAt(0));
        }

        private IEnumerator LoadScene(string sceneName, int index)
        {
            yield return null;

            op[index] = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            op[index].allowSceneActivation = false;

            float timer = 0f;
            float progress = 0f;

            while (op[index].progress != 0.9f)
            {
                yield return null;

                timer += Time.deltaTime;

                for (int j = 0; j < op.Length; j++)
                {
                    progress += op[j].progress;
                }

                progress = progress / op.Length;

                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, totalProgress + progress, Time.deltaTime);

                if (progressBar.fillAmount >= progress)
                {
                    timer = 0f;
                }

            }

            timer = 0f;

            totalProgress += 1f / op.Length;

            while (progressBar.fillAmount < totalProgress)
            {
                yield return null;

                timer += Time.deltaTime;

                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, totalProgress, timer);
            }

            Debug.Log("Loading " + index + " : " + sceneName);

            if (index == op.Length - 1)
            {
                for (int i = 0; i < op.Length; i++)
                {
                    op[i].allowSceneActivation = true;
                }

                while (!op[0].isDone)
                {
                    yield return null;

                    if (op[0].progress == 1.0f)
                    {
                        UnloadScene();
                    }
                }

                Debug.Log("Loading Success");
            }
        }
    }
}
