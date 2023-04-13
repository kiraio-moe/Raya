using Random = UnityEngine.Random;
using Raya.Inputs;
using System;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace Raya.Player
{
    [AddComponentMenu("Raya/Player/Player Controller")]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(PlayerInput))]
    [RequireComponent(typeof(PlayerInputs))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Stats")]
        [SerializeField] float m_WalkSpeed = 6f;
        [SerializeField] float m_SprintSpeed = 10f;
        [SerializeField] bool m_IgnoreYInput = true;
        float _moveSpeed;

        [Header("Graphics")]
        [SerializeField] SpriteRenderer m_CharacterSpriteRenderer;
        [SerializeField] bool m_SpriteFacingRight = true;

        [Header("Animator")]
        [SerializeField] Animator m_CharacterAnimator;
        [SerializeField] string m_AnimatorIdleParameter = "Idle";
        [SerializeField] string m_AnimatorIdleCountParameter = "IdleTime";
        [SerializeField] string m_AnimatorWalkParameter = "Walk";
        [SerializeField] string m_AnimatorRunParameter = "Run";
        [Space]
        [SerializeField] string m_AnimatorRandomizerParameter = "Random";
        [SerializeField] int m_AnimatorRandomRange = 10;

        bool _canMove = true;
        bool _crouching;
        public float _idleTime { get; set;}
        int _animatorRandomizer;

        Rigidbody2D _rigidBody2D;
        PlayerInputs _input;

        void Awake()
        {
            _rigidBody2D = GetComponent<Rigidbody2D>();
            _input = GetComponent<PlayerInputs>();
        }

        void Update()
        {
            Idle();
            Move(_input.walk);
        }

        void Idle()
        {
            // Track idle time
            if (_input.walk != Vector2.zero)
            {
                _idleTime = 0f;

                m_CharacterAnimator.SetBool(m_AnimatorIdleParameter, false);
            }
            else
            {
                _idleTime += Time.deltaTime;

                m_CharacterAnimator.SetBool(m_AnimatorIdleParameter, true);
                m_CharacterAnimator.SetInteger(m_AnimatorRandomizerParameter, Random.Range(-m_AnimatorRandomRange, m_AnimatorRandomRange));
            }

            m_CharacterAnimator.SetFloat(m_AnimatorIdleCountParameter, _idleTime);
        }

        /// <summary>
        /// Move player according to direction.
        /// </summary>
        void Move(Vector2 direction)
        {
            // Check if we can move, if not return.
            if (!_canMove)
                return;

            if ((m_IgnoreYInput && direction.y != 0) || (direction == Vector2.zero))
                m_CharacterAnimator.SetBool(m_AnimatorWalkParameter, false);
            else if (!m_IgnoreYInput || direction.x != 0)
                m_CharacterAnimator.SetBool(m_AnimatorWalkParameter, true);

            // Flip player graphic according to direction value
            Flip(direction);

            // TODO: Implement Run
            _moveSpeed = _input.walk != Vector2.zero ? m_WalkSpeed : m_SprintSpeed;

            if (m_IgnoreYInput)
                _rigidBody2D.velocity = new Vector2((direction.x * _moveSpeed) * Time.smoothDeltaTime, _rigidBody2D.velocity.y);
            else
                _rigidBody2D.velocity = (direction * _moveSpeed) * Time.smoothDeltaTime;
        }

        /// <summary>
        /// Flip player transform according to x direction.
        /// </summary>
        void Flip(Vector2 direction)
        {
            if ((direction.x < 0 && m_SpriteFacingRight) || (direction.x > 0 && !m_SpriteFacingRight))
            {
                m_SpriteFacingRight = !m_SpriteFacingRight;
                transform.Rotate(new Vector2(0, 180));
            }
        }
    }
}
