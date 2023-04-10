using Raya.Inputs;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace Raya.Player
{
    [AddComponentMenu("Raya/Player/Player Controller")]
    [RequireComponent(typeof(PlayerInput))]
    [RequireComponent(typeof(PlayerInputs))]
    public class PlayerController : MonoBehaviour
    {
        PlayerInputs _input;

        void Awake()
        {
            _input = GetComponent<PlayerInputs>();
        }
    }
}
