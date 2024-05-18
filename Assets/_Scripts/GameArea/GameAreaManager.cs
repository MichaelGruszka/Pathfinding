using GameArea.Data;
using GameArea.Utils;
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

        [ContextMenu(nameof(BuildArea))]
        private void BuildArea()
        {
            var viewScale = _GameAreaView.transform.lossyScale;
            GridUtils.CreateNewGrid(_Width, _Height, new float2(viewScale.x, viewScale.z), _GameAreaDataCache);
            GridUtils.SetIsObstacle(5,true,_GameAreaDataCache);
            GridUtils.SetIsObstacle(4,true,_GameAreaDataCache);
            GridUtils.SetIsObstacle(3,true,_GameAreaDataCache);
            GridUtils.SetIsObstacle(7,true,_GameAreaDataCache);
            GridUtils.SetIsObstacle(9,true,_GameAreaDataCache);
            GridUtils.SetIsObstacle(15,true,_GameAreaDataCache);
            GridUtils.SetIsObstacle(32,true,_GameAreaDataCache);
            GridUtils.SetIsObstacle(49,true,_GameAreaDataCache);
            GridUtils.SetIsObstacle(0,true,_GameAreaDataCache);
            
        }
    }
}
