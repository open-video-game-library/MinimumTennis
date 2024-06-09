#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

public class MissingDelete
{
    // シーン中に存在するMissingなスクリプトを削除する
    [MenuItem("Project/MissingDelete/Scene", false, 3)]
    public static void DeleteInScene()
    {
        GameObject[] all = Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[];
        for (int i = 0; i < all.Length; ++i)
        {
            GameObjectUtility.RemoveMonoBehavioursWithMissingScript(all[i]);
        }
        Debug.Log("シーン中のMissingなスクリプトの削除が完了しました");
    }
    // プレハブに含まれるMissingなスクリプトを削除する
    [MenuItem("Project/MissingDelete/Prefab", false, 3)]
    public static void DeleteInPrefab()
    {
        string[] allGUID = AssetDatabase.FindAssets("t:prefab");
        for (int i = 0; i < allGUID.Length; ++i)
        {
            string path = AssetDatabase.GUIDToAssetPath(allGUID[i]);
            GameObject g = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            GameObjectUtility.RemoveMonoBehavioursWithMissingScript(g);
        }
        AssetDatabase.Refresh();
        Debug.Log("全プレハブ中のMissingなスクリプトの削除が完了しました");
    }
}
#endif