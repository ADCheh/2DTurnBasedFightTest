using System.Collections.Generic;
using Infrastructure.Services;
using Infrastructure.States;
using UnityEngine;
using UnityEngine.Events;

namespace Infrastructure.Battle
{
    public interface IBattleController : IService
    {
        UnityEvent FightHandled { get; }
        List<GameObject> PlayerCharacters { get; set; }
        List<GameObject> EnemyCharacters { get; set; }
        void InitPlayerCharacters(List<GameObject> playerCharacters);
        void InitEnemyCharacters(List<GameObject> enemyCharacters);
        void GetFightPositions();
        GameObject GetPlayerCharacter();
        GameObject GetEnemyCharacter();
        void SetActivePlayerCharacter(GameObject character);
        void SetActiveEnemyCharacter(GameObject character);
        void ClearActiveCharacters();
        void EnableEnemyCharacters();

        void HandleFight(IExitableState currentState);
    }
}