using System.Collections;
using UnityEngine;

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
    [SerializeField] private RecallStates _recallState;

    [SerializeField] private LineController _lineController;

    private RecallRevelationData _recallRevelationData;
    private RecallConclusionData _recallConclusionData;

    [Header("[CONCLUSION]")]
    private int _startIdx, _midIdx, _endIdx;
    private string playerConclusion;

    // Start is called before the first frame update
    void Start()
    {
        _recallRevelationData = GameManager.instance.currentRevelationData;
        _recallConclusionData = GameManager.instance.currentConclusionData;

        _recallUI = GetComponent<MemoryRecallUI>();
        ChangeState(RecallStates.RECALL_REVELATION);
    }

    // Update is called once per frame
    void Update()
    {
        if (_recallState == RecallStates.RECALL_REVELATION)
        {
            _lineController.UpdateLine();

            if (_lineController.isMatched)
                ChangeState(RecallStates.RECALL_CONCLUSION);
        }
    }

    public void ChangeState(RecallStates state)
    {
        _recallState = state;
        SetUI(_recallState);
    }

    private void SetUI(RecallStates state)
    {
        StartCoroutine(_recallUI.ToggleUI(state));
        SetText(state);
    }

    public void SetText(RecallStates state)
    {
        switch (state)
        {
            case RecallStates.RECALL_REVELATION:
                _recallUI.memoryPieceTextBox.text = _recallRevelationData.memoryPieceText;
                for (int i = 0; i < _recallUI.inferenceTexts.Length; i++)
                    _recallUI.inferenceTexts[i].text = _recallRevelationData.inferenceTexts[i];
                break;
            case RecallStates.RECALL_CONCLUSION:
                _recallUI.conclusionTexts[0].text = _recallConclusionData.conclusionStartOptions[Random.Range(0, _recallConclusionData.conclusionStartOptions.Length)];
                _recallUI.conclusionTexts[1].text = _recallConclusionData.conclusionMiddleOptions[Random.Range(0, _recallConclusionData.conclusionMiddleOptions.Length)];
                _recallUI.conclusionTexts[2].text = _recallConclusionData.conclusionEndOptions[Random.Range(0, _recallConclusionData.conclusionEndOptions.Length)];
                break;
        }
    }

    #region CONCLUSION PHASE
    public void OnConfirmButton()
    {
        if (CheckConclusion())
            StartCoroutine(CompleteStage());
        else
        {
            // instead of using SetUI, which randomises the text, we want to keep the text the same
            // hence we'll use _recallUI.ToggleUI instead.
            StartCoroutine(_recallUI.ToggleUI(RecallStates.RECALL_CONCLUSION));
        }
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

    private IEnumerator CompleteStage()
    {
        ChangeState(RecallStates.RECALL_COMPLETE);

        AudioManager.instance.PlayClip(AudioManager.instance.BGMSource, AudioManager.instance.memoryRecallCompleteJingle);

        yield return new WaitForSeconds(AudioManager.instance.memoryRecallCompleteJingle.length);

        GameManager.instance.ChangeState(GameState.GAME_ENDING); // for beta purposes
    }
    #endregion
}