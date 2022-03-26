using UnityEngine;

namespace Game
{
    public interface IPlayerTool
    {
        void Use(RaycastHit raycastHit);
    }
}