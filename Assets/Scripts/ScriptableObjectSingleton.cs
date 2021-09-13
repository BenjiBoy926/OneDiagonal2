using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptableObjectSingleton<BaseType> : ScriptableObject where BaseType : ScriptableObject
{
    #region Public Properties
    public static string ObjectName => nameof(BaseType);
    public static string ObjectPath => "Resources/" + ObjectName;
    #endregion

    #region Protected Fields
    protected static BaseType Instance
    {
        get
        {
            if (!instance)
            {
                // Load the resource
                instance = Resources.Load<BaseType>(ObjectPath);

                // If the instance still could not be loaded then throw an exception
                if (!instance) throw new MissingReferenceException(
                    nameof(LevelSettings) +  ": no instance of type " + nameof(LevelSettings) +
                    " could be loaded from the resources folder. Make sure an instance of type " +
                    nameof(LevelSettings) + " with the name " + ObjectName +
                    " exists in the assets folder at the path: " + ObjectPath);
            }
            // If instance is not null return it
            return instance;
        }
    }
    #endregion

    #region Private Fields
    private static BaseType instance = null;
    #endregion
}
