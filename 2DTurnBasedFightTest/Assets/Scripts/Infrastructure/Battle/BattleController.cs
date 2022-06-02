using System.Collections;
using System.Collections.Generic;
using Infrastructure.States;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace Infrastructure.Battle
{
    public class BattleController : IBattleController
    {
        public int RoundCounter { get; set; }
        public UnityEvent FightHandled { get; }
        public List<GameObject> PlayerCharacters { get; set; }
        public List<GameObject> EnemyCharacters { get; set; }
        
        private GameObject _activePlayerCharacter;
        private GameObject _activeEnemyCharacter;

        private GameObject _playerFightPosition;
        private GameObject _enemyFightPosition;

        private Vector3 _playerFighterInitialPosition;
        private Vector3 _enemyFighterInitialPosition;

        private readonly List<GameObject> _playerCharactersToPick = new List<GameObject>();
        private readonly List<GameObject> _enemyCharactersToPick = new List<GameObject>();
        private readonly ICoroutineRunner _coroutineRunner;

        private const string PlayerFightPositionTag = "PlayerFightPosition";
        private const string EnemyFightPositionTag = "EnemyFightPosition";

        public BattleController(ICoroutineRunner coroutineRunner)
        {
            _coroutineRunner = coroutineRunner;
            FightHandled = new UnityEvent();
            RoundCounter = 1;
        }

        public void InitPlayerCharacters(List<GameObject> playerCharacters)
        {
            PlayerCharacters = playerCharacters;
            ReloadCharactersToPick(PlayerCharacters,_playerCharactersToPick);
        }

        public void InitEnemyCharacters(List<GameObject> enemyCharacters)
        {
            EnemyCharacters = enemyCharacters;
            ReloadCharactersToPick(EnemyCharacters,_enemyCharactersToPick);
        }

        public void GetFightPositions()
        {
            _playerFightPosition = GameObject.FindWithTag(PlayerFightPositionTag);
            _enemyFightPosition = GameObject.FindWithTag(EnemyFightPositionTag);
        }

        public GameObject GetPlayerCharacter(IExitableState state)
        {
            if (state is EnemyTurnState)
            {
                var randomIndex = Random.Range(0, PlayerCharacters.Count);
                _activePlayerCharacter = PlayerCharacters[randomIndex];
            }
            else
            {
                var randomIndex = Random.Range(0, _playerCharactersToPick.Count);
                _activePlayerCharacter = _playerCharactersToPick[randomIndex];
                _playerCharactersToPick.RemoveAt(randomIndex);
            }
            if (_playerCharactersToPick.Count == 0)
            {
                RoundCounter++;
                ReloadCharactersToPick(PlayerCharacters,_playerCharactersToPick);
            }
            _activePlayerCharacter.transform.localScale = new Vector3(1.1f, 1.1f, 1f);
            
            return _activePlayerCharacter;
        }

        public GameObject GetEnemyCharacter()
        {
            if (_enemyCharactersToPick.Count == 0)
            {
                ReloadCharactersToPick(EnemyCharacters,_enemyCharactersToPick);
            }
            var randomIndex = Random.Range(0, _enemyCharactersToPick.Count);
            _activeEnemyCharacter = EnemyCharacters[randomIndex];
            _enemyCharactersToPick.RemoveAt(randomIndex);
            _activeEnemyCharacter.transform.localScale = new Vector3(1.1f, 1.1f, 1f);

            return _activeEnemyCharacter;
        }

        public void SetActiveEnemyCharacter(GameObject character)
        {
            _activeEnemyCharacter = character;
            _activeEnemyCharacter.transform.localScale = new Vector3(1.1f, 1.1f, 1f);
            SwitchEnemyColliders(false);
        }

        public void ClearActiveCharacters()
        {
            if (_activePlayerCharacter != null)
            {
                _activePlayerCharacter.transform.localScale = new Vector3(1f, 1f, 1f);
                _activePlayerCharacter.GetComponent<AnimationController>().attackComplete.RemoveAllListeners();
                _activePlayerCharacter = null;
            }
            
            if (_activeEnemyCharacter != null)
            {
                _activeEnemyCharacter.transform.localScale = new Vector3(1f, 1f, 1f);
                _activeEnemyCharacter.GetComponent<AnimationController>().attackComplete.RemoveAllListeners();
                _activeEnemyCharacter = null;
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
            _playerFighterInitialPosition = _activePlayerCharacter.GetComponentInParent<Transform>().position;
            _enemyFighterInitialPosition = _activeEnemyCharacter.GetComponentInParent<Transform>().position;
            
            SetCharactersSortingOrder(2);

            GameObject.FindWithTag("BattleUI").GetComponent<BattleHudController>().SetFightCurtain(true);

            _coroutineRunner.StartCoroutine(ExecuteFight(currentState));
        }

        private void ReloadCharactersToPick(List<GameObject> initialCharactersList,
            List<GameObject> charactersListToFill)
        {
            foreach (var character in initialCharactersList)
            {
                charactersListToFill.Add(character);
            }
        }

        private void SetCharactersSortingOrder(int orderId)
        {
            _activePlayerCharacter.GetComponent<Renderer>().sortingOrder = orderId;
            _activeEnemyCharacter.GetComponent<Renderer>().sortingOrder = orderId;
        }

        private void CharactersGoIdle()
        {
            _coroutineRunner.StartCoroutine(CleanUpAfterFight());
        }

        private IEnumerator CleanUpAfterFight()
        {
            while (_activePlayerCharacter.transform.position != _playerFighterInitialPosition &&
                   _activeEnemyCharacter.transform.position != _enemyFighterInitialPosition)
            {
                _activePlayerCharacter.transform.position = Vector3.Lerp(_activePlayerCharacter.transform.position,
                    _playerFighterInitialPosition, Time.deltaTime * 10f);
                _activeEnemyCharacter.transform.position = Vector3.Lerp(_activeEnemyCharacter.transform.position,
                    _enemyFighterInitialPosition, Time.deltaTime * 10f);
                yield return null;
            }
            
            SetCharactersSortingOrder(0);
            
            _activePlayerCharacter.GetComponent<AnimationController>().GoIdle();
            _activeEnemyCharacter.GetComponent<AnimationController>().GoIdle();
            
            GameObject.FindWithTag("BattleUI").GetComponent<BattleHudController>().SetFightCurtain(false);
            
            ClearActiveCharacters();
            
            FightHandled?.Invoke();
        }

        private IEnumerator ExecuteFight(IExitableState currentState)
        {
            while (_activePlayerCharacter.transform.position != _playerFightPosition.transform.position &&
                   _activeEnemyCharacter.transform.position != _enemyFightPosition.transform.position)
            {
                _activePlayerCharacter.transform.position = Vector3.Lerp(_activePlayerCharacter.transform.position,
                    _playerFightPosition.transform.position, Time.deltaTime*10f);
                _activeEnemyCharacter.transform.position = Vector3.Lerp(_activeEnemyCharacter.transform.position,
                    _enemyFightPosition.transform.position, Time.deltaTime*10f);
                yield return null;
            }
            var playerAnimation = _activePlayerCharacter.GetComponent<AnimationController>();
            var enemyAnimation = _activeEnemyCharacter.GetComponent<AnimationController>();

            if (currentState is EnemyTurnState)
            {
                enemyAnimation.attackComplete.AddListener(CharactersGoIdle);
                enemyAnimation.PlayAttack();
                playerAnimation.PlayDamage();
            }
            if (currentState is PlayerTurnState)
            {
                playerAnimation.attackComplete.AddListener(CharactersGoIdle);
                playerAnimation.PlayAttack();
                enemyAnimation.PlayDamage();
            }
        }
    }
}