/// <summary>
/// 番場宥輝
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


/// <summary>
/// AudioManagerのエディタ拡張
/// </summary>
[CustomEditor(typeof(AudioManager))]
public class AudioManagerEditor : Editor {

    /// <summary>
    /// インスペクタ拡張
    /// </summary>
    public override void OnInspectorGUI()
    {
        AudioManager audioManager = target as AudioManager;
        EditorGUILayout.PropertyField(serializedObject.FindProperty("titleClip"), new GUIContent("タイトルBGM"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("MainClip"), new GUIContent("ゲームBGM"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("resultClip"), new GUIContent("リザルトBGM"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("gameOverClip"), new GUIContent("ゲームオーバーBGM"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("decideClip"), new GUIContent("決定音"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("cancelClip"), new GUIContent("キャンセルSE"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("selectClip"), new GUIContent("カーソルのセレクト音"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("gameStartClip"), new GUIContent("ゲーム開始時SE"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("enemyTypeGunStandbyClip"), new GUIContent("エネミー(銃)の構え時SE"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("enemyTypeGunShotClip"), new GUIContent("エネミー(銃)の発射SE"));


        serializedObject.ApplyModifiedProperties();
    }

}
