using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPointManager : MonoBehaviour
{
    public List<KeyPoint> onDisplay = new();
    public KeyPointDeck deck;

    public bool[] availableDisplaySlots;

    public KeyPoint selectedKeyPoint;

    public void PutOnDisplay()
    {
        KeyPointData randData = deck.listOfData[Random.Range(0, deck.listOfData.Count)];

        for (int i = 0; i < availableDisplaySlots.Length; i++)
        {
            if (availableDisplaySlots[i] == true)
            {
                onDisplay[i].keyPointData = randData;
                onDisplay[i].displayIndex = i;
                onDisplay[i].gameObject.SetActive(true);

                availableDisplaySlots[i] = false;

                break;
            }
        }
    }

    public void RemoveFromDisplay(KeyPoint target)
    {
        if (target.isActiveAndEnabled)
        {
            availableDisplaySlots[target.displayIndex] = true;

            target.gameObject.SetActive(false);
        }
    }

    public void SetTarget(KeyPoint selected = null)
    {
        for (int i = 0; i < onDisplay.Count; i++)
        {
            if (selectedKeyPoint == null)
            {
                selectedKeyPoint = onDisplay[0];
                onDisplay[0].target.SetActive(true);
            }
            else
            {
                if (onDisplay[i] == selected)
                {
                    selectedKeyPoint = onDisplay[i];
                    onDisplay[i].target.SetActive(true);
                }
                else
                    onDisplay[i].target.SetActive(false);
            }
        }
    }

    public void ClearDisplay()
    {
        for (int i = 0; i < onDisplay.Count; i++)
            RemoveFromDisplay(onDisplay[i]);
    }
}
