using UnityEngine;

namespace Le0der.Toolkits.Adjust
{
    [RequireComponent(typeof(Camera))]
    public class DynamicViewport : MonoBehaviour
    {
        // [Header("是否适配安全区域")]
        // [SerializeField] private bool fitToSafeArea = false;

        [Header("目标分辨率")]
        [SerializeField] private Vector2 targetResolution = new Vector2(1920, 1080); // 目标分辨率


        private Camera cam;

        void Awake()
        {
            cam = GetComponent<Camera>();
        }

        void Start()
        {
            UpdateViewport();
        }

        void OnRectTransformDimensionsChange()
        {
            UpdateViewport();
        }

        void UpdateViewport()
        {
            float screenWidth = Screen.width;
            float screenHeight = Screen.height;

            float screenAspect = screenWidth / screenHeight;
            float targetAspect = targetResolution.x / targetResolution.y;

            float viewWidth, viewHeight;
            float offsetX = 0f, offsetY = 0f;

            if (screenAspect > targetAspect)
            {
                // 屏幕更宽 → 缩小高度，左右居中
                viewHeight = 1f;
                viewWidth = targetAspect / screenAspect;
                offsetX = (1f - viewWidth) / 2f;
            }
            else
            {
                // 屏幕更高 → 缩小宽度，上下居中
                viewWidth = 1f;
                viewHeight = screenAspect / targetAspect;
                offsetY = (1f - viewHeight) / 2f;
            }

            cam.rect = new Rect(offsetX, offsetY, viewWidth, viewHeight);
        }
    }
}