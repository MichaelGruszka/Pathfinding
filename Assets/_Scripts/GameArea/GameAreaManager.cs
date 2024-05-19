using System;
using System.Collections;
using System.Diagnostics;
using GameArea.Data;
using GameArea.Utils;
using PathFinding;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
namespace GameArea
{
    public class GameAreaManager : MonoBehaviour
    {
        [SerializeField] private GameAreaData _GameAreaDataCache;
        [SerializeField] private int _Width;
        [SerializeField] private int _Height;
        [SerializeField] private GameAreaView _GameAreaView;
        [SerializeField] private Camera _Camera;
        [SerializeField] private LayerMask _AreaLayerMask;
        private Coroutine _findPathCoroutineHandle;
        public event Action<NativeList<int2>> PathFound;
        public event Action NewAreaCreated;
        public event Action<TimeSpan> PathFoundTime;

        private void Start()
        {
            _Width = _GameAreaDataCache.Width;
            _Height = _GameAreaDataCache.Height;
        }
        public void BuildArea(int width, int height)
        {
            _Width = width;
            _Height = height;
            var viewScale = _GameAreaView.transform.lossyScale;
            GridUtils.CreateNewGrid(_Width, _Height, new float2(viewScale.x, viewScale.z), _GameAreaDataCache);
            _GameAreaView.UpdateGrid();
            NewAreaCreated?.Invoke();
        }
        public void FindPath(Vector3 start, Vector3 end)
        {
            if (_findPathCoroutineHandle == null && !_GameAreaDataCache.Grid[GridUtils.GetIndex(end, _GameAreaDataCache)].Obstacle)
            {
                _findPathCoroutineHandle = StartCoroutine(FindPathCoroutine(start, end));
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                var ray = _Camera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    var hitPos = hit.point;
                    hitPos = new Vector3(hitPos.x + 0.5f, 0, hitPos.z + 0.5f); // adjust to better UX
                    var cellIndex = GridUtils.GetIndex(hitPos, _GameAreaDataCache);
                    GridUtils.SetIsObstacle(cellIndex, !_GameAreaDataCache.Grid[cellIndex].Obstacle, _GameAreaDataCache);
                    _GameAreaView.UpdateCell(cellIndex);
                }
            }
        }

        private IEnumerator FindPathCoroutine(Vector3 startPos, Vector3 endPos)
        {
            var watch = new Stopwatch();
            watch.Start();
            int2 start = new int2((int)startPos.x, (int)startPos.z);
            int2 end = new int2((int)endPos.x, (int)endPos.z);

            int initialCapacity = _GameAreaDataCache.Width * _GameAreaDataCache.Height;
            NativeArray<GridCell> nativeGrid = new NativeArray<GridCell>(_GameAreaDataCache.Grid, Allocator.TempJob);
            NativeList<int2> path = new NativeList<int2>(Allocator.TempJob);
            NativeArray<bool> openSet = new NativeArray<bool>(initialCapacity, Allocator.TempJob);
            NativeArray<bool> closedSet = new NativeArray<bool>(initialCapacity, Allocator.TempJob);

            PathfindingJob job = new PathfindingJob
            {
                GridSize = new int2(_GameAreaDataCache.Width, _GameAreaDataCache.Height),
                Grid = nativeGrid,
                Start = start,
                End = end,
                Path = path,
                OpenSet = openSet,
                ClosedSet = closedSet,
            };

            JobHandle handle = job.Schedule();
            while (!handle.IsCompleted)
            {
                yield return null;
            }
            watch.Stop();
            handle.Complete();
            PathFoundTime?.Invoke(watch.Elapsed);
            PathFound?.Invoke(path);
            nativeGrid.Dispose();
            path.Dispose();
            openSet.Dispose();
            closedSet.Dispose();
            _findPathCoroutineHandle = null;
        }
    }
}
