using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum RecallStates
{
    RECALL_REVELATION,
    RECALL_CONCLUSION,
    RECALL_COMPLETE
}

[RequireComponent(typeof(MemoryRecallUI))]
public class MemoryRecallSystem : MonoBehaviour
{
    private MemoryRecallUI _recallUI;

    [SerializeField] private RecallRevelationData[] _recallRevelationsData;
    [SerializeField] private RecallConclusionData _recallConclusionData;

    private int _startIdx, _midIdx, _endIdx;
    private string playerConclusion;

    // Start is called before the first frame update
    void Start()
    {
        _recallUI = GetComponent<MemoryRecallUI>();
        StartCoroutine(_recallUI.ToggleUI(RecallStates.RECALL_REVELATION));
    }

    // Update is called once per frame
    void Update()
    {
        // for testing purposes
        if (Input.GetKey(KeyCode.Alpha1))
        {
            StartCoroutine(_recallUI.ToggleUI(RecallStates.RECALL_CONCLUSION));
            SetText(RecallStates.RECALL_CONCLUSION);
        }
    }

    public void SetText(RecallStates state)
    {
        switch (state)
        {
            case RecallStates.RECALL_REVELATION:
                break;
            case RecallStates.RECALL_CONCLUSION:
                _recallUI.conclusionTexts[0].text = _recallConclusionData.conclusionStartOptions[0];
                _recallUI.conclusionTexts[1].text = _recallConclusionData.conclusionMiddleOptions[0];
                _recallUI.conclusionTexts[2].text = _recallConclusionData.conclusionEndOptions[0];
                break;
        }
    }

    public void OnConfirmButton()
    {
        if (CheckConclusion())
            StartCoroutine(_recallUI.ToggleUI(RecallStates.RECALL_COMPLETE));
        else
            StartCoroutine(_recallUI.ToggleUI(RecallStates.RECALL_CONCLUSION));
    }

    public void OnSelectUp(int index)
    {
        string text = "";

        switch (index)
        {
            case 0:
                if (_startIdx < _recallConclusionData.conclusionStartOptions.Length - 1)
                    _startIdx++;
                else
                    _startIdx = 0;

                text = _recallConclusionData.conclusionStartOptions[_startIdx];
                break;
            case 1:
                if (_midIdx < _recallConclusionData.conclusionMiddleOptions.Length - 1)
                    _midIdx++;
                else
                    _midIdx = 0;

                text = _recallConclusionData.conclusionMiddleOptions[_midIdx];
                break;
            case 2:
                if (_endIdx < _recallConclusionData.conclusionEndOptions.Length - 1)
                    _endIdx++;
                else
                    _endIdx = 0;

                text = _recallConclusionData.conclusionEndOptions[_endIdx];
                break;
        }

        _recallUI.SetText(_recallUI.conclusionTexts[index], text);
    }

    public void OnSelectDown(int index)
    {
        string text = "";

        switch (index)
        {
            case 0:
                if (_startIdx > 0)
                    _startIdx--;
                else
                    _startIdx = _recallConclusionData.conclusionStartOptions.Length - 1;

                text = _recallConclusionData.conclusionStartOptions[_startIdx];
                break;
            case 1:
                if (_midIdx > 0)
                    _midIdx--;
                else
                    _midIdx = _recallConclusionData.conclusionMiddleOptions.Length - 1;

                text = _recallConclusionData.conclusionMiddleOptions[_midIdx];
                break;
            case 2:
                if (_endIdx > 0)
                    _endIdx--;
                else
                    _endIdx = _recallConclusionData.conclusionEndOptions.Length - 1;

                text = _recallConclusionData.conclusionEndOptions[_endIdx];
                break;
        }

        _recallUI.SetText(_recallUI.conclusionTexts[index], text);
    }

    public bool CheckConclusion()
    {
        playerConclusion = _recallUI.conclusionTexts[0].text + " " + _recallUI.conclusionTexts[1].text + " " + _recallUI.conclusionTexts[2].text;

        if (playerConclusion == _recallConclusionData.correctConclusion)
            return true;
        else
            return false;
    }
}