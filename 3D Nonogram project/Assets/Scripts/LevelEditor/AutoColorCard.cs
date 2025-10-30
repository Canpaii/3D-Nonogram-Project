using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteAlways]
public class AutoColorCard : MonoBehaviour
{
    [Header("Grid Size")]
    [Min(1)] public int columns = 12;   // Hue steps (or grayscale steps in the grayscale row)
    [Min(1)] public int rows = 6;       // Saturation steps (COLOR rows only, excludes grayscale row)
    [Min(1)] public int valueRows = 4;  // Value steps per saturation band (COLOR rows only)

    [Header("Button Prefab")]
    public Button buttonPrefab;

    [Header("Colors")]
    [Range(0f, 1f)] public float alpha = 1f;

    [Tooltip("Add a single grayscale row (S=0) with V sweeping from black to white across the columns.")]
    public bool addGrayscaleRow = true;

    [Tooltip("If true, the grayscale row is placed at the top; otherwise at the bottom.")]
    public bool grayscaleOnTop = true;

    [Tooltip("Minimum saturation used for COLORED rows (avoid near-gray filler).")]
    [Range(0f, 1f)] public float minSForColorRows = 0.15f;

    [Tooltip("Minimum value for COLORED rows (avoid black filler).")]
    [Range(0f, 1f)] public float minVForColorRows = 0.15f;

    [Tooltip("Maximum value for COLORED rows (avoid pure white).")]
    [Range(0f, 1f)] public float maxVForColorRows = 0.95f;

    [Header("Editor Behavior")]
    [Tooltip("Rebuilds the grid in the Editor whenever values change.")]
    public bool autoRegenerateInEditor = true;

    [Tooltip("Enforces GridLayoutGroup.FixedColumnCount = columns.")]
    public bool enforceGridColumns = true;

    private GridLayoutGroup grid;

    void OnEnable()
    {
        grid = GetComponent<GridLayoutGroup>();
        if (!grid)
        {
            Debug.LogError($"{nameof(AutoColorCard)} requires a GridLayoutGroup on the same GameObject.");
            return;
        }

        if (enforceGridColumns)
        {
            grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            grid.constraintCount = Mathf.Max(1, columns);
        }

        if (buttonPrefab != null)
        {
            if (Application.isPlaying)
                Regenerate();
#if UNITY_EDITOR
            else if (autoRegenerateInEditor)
                RegenerateDelayed();
#endif
        }
    }

#if UNITY_EDITOR
    void OnValidate()
    {
        if (!isActiveAndEnabled) return;

        grid = GetComponent<GridLayoutGroup>();
        if (grid && enforceGridColumns)
        {
            grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            grid.constraintCount = Mathf.Max(1, columns);
        }

        // Clamp ranges so min <= max
        maxVForColorRows = Mathf.Clamp01(maxVForColorRows);
        minVForColorRows = Mathf.Clamp(minVForColorRows, 0f, maxVForColorRows);
        minSForColorRows = Mathf.Clamp01(minSForColorRows);

        if (buttonPrefab != null && autoRegenerateInEditor)
            RegenerateDelayed();
    }

    void RegenerateDelayed()
    {
        EditorApplication.delayCall -= Regenerate;
        EditorApplication.delayCall += Regenerate;
    }
#endif

    [ContextMenu("Generate Buttons")]
    public void Regenerate()
    {
        if (buttonPrefab == null)
        {
            Debug.LogWarning("Assign a Button prefab before generating.");
            return;
        }
        if (this == null) return;
        // Clear children safely
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            var go = transform.GetChild(i).gameObject;
#if UNITY_EDITOR
            if (!Application.isPlaying) DestroyImmediate(go);
            else Destroy(go);
#else
            Destroy(go);
#endif
        }

        int satSteps = Mathf.Max(1, rows);
        int valSteps = Mathf.Max(1, valueRows);

        int extra = addGrayscaleRow ? 1 : 0;
        int totalRows = satSteps * valSteps + extra;

        if (grid == null) grid = GetComponent<GridLayoutGroup>();
        if (grid && enforceGridColumns)
        {
            grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            grid.constraintCount = Mathf.Max(1, columns);
        }

        // Build rows
        for (int r = 0; r < totalRows; r++)
        {
            bool isGrayRow = addGrayscaleRow && ((grayscaleOnTop && r == 0) || (!grayscaleOnTop && r == totalRows - 1));

            if (isGrayRow)
            {
                // Single grayscale row: S=0, V sweeps across columns (black -> white)
                for (int c = 0; c < columns; c++)
                {
                    float p = (columns == 1) ? 1f : (float)c / (columns - 1);
                    Color col = Color.HSVToRGB(0f, 0f, p, true); // hue ignored when S=0
                    col.a = alpha;

                    var btn = Instantiate(buttonPrefab, transform);
                    btn.name = $"Btn_Gray_V{Mathf.RoundToInt(p * 100f)}";
                    var img = btn.GetComponent<Image>();
                    if (img) img.color = col;
                }
                continue;
            }

            // Colored rows (avoid extremes to reduce black/white filler)
            // Map this visual row index to saturation + value band indices
            int offset = addGrayscaleRow && grayscaleOnTop ? 1 : 0; // shift if gray row is on top
            int rr = r - offset;
            if (addGrayscaleRow && !grayscaleOnTop)
            {
                // gray at bottom: no shift needed
                rr = r;
            }
            if (addGrayscaleRow && !grayscaleOnTop && r == totalRows - 1)
                continue; // already handled gray row above

            int satIndex = rr / valSteps;
            int valIndex = rr % valSteps;

            float s = (satSteps == 1)
                ? Mathf.Max(0.0001f, minSForColorRows)
                : Mathf.Lerp(minSForColorRows, 1f, (float)satIndex / (satSteps - 1));

            float v = (valSteps == 1)
                ? Mathf.Clamp01((minVForColorRows + maxVForColorRows) * 0.5f)
                : Mathf.Lerp(minVForColorRows, maxVForColorRows, (float)valIndex / (valSteps - 1));

            for (int c = 0; c < columns; c++)
            {
                // go [0 .. 1) instead of [0 .. 1]
                float hue01 = (columns == 1) ? 0f : (float)c / columns;

                Color col = Color.HSVToRGB(hue01, s, v, true);
                col.a = alpha;

                var btn = Instantiate(buttonPrefab, transform);
                btn.name =
                    $"Btn_H{Mathf.RoundToInt(hue01 * 360f)}_S{Mathf.RoundToInt(s * 100f)}_V{Mathf.RoundToInt(v * 100f)}";

                var img = btn.GetComponent<Image>();
                if (img) img.color = col;
            }

        }
    }
}
