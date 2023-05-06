using System;
using UnityEngine;

namespace DefaultNamespace.UI
{
    public class Billboard : MonoBehaviour
    {
       /*private Camera _camera;
        
        private void Awake()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            //transform.rotation =ExtraMathf.GetRotation(transform.position, _camera.transform.position, Vector3.up);
            transform.LookAt(_camera.transform);
        }*/
        
        private Transform camTransform;

        Quaternion originalRotation;

        void Awake()
        {
            camTransform = Camera.main.transform;
            originalRotation = transform.rotation;
        }

        void Update()
        {
            transform.rotation = camTransform.rotation * originalRotation;   
        }
    }
}