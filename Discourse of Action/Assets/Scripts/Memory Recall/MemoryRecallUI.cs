using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MemoryRecallUI : MonoBehaviour
{
    [Header("UI GROUPS")]
    [SerializeField] private GameObject _revelationGroup;
    [SerializeField] private GameObject _revelationNodes;
    [SerializeField] private GameObject _conclusionGroup;
    [SerializeField] private GameObject _recallCompleteGO;

    [Header("[UI ELEMENTS]")]
    [SerializeField] private SpriteRenderer _backgroundImage;
    public TMP_Text memoryPieceTextBox;
    public TMP_Text[] inferenceTexts;
    public TMP_Text[] conclusionTexts;

    [Header("SPRITE ASSETS")]
    [SerializeField] private Sprite _startImage;
    [SerializeField] private Sprite _completeImage;

    public IEnumerator ToggleUI(RecallStates state)
    {
        _revelationGroup.SetActive(false);
        _revelationNodes.SetActive(false);
        _conclusionGroup.SetActive(false);

        yield return new WaitForSeconds(2);

        switch (state)
        {
            case RecallStates.RECALL_REVELATION:
                _revelationGroup.SetActive(true);
                _revelationNodes.SetActive(true);
                _conclusionGroup.SetActive(false);

                _backgroundImage.sprite = _startImage;

                break;
            case RecallStates.RECALL_CONCLUSION:
                _revelationGroup.SetActive(false);
                _revelationNodes.SetActive(false);
                _conclusionGroup.SetActive(true);

                _backgroundImage.sprite = _startImage;

                break;
            case RecallStates.RECALL_COMPLETE:
                _recallCompleteGO.SetActive(true);

                yield return new WaitForSeconds(1);

                _backgroundImage.sprite = _completeImage;

                yield return new WaitForSeconds(2);

                GameManager.instance.ChangeState(GameState.GAME_OVERWORLD);
                break;
        }
    }

    public void SetText(TMP_Text target, string text)
    {
        target.text = text;
    }
}
