using System.Collections.Generic;
using GameArea.Data;
using GameArea.Utils;
using Unity.Mathematics;
using UnityEngine;
namespace GameArea
{
    public class GameAreaView : MonoBehaviour
    {
        [SerializeField] private GameObject _ObstaclePrefab;
        [SerializeField] private GameAreaData _GameAreaCachedData;
        private Dictionary<int, GameObject> _GridIndexToObstacleObject = new Dictionary<int, GameObject>();

        [ContextMenu(nameof(UpdateGrid))]
        public void UpdateGrid()
        {
            for (int i = 0; i < _GameAreaCachedData.Grid.Count; i++)
            {
                if (_GameAreaCachedData.Grid[i].Obstacle)
                {
                    TryAddObstacle(i);
                }
                else
                {
                    TryRemoveObstacle(i);
                }
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
            _GridIndexToObstacleObject.Remove(index);
        }
    }
}
