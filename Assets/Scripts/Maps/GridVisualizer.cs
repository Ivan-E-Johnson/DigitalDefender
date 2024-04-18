using UnityEngine;

namespace Maps
{
    public class GridVisualizer : MonoBehaviour
    {
        public GameObject groundPrefab;


        public void VisualizeGrid(int width, int length)
        {
            var position = new Vector3Int(width / 2, 0, length / 2);
            var rotation =
                Quaternion.Euler(90, 0, 0); // Rotate the ground 90 degrees on the x-axis so you can see from above

            var board = Instantiate(groundPrefab, position, rotation);
            board.transform.localScale =
                new Vector3Int(width, length, 1); // Set the scale of the ground to the width and length of the grid
        }
    }
}