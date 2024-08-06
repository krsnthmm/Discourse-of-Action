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
    [SerializeField] private MemoryRecallData _recallData;
    [SerializeField] private RecallStates _recallState;

    [SerializeField] private LineController _lineController;

    private int _revelationIndex;

    [Header("[CONCLUSION]")]
    private int _startIdx, _midIdx, _endIdx;
    private string playerConclusion;

    // Start is called before the first frame update
    void Start()
    {
        _recallData = GameManager.instance.currentRecallData;

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
            {
                _lineController.isMatched = false;
                _revelationIndex++;

                if (_revelationIndex >= _recallData.revelationDatas.Length)
                    ChangeState(RecallStates.RECALL_CONCLUSION);
                else
                    ChangeState(RecallStates.RECALL_REVELATION);
            }
        }
    }

    public void ChangeState(RecallStates state)
    {
        if (state == RecallStates.RECALL_REVELATION)
        {
            if (!_recallData.revelationDatas[_revelationIndex].isEverythingCorrect)
            {
                _lineController.isEverythingCorrect = false;
                _lineController.SetCorrectNodeID(_recallData.revelationDatas[_revelationIndex].correctInference);
            }
            else
                _lineController.isEverythingCorrect = true;
        }

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
                _recallUI.memoryPieceTextBox.text = _recallData.revelationDatas[_revelationIndex].memoryPieceText;
                for (int i = 0; i < _recallUI.inferenceTexts.Length; i++)
                    _recallUI.inferenceTexts[i].text = _recallData.revelationDatas[_revelationIndex].inferenceTexts[i];
                break;
            case RecallStates.RECALL_CONCLUSION:
                _recallUI.conclusionTexts[0].text = _recallData.conclusionData.conclusionStartOptions[Random.Range(0, _recallData.conclusionData.conclusionStartOptions.Length)];
                _recallUI.conclusionTexts[1].text = _recallData.conclusionData.conclusionMiddleOptions[Random.Range(0, _recallData.conclusionData.conclusionMiddleOptions.Length)];
                _recallUI.conclusionTexts[2].text = _recallData.conclusionData.conclusionEndOptions[Random.Range(0, _recallData.conclusionData.conclusionEndOptions.Length)];
                break;
        }
    }

    #region CONCLUSION PHASE
    public void OnConfirmButton()
    {
        AudioManager.instance.PlayClip(AudioManager.instance.SFXSource, AudioManager.instance.buttonSFX);

        if (CheckConclusion())
            StartCoroutine(CompleteStage());
        else
        {
            // instead of using SetUI, which randomises the text options with SetText, we want to keep the text the same
            // hence we'll use _recallUI.ToggleUI instead.
            StartCoroutine(_recallUI.ToggleUI(RecallStates.RECALL_CONCLUSION));
        }
    }

    public void OnSelectUp(int index)
    {
        string text = "";

        AudioManager.instance.PlayClip(AudioManager.instance.SFXSource, AudioManager.instance.buttonSFX);

        switch (index)
        {
            case 0:
                if (_startIdx < _recallData.conclusionData.conclusionStartOptions.Length - 1)
                    _startIdx++;
                else
                    _startIdx = 0;

                text = _recallData.conclusionData.conclusionStartOptions[_startIdx];
                break;
            case 1:
                if (_midIdx < _recallData.conclusionData.conclusionMiddleOptions.Length - 1)
                    _midIdx++;
                else
                    _midIdx = 0;

                text = _recallData.conclusionData.conclusionMiddleOptions[_midIdx];
                break;
            case 2:
                if (_endIdx < _recallData.conclusionData.conclusionEndOptions.Length - 1)
                    _endIdx++;
                else
                    _endIdx = 0;

                text = _recallData.conclusionData.conclusionEndOptions[_endIdx];
                break;
        }

        _recallUI.SetText(_recallUI.conclusionTexts[index], text);
    }

    public void OnSelectDown(int index)
    {
        string text = "";

        AudioManager.instance.PlayClip(AudioManager.instance.SFXSource, AudioManager.instance.buttonSFX);

        switch (index)
        {
            case 0:
                if (_startIdx > 0)
                    _startIdx--;
                else
                    _startIdx = _recallData.conclusionData.conclusionStartOptions.Length - 1;

                text = _recallData.conclusionData.conclusionStartOptions[_startIdx];
                break;
            case 1:
                if (_midIdx > 0)
                    _midIdx--;
                else
                    _midIdx = _recallData.conclusionData.conclusionMiddleOptions.Length - 1;

                text = _recallData.conclusionData.conclusionMiddleOptions[_midIdx];
                break;
            case 2:
                if (_endIdx > 0)
                    _endIdx--;
                else
                    _endIdx = _recallData.conclusionData.conclusionEndOptions.Length - 1;

                text = _recallData.conclusionData.conclusionEndOptions[_endIdx];
                break;
        }

        _recallUI.SetText(_recallUI.conclusionTexts[index], text);
    }

    public bool CheckConclusion()
    {
        playerConclusion = _recallUI.conclusionTexts[0].text + " " + _recallUI.conclusionTexts[1].text + " " + _recallUI.conclusionTexts[2].text;

        if (playerConclusion == _recallData.conclusionData.correctConclusion)
            return true;
        else
            return false;
    }

    private IEnumerator CompleteStage()
    {
        ChangeState(RecallStates.RECALL_COMPLETE);

        AudioManager.instance.PlayClip(AudioManager.instance.BGMSource, AudioManager.instance.memoryRecallCompleteJingle);
        GameManager.instance.currentLevel++;

        yield return new WaitForSeconds(AudioManager.instance.memoryRecallCompleteJingle.length);

        AudioManager.instance.StopClip(AudioManager.instance.BGMSource);

        if (GameManager.instance.enemyToBattle.enemyType != EnemyData.EnemyType.ENEMY_BOSS_IRIS_PHASE1)
        {
            GameManager.instance.ChangeState(GameState.GAME_OVERWORLD);
            GameManager.instance.playerController.transform.position = GameManager.instance.playerSpawn.position;
        }
        else
        {
            GameManager.instance.enemyToBattle.enemyType = EnemyData.EnemyType.ENEMY_BOSS_IRIS_PHASE2;
            GameManager.instance.ChangeState(GameState.GAME_BATTLE);
        }
    }
    #endregion
}