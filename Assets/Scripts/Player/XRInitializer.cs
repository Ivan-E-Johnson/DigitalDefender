using System.Collections;
using System.Collections.Generic;
using System.Net;
using Maps;
using UnityEngine;

public class XRInitializer : MonoBehaviour
{

    public GameObject rig; 
        
    // Start is called before the first frame update
    void Start()
    {
        var mapGenerator = Maps.MapGenerator.Instance;
        rig.transform.position = mapGenerator.getEndPositionVec3();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
