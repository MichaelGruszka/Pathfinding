using Unity.Mathematics;
namespace GameArea.Data
{
    public interface IGameDataWrite
    {
        void Save(int width, int height, float2 scale,  GridCell[] grid);
    }
}
