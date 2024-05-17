using GameArea.Data;
using Unity.Mathematics;
using UnityEngine;
namespace GameArea.Utils
{
    public static class GridUtils
    {
        public static void CreateNewGrid(int width, int height, float2 scale, IGameDataWrite dataToWrite)
        {
            GridCell[] grid = new GridCell[width * height];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    grid[y * width + x] = new GridCell
                    {
                        X = x,
                        Y = y,
                        Obstacle = false
                    };
                }
            }
            dataToWrite.Save(width, height, scale, grid);
        }
        public static int GetIndex(Vector3 position, float2 scale)
        {
            int x = Mathf.FloorToInt(position.x / scale.x);
            int y = Mathf.FloorToInt(position.y / scale.y);
            return y * Mathf.FloorToInt(1 / scale.x) + x;
        }
        public static Vector3 GetPosition(int index, float2 scale)
        {
            int width = Mathf.FloorToInt(1 / scale.x); // Assuming the grid width based on scale
            int x = index % width;
            int y = index / width;
            return new Vector3(x * scale.x, y * scale.y, 0);
        }
    }
}
