using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

public class DragAndDrop : MonoBehaviour
{
    private bool isDragging = false;
    private Vector3 offset;
    private Camera mainCamera;
    private float snapThreshold;
    private bool isSnapped = false;

    private void Start()
    {
        mainCamera = Camera.main;
        snapThreshold = 0.9f;
    }

    void OnMouseDown()
    {
        if (!isSnapped)
        {
            isDragging = true;
            gameObject.GetComponent<SortingGroup>().sortingOrder = 5;
        }
    }

    void OnMouseUp()
    {
        isDragging = false;

        if (!isSnapped)
        {
            // Zapisz offset mi�dzy pozycj� obiektu a pozycj� kursora
            offset = transform.position - GetMouseWorldPosition();

            Vector3 targetPosition = GetComponent<Puzzle>().correctPosition;

            // Sprawd�, czy puzzel jest blisko docelowej pozycji
            if (Vector3.Distance(transform.position, targetPosition) <= snapThreshold)
            {
                gameObject.GetComponent<SortingGroup>().sortingOrder = 1;
                // Ustawienie puzzla dok�adnie na docelowej pozycji
                transform.position = targetPosition;
                isSnapped = true;
                GameManager.Instance.SpawnRandomPuzzle();
                AudioManager.Instance.Play(AudioManager.SoundName.SnapSound);
            }
        }
    }

    void Update()
    {
        if (isDragging)
        {
            // Przemieszczanie obiektu w �lad za myszk�
            transform.position = GetMouseWorldPosition() + offset;
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = mainCamera.WorldToScreenPoint(transform.position).z; // Z odleg�o�ci� Z obiektu
        return mainCamera.ScreenToWorldPoint(mousePoint);
    }

}
