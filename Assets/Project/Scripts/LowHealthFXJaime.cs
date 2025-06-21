using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// script animation that makes the player vision worse as sanity goes down
/// it toggles the visibility of a series of panels/frames overlayed
/// making a contiunous fx of a blinking reddened view
/// </summary>
public class LowHealthFXJaime : MonoBehaviour
{
    [SerializeField] private float spreadFactor = 0.02f;
    [SerializeField] private float maxAlpha = 1f;
    [SerializeField] private Image[] VfxPanes;
    void Update()
    {
        float step = GameManager.Instance.maxSanityLevel / VfxPanes.Length;

        for (int i = 0; i < VfxPanes.Length; i++)
        {
            var pane = VfxPanes[i];

            float xSpike = step * i;

            float value = - spreadFactor * Mathf.Pow(GameManager.Instance.currentSanityLevel - xSpike, 2f) + maxAlpha;

            pane.color = new Color(1f, 1f, 1f, Mathf.Clamp(value, 0, 1f));
        }

    }
}
