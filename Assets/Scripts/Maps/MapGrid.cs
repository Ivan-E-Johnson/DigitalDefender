using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace DigitalDefender
{
    public class MapGrid
    {
        private int width, length;
        private MapCell[,] cellGrid;

        public int Width { get => width; }
        public int Length { get => length; }

        public MapGrid(int width, int length)
        {
            this.width = width;
            this.length = length;
            GenerateGrid();
        }
        private void GenerateGrid()
        {
            cellGrid = new MapCell[length, width];
            for (int row = 0; row < length; row++)
            {
                for (int col = 0; col < width; col++)
                {
                    cellGrid[row, col] = new MapCell(row, col);
                }
            }
        }
        public void SetCell(int x, int z, MapCellObjectType objectType,bool isTaken = false)
        {
            cellGrid[z, x].ObjectType = objectType;
            cellGrid[z,x].IsTaken = isTaken;
        }
        public void SetCell(float x, float z, MapCellObjectType objectType,bool isTaken = false)
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
            return x >= 0 && x < width && z >= 0 && z < length;
        }
        public MapCell GetCell(int x, int z)
        {
            if (IsCellValid(x, z))
            {
                return cellGrid[z, x];
            }
            return null;
        }
        public MapCell GetCell(float x, float z)
        {
            // This is a float version of the GetCell method
            return GetCell((int)x, (int)z);
        }

        public int CalculateIndexFromCoordinates(int x, int z)
        {
            return z * width + x;
        }
        public int CalculateIndexFromCoordinates(float x, float z)
        {
            // This is a float version of the CalculateIndexFromCoordinates method
            return CalculateIndexFromCoordinates((int)x, (int)z);
        }
        
        
        public void TestCheckCoordinates()
        {
            Debug.Log("number of rows: " + cellGrid.GetLength(0));
            Debug.Log("number of columns: " + cellGrid.GetLength(1));
            for (int i =0; i < cellGrid.GetLength(0); i++)
            {
                StringBuilder msg = new StringBuilder();

                for (int j = 0; j < cellGrid.GetLength(1); j++)
                {
                    msg.Append(j+ "," + i + " ");
                }
                Debug.Log(msg.ToString());
            }
        }
        public Vector3 CalculateCoordinatesFromIndex(int index)
        {
            int x = index % width;
            int z = index / width;
            return new Vector3(x, 0, z);
        }

        public bool IsPositionValid(Vector3 position)
        {
            return IsCellValid((int)position.x, (int)position.z);
        }
    }
}