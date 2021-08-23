using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public static class TagDrawerUtility
{
    public static string TagPopup(Rect rect, string current, GUIContent label)
    {
        //generate the taglist + custom tags
        List<string> tagList = new List<string>();

        tagList.Add("<NoTag>");
        tagList.AddRange(UnityEditorInternal.InternalEditorUtility.tags);

        int index = -1;

        if (current == "")
        {
            //The tag is empty
            index = 0; //first index is the special <notag> entry
        }
        else
        {
            // Find the index of the current tag
            index = tagList.FindIndex(x => x == current);
        }

        //Draw the popup box with the current selected index
        index = EditorGUI.Popup(rect, label.text, index, tagList.ToArray());

        //Adjust the actual string value of the property based on the selection
        if (index == 0)
        {
            return "";
        }
        else if (index >= 1)
        {
            return tagList[index];
        }
        else
        {
            return "";
        }
    }
}