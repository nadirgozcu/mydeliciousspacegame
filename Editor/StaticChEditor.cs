using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(StaticCharacter), true)]
public class LevelScriptEditor : Editor
{

    StaticObject staticCharacters;
    StaticCharacter myTarget;
    float distance;
    bool showList = false;
    void InstantiateFromList(StaticCharacter target, int index)
    {
        StaticCharacter newObj = ((GameObject)GameObject.Instantiate(staticCharacters.staticObjects[index])).GetComponent<StaticCharacter>();

        for (int i = 0; i < target.childrenCharacters.Count; i++)
        {
            newObj.childrenCharacters.Add(target.childrenCharacters[i]);
            target.childrenCharacters[i].parentCharacter = newObj;
        }
        if (target.parentCharacter != null) { 
            target.parentCharacter.childrenCharacters.Remove(target);
            target.parentCharacter.childrenCharacters.Add(newObj);
            newObj.parentCharacter = target.parentCharacter;
        }
        newObj.listIndex = index;
        newObj.transform.SetParent(target.transform.parent);
        newObj.parentDir = target.parentDir;
        newObj.transform.position = target.transform.position;
        if (newObj.GetComponent<AttackingUnitI>() != null) newObj.instanceType = StaticCharacter.INSTYPE.Tower;
        else if (newObj.GetComponent<CenterStaticCharacter>() != null) newObj.instanceType = StaticCharacter.INSTYPE.Center;
        else newObj.instanceType = StaticCharacter.INSTYPE.Tube;
        newObj.UpdateSprite();
        if (newObj.parentCharacter)
            newObj.parentCharacter.UpdateSprite();
        Selection.activeGameObject = newObj.gameObject;
        DestroyImmediate(target.gameObject);
    }
    void showObjList()
    {
        if (staticCharacters.staticObjects.Contains(myTarget.gameObject))
            myTarget.listIndex = staticCharacters.staticObjects.IndexOf(myTarget.gameObject);
        showList = EditorGUILayout.Foldout(showList, "Show Object List");
        
        int lastNull = -1;
        string[] names = new string[staticCharacters.staticObjects.Count];
        
        for (int i = 0; i < staticCharacters.staticObjects.Count; i++)
        {
            if (staticCharacters.staticObjects[i] == null)
            {
                lastNull = i;
                continue;
                
            }
            names[i] = staticCharacters.staticObjects[i].name;
            if (showList)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(staticCharacters.staticObjects[i].name);
                if (GUILayout.Button("Remove"))
                {
                    staticCharacters.staticObjects[i] = null;
                    continue;
                }
                EditorGUILayout.EndHorizontal();
            }
        }
        if (showList)
        {
            GameObject temp = null;
            temp = (GameObject)EditorGUILayout.ObjectField("Add Object : ", temp, typeof(GameObject), true);
            if (temp != null && !staticCharacters.staticObjects.Contains(temp))
            {
                if (lastNull != -1)
                    staticCharacters.staticObjects[lastNull] = temp;
                else
                    staticCharacters.staticObjects.Add(temp);
            }
        }

        int tempIndex = EditorGUILayout.Popup(myTarget.listIndex, names);
        if(tempIndex != myTarget.listIndex)
        {
            InstantiateFromList(myTarget, tempIndex);
        }
    }
    public override void OnInspectorGUI()
    {
        myTarget = (StaticCharacter)target;
        staticCharacters = ResourceCash.Cash.ScriptableObjects__towerlist_asset_StaticObject;
        if (myTarget.GetComponent<AttackingUnitI>() != null) myTarget.instanceType = StaticCharacter.INSTYPE.Tower;
        else if (myTarget.GetComponent<CenterStaticCharacter>() != null) myTarget.instanceType = StaticCharacter.INSTYPE.Center;
        else myTarget.instanceType = StaticCharacter.INSTYPE.Tube;

        distance = 0.95f;

        
        showObjList();
        if (serializedObject == null) return;


        myTarget.spriteRoot = (Texture2D)EditorGUILayout.ObjectField(myTarget.spriteRoot, typeof(Texture2D), true);

        EditorGUILayout.LabelField("Add Tube");
        EditorGUILayout.BeginHorizontal();
        GUI.enabled = !myTarget.north;
        if (GUILayout.Button("North"))
        {

            GameObject newObj = (GameObject)GameObject.Instantiate(staticCharacters.staticObjects[0]);
            newObj.transform.position = myTarget.gameObject.transform.position + new Vector3(0, distance, 0);
            newObj.transform.SetParent(myTarget.transform.parent);
            Selection.activeGameObject = newObj;
            StaticCharacter child = newObj.GetComponent<StaticCharacter>();
            child.parentDir = StaticCharacter.DIRECTION.SOUTH;
            myTarget.childrenCharacters.Add(child);
            child.parentCharacter = myTarget;
            myTarget.UpdateSprite();
            child.UpdateSprite();

        }
        GUI.enabled = !myTarget.south;
        if (GUILayout.Button("South"))
        {
            GameObject newObj = (GameObject)GameObject.Instantiate(staticCharacters.staticObjects[0]);
            newObj.transform.position = myTarget.gameObject.transform.position + new Vector3(0, -distance, 0);
            newObj.transform.SetParent(myTarget.transform.parent);
            Selection.activeGameObject = newObj;
            StaticCharacter child = newObj.GetComponent<StaticCharacter>();
            child.parentDir = StaticCharacter.DIRECTION.NORTH;
            myTarget.childrenCharacters.Add(child);
            child.parentCharacter = myTarget;
            myTarget.UpdateSprite();
            child.UpdateSprite();
        }
        GUI.enabled = !myTarget.west;
        if (GUILayout.Button("West"))
        {
            GameObject newObj = (GameObject)GameObject.Instantiate(staticCharacters.staticObjects[0]);
            newObj.transform.position = myTarget.gameObject.transform.position + new Vector3(-distance, 0, 0);
            newObj.transform.SetParent(myTarget.transform.parent);
            Selection.activeGameObject = newObj;
            StaticCharacter child = newObj.GetComponent<StaticCharacter>();
            child.parentDir = StaticCharacter.DIRECTION.EAST;
            myTarget.childrenCharacters.Add(child);
            child.parentCharacter = myTarget;
            myTarget.UpdateSprite();
            child.UpdateSprite();
        }
        GUI.enabled = !myTarget.east;
        if (GUILayout.Button("East"))
        {
            GameObject newObj = (GameObject)GameObject.Instantiate(staticCharacters.staticObjects[0]);
            newObj.transform.position = myTarget.gameObject.transform.position + new Vector3(distance, 0, 0);
            newObj.transform.SetParent(myTarget.transform.parent);
            Selection.activeGameObject = newObj;
            StaticCharacter child = newObj.GetComponent<StaticCharacter>();
            child.parentDir = StaticCharacter.DIRECTION.WEST;
            myTarget.childrenCharacters.Add(child);
            child.parentCharacter = myTarget;
            myTarget.UpdateSprite();
            child.UpdateSprite();
        }
        GUI.enabled = true;
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.LabelField("Add Tube");

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("Destroy This"))
        {
            if (myTarget.parentCharacter != null)
            {
                myTarget.parentCharacter.childrenCharacters.Remove(myTarget);
                myTarget.parentCharacter.UpdateSprite();
            }
            DestroyTube(myTarget);
        }
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        if (GUILayout.Button("UpdateChildren"))
        {
            UpdateChildren(myTarget);
            return;
        }
        if(serializedObject.targetObject != null)
            DrawDefaultInspector();
        if (GUI.changed)
        {
            EditorUtility.SetDirty(staticCharacters);
        }

    }

    void UpdateChildren(StaticCharacter target)
    {
        for (int i = target.childrenCharacters.Count - 1; i >= 0; i--)
        {
            UpdateChildren(target.childrenCharacters[i]);
        }
        InstantiateFromList(target, target.listIndex);
    }
    void DestroyTube(StaticCharacter target)
    {
        for (int i = 0; i < target.childrenCharacters.Count; i++)
            DestroyTube(target.childrenCharacters[i]);
        DestroyImmediate(target.gameObject);
    }
}