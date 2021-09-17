using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class TutorialSettings : ScriptableObjectSingleton<TutorialSettings>
{
    #region Private Properties
    private static TutorialSettings Instance => GetOrCreateInstance(nameof(TutorialSettings), nameof(TutorialSettings));
    #endregion

    #region Private Editor Fields
    [SerializeField]
    [Tooltip("List of tutorials")]
    private LevelTutorialData[] tutorials;
    #endregion

    #region Public Methods
    public static TutorialData[] GetTutorialsForLevel(LevelID level)
    {
        LevelTutorialData[] matches = Instance.tutorials.Where(x => level == x.Level).ToArray();

        if (matches.Length > 0)
        {
            if(matches.Length > 1)
            {
                Debug.LogWarning(nameof(TutorialSettings) + ": you have multiple tutorials setup with the level id '" +
                    level.ToString() + "', which is not supported. You should collapse the tutorials into a single list");
            }
            return matches[0].Tutorials;
        }
        else return null;
    }
    #endregion
}
