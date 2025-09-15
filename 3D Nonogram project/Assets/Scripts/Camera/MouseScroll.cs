using Unity.Cinemachine;
using UnityEngine;

public class MouseScroll : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private float scrollSpeed = 5f;
    [SerializeField] private float minFOV = 20f;
    [SerializeField] private float maxFOV = 60f;

    private void Update()
    {
        if (virtualCamera == null) return;

        float scroll = Input.mouseScrollDelta.y;
        if (Mathf.Abs(scroll) > 0.01f)
        {
            float newFOV = virtualCamera.m_Lens.FieldOfView - scroll * scrollSpeed;
            virtualCamera.m_Lens.FieldOfView = Mathf.Clamp(newFOV, minFOV, maxFOV);
        }
    }
}


