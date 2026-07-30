using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using System.Collections;

public class OrientationChange : MonoBehaviour
{
    public enum OrientationMode
    {
        Landscape,
        DesktopPortrait,
        MobilePortrait
    }

    [Header("UI References")]
    [SerializeField] private RectTransform UIWrapper;
    [SerializeField] private CanvasScaler CanvasScaler;

    [Header("Transition Settings")]
    [SerializeField] private float transitionDuration = 0.2f;
    [SerializeField] private float waitForRotation = 0.2f;

    [Header("Device Detection Settings")]
    [SerializeField] private string mobileKeyword = "mobile";
    [SerializeField] private string currentDevice = "";

    public static event Action<OrientationMode, int, int> OnOrientationChanged;
    public event Action<OrientationMode, int, int> OnOrientationChangedInstance;

    private Vector2 ReferenceAspect;
    private Tween matchTween;
    private Tween rotationTween;
    private Coroutine rotationRoutine;
    private bool isLandscape;
    private OrientationMode currentMode = OrientationMode.Landscape;

    private int lastWidth = 0;
    private int lastHeight = 0;

    public string CurrentDevice => currentDevice;
    public OrientationMode CurrentMode => currentMode;
    public bool IsLandscape => isLandscape;
    public bool IsMobile => IsMobileDevice();

    private void Awake()
    {
        if (CanvasScaler != null)
        {
            ReferenceAspect = CanvasScaler.referenceResolution;
        }
        else
        {
            ReferenceAspect = new Vector2(1920, 1080);
        }
    }

    private void Start()
    {
        ApplyMatch(Screen.width, Screen.height);
    }

    public void DeviceCheck(string device)
    {
        Debug.Log($"[OrientationChange] Device detected: {device}");
        currentDevice = device;
        int w = lastWidth > 0 ? lastWidth : Screen.width;
        int h = lastHeight > 0 ? lastHeight : Screen.height;
        ApplyMatch(w, h);
    }

    public bool IsMobileDevice()
    {
        if (!string.IsNullOrEmpty(currentDevice))
        {
            return currentDevice.ToLower().Contains(mobileKeyword.ToLower());
        }
        return SystemInfo.deviceType == DeviceType.Handheld;
    }

    public void SwitchDisplay(string dimensions)
    {
        if (rotationRoutine != null) StopCoroutine(rotationRoutine);
        rotationRoutine = StartCoroutine(RotationCoroutine(dimensions));
    }

    private IEnumerator RotationCoroutine(string dimensions)
    {
        yield return new WaitForSecondsRealtime(waitForRotation);
        string[] parts = dimensions.Split(',');
        if (parts.Length == 2 && int.TryParse(parts[0], out int width) && int.TryParse(parts[1], out int height) && width > 0 && height > 0)
        {
            ApplyMatch(width, height);
        }
        else
        {
            Debug.LogWarning("Unity: Invalid format received in SwitchDisplay");
        }
    }

    private void ApplyMatch(int width, int height)
    {
        lastWidth = width;
        lastHeight = height;
        isLandscape = width > height;
        bool isMobile = IsMobileDevice();

        if (isLandscape)
        {
            currentMode = OrientationMode.Landscape;
        }
        else if (isMobile)
        {
            currentMode = OrientationMode.MobilePortrait;
        }
        else
        {
            currentMode = OrientationMode.DesktopPortrait;
        }

        // Apply Rotation: DesktopPortrait gets -90 degrees rotation, MobilePortrait & Landscape get 0 degrees.
        Quaternion targetRotation = (currentMode == OrientationMode.DesktopPortrait) ? Quaternion.Euler(0, 0, -90) : Quaternion.identity;
        if (UIWrapper != null)
        {
            if (rotationTween != null && rotationTween.IsActive()) rotationTween.Kill();
            rotationTween = UIWrapper.DOLocalRotateQuaternion(targetRotation, transitionDuration).SetEase(Ease.OutCubic);
        }

        // Calculate CanvasScaler Match Width/Height
        if (CanvasScaler != null)
        {
            Vector2 refRes = CanvasScaler.referenceResolution;
            float refW = refRes.x;
            float refH = refRes.y;

            float widthScale = (float)width / refW;
            float heightScale = (float)height / refH;

            float targetScale;
            if (currentMode == OrientationMode.MobilePortrait)
            {
                float mobileWScale = (float)width / refW;
                float mobileHScale = (float)height / refH;
                targetScale = Mathf.Min(mobileWScale, mobileHScale);
            }
            else if (isLandscape)
            {
                targetScale = Mathf.Min(widthScale, heightScale);
            }
            else // DesktopPortrait
            {
                float portraitWidthScale = (float)height / refW;
                float portraitHeightScale = (float)width / refH;
                targetScale = Mathf.Min(portraitWidthScale, portraitHeightScale);
            }

            float targetMatch;
            if (Mathf.Abs(heightScale - widthScale) < 0.0001f)
            {
                targetMatch = 0.5f;
            }
            else
            {
                float logRatio = Mathf.Log(heightScale / widthScale);
                targetMatch = Mathf.Log(targetScale / widthScale) / logRatio;
                targetMatch = Mathf.Clamp01(targetMatch);
            }

            if (matchTween != null && matchTween.IsActive()) matchTween.Kill();
            matchTween = DOTween.To(() => CanvasScaler.matchWidthOrHeight, x => CanvasScaler.matchWidthOrHeight = x, targetMatch, transitionDuration).SetEase(Ease.InOutQuad);
        }

        Debug.Log($"[OrientationChange] Dimensions: {width}x{height}, Mode: {currentMode}, isLandscape: {isLandscape}, isMobile: {isMobile}");

        // Notify Listeners (including OCController)
        OnOrientationChanged?.Invoke(currentMode, width, height);
        OnOrientationChangedInstance?.Invoke(currentMode, width, height);
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            int w = lastHeight > 0 ? lastHeight : Screen.height;
            int h = lastWidth > 0 ? lastWidth : Screen.width;
            SwitchDisplay(w + "," + h);
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            string nextDevice = (currentDevice.ToLower() == "mobile") ? "desktop" : "mobile";
            DeviceCheck(nextDevice);
        }
    }
#endif
}