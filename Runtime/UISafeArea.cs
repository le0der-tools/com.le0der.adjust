using UnityEngine;
using UnityEngine.Events;

namespace Le0der.Toolkits.Adjust
{
    /// <summary>
    /// 安全区管理器：用于适配全面屏（刘海屏、水滴屏等）
    /// </summary>
    public class UISafeArea : MonoBehaviour
    {
        private Canvas canvas;
        private RectTransform rectTf;


        private Rect lastSafeArea;
        private Vector2 lastScreenSize;
        private ScreenOrientation lastOrientation;

        /// <summary>
        /// 安全区变化事件（外部可以监听）
        /// </summary>
        public static event UnityAction<Rect> OnSafeAreaChanged;

        private void Awake()
        {
            canvas = GetComponentInParent<Canvas>(); // 获取关联的Canvas
            rectTf = GetComponent<RectTransform>();

            ApplySafeArea(Screen.safeArea, new Vector2(Screen.width, Screen.height));
            CacheCurrent();
        }

        private void Update()
        {
            // 只在方向变化时检查安全区
            if (Screen.orientation != lastOrientation)
            {
                // 详细检查变化
                if (CheckForChanges())
                {
                    // 应用新的安全区
                    var screenSize = new Vector2(Screen.width, Screen.height);
                    ApplySafeArea(Screen.safeArea, screenSize);

                    // 缓存当前状态
                    CacheCurrent();
                }
            }
        }

        /// <summary>
        /// 检查屏幕安全区、分辨率或方向是否发生变化
        /// </summary>
        private bool CheckForChanges()
        {
            var safeArea = Screen.safeArea;
            var screenSize = new Vector2(Screen.width, Screen.height);
            var orientation = Screen.orientation;

            // 即使方向变化了，也检查安全区和屏幕尺寸是否真的变化了
            if (safeArea.Equals(lastSafeArea) &&
                screenSize.Equals(lastScreenSize) &&
                orientation == lastOrientation)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 应用安全区域到当前 RectTransform
        /// </summary>
        private void ApplySafeArea(Rect safeArea, Vector2 screenSize)
        {
            var anchorMin = safeArea.position;
            var anchorMax = safeArea.size;
            anchorMin.x /= screenSize.x;
            anchorMin.y /= screenSize.y;
            anchorMax.x /= screenSize.x;
            anchorMax.y /= screenSize.y;

            rectTf.anchoredPosition = Vector2.zero;
            rectTf.sizeDelta = Vector2.zero;
            rectTf.anchorMin = IsFinite(anchorMin) ? anchorMin : Vector2.zero;
            rectTf.anchorMax = IsFinite(anchorMax) ? anchorMax : Vector2.one;

            OnSafeAreaChanged?.Invoke(safeArea);
        }

        /// <summary>
        /// 缓存当前的屏幕状态用于比较
        /// </summary>
        private void CacheCurrent()
        {
            lastSafeArea = Screen.safeArea;
            lastScreenSize = new Vector2(Screen.width, Screen.height);
            lastOrientation = Screen.orientation;
        }

        /// <summary>
        /// 检查向量是否为有限值（非NaN或Infinity）
        /// </summary>
        private bool IsFinite(Vector2 v) => IsFinite(v.x) && IsFinite(v.y);

        /// <summary>
        /// 检查浮点数是否为有限值
        /// </summary>
        private bool IsFinite(float f) => !float.IsNaN(f) && !float.IsInfinity(f);
    }
}