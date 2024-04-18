using System.Text;
using Maps;
using UnityEngine;

namespace DigitalDefender
{
    public class MapGrid
    {
        private MapCell[,] cellGrid;

        public int Width { get; }

        public int Length { get; }

        public MapGrid(int width, int length)
        {
            Width = width;
            Length = length;
            GenerateGrid();
        }

        private void GenerateGrid()
        {
            cellGrid = new MapCell[Length, Width];
            for (var row = 0; row < Length; row++)
            {
                for (var col = 0; col < Width; col++)
                {
                    cellGrid[row, col] = new MapCell(row, col);
                }
            }
        }

        public void SetCell(int x, int z, MapCellObjectType objectType, bool isTaken = false)
        {
            cellGrid[z, x].ObjectType = objectType;
            cellGrid[z, x].IsTaken = isTaken;
        }

        public void SetCell(float x, float z, MapCellObjectType objectType, bool isTaken = false)
        {
            // This is a float version of the SetCell method
            SetCell((int)x, (int)z, objectType, isTaken);
        }

        public bool IsCellTaken(int x, int z)
        {
            return cellGrid[z, x].IsTaken;
        }

        public bool IsCellTaken(float x, float z)
        {
            // This is a float version of the IsCellTaken method
            return IsCellTaken((int)x, (int)z);
        }

        public bool IsCellValid(int x, int z)
        {
            // This method checks if the cell is within the bounds of the grid
            return x >= 0 && x < Width && z >= 0 && z < Length;
        }

        public bool IsCellValid(Vector3Int position)
        {
            return IsCellValid(position.x, position.z);
        }

        public MapCell GetCell(int x, int z)
        {
            if (IsCellValid(x, z)) return cellGrid[z, x];
            return null;
        }

        public int CalculateIndexFromCoordinates(int x, int z)
        {
            return z * Width + x;
        }

        public int CalculateIndexFromCoordinates(float x, float z)
        {
            // This is a float version of the CalculateIndexFromCoordinates method
            return CalculateIndexFromCoordinates((int)x, (int)z);
        }

        public int CalculateIndexFromVector3Int(Vector3Int position)
        {
            return CalculateIndexFromCoordinates(position.x, position.z);
        }


        public void TestCheckCoordinates()
        {
            Debug.Log("number of rows: " + cellGrid.GetLength(0));
            Debug.Log("number of columns: " + cellGrid.GetLength(1));
            for (var i = 0; i < cellGrid.GetLength(0); i++)
            {
                var msg = new StringBuilder();

                for (var j = 0; j < cellGrid.GetLength(1); j++)
                {
                    msg.Append(j + "," + i + " ");
                }

                Debug.Log(msg.ToString());
            }
        }

        public Vector3Int CalculateCoordinatesFromIndex(int index)
        {
            var x = index % Width;
            var z = index / Width;
            return new Vector3Int(x, 0, z);
        }
    }
}