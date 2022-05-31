using Infrastructure.Battle;
using Infrastructure.Services;
using UnityEngine;

namespace Infrastructure.States
{
    public class PlayerTurnState : IState
    {
        private readonly BattleStateMachine _battleStateMachine;
        private readonly BattleHudController _battleHudController;

        private readonly IBattleController _battleController;


        public PlayerTurnState(BattleStateMachine battleStateMachine)
        {
            _battleStateMachine = battleStateMachine;

            _battleController = AllServices.Container.Single<IBattleController>();
        }
        public void Enter()
        {
            //Выдать перса
            //Включить кнопки
            //
            ConfigureBattleUi();
            _battleController.GetPlayerCharacter();
        }

        public void Exit()
        {
            var battleUI = GameObject.FindWithTag("BattleUI").GetComponent<BattleHudController>(); 
            battleUI.SkipTurnButton.onClick.RemoveAllListeners();
            battleUI.AttackButton.onClick.RemoveAllListeners();
            battleUI.EnablePlayerButtons(false);
            battleUI.SetFightCurtain(false);
            
            _battleController.ClearActiveCharacters();
        }

        private void ConfigureBattleUi()
        {
            var battleUI = GameObject.FindWithTag("BattleUI").GetComponent<BattleHudController>(); 
            battleUI.EnablePlayerButtons(true);
            battleUI.SkipTurnButton.onClick.AddListener(HandleSkipButtonClick);
            battleUI.AttackButton.onClick.AddListener(HandleAttackButtonClick);
        }

        private void HandleSkipButtonClick()
        {
            Debug.Log("Skip clicked!");
            
            _battleStateMachine.Enter<EnemyTurnState>();
        }

        private void HandleAttackButtonClick()
        {
            Debug.Log("Attack clicked!");
            
            _battleController.EnableEnemyCharacters();

            foreach (var enemyCharacter in _battleController.EnemyCharacters)
            {
                enemyCharacter.GetComponent<BoxCollider2D>().enabled = true;
                enemyCharacter.GetComponent<AnimationController>().characterClicked.AddListener(EnemyTargetSelected);
            }
        }

        private void EnemyTargetSelected(GameObject enemyTarget)
        {
            _battleController.SetActiveEnemyCharacter(enemyTarget);
            
            _battleController.FightHandled.AddListener(EndPlayerTurn);
            
            _battleController.HandleFight(this);
        }

        private void EndPlayerTurn()
        {
            _battleStateMachine.Enter<EnemyTurnState>();
        }
    }
}