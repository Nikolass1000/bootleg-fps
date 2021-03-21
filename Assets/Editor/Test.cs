using UnityEngine;
using System.Collections.Generic;
[CreateAssetMenu( fileName = "Yes", menuName = "No/Yes" )]
public class Test : ScriptableObject {
    public Dictionary<string, string> dict;
    [ContextMenu("Do the thing")]
    void CreateDictionary() {
        dict = new Dictionary<string, string>();
    }
}