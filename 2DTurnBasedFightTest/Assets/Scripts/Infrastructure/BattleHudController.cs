using System;
using Infrastructure.Services;
using UnityEngine;
using UnityEngine.UI;

namespace Infrastructure
{
    public class BattleHudController : MonoBehaviour, IService
    {
        public Button AttackButton;
        public Button SkipTurnButton;
        public GameObject FightCurtain;

        public void Start()
        {
            AllServices.Container.RegisterSingle<BattleHudController>(this);
        }

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