using System.Collections.Generic;
using UnityEngine;
namespace GameArea
{
    public class GameAreaView : MonoBehaviour
    {
        [SerializeField] private GameObject _ObstaclePrefab;
        private Dictionary<int, GameObject> _GridIndexToObstacleObject = new Dictionary<int, GameObject>();

        public void ShowObstacle(Vector3 position)
        {
            
        }
    }
}
