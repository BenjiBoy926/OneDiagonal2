using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class FadeInPanel : MonoBehaviour
{
    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Image to change the color on for the panel")]
    private Image image;
    [SerializeField]
    [Tooltip("Time it takes for the panel to fade away")]
    private float fadeTime = 0.3f;
    #endregion

    #region Private Fields
    private static string resourcePath = nameof(FadeInPanel);
    private static bool callbackIsAdded = false;
    #endregion

    #region Monobehaviour Messages
    private void Start()
    {
        image.DOColor(Color.clear, fadeTime).OnComplete(() => Destroy(gameObject));
    }
    #endregion

    #region Private Methods
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void SetupCallback()
    {
        if(!callbackIsAdded)
        {
            SceneManager.sceneLoaded += CreateFromResource;
            callbackIsAdded = true;
        }
    }
    private static void CreateFromResource(Scene scene, LoadSceneMode mode)
    {
        GameObject[] roots = scene.GetRootGameObjects();

        if(roots.Length > 0)
        {
            // Load the panel from resources
            GameObject root = roots[0];
            GameObject panel = Resources.Load<GameObject>(resourcePath);

            // If we successfully loaded the panel then create it under the root
            if(panel)
            {
                Instantiate(panel, root.transform);
                panel.SetActive(true);
            }
            // Log warning if no resource is found
            else
            {
                Debug.LogWarning(nameof(FadeInPanel) + ": could not find any GameObject in any Resources folder at the path '" + 
                    resourcePath + "'");
            }
        }
    }
    #endregion
}
