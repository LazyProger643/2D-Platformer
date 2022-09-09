#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using System.Collections.Generic;

namespace LazyProger.ScenesConstantNames
{
    public class ScenePostprocessor : AssetPostprocessor
    {
        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            var sceneNameList = new List<string>();
            bool wasSceneListChanged = false;

            foreach (var importedAsset in importedAssets)
            {
                if (Path.GetExtension(importedAsset) == ScenesSettings.SceneExtension)
                {
                    wasSceneListChanged = true;
                    break;
                }
            }

            foreach (var deletedAsset in deletedAssets)
            {
                if (Path.GetExtension(deletedAsset) == ScenesSettings.SceneExtension)
                {
                    wasSceneListChanged = true;
                    break;
                }
            }

            for (var i = 0; i < movedAssets.Length; i++)
            {
                if (Path.GetExtension(movedAssets[i]) == ScenesSettings.SceneExtension)
                {
                    wasSceneListChanged = true;
                    break;
                }
            }

            if (wasSceneListChanged)
            {
                ScenesStorage.Delete();

                foreach (var guid in AssetDatabase.FindAssets("t:SceneAsset"))
                {
                    sceneNameList.Add(Path.GetFileNameWithoutExtension(AssetDatabase.GUIDToAssetPath(guid)));
                }
            }

            if (sceneNameList.Count > 0)
            {
                ScenesStorage.Save(ScenesGenerator.Generate(sceneNameList));
            }
        }
    }
}
#endif