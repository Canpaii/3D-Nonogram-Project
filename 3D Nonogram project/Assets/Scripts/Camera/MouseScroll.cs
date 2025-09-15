using Unity.Cinemachine;
using UnityEngine;

public class MouseScroll : MonoBehaviour
{
    [SerializeField] private CinemachineCamera virtualCamera;
    [SerializeField] private float scrollSpeed;
    [SerializeField] private float minFOV;
    [SerializeField] private float maxFOV;

    private void Update()
    {
        if (virtualCamera == null) return;

        float scroll = Input.mouseScrollDelta.y;
        if (Mathf.Abs(scroll) > 0.01f)
        {
            float newFOV = virtualCamera.Lens.FieldOfView - scroll * scrollSpeed;
            virtualCamera.Lens.FieldOfView = Mathf.Clamp(newFOV, minFOV, maxFOV);
        }
    }
}


