using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
namespace EHTool.LangKit {

    public class LangXMLWatcher : AssetPostprocessor {
        // 에셋이 임포트되거나 변경될 때 호출됩니다.
        // assetPath는 변경된 파일의 경로입니다.

        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            foreach (var assetPath in importedAssets)
            {
                if (!assetPath.Contains("XML/String/") && !assetPath.Contains("Json/String/")) continue;

                LangManager.Instance.UpdateData();
                Debug.Log(assetPath + " file update");
            }
        }
    }
}
#endif