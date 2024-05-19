using GameArea.Data;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace PathFinding
{
    [BurstCompile]
    public struct PathfindingJob : IJob
    {
        [ReadOnly] public int2 GridSize;
        public NativeArray<GridCell> Grid;
        [ReadOnly] public int2 Start;
        [ReadOnly] public int2 End;
        public NativeList<int2> Path;
        public NativeArray<bool> OpenSet;
        public NativeArray<bool> ClosedSet;
        private int GetIndex(int2 pos)
        {
            return pos.y * GridSize.x + pos.x;
        }
        [BurstCompile]
        public void Execute()
        {
            int size = GridSize.x * GridSize.y;

            for (int i = 0; i < size; i++)
            {
                OpenSet[i] = false;
                ClosedSet[i] = false;
                var gridCell = Grid[i];
                gridCell.gScore = float.MaxValue;
                gridCell.fScore = float.MaxValue;
                Grid[i] = gridCell;
            }

            int startIndex = GetIndex(Start);
            var startGridCell = Grid[startIndex];
            startGridCell.gScore = 0;
            startGridCell.fScore = SquaredDistance(Start, End);
            Grid[startIndex] = startGridCell;
            OpenSet[startIndex] = true;

            while (HasOpenNodes())
            {
                int currentIndex = GetLowestFScoreNode();
                int2 currentPos = IndexToPos(currentIndex);

                if (currentPos.Equals(End))
                {
                    ReconstructPath(currentIndex);
                    break;
                }

                OpenSet[currentIndex] = false;
                ClosedSet[currentIndex] = true;
                GetNeighbors(currentPos, out var neighbor1, out var neighbor2, out var neighbor3, out var neighbor4);
                EvaluateNeighbor(neighbor1, currentIndex, currentPos);
                EvaluateNeighbor(neighbor2, currentIndex, currentPos);
                EvaluateNeighbor(neighbor3, currentIndex, currentPos);
                EvaluateNeighbor(neighbor4, currentIndex, currentPos);
            }
        }
        private void EvaluateNeighbor(int2 neighborPos, int currentInd, int2 currentPosition)
        {
            if (neighborPos.x >= 0)
            {
                int neighborIndex = GetIndex(neighborPos);
                if (ClosedSet[neighborIndex] || Grid[neighborIndex].Obstacle)
                    return;

                float tentativeGScore = Grid[currentInd].gScore + SquaredDistance(currentPosition, neighborPos);

                if (!OpenSet[neighborIndex] || tentativeGScore < Grid[neighborIndex].gScore)
                {
                    var neighborGridCell = Grid[neighborIndex];
                    neighborGridCell.cameFromIndex = currentInd;
                    neighborGridCell.gScore = tentativeGScore;
                    neighborGridCell.fScore = tentativeGScore + SquaredDistance(neighborPos, End);
                    Grid[neighborIndex] = neighborGridCell;
                    OpenSet[neighborIndex] = true;
                }
            }
        }
        private static float SquaredDistance(int2 a, int2 b)
        {
            int dx = a.x - b.x;
            int dy = a.y - b.y;
            return dx * dx + dy * dy;
        }

        private bool HasOpenNodes()
        {
            int size = GridSize.x * GridSize.y;
            for (int i = 0; i < size; i++)
            {
                if (OpenSet[i])
                    return true;
            }
            return false;
        }

        private int GetLowestFScoreNode()
        {
            int lowestIndex = -1;
            float lowestFScore = float.MaxValue;

            for (int i = 0; i < OpenSet.Length; i++)
            {
                if (OpenSet[i] && Grid[i].fScore < lowestFScore)
                {
                    lowestFScore = Grid[i].fScore;
                    lowestIndex = i;
                }
            }

            return lowestIndex;
        }

        private int2 IndexToPos(int index)
        {
            int y = index / GridSize.x;
            int x = index % GridSize.x;
            return new int2(x, y);
        }

        private void GetNeighbors(int2 pos, out int2 neighbor1, out int2 neighbor2, out int2 neighbor3, out int2 neighbor4)
        {
            neighbor1 = new int2(-1, -1);
            neighbor2 = new int2(-1, -1);
            neighbor3 = new int2(-1, -1);
            neighbor4 = new int2(-1, -1);

            if (pos.x > 0) neighbor1 = (new int2(pos.x - 1, pos.y));
            if (pos.x < GridSize.x - 1) neighbor2 = (new int2(pos.x + 1, pos.y));
            if (pos.y > 0) neighbor3 = (new int2(pos.x, pos.y - 1));
            if (pos.y < GridSize.y - 1) neighbor4 = (new int2(pos.x, pos.y + 1));
        }

        private void ReconstructPath(int currentIndex)
        {
            while (currentIndex != -1)
            {
                var currentPose = IndexToPos(currentIndex);
                Path.Add(currentPose);
                currentIndex = Grid[currentIndex].cameFromIndex;
            }
        }
    }

}
