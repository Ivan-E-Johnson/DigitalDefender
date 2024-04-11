using UnityEngine;

namespace DigitalDefender
{
    public class GridVisualizer : MonoBehaviour
    {
        public GameObject groundPrefab;


        public void VisulaizeGrid(int width, int length)
        {
            Vector3 position = new Vector3(width / 2f, 0, length / 2f);
            Quaternion rotation = Quaternion.Euler(90, 0, 0); // Rotate the ground 90 degrees on the x-axis so you can see from above
            
            var board = Instantiate(groundPrefab, position, rotation);
            board.transform.localScale = new Vector3(width, length, 1); // Set the scale of the ground to the width and length of the grid
            
        }

    }
}