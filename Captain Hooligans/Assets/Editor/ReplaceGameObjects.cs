using UnityEngine;
using UnityEditor;
using System.Collections;
// CopyComponents - by Michael L. Croswell for Colorado Game Coders, LLC
// March 2010

/// <summary>
/// Wizard to replace multiple gameobjects at once. Replaces the immediate childrens
/// of Replace object with useGameObjects.
/// </summary>
public class ReplaceGameObjects : ScriptableWizard
{ 
    public bool copyValues = true; 
    public GameObject useGameObject;
    public GameObject Replace;
    private ArrayList objectsToDestroy = new ArrayList();
    private ArrayList newObjects = new ArrayList();
    
    [MenuItem ("Custom/Replace GameObjects")]
    
    
    static void CreateWizard ()
    {
        ScriptableWizard.DisplayWizard("Replace GameObjects", typeof(ReplaceGameObjects), "Replace"); 
    }
    
    void OnWizardCreate ()
    {
        Transform[] Replaces;



        foreach (Transform t in Replace.transform)
        {
            GameObject newObject;
            newObject = (GameObject)EditorUtility.InstantiatePrefab(useGameObject);
            newObject.transform.position = t.position;
            newObject.transform.rotation = t.rotation;

            newObjects.Add(newObject);
            objectsToDestroy.Add(t.gameObject);        
        }

        foreach (GameObject o in objectsToDestroy)
        {
            DestroyImmediate(o);
        }

        foreach (GameObject o in newObjects)
        {
            o.transform.parent = Replace.transform;
        }
    }
}