using System;
using System.Collections.Generic;
using Infrastructure.Services;
using Infrastructure.States;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace Infrastructure.Battle
{
    public class BattleController : IBattleController
    {
        private const string PlayerFightPositionTag = "PlayerFightPosition";
        private const string EnemyFightPositionTag = "EnemyFightPosition";

        public UnityEvent FightHandled { get; }
        public List<GameObject> PlayerCharacters { get; set; }
        public List<GameObject> EnemyCharacters { get; set; }

        public GameObject ActivePlayerCharacter;
        public GameObject ActiveEnemyCharacter;

        public GameObject PlayerFightPosition;
        public GameObject EnemyFightPosition;

        private Vector3 _playerFighterInitialPosition;
        private Vector3 _enemyFighterInitialPosition;

        public BattleController()
        {
            FightHandled = new UnityEvent();
        }

        public GameObject GetPlayerCharacter()
        {
            var randomIndex = Random.Range(0, PlayerCharacters.Count);
            ActivePlayerCharacter = PlayerCharacters[randomIndex];
            ActivePlayerCharacter.transform.localScale = new Vector3(1.1f, 1.1f, 1f);
            return ActivePlayerCharacter;
        }
        
        public GameObject GetEnemyCharacter()
        {
            var randomIndex = Random.Range(0, EnemyCharacters.Count);
            ActiveEnemyCharacter = EnemyCharacters[randomIndex];
            ActiveEnemyCharacter.transform.localScale = new Vector3(1.1f, 1.1f, 1f);
            return ActiveEnemyCharacter;
        }

        public void SetActivePlayerCharacter(GameObject character)
        {
            ActivePlayerCharacter = character;
            ActivePlayerCharacter.transform.localScale = new Vector3(1.1f, 1.1f, 1f);
        }

        public void SetActiveEnemyCharacter(GameObject character)
        {
            ActiveEnemyCharacter = character;
            ActiveEnemyCharacter.transform.localScale = new Vector3(1.1f, 1.1f, 1f);
            DisableEnemyCharacters();
        }

        public void InitPlayerCharacters(List<GameObject> playerCharacters)
        {
            PlayerCharacters = playerCharacters;
        }

        public void InitEnemyCharacters(List<GameObject> enemyCharacters)
        {
            EnemyCharacters = enemyCharacters;
        }

        public void GetFightPositions()
        {
            PlayerFightPosition = GameObject.FindWithTag(PlayerFightPositionTag);
            EnemyFightPosition = GameObject.FindWithTag(EnemyFightPositionTag);
        }

        public void ClearActiveCharacters()
        {
            if (ActivePlayerCharacter != null)
            {
                ActivePlayerCharacter.transform.localScale = new Vector3(1f, 1f, 1f);
                ActivePlayerCharacter = null;
            }
            
            if (ActiveEnemyCharacter != null)
            {
                ActiveEnemyCharacter.transform.localScale = new Vector3(1f, 1f, 1f);
                ActiveEnemyCharacter = null;
            }
                
        }

        public void EnableEnemyCharacters()
        {
            foreach (var character in EnemyCharacters)
            {
                character.GetComponent<BoxCollider2D>().enabled = true;
            }
        }

        private void DisableEnemyCharacters()
        {
            foreach (var character in EnemyCharacters)
            {
                character.GetComponent<BoxCollider2D>().enabled = false;
            }
        }

        public void HandleFight(IExitableState currentState)
        {
            if (currentState is PlayerTurnState)
            {
                _playerFighterInitialPosition = ActivePlayerCharacter.transform.position;
                _enemyFighterInitialPosition = ActiveEnemyCharacter.transform.position;

                ActivePlayerCharacter.transform.position = PlayerFightPosition.transform.position;
                ActiveEnemyCharacter.transform.position = EnemyFightPosition.transform.position;
                
                GameObject.FindWithTag("BattleUI").GetComponent<BattleHudController>().SetFightCurtain(true);

                var playerAnimation = ActivePlayerCharacter.GetComponent<AnimationController>();
                var enemyAnimation = ActiveEnemyCharacter.GetComponent<AnimationController>();
                
                playerAnimation.attackComplete.AddListener(CharactersGoIdle);
                playerAnimation.PlayAttack();
                enemyAnimation.PlayDamage();
            }

            if (currentState is EnemyTurnState)
            {
                
            }

            //FightHandled?.Invoke();
        }

        private void CharactersGoIdle()
        {
            Debug.Log("CharactersGoIdle");

            ActivePlayerCharacter.transform.position = _playerFighterInitialPosition;
            ActiveEnemyCharacter.transform.position = _enemyFighterInitialPosition;
            
            ActivePlayerCharacter.GetComponent<AnimationController>().GoIdle();
            ActiveEnemyCharacter.GetComponent<AnimationController>().GoIdle();
            
            GameObject.FindWithTag("BattleUI").GetComponent<BattleHudController>().SetFightCurtain(false);
            
            FightHandled?.Invoke();
            
            
        }
    }
}