using System.Collections;
using System.Collections.Generic;
using Unity.Properties;
using UnityEngine;

public class Photograher : MonoBehaviour
{
    public float Pitch { get; private set; }
    public float Yaw { get; private set; }
    public float mouseSensitivity = 100f;
    public float cameraRotationSpeed = 100f;
    public float cameraYspeed = 5f;
    private Transform _target;
    private Transform _camera;
    [SerializeField]
    public AnimationCurve armLengthCurve;
    private void Awake()
    {
        _camera = transform.GetChild(0);
    }
    void Start()
    {
        
    }
    public void InitCamera(Transform target)
    {
        _target = target;
        transform.position = target.position;
    }

    void Update()
    {
        UpdateRotation();
        UpdatePosition();
        UpdateArmLength();
    }
    private void UpdateRotation()
    {
        // 仅在按住右键（鼠标按钮 1）时响应鼠标移动
        if (Input.GetMouseButton(1))
        {
            Yaw += Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            Pitch += Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        }

        // 仍然接收来自虚拟轴（例如手柄/自定义输入）的旋转输入
        Yaw += Input.GetAxis("CameraRateX") * cameraRotationSpeed * Time.deltaTime;
        Pitch += Input.GetAxis("CameraRateY") * cameraRotationSpeed * Time.deltaTime;
        Pitch = Mathf.Clamp(Pitch, -90f, 90f);

        transform.rotation = Quaternion.Euler(-Pitch, Yaw, 0f);
    }
    private void UpdatePosition()
    {
        Vector3 position = _target.position;
        float newY = Mathf.Lerp(transform.position.y, _target.position.y, Time.deltaTime * cameraYspeed);
        transform.position = new Vector3(position.x, newY, _target.position.z);
    }
    private void UpdateArmLength()
    {
        _camera.localPosition = new Vector3(0, 0, armLengthCurve.Evaluate(Pitch) * -1);
    }
}