using System;
using System.Collections;
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

        private ICoroutineRunner _coroutineRunner;

        public BattleController(ICoroutineRunner coroutineRunner)
        {
            _coroutineRunner = coroutineRunner;
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
            SwitchEnemyColliders(false);
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
                ActivePlayerCharacter.GetComponent<AnimationController>().attackComplete.RemoveAllListeners();
                ActivePlayerCharacter = null;
            }
            
            if (ActiveEnemyCharacter != null)
            {
                ActiveEnemyCharacter.transform.localScale = new Vector3(1f, 1f, 1f);
                ActiveEnemyCharacter.GetComponent<AnimationController>().attackComplete.RemoveAllListeners();
                ActiveEnemyCharacter = null;
            }
        }

        public void SwitchEnemyColliders(bool isActive)
        {
            foreach (var character in EnemyCharacters)
            {
                character.GetComponent<BoxCollider2D>().enabled = isActive;
            }
        }


        public void HandleFight(IExitableState currentState)
        {
            _playerFighterInitialPosition = ActivePlayerCharacter.GetComponentInParent<Transform>().position;
            _enemyFighterInitialPosition = ActiveEnemyCharacter.GetComponentInParent<Transform>().position;

            //Vector3.Lerp(ActivePlayerCharacter.transform.position, PlayerFightPosition.transform.position, 0.1f);
            //Vector3.Lerp(ActiveEnemyCharacter.transform.position, EnemyFightPosition.transform.position, 0.1f);
            
            ActivePlayerCharacter.transform.position = PlayerFightPosition.transform.position;
            ActiveEnemyCharacter.transform.position = EnemyFightPosition.transform.position;

            ActivePlayerCharacter.GetComponent<Renderer>().sortingOrder = 2;
            ActiveEnemyCharacter.GetComponent<Renderer>().sortingOrder = 2;
                
            GameObject.FindWithTag("BattleUI").GetComponent<BattleHudController>().SetFightCurtain(true);

            var playerAnimation = ActivePlayerCharacter.GetComponent<AnimationController>();
            var enemyAnimation = ActiveEnemyCharacter.GetComponent<AnimationController>();

            if (currentState is EnemyTurnState)
            {
                enemyAnimation.attackComplete.AddListener(CharactersGoIdle);
                enemyAnimation.PlayAttack();
                playerAnimation.PlayDamage();

                Debug.Log("------------------------------Enemy--------------------");
            }
            /*else
            {
                playerAnimation.attackComplete.AddListener(CharactersGoIdle);
                playerAnimation.PlayAttack();
                enemyAnimation.PlayDamage();

                Debug.Log("------------------------------Player--------------------");
            }*/

            if (currentState is PlayerTurnState)
            {
                playerAnimation.attackComplete.AddListener(CharactersGoIdle);
                playerAnimation.PlayAttack();
                enemyAnimation.PlayDamage();

                Debug.Log("------------------------------Player--------------------");
            }
        }

        private void CharactersGoIdle()
        {
            Debug.Log("CharactersGoIdle");
            
            ActivePlayerCharacter.GetComponent<AnimationController>().attackComplete.RemoveAllListeners();
            ActiveEnemyCharacter.GetComponent<AnimationController>().attackComplete.RemoveAllListeners();
            
            ActivePlayerCharacter.transform.position = _playerFighterInitialPosition;
            ActiveEnemyCharacter.transform.position = _enemyFighterInitialPosition;
            
            ActivePlayerCharacter.GetComponent<Renderer>().sortingOrder = 0;
            ActiveEnemyCharacter.GetComponent<Renderer>().sortingOrder = 0;

            ActivePlayerCharacter.GetComponent<AnimationController>().GoIdle();
            ActiveEnemyCharacter.GetComponent<AnimationController>().GoIdle();
            
            GameObject.FindWithTag("BattleUI").GetComponent<BattleHudController>().SetFightCurtain(false);
            
            ClearActiveCharacters();

            _coroutineRunner.StartCoroutine(PlacementDelay());
            
            //FightHandled?.Invoke();
        }

        private IEnumerator PlacementDelay()
        {
            yield return null;
            FightHandled?.Invoke();
        }
    }
}