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
        [ReadOnly] public NativeArray<GridCell> Grid;
        [ReadOnly] public int2 Start;
        [ReadOnly] public int2 End;
        public NativeList<int2> Path;
        public NativeArray<bool> OpenSet;
        public NativeArray<bool> ClosedSet;
        public NativeHashMap<int2, int2> CameFrom;
        public NativeHashMap<int2, float> GScore;
        public NativeHashMap<int2, float> FScore;

        private int GetIndex(int2 pos)
        {
            return pos.y * GridSize.x + pos.x;
        }

        public void Execute()
        {
            int size = GridSize.x * GridSize.y;

            for (int i = 0; i < size; i++)
            {
                OpenSet[i] = false;
                ClosedSet[i] = false;
            }

            OpenSet[GetIndex(Start)] = true;
            GScore[Start] = 0;
            FScore[Start] = math.distance(Start, End);

            while (HasOpenNodes())
            {
                int2 current = GetLowestFScoreNode();

                if (current.Equals(End))
                {
                    ReconstructPath(current);
                    break;
                }

                OpenSet[GetIndex(current)] = false;
                ClosedSet[GetIndex(current)] = true;

                NativeArray<int2> neighbors = GetNeighbors(current);

                foreach (int2 neighbor in neighbors)
                {
                    if (ClosedSet[GetIndex(neighbor)] || Grid[GetIndex(neighbor)].Obstacle)
                        continue;

                    float tentativeGScore = GScore[current] + math.distance(current, neighbor);

                    if (!OpenSet[GetIndex(neighbor)] || tentativeGScore < GScore[neighbor])
                    {
                        CameFrom[neighbor] = current;
                        GScore[neighbor] = tentativeGScore;
                        FScore[neighbor] = GScore[neighbor] + math.distance(neighbor, End);
                        if (!OpenSet[GetIndex(neighbor)])
                            OpenSet[GetIndex(neighbor)] = true;
                    }
                }

                neighbors.Dispose();
            }
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

        private int2 GetLowestFScoreNode()
        {
            int2 lowestNode = default;
            float lowestFScore = float.MaxValue;

            for (int i = 0; i < OpenSet.Length; i++)
            {
                if (OpenSet[i] && FScore.TryGetValue(IndexToPos(i), out float score) && score < lowestFScore)
                {
                    lowestFScore = score;
                    lowestNode = IndexToPos(i);
                }
            }

            return lowestNode;
        }

        private int2 IndexToPos(int index)
        {
            int y = index / GridSize.x;
            int x = index % GridSize.x;
            return new int2(x, y);
        }

        private NativeArray<int2> GetNeighbors(int2 pos)
        {
            NativeList<int2> neighbors = new NativeList<int2>(Allocator.Temp);
            if (pos.x > 0) neighbors.Add(new int2(pos.x - 1, pos.y));
            if (pos.x < GridSize.x - 1) neighbors.Add(new int2(pos.x + 1, pos.y));
            if (pos.y > 0) neighbors.Add(new int2(pos.x, pos.y - 1));
            if (pos.y < GridSize.y - 1) neighbors.Add(new int2(pos.x, pos.y + 1));
            return neighbors;
        }

        private void ReconstructPath(int2 current)
        {
            NativeList<int2> totalPath = new NativeList<int2>(Allocator.Temp);
            totalPath.Add(current);

            while (CameFrom.ContainsKey(current))
            {
                current = CameFrom[current];
                totalPath.Add(current);
            }

            for (int i = totalPath.Length - 1; i >= 0; i--)
            {
                Path.Add(totalPath[i]);
            }

            totalPath.Dispose();
        }
    }
}
