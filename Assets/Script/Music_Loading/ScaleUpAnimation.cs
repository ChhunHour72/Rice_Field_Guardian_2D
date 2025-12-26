using UnityEngine;
using UnityEngine.UI; // Only needed if you're attaching to UI Image/Text, otherwise remove this line

[RequireComponent(typeof(RectTransform))] // For UI elements
public class ScaleUpAnimation : MonoBehaviour
{
    [Header("Animation Settings")]
    [SerializeField] private float targetScale = 1f;         // Final scale (usually 1)
    [SerializeField] private float animationDuration = 0.5f; // How long the animation takes
    [SerializeField] private AnimationCurve easeCurve = AnimationCurve.EaseInOut(0, 0, 1, 1); // Smooth easing

    [Header("Options")]
    [SerializeField] private bool playOnStart = true;        // Play automatically when scene starts
    [SerializeField] private bool playOnEnable = true;       // Play every time the object is enabled (e.g., when panel appears)

    private RectTransform rectTransform;
    private Vector3 originalScale;
    private Coroutine animationCoroutine;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        originalScale = rectTransform.localScale;
    }

    private void Start()
    {
        if (playOnStart)
        {
            PlayScaleUp();
        }
    }

    private void OnEnable()
    {
        if (playOnEnable)
        {
            PlayScaleUp();
        }
    }

    /// <summary>
    /// Call this method to trigger the scale-up animation manually (e.g., from a button or another script)
    /// </summary>
    public void PlayScaleUp()
    {
        if (animationCoroutine != null)
        {
            StopCoroutine(animationCoroutine);
        }

        animationCoroutine = StartCoroutine(ScaleUpCoroutine());
    }

    private System.Collections.IEnumerator ScaleUpCoroutine()
    {
        // Start from small scale
        rectTransform.localScale = Vector3.zero;

        float elapsedTime = 0f;

        while (elapsedTime < animationDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / animationDuration);
            float curveValue = easeCurve.Evaluate(t);

            Vector3 currentScale = Vector3.LerpUnclamped(Vector3.zero, originalScale * targetScale, curveValue);
            rectTransform.localScale = currentScale;

            yield return null;
        }

        // Ensure it ends exactly at target
        rectTransform.localScale = originalScale * targetScale;

        animationCoroutine = null;
    }

    /// <summary>
    /// Optional: Reset to original scale (useful if you want to replay the animation later)
    /// </summary>
    public void ResetScale()
    {
        rectTransform.localScale = originalScale;
    }
}