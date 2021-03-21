using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
[CustomEditor(typeof (Test))]
public class EditorTest : Editor {
    class DictionaryEditorWindow : EditorWindow {  
        public Dictionary<string, string> d;
        string l = "";
        string v = "";
        Dictionary<string, bool> editMode;
        Dictionary<string, string> tempName;
        int counter = 1;
        void OnGUI() {
            if (editMode == null) editMode = new Dictionary<string, bool>();
            if (tempName == null) tempName = new Dictionary<string, string>();
            if (d == null) {
                Close();
                return;
            }
            l = EditorGUILayout.TextField("Key", l);
            GUILayout.Label("Value");
            v = EditorGUILayout.TextArea(v);
            EditorGUILayout.BeginHorizontal();
            if (d.ContainsKey(l)) {
                if (GUILayout.Button("Edit")) d[l] = v;
                if (v.Length < 1) {
                    if (GUILayout.Button("Remove")) d.Remove(l);
                }
            } else {
                if (GUILayout.Button("Add")) {
                    d[l] = v;
                    l = $"{l} ({counter})";
                    counter++;
                }
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Separator();
            string[] keys = new string[d.Keys.Count];
            d.Keys.CopyTo(keys, 0);
            foreach (string key in keys) {
                if (!editMode.ContainsKey(key)) editMode[key] = false;
                if (!tempName.ContainsKey(key)) tempName[key] = key;
                string v = d[key];
                editMode[key] = EditorGUILayout.BeginFoldoutHeaderGroup(editMode[key], tempName[key]);
                //if (editMode[key]) {
                    //EditorGUILayout.EndHorizontal();
                    if (editMode[key]) {
                        EditorGUILayout.BeginHorizontal();
                        tempName[key] = EditorGUILayout.TextField(tempName[key]);
                        if (GUILayout.Button("Remove")) {
                            d.Remove(key);
                            OnGUI();
                            break;
                        }
                        EditorGUILayout.EndHorizontal();
                        v = EditorGUILayout.TextArea(v);
                            if (d[key] != v) {
                                d[key] = v;
                            }
                        if (GUILayout.Button("Apply")) {
                            if (tempName[key] != key) {
                                if (d.ContainsKey(tempName[key])) tempName[key] = key;
                                d.Remove(key);
                                d[tempName[key]] = v;
                                tempName.Remove(key);
                                OnGUI();
                                break;
                            }
                        }
                    }
                    /*
                    if (GUILayout.Button("Cancel")) {
                        editMode[key] = false;
                    }*/
                    //EditorGUILayout.BeginHorizontal();
                //} else {
                    /*if (GUILayout.Button("Edit")) {
                        editMode[key] = true;
                    }*/
                //}
                EditorGUILayout.EndFoldoutHeaderGroup();
                if (editMode[key]) EditorGUILayout.Separator();
            }
        }
    }
    public override void OnInspectorGUI() {
        GUILayout.Label("No");
        if (GUILayout.Button("Edit")) {
            var w = ScriptableObject.CreateInstance<DictionaryEditorWindow>();
            if (((Test)target).dict == null) ((Test)target).dict = new Dictionary<string, string>();
            w.d = ((Test)target).dict;
            w.Show();
        }
    }
    public static void EditDictionary(Dictionary<string, string> dict) {
        var w = ScriptableObject.CreateInstance<DictionaryEditorWindow>();
        w.d = dict;
        w.Show();
    }
}