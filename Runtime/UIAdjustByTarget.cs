using UnityEngine;
using UnityEngine.EventSystems; // 需要这个才能继承 UIBehaviour

namespace Le0der.Toolkits.Adjust
{
    public class UIAdjustByTarget : UIBehaviour
    {
        [SerializeField] private RectTransform parentRect;
        [SerializeField] private RectTransform targetRect;
        [SerializeField] private AdjustType AdjustType;

        private Vector2 _designSize;
        private Vector2 _lastParentSize;

        protected override void Start()
        {
            base.Start();

            if (targetRect == null || parentRect == null)
            {
                Debug.LogError("UIAdjustByTarget: RectTransform references are not assigned.");
                enabled = false;
                return;
            }

            _designSize = targetRect.sizeDelta;
            _lastParentSize = parentRect.rect.size;

            ApplyAdjust();
        }

        protected override void OnRectTransformDimensionsChange()
        {
            base.OnRectTransformDimensionsChange();

            if (parentRect == null || targetRect == null) return;

            if (_designSize == Vector2.zero) return;

            Vector2 currentSize = parentRect.rect.size;

            if (_lastParentSize != currentSize)
            {
                _lastParentSize = currentSize;
                ApplyAdjust();
            }
        }

        private void ApplyAdjust()
        {
            float scaleX = parentRect.rect.width / _designSize.x;
            float scaleY = parentRect.rect.height / _designSize.y;

            float scale = AdjustType switch
            {
                AdjustType.Width => scaleX,
                AdjustType.Height => scaleY,
                AdjustType.WidthOrHeight => Mathf.Min(scaleX, scaleY),
                _ => 1f
            };

            targetRect.localScale = Vector3.one * scale;

            Debug.Log("同步适配");
        }
    }

    public enum AdjustType
    {
        Width,
        Height,
        WidthOrHeight,
    }

}