using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace QbGameLib_Utils.Component.Scene
{
    public class SceneManagerComponent : MonoBehaviour
    {
        public static SceneManagerComponent Instance { get; private set; }
        public UnityEvent<string> OnSceneLoaded;
        public UnityEvent<string> OnSceneAdd;
        public UnityEvent<string> OnSceneUnload;
        [SerializeField] private QbGameLib_Utils.Component.Scene.LoaderScreen _loaderScreen;

        public QbGameLib_Utils.Component.Scene.LoaderScreen LoaderScreen => _loaderScreen;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(this);
                return;
            }
            Instance =  this;
            DontDestroyOnLoad(gameObject);
        }

        public void LoadScene(string sceneName)
        {
            StartCoroutine(LoadTargetScene(sceneName));
        }

        private IEnumerator LoadTargetScene(string sceneName)
        {
            _loaderScreen?.gameObject.SetActive(true);
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName,LoadSceneMode.Single);
            operation.allowSceneActivation = false;
            while (!operation.isDone)
            {
                LoaderScreen.SendProgress(operation.progress);
                Debug.Log(operation.progress);
                if (operation.progress >= 0.9f)
                {
                    operation.allowSceneActivation = true;
                }
                yield return null;
            }
            Debug.Log("Loading scene " + sceneName);
            _loaderScreen?.gameObject.SetActive(false);
            OnSceneLoaded.Invoke(sceneName);
        }

        public void AddScene(string sceneName)
        {
            StartCoroutine(LoadTargetAdditionScene(sceneName));
        }

        private IEnumerator LoadTargetAdditionScene(string sceneName)
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName,LoadSceneMode.Additive);
            while (!operation.isDone)
            {
                yield return null;
            }
            OnSceneAdd.Invoke(sceneName);
        }


        public void UnloadScene(string sceneName)
        {
            StartCoroutine(UnloadTargetAdditionScene(sceneName));
        }

        private IEnumerator UnloadTargetAdditionScene(string sceneName)
        {
            AsyncOperation operation = SceneManager.UnloadSceneAsync(sceneName);
            while (!operation.isDone)
            {
                yield return null;
            }
            OnSceneUnload.Invoke(sceneName);
        }
    }
}