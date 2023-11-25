using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRenderer : MonoBehaviour
{
    private static LineRenderer _instance;

    public GameObject originObject;

    private void Awake() {
        _instance = this;
    }

    public static List<Transform> DrawPath(List<Transform> prePath, List<Vector3> positions, Vector3 origin)
    {
        Vector3 previousPos = origin;
        int i = 0;
        for(i = 0; i < positions.Count; i++)
        {
            if(i >= prePath.Count)
            {
                //Need to create new path objects
                prePath.Add(Instantiate(_instance.originObject).transform);
            }

            prePath[i].position = positions[i];

            previousPos = prePath[i].position;   
        }

        if(i < prePath.Count)
        {
            //Remove any unsed path objects
            for(int j = i; j < prePath.Count; j++)
            {
                GameObject segment = prePath[i].gameObject;
                Destroy(segment);
                prePath.RemoveAt(i);
            }
        }

        return prePath;
    } 

    public static List<Transform> ResetPath(List<Transform> prePath)
    {
        int cachedLength = prePath.Count;
        for(int i = 0; i < cachedLength; i++)
        {
            GameObject segment = prePath[0].gameObject;
            Destroy(segment);
            prePath.RemoveAt(0);
        }

        return prePath;
    }
}
