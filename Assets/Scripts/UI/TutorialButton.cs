using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialButton : MonoBehaviour
{
    #region Public Properties
    public TutorialManager TutorialManager
    {
        get => tutorialManager;
        set => tutorialManager = value;
    }
    public Button Button => button;
    public TutorialData Tutorial
    {
        get => tutorial;
        set
        {
            tutorial = value;
            textComponent.text = value.Title;
        }
    }
    #endregion

    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Tutorial manager used to display the tutorial")]
    private TutorialManager tutorialManager;
    [SerializeField]
    [Tooltip("Button the shows the tutorial given on the tutorial manager")]
    private Button button;
    [SerializeField]
    [Tooltip("Tutorial to display on the tutorial manager")]
    private TutorialData tutorial;
    [SerializeField]
    [Tooltip("Component used to display the title of the tutorial that is added")]
    private TextMeshProUGUI textComponent;
    #endregion

    #region Public Methods
    public void OpenTutorial()
    {
        tutorialManager.OpenTutorial(tutorial, false);
    }
    #endregion

    #region Monobehaviour Messages
    private void Start()
    {
        textComponent.text = tutorial.Title;
    }
    private void OnEnable()
    {
        button.onClick.AddListener(OpenTutorial);
    }
    private void OnDisable()
    {
        button.onClick.RemoveListener(OpenTutorial);
    }
    #endregion
}
