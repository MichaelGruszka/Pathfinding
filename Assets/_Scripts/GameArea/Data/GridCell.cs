using System;
namespace GameArea.Data
{
    [Serializable]
    public struct GridCell
    {
        public int X;
        public int Y;
        public int Index;
        public bool Obstacle;
        public int cameFromIndex;
        public float gScore;
        public float fScore;
    }
}
