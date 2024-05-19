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
                        Obstacle = false,
                        Index = y * x + y,
                        cameFromIndex = -1
                    };
                }
            }
            dataToWrite.Save(width, height, scale, grid);
        }
        public static void SetIsObstacle(int index, bool isObstacle, IGameDataWrite gameData)
        {
            gameData.IsObstacle(index, isObstacle);
        }
        public static int GetIndex(Vector3 position, GameAreaData gameAreaData)
        {
            int width = gameAreaData.Width;
            float2 scale = gameAreaData.Scale;

            int x = Mathf.FloorToInt(position.x / scale.x);
            int y = Mathf.FloorToInt(position.z / scale.y);

            return y * width + x;
        }
        public static Vector3 GetPosition(int index, GameAreaData gameAreaData)
        {
            int width = gameAreaData.Width;
            float2 scale = gameAreaData.Scale;

            int x = index % width;
            int y = index / width;

            return new Vector3(x * scale.x, 0, y * scale.y);
        }
    }
}
