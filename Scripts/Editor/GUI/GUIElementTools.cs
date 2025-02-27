using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UIElements;

namespace EditorTools.InspectorTools
{
    public class GUIElementTools
    {
        /// <summary>
        /// Adds all elements to the provided element
        /// </summary>
        /// <param name="element">Element to add the elements to</param>
        /// <param name="elements">Elements to add to element</param>
        public static void addAllToElement(VisualElement element, VisualElement[] elements)
        {
            foreach (VisualElement e in elements)
            {
                element.contentContainer.Add(e);
            }
        }

        /// <summary>
        /// Gets a list of all choosable scenes
        /// </summary>
        /// <returns>A list of all scenes in the build settings</returns>
        public static List<string> getSceneList()
        {
            List<string> scenes = new List<string>();
            
            foreach(var scene in EditorBuildSettings.scenes)
            {
                if(scene.enabled)
                    scenes.Add(scene.path);
            }

            return scenes;
        }
    }
}