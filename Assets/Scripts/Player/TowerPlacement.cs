using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPlacement : MonoBehaviour
{
    private GameObject CurrentPlacingTower;
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (CurrentPlacingTower != null)
        {

        }
    }

    public void SetTowerToPlace(GameObject tower)
    {
        CurrentPlacingTower = Instantiate(tower, Vector3.zero, Quaternion.identity);
        
    }
}
