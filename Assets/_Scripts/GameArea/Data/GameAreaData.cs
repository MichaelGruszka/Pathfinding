using System.Collections.Generic;
using Unity.Mathematics;
using Utils;
using UnityEngine;
namespace GameArea.Data
{
    [CreateAssetMenu(menuName = "GameArea/Data/" + nameof(GameAreaData), fileName = nameof(GameAreaData), order = 1)]
    public class GameAreaData : ScriptableObject, IGameDataWrite
    {
        [SerializeField, ReadOnly] private int _Width;
        [SerializeField, ReadOnly] private int _Height;
        [SerializeField, ReadOnly] private float2 _Scale;
        [SerializeField, ReadOnly] private GridCell[] _Grid;

        public int Width => _Width;
        public int Height => _Height;
        public float2 Scale => _Scale;
        public IReadOnlyCollection<GridCell> Grid => _Grid;
        
        void IGameDataWrite.Save(int width, int height, float2 scale, GridCell[] grid)
        {
            _Width = width;
            _Height = height;
            _Scale = scale;
            _Grid = grid;
        }
    }
}
