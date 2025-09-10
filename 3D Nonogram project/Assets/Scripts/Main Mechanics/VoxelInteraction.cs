using UnityEngine;
using UnityEngine.InputSystem;


public class VoxelInteraction : MonoBehaviour
{
    public InteractionType interactionType { get; private set; }
    private Camera _cam;

    private void Start()
    {
        _cam = Camera.main;
    }
    public void SetInteractionType(InteractionType iType)
    {
        interactionType = iType;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            HandleClick();
        }
    }

    private void HandleClick()
    {
        Ray ray = _cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            print(hitInfo);
            hitInfo.collider.gameObject.GetComponent<Voxel>().HandleClick(interactionType);
        }
    }
}
