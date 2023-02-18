using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_EventHandler : MonoBehaviour, IPointerClickHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
{
	public Action<PointerEventData> OnClickHandler = null;
	public Action<PointerEventData> OnDragHandler = null;
	public Action<PointerEventData> OnBeginDragHandler = null;
	public Action<PointerEventData> OnEndDragHandler = null;

	public void OnPointerClick(PointerEventData eventData) //? pointerId -1 좌클릭 -2 우클릭 -3 휠버튼
	{
		if (OnClickHandler != null)
			OnClickHandler.Invoke(eventData);
	}

	public void OnDrag(PointerEventData eventData)
	{
		if (OnDragHandler != null)
			OnDragHandler.Invoke(eventData);
	}

    public void OnBeginDrag(PointerEventData eventData)
    {
		if (OnBeginDragHandler != null)
			OnBeginDragHandler.Invoke(eventData);
	}

    public void OnEndDrag(PointerEventData eventData)
    {
		if (OnEndDragHandler != null)
			OnEndDragHandler.Invoke(eventData);
	}


	public void Initialization()
    {
		OnClickHandler = null;
		OnDragHandler = null;
		OnBeginDragHandler = null;
		OnEndDragHandler = null;
	}
}
