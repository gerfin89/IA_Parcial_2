using UnityEngine;
using UnityEngine.UI;

public class DangerOverlayEffect : MonoBehaviour
{
    [SerializeField] private Image dangerImage;
    [SerializeField] private float pulseSpeed = 4f;
    [SerializeField] private float minAlpha = 0.15f;
    [SerializeField] private float maxAlpha = 0.65f;

    private bool isDangerActive;

    private void Update()
    {
        float targetAlpha = 0f;

        if (isDangerActive)
        {
            float pulse = (Mathf.Sin(Time.time * pulseSpeed) + 1f) / 2f;
            targetAlpha = Mathf.Lerp(minAlpha, maxAlpha, pulse);
        }

        Color color = dangerImage.color;
        color.a = Mathf.MoveTowards(
            color.a,
            targetAlpha,
            Time.deltaTime * 2f
        );

        dangerImage.color = color;
    }

    public void SetDanger(bool active)
    {
        isDangerActive = active;
    }
    private int activeThreats;

    public void AddThreat()
    {
        activeThreats++;
        SetDanger(activeThreats > 0);
    }

    public void RemoveThreat()
    {
        activeThreats = Mathf.Max(0, activeThreats - 1);
        SetDanger(activeThreats > 0);
    }
}