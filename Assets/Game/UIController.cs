using UnityEngine;

namespace Game
{
    public interface IUIController
    {
        void Move(Vector3 move);
        void PlayerSubmit();
        void MoveReset();
    }
}