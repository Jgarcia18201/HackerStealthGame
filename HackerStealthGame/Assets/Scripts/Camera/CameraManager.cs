using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private Transform target;
    private Vector3 _offset;

    private void Awake()
    {
        _offset = transform.position - target.position;
    }

    private void LateUpdate()
    {
        transform.position = target.position + _offset;
    }
        // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
