#if UNITY_EDITOR
using System.IO;
using UnityEditor;

namespace LazyProger.ScenesConstantNames
{
    public static class ScenesStorage
    {
        public static void Save(string sourceCode)
        {
            var path = GetFilePath();

            Directory.CreateDirectory(ScenesSettings.SavingDirectory);

            if (File.Exists(path) && File.ReadAllText(path) == sourceCode)
            {
                return;
            }

            File.WriteAllText(path, sourceCode);
            AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
        }

        public static void Delete()
        {
            var path = GetFilePath();

            if (File.Exists(path))
            {
                AssetDatabase.DeleteAsset(path);
                AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
            }
        }

        private static string GetFilePath()
        {
            return ScenesSettings.SavingDirectory + ScenesSettings.ClassName + ScenesSettings.ClassExtension;
        }
    }
}
#endif
