using System.Collections;
using Infrastructure.Battle;
using Infrastructure.Services;
using UnityEngine;

namespace Infrastructure.States
{
    public class PlayerTurnState : IState
    {
        private readonly BattleStateMachine _battleStateMachine;
        private readonly IBattleController _battleController;
        private readonly ICoroutineRunner _coroutineRunner;
        
        private BattleHudController _battleUI;


        public PlayerTurnState(BattleStateMachine battleStateMachine, ICoroutineRunner coroutineRunner)
        {
            _battleStateMachine = battleStateMachine;
            _coroutineRunner = coroutineRunner;
            _battleController = AllServices.Container.Single<IBattleController>();
        }
        public void Enter()
        {
            //_battleController.ClearActiveCharacters();
            /*foreach (var enemyCharacter in _battleController.EnemyCharacters)
            {
                enemyCharacter.GetComponent<AnimationController>().characterClicked.RemoveAllListeners();
            }*/
            
            if(_battleUI == null)
                _battleUI = GameObject.FindWithTag("BattleUI").GetComponent<BattleHudController>(); 
            
            ConfigureBattleUi();
            _battleController.GetPlayerCharacter();
        }

        public void Exit()
        {
            _battleUI.EnablePlayerButtons(false);
            _battleUI.SetFightCurtain(false);
            
            //_battleController.ClearActiveCharacters();
            //_battleController.FightHandled.RemoveListener(EndPlayerTurn);
        }

        private void ConfigureBattleUi()
        {
            _battleUI.EnablePlayerButtons(true);
            
            
            _battleUI.SkipTurnButton.onClick.AddListener(HandleSkipButtonClick);
            
            _battleUI.AttackButton.onClick.AddListener(HandleAttackButtonClick);
        }

        private void HandleSkipButtonClick()
        {
            Debug.Log("Skip clicked!");
            
            ClearButtonsListeners();

            _battleController.ClearActiveCharacters();
            
           EndPlayerTurn();
        }

        private void HandleAttackButtonClick()
        {
            Debug.Log("Attack clicked!");
            
            ClearButtonsListeners();

            foreach (var character in _battleController.EnemyCharacters)
            {
                character.GetComponent<AnimationController>().characterClicked.AddListener(EnemyTargetSelected);
            }
            
            _battleController.SwitchEnemyColliders(true);
            
            //_battleController.FightHandled.AddListener(EndPlayerTurn);
            

        }

        private void ClearButtonsListeners()
        {
            _battleUI.SkipTurnButton.onClick.RemoveAllListeners();
            _battleUI.AttackButton.onClick.RemoveAllListeners();
        }

        private void EnemyTargetSelected()
        {
            _battleController.SwitchEnemyColliders(false);
            
            foreach (var character in _battleController.EnemyCharacters)
            {
                character.GetComponent<AnimationController>().characterClicked.RemoveAllListeners();
            }

            _battleController.FightHandled.AddListener(EndPlayerTurn);
            
            _battleController.HandleFight(this);
        }

        private void EndPlayerTurn()
        {
            //_coroutineRunner.StartCoroutine(PlayerTurnEndDelay());
            _battleController.FightHandled.RemoveAllListeners();

            _coroutineRunner.StartCoroutine(PlayerTurnEndDelay());
            //_battleController.ClearActiveCharacters();
            //_battleStateMachine.Enter<EnemyTurnState>();
        }

        private IEnumerator PlayerTurnEndDelay()
        {
            yield return new WaitForSeconds(1f);
            
            _battleStateMachine.Enter<EnemyTurnState>();
        }
    }
}