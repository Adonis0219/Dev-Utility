using UnityEngine;

public static class TransformExtensions
{
    public static void ResetTransform(this Transform transform)
    {
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
        transform.localScale = Vector3.one;
    }
}

public static class GameObjectExtensions
{
    // Recursive : Àç±ÍÀû
    public static void SetActiveRecursive(this GameObject gameObject, bool isActive)
    {
        gameObject.SetActive(isActive);

        foreach (Transform child in gameObject.transform)
        {
            child.gameObject.SetActiveRecursive(isActive);
        }
    }
}