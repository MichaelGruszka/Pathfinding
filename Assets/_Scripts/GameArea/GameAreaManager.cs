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
        }
    }
}
