// (c) Copyright 'ZNE Edu' 2013. All rights reserved.

using UnityEngine;
using UnityEditor;
using System.Collections;

[CanEditMultipleObjects]
[CustomEditor(typeof(zneGifMovie))]
public class Editor_zneGifMovie : Editor
{

    SerializedObject _objects;
    SerializedProperty _prop_error;
    SerializedProperty _prop_name;

    SerializedProperty _prop_gif_asset;
    SerializedProperty _prop_filename;
    SerializedProperty _prop_load_method;
	SerializedProperty _prop_filter_mode;
	SerializedProperty _prop_wrap_mode;
    

    SerializedProperty _prop_targets;
    SerializedProperty _prop_auto_taxture;
    SerializedProperty _prop_send_message;

    SerializedProperty _prop_play_on_wake;
    SerializedProperty _prop_loop_type;
    SerializedProperty _prop_speed;

    SerializedProperty _remove_cash_on_destroy;

    void OnEnable()
    {
        _objects = new SerializedObject(targets);
        _prop_error = _objects.FindProperty("_error");
        _prop_name = _objects.FindProperty("_name");
		
        _prop_gif_asset = _objects.FindProperty("_gifInStreamingAssets");
        _prop_filename = _objects.FindProperty("_filename");
        _prop_load_method = _objects.FindProperty("_loadMethod");
		_prop_filter_mode = _objects.FindProperty("_textureFilterMode");
		_prop_wrap_mode = _objects.FindProperty("_textureWrapMode");
	
        _prop_targets = _objects.FindProperty("_targets");
        _prop_auto_taxture = _objects.FindProperty("_autoChangeTexture");
        _prop_send_message = _objects.FindProperty("_sendMessageOnFrame");
		
        _prop_play_on_wake = _objects.FindProperty("_playOnAwake");
        _prop_loop_type = _objects.FindProperty("_loopType");
        _prop_speed = _objects.FindProperty("_speed");

        _remove_cash_on_destroy = _objects.FindProperty("_removeCashOnDestroy");
    }

    public override void OnInspectorGUI()
    {
        _objects.Update();

        // Error
        if (_prop_error.stringValue != null && _prop_error.stringValue != "")
            EditorGUILayout.HelpBox(_prop_error.stringValue, MessageType.Error);

        // Name
        EditorGUILayout.PropertyField(_prop_name);

        // GIF asset setting
        EditorGUILayout.Separator();
        EditorGUILayout.PropertyField(_prop_gif_asset);
        if (_prop_filename.stringValue != null && _prop_filename.stringValue != "")
            EditorGUILayout.LabelField("Filename", _prop_filename.stringValue);
        EditorGUILayout.PropertyField(_prop_load_method);
		EditorGUILayout.PropertyField(_prop_filter_mode);
        EditorGUILayout.PropertyField(_prop_wrap_mode);
		
        // About target
        EditorGUILayout.Separator();
        EditorGUILayout.PropertyField(_prop_targets, true);
        EditorGUILayout.PropertyField(_prop_auto_taxture);
        EditorGUILayout.PropertyField(_prop_send_message);

        string[] loop_type_name = new string[] { "Once", "Loop" };

        // Play setting
        EditorGUILayout.Separator();
        EditorGUILayout.PropertyField(_prop_play_on_wake);
        _prop_loop_type.intValue = EditorGUILayout.Popup("Loop Type", _prop_loop_type.intValue, loop_type_name);
        EditorGUILayout.PropertyField(_prop_speed);

        // Remove (GIF clip) cash on destroy  
        EditorGUILayout.PropertyField(_remove_cash_on_destroy);
        

        _objects.ApplyModifiedProperties();
    }

}
