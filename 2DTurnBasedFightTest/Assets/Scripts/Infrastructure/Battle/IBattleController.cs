using System.Collections.Generic;
using Infrastructure.Services;
using Infrastructure.States;
using UnityEngine;
using UnityEngine.Events;

namespace Infrastructure.Battle
{
    public interface IBattleController : IService
    {
        int RoundCounter { get; set; }
        UnityEvent FightHandled { get; }
        List<GameObject> PlayerCharacters { get; set; }
        List<GameObject> EnemyCharacters { get; set; }
        void InitPlayerCharacters(List<GameObject> playerCharacters);
        void InitEnemyCharacters(List<GameObject> enemyCharacters);
        void GetFightPositions();
        GameObject GetPlayerCharacter(IExitableState state);
        GameObject GetEnemyCharacter();
        void SetActivePlayerCharacter(GameObject character);
        void SetActiveEnemyCharacter(GameObject character);
        void ClearActiveCharacters();
        void SwitchEnemyColliders(bool isActive);

        void HandleFight(IExitableState currentState);
    }
}