#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;

namespace QbGameLib_Utils.Editor.Utils
{
    public static class Cached
    {
        public static int GetIndexPropertyHash(this SerializedProperty property)
        {
            if (property.serializedObject.targetObject == null) return 0;
            var spath = property.propertyPath;
            int num = property.serializedObject.targetObject.GetInstanceID() ^ spath.GetHashCode();
            if (property.propertyType == SerializedPropertyType.ObjectReference)
                num ^= property.objectReferenceInstanceIDValue;
            return num;
        }
        
        private static Dictionary<int, ReorderableList> _lstCache = new();
        private static void ClearCachedReorderableList()=>_lstCache.Clear();
        public static ReorderableList GetCachedReorderableList(SerializedProperty property)
        {
            ReorderableList reorderableList;
            int hash = property.GetIndexPropertyHash();
            if(_lstCache.Count>100) ClearCachedReorderableList();
            if (_lstCache.TryGetValue(hash, out reorderableList))
            {
                reorderableList.serializedProperty=property;
                reorderableList.list = null;
                reorderableList.drawHeaderCallback = null;
                reorderableList.drawElementCallback = null;
                reorderableList.onAddCallback = null;
                reorderableList.onRemoveCallback = null;
                reorderableList.onSelectCallback = null;
                reorderableList.onChangedCallback = null;
                reorderableList.onReorderCallback = null;
                reorderableList.onCanRemoveCallback = null;
                reorderableList.onAddDropdownCallback = null;
            }
            if (reorderableList == null)
            {
                reorderableList = new ReorderableList(property.serializedObject, property);
                _lstCache[hash] = reorderableList;
            }
            return reorderableList;
        }
    }
}
#endif