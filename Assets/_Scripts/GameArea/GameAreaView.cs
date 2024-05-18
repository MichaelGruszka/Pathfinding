using System.Collections.Generic;
using GameArea.Data;
using GameArea.Utils;
using UnityEngine;
namespace GameArea
{
    public class GameAreaView : MonoBehaviour
    {
        [SerializeField] private GameObject _ObstaclePrefab;
        [SerializeField] private GameAreaData _GameAreaCachedData;
        [SerializeField] private Transform _BackgroundPlane;
        private Dictionary<int, GameObject> _GridIndexToObstacleObject = new Dictionary<int, GameObject>();

        private void Start()
        {
            for (int i = 1; i < transform.childCount; i++) // avoid background
            {
                Destroy(transform.GetChild(i).gameObject);
            }
            UpdateGrid();
        }
        public void UpdateCell(int index)
        {
            if (_GameAreaCachedData.Grid[index].Obstacle)
            {
                TryAddObstacle(index);
            }
            else
            {
                TryRemoveObstacle(index);
            }
        }
        [ContextMenu(nameof(UpdateGrid))]
        public void UpdateGrid()
        {
            _BackgroundPlane.localPosition = new Vector3(_GameAreaCachedData.Width / 2, 0, _GameAreaCachedData.Height / 2);
            _BackgroundPlane.localScale = new Vector3(_GameAreaCachedData.Width / 10, 1, _GameAreaCachedData.Height / 10);
            for (int i = 0; i < _GameAreaCachedData.Grid.Length; i++)
            {
                UpdateCell(i);
            }
        }
        private void TryAddObstacle(int index)
        {
            if (_GridIndexToObstacleObject.ContainsKey(index))
            {
                return;
            }
            _GridIndexToObstacleObject[index] = Instantiate(
                _ObstaclePrefab,
                GridUtils.GetPosition(index, _GameAreaCachedData),
                Quaternion.identity,
                transform);
            _GridIndexToObstacleObject[index].name = GridUtils.GetPosition(index, _GameAreaCachedData).ToString();
        }
        private void TryRemoveObstacle(int index)
        {
            if (_GridIndexToObstacleObject.TryGetValue(index, out var obstacle))
            {
                Destroy(obstacle);
            }
            _GridIndexToObstacleObject.Remove(index);
        }
    }
}
