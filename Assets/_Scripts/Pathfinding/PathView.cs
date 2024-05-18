using GameArea;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;
namespace PathFinding
{
    public class PathView : MonoBehaviour
    {
        [SerializeField] private LineRenderer _LineRenderer;
        [SerializeField] private GameAreaManager _GameAreaManager;
        private void Start()
        {
            _GameAreaManager.PathFound += ShowPath;
        }
        private void OnDestroy()
        {
            _GameAreaManager.PathFound -= ShowPath;
        }

        private void ShowPath(NativeList<int2> path)
        {
            _LineRenderer.positionCount = path.Length;
            for (int i = 0; i < path.Length; i++)
            {
                _LineRenderer.SetPosition(i, new Vector3(path[i].x, 0, path[i].y));
            }
        }
    }
}
