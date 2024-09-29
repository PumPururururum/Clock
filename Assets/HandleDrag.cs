using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class HandleDrag : MonoBehaviour
{
    [SerializeField]
    private Canvas canvas;
    private UserClockSetter clockSetter;

    private void Start()
    {
        clockSetter = GameObject.Find("ClockSetter").GetComponent<UserClockSetter>();
    }
    public void DragHandler(BaseEventData data)
    {
        if(clockSetter.settingTime)
        {

            PointerEventData pointerData = (PointerEventData)data;

            Vector2 position;
            RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)canvas.transform, pointerData.position, canvas.worldCamera, out position);

            float angle = Mathf.Atan2(position.y, position.x) * Mathf.Rad2Deg - 90f;

            transform.DORotate(new Vector3(0f, 0f, angle), 0, RotateMode.Fast);
        }
    }
}
