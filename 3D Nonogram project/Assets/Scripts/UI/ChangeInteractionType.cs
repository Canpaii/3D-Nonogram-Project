using UnityEngine;

public enum InteractionType
{
    Drag = 0, 
    Mark = 1, 
    Paint = 2,
    Destroy = 3,
}

public class ChangeInteractionType : MonoBehaviour
{
    // Changes between all interaction types you can have with a voxel 
    public VoxelInteraction voxelI;

    public void OnInteractionTypeChange(int iType)
    {
       voxelI.SetInteractionType((InteractionType)iType);
    }

}
