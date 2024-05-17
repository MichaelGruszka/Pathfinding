using System;
namespace GameArea.Data
{
    [Serializable]
    public struct GridCell
    {
        public int X;
        public int Y;
        public bool Obstacle;
    }
}
