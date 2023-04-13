using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace Raya.Inputs
{
    [AddComponentMenu("")]
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerInputs : MonoBehaviour
    {
        public Vector2 walk;
        public bool run;

#if ENABLE_INPUT_SYSTEM
        public void OnWalk(InputValue value)
        {
            walk = value.Get<Vector2>();
        }

        public void OnRun(InputValue value)
        {
            run = value.Get<bool>();
        }
#endif // ENABLE_INPUT_SYSTEM
    }
}
