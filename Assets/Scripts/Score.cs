using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{
    public int Id;

    [SerializeField]
    private Color _activeColor, _inActiveColor, _targetColor;

    [SerializeField]
    private SpriteRenderer _sr;

    private void OnEnable()
    {
        GameManager.UpdateColor += OnTargetSet;
        GameManager.UpdateMoveColor += OnMoveSet;
    }

    private void OnDisable()
    {
        GameManager.UpdateColor -= OnTargetSet;
        GameManager.UpdateMoveColor -= OnMoveSet;
    }

    private void OnTargetSet(int targetId)
    {
        _sr.color = targetId == Id ? _targetColor : _inActiveColor;
    }

    private void OnMoveSet(int moveId)
    {
        _sr.color = moveId == Id ? _activeColor : _sr.color;
    }
}
