using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 _offset;

    private void Awake()
    {
        
    }

        void Start()
    {
        _offset = transform.localPosition - target.position;
        transform.position = target.position + _offset;
    }

    private void LateUpdate()
    {
        transform.position = target.position + _offset;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
