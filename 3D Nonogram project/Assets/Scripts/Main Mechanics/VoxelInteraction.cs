using UnityEngine;

public enum InteractionType
{
    Mark = 0,
    Paint = 1,
    Destroy = 2,
}
public class VoxelInteraction : MonoBehaviour
{
    public InteractionType interactionType { get; private set; }
    private Camera _cam;

    private void Start()
    {
        _cam = Camera.main;
    }
    public void SetInteractionType(int i)
    {
        this.interactionType = (InteractionType)i;
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
            hitInfo.collider.gameObject.GetComponent<Voxel>().HandleClick(interactionType);
        }
    }
}
