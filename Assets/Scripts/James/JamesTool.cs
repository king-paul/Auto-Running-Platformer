using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class JamesTool : Editor
{
    [MenuItem("James' Tools")]

    void OnGUI()
    {
        GUILayout.Label("Placement Tools");

        if (GUILayout.Button("Place on Ground"))
        {
            PlaceOnGround();
        }
        
        if (GUILayout.Button("Other tool"))
        {
            OtherTool();
        }
    }

    void PlaceOnGround()
    {
        // Loop all selected objects
        Transform[] objectList = Selection.transforms;
        foreach (Transform obj in objectList)
        {
            float heightPoint = -99999;
            bool hitSuccess = false;

            RaycastHit[] hits = Physics.RaycastAll(obj.position, Vector3.down, heightPoint);
            foreach (RaycastHit hit in hits)
            {
                Transform root = hit.collider.transform.root;
                if (root == obj)
                    continue;

                if (hit.point.y > heightPoint)
                    heightPoint = hit.point.y;

                hitSuccess = true;
            }

            if (hitSuccess)
            {
                Vector3 pos = obj.transform.position;
                pos.y = heightPoint + obj.GetComponent<Collider>().bounds.size.y;
            }
        }
    }

    void OtherTool()
    {

    }
}

