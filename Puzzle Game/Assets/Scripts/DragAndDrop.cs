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
    [SerializeField] private ParticleSystem blowPS;
    private bool wasPlayed = false;
    private bool dragingEnabled;

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
            offset = transform.position - GetMouseWorldPosition();

            Vector3 targetPosition = GetComponent<Puzzle>().correctPosition;

            if (Vector3.Distance(transform.position, targetPosition) <= snapThreshold)
            {
                gameObject.GetComponent<SortingGroup>().sortingOrder = 1;
                transform.position = targetPosition;
                isSnapped = true;
                PuzzleManager.Instance.SpawnRandomPuzzle();
                AudioManager.Instance.Play(AudioManager.SoundName.SnapSound);
                if(!wasPlayed)
                {
                    blowPS.Play();
                    wasPlayed = true;
                }
                    
            }
        }
    }

    void Update()
    {
        if (isDragging)
        {
            transform.position = GetMouseWorldPosition() + offset;
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = mainCamera.WorldToScreenPoint(transform.position).z;
        return mainCamera.ScreenToWorldPoint(mousePoint);
    }

}
