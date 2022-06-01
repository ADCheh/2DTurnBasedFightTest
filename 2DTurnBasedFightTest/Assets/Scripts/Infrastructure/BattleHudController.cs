using System;
using Infrastructure.Services;
using UnityEngine;
using UnityEngine.UI;

namespace Infrastructure
{
    public class BattleHudController : MonoBehaviour
    {
        public Button AttackButton;
        public Button SkipTurnButton;
        public GameObject FightCurtain;

        public void SetFightCurtain(bool isActive)
        {
            FightCurtain.SetActive(isActive);
        }

        public void EnablePlayerButtons(bool isInteractable)
        {
            AttackButton.interactable = isInteractable;
            SkipTurnButton.interactable = isInteractable;
        }
    }
}