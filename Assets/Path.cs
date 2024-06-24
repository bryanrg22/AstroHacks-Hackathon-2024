using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour
{
    // Start is called before the first frame update
    public SearchThing thing;
    public LineRenderer lineRenderer;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        List<Vector3> vector3s = new List<Vector3>() { transform.position };

        foreach(PlanetScript planet in thing.pathToFollow)
        {
            vector3s.Add(new Vector3(planet.transform.position.x,planet.transform.position.y,89));
        }
        lineRenderer.positionCount = vector3s.Count;
        lineRenderer.SetPositions(vector3s.ToArray());

    }
}
