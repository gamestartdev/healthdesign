using UnityEngine;
using System.Collections;
using Newtonsoft.Json;

public class WorldState : MonoBehaviour {
    public WorldObject[] FoodsModels;
    private int nextBlockIndex;
    private string keyPrefix = "WorldState_";
    JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };

    void Start() {
        int index = 0;
        while (PlayerPrefs.HasKey(keyPrefix + index)) {
            string value = PlayerPrefs.GetString(keyPrefix + index);
            Debug.Log("Val: "+value);
            var deserializeObject = JsonConvert.DeserializeObject<WorldBlock>(value, settings);
            Debug.Log(deserializeObject);
            index++;
        }
    }

//    void OnGUI() {
//        if (GUILayout.Button("Reset")) {
//            PlayerPrefs.DeleteAll();
//        }
//    }

    public void Save(object worldBlock) {
        var json = JsonConvert.SerializeObject(worldBlock, Formatting.None, settings);
        Debug.Log("json: "+json);
        PlayerPrefs.SetString(keyPrefix + nextBlockIndex++, json);
        Debug.Log(json);
    }
}
