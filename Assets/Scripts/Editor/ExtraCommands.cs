using UnityEditor;
using UnityEngine;
using System.Collections;

public static class ExtraCommands
{
    [MenuItem("GameObject/Zero Parent Position Without Affecting Chldren")]
    public static void ZeroParentPosition()
    {
        var parent = Selection.activeTransform;
        var children = new Transform[parent.childCount];
        for (int i = 0; i < children.Length; i++) {
            children[i] = parent.GetChild(i);
        }

        var parentOffset = parent.localPosition;
        for (int i = 0; i < children.Length; i++) {
            children[i].localPosition += parentOffset;
        }

        parent.localPosition -= parentOffset;
    }
}