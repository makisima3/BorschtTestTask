using System;
using Code.Player.Configs;
using Code.Player.Enums;
using Code.Player.Shooting;
using Code.UI;
using Plugins.MyUtils;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace Code.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        [Inject] private PlayerActionConfig actionConfig;
        [Inject] private Joystick joystick;
        [Inject] private ShootController shootController;
        [Inject] private Collector collector;
        [Inject] private RestartView restartView;
        [Inject] private PlayerAnimationsConfig playerAnimationsConfig;

        [SerializeField] private Animator animator;
        [SerializeField] private Transform view;
        [SerializeField] private bool keyboardController;

        private CharacterController _characterController;
        private Vector3 _startPosition;
        private bool _isMoving;

        public UnityEvent OnStop { get; private set; }
        public UnityEvent OnStartMove { get; private set; }

        private void Awake()
        {
            OnStop = new UnityEvent();
            OnStartMove = new UnityEvent();
            _characterController = GetComponent<CharacterController>();
            
            _startPosition = transform.position;
        }

        private void Start()
        {
            restartView.OnRestart.AddListener(() =>
            {
                transform.position = _startPosition;
            });
        }

        private void Move(Vector3 direction)
        {
            _characterController.Move(direction * actionConfig.Speed * Time.deltaTime);
        }

        private Vector3 GetRelativeDirection(Vector3 direction)
        {
            var inverseRotation = Quaternion.Inverse(view.rotation);
            var localDirection = inverseRotation * direction;
            
            var direction2D = new Vector2(localDirection.x, localDirection.z).normalized;
            var forward2D = new Vector2(transform.forward.x, transform.forward.z).normalized;
            var angle = Vector2.SignedAngle(forward2D, direction2D);
            var angleRad = angle * Mathf.Deg2Rad;
            var sin = Mathf.Sin(angleRad);
            var cos = Mathf.Cos(angleRad);
            var rotatedDirection = new Vector2(
                cos * direction2D.x - sin * direction2D.y,
                sin * direction2D.x + cos * direction2D.y);
            var relativeDirection = new Vector3(rotatedDirection.x, 0f, rotatedDirection.y);

            return relativeDirection;
        }
        
        
        private Vector3 GetRelativeDirection(Vector3 direction, Transform view)
        {
            Quaternion inverseRotation = Quaternion.Inverse(view.rotation);
            Vector3 localDirection = inverseRotation * direction;

            return localDirection.normalized;
        }

        private void Update()
        {
            var direction = (Vector3)joystick.Direction;
            direction.z = direction.y;
            direction.y = 0f;

            if (keyboardController)
            {
                direction.x = Input.GetAxis("Horizontal");
                direction.z = Input.GetAxis("Vertical");
            }
            
            var isMoving = direction != Vector3.zero;

            if(!shootController.IsShooting && isMoving)
                view.rotation = ExtraMathf.GetRotation(direction, Vector3.up);
            
            var speed = direction.magnitude;
            var relativeDirection = GetRelativeDirection(direction,view);

            
            animator.SetBool(playerAnimationsConfig.GetAnimationKey(PlayerAnimationType.MoveBool), isMoving);
            animator.SetFloat(playerAnimationsConfig.GetAnimationKey(PlayerAnimationType.Speed), speed);
            animator.SetFloat(playerAnimationsConfig.GetAnimationKey(PlayerAnimationType.DirectionX), relativeDirection.x);
            animator.SetFloat(playerAnimationsConfig.GetAnimationKey(PlayerAnimationType.DirectionZ), relativeDirection.z);

            if(isMoving)
                Move(direction);

            if (isMoving != _isMoving)
            {
                if (isMoving)
                    OnStartMove.Invoke();
                else
                    OnStop.Invoke();

                _isMoving = isMoving;
            }
        }
    }
}