using System.Collections;
using System.Collections.Generic;
using Siccity.GLTFUtility;
using UnityEngine;

public class TEst : MonoBehaviour
{

    public string path = "AssetAssets/Models/ezreal.glb";

    private void Start()
    {
        GameObject model = Importer.LoadFromFile(path);
        model.transform.position = Vector3.zero;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
        }
    }
}
