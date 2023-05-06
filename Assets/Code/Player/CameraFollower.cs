using System;
using UnityEngine;
using Zenject;

namespace Code.Player
{
    public class CameraFollower : MonoBehaviour
    {
        [Inject] private PlayerController playerController;
        
        [SerializeField] private float speed = 1f;

        private Vector3 _offset;

        private void Awake()
        {
            _offset = transform.position - playerController.transform.position;
        }

        private void Update()
        {
            transform.position = Vector3.Lerp(transform.position, playerController.transform.position + _offset, speed * Time.deltaTime);
        }
    }
}