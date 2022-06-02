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
        private const string PlayerFightPositionTag = "PlayerFightPosition";
        private const string EnemyFightPositionTag = "EnemyFightPosition";

        public int RoundCounter { get; set; }
        public UnityEvent FightHandled { get; }
        public List<GameObject> PlayerCharacters { get; set; }
        public List<GameObject> EnemyCharacters { get; set; }

        private List<GameObject> _playerCharactersToPick = new List<GameObject>();
        private List<GameObject> _enemyCharactersToPick = new List<GameObject>();

        private GameObject _activePlayerCharacter;
        private GameObject _activeEnemyCharacter;

        private GameObject _playerFightPosition;
        private GameObject _enemyFightPosition;

        private Vector3 _playerFighterInitialPosition;
        private Vector3 _enemyFighterInitialPosition;

        private readonly ICoroutineRunner _coroutineRunner;

        public BattleController(ICoroutineRunner coroutineRunner)
        {
            _coroutineRunner = coroutineRunner;
            FightHandled = new UnityEvent();
            RoundCounter = 1;
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
                
                foreach (var character in PlayerCharacters)
                {
                    _playerCharactersToPick.Add(character);
                }
            }

            _activePlayerCharacter.transform.localScale = new Vector3(1.1f, 1.1f, 1f);
            
            return _activePlayerCharacter;
        }
        
        public GameObject GetEnemyCharacter()
        {
            if (_enemyCharactersToPick.Count == 0)
            {

                foreach (var character in EnemyCharacters)
                {
                    _enemyCharactersToPick.Add(character);
                }
            }
            
            //var randomIndex = Random.Range(0, EnemyCharacters.Count);
            var randomIndex = Random.Range(0, _enemyCharactersToPick.Count);
            
            _activeEnemyCharacter = EnemyCharacters[randomIndex];
            
            _enemyCharactersToPick.RemoveAt(randomIndex);
            
            _activeEnemyCharacter.transform.localScale = new Vector3(1.1f, 1.1f, 1f);

            return _activeEnemyCharacter;
        }

        public void SetActivePlayerCharacter(GameObject character)
        {
            _activePlayerCharacter = character;
            _activePlayerCharacter.transform.localScale = new Vector3(1.1f, 1.1f, 1f);
        }

        public void SetActiveEnemyCharacter(GameObject character)
        {
            _activeEnemyCharacter = character;
            _activeEnemyCharacter.transform.localScale = new Vector3(1.1f, 1.1f, 1f);
            SwitchEnemyColliders(false);
        }

        public void InitPlayerCharacters(List<GameObject> playerCharacters)
        {
            PlayerCharacters = playerCharacters;
            
            foreach (var character in PlayerCharacters)
            {
                _playerCharactersToPick.Add(character);
            }
        }

        public void InitEnemyCharacters(List<GameObject> enemyCharacters)
        {
            EnemyCharacters = enemyCharacters;
            
            foreach (var character in EnemyCharacters)
            {
                _enemyCharactersToPick.Add(character);
            }
        }

        public void GetFightPositions()
        {
            _playerFightPosition = GameObject.FindWithTag(PlayerFightPositionTag);
            _enemyFightPosition = GameObject.FindWithTag(EnemyFightPositionTag);
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
            /*_coroutineRunner.StartCoroutine(MoveToFightPosition(_activePlayerCharacter.transform,
                _playerFightPosition.transform.position));
            
            _coroutineRunner.StartCoroutine(MoveToFightPosition(_activeEnemyCharacter.transform,
                _enemyFightPosition.transform.position));*/
            
            //_activePlayerCharacter.transform.position = _playerFightPosition.transform.position;
            //_activeEnemyCharacter.transform.position = _enemyFightPosition.transform.position;

            _activePlayerCharacter.GetComponent<Renderer>().sortingOrder = 2;
            _activeEnemyCharacter.GetComponent<Renderer>().sortingOrder = 2;
                
            GameObject.FindWithTag("BattleUI").GetComponent<BattleHudController>().SetFightCurtain(true);

            _coroutineRunner.StartCoroutine(TestFight(currentState));
            /*_playerFighterInitialPosition = _activePlayerCharacter.GetComponentInParent<Transform>().position;
            _enemyFighterInitialPosition = _activeEnemyCharacter.GetComponentInParent<Transform>().position;
            
            
            _activePlayerCharacter.transform.position = Vector3.Lerp(_activePlayerCharacter.transform.position, _playerFightPosition.transform.position, 3f);
            _activeEnemyCharacter.transform.position = Vector3.Lerp(_activeEnemyCharacter.transform.position, _enemyFightPosition.transform.position, 3f);

            _coroutineRunner.StartCoroutine(MoveToFightPosition(_activePlayerCharacter.transform,
                _playerFightPosition.transform.position));
            
            _coroutineRunner.StartCoroutine(MoveToFightPosition(_activeEnemyCharacter.transform,
                _enemyFightPosition.transform.position));
            
            //_activePlayerCharacter.transform.position = _playerFightPosition.transform.position;
            //_activeEnemyCharacter.transform.position = _enemyFightPosition.transform.position;

            _activePlayerCharacter.GetComponent<Renderer>().sortingOrder = 2;
            _activeEnemyCharacter.GetComponent<Renderer>().sortingOrder = 2;
                
            GameObject.FindWithTag("BattleUI").GetComponent<BattleHudController>().SetFightCurtain(true);

            var playerAnimation = _activePlayerCharacter.GetComponent<AnimationController>();
            var enemyAnimation = _activeEnemyCharacter.GetComponent<AnimationController>();

            if (currentState is EnemyTurnState)
            {
                enemyAnimation.attackComplete.AddListener(CharactersGoIdle);
                enemyAnimation.PlayAttack();
                playerAnimation.PlayDamage();

                Debug.Log("------------------------------Enemy--------------------");
                
            }

            if (currentState is PlayerTurnState)
            {
                playerAnimation.attackComplete.AddListener(CharactersGoIdle);
                playerAnimation.PlayAttack();
                enemyAnimation.PlayDamage();

                Debug.Log("------------------------------Player--------------------");
            }*/
        }

        private void CharactersGoIdle()
        {
            Debug.Log("CharactersGoIdle");
            
            _activePlayerCharacter.GetComponent<AnimationController>().attackComplete.RemoveAllListeners();
            _activeEnemyCharacter.GetComponent<AnimationController>().attackComplete.RemoveAllListeners();
            
            
            
            //_activePlayerCharacter.transform.position = _playerFighterInitialPosition;
            //_activeEnemyCharacter.transform.position = _enemyFighterInitialPosition;

            _coroutineRunner.StartCoroutine(TestBackPlacement());
            
            /*_activePlayerCharacter.GetComponent<AnimationController>().GoIdle();
            _activeEnemyCharacter.GetComponent<AnimationController>().GoIdle();
            
            GameObject.FindWithTag("BattleUI").GetComponent<BattleHudController>().SetFightCurtain(false);
            
            ClearActiveCharacters();

            _coroutineRunner.StartCoroutine(PlacementDelay());
            
            //FightHandled?.Invoke();*/
        }

        private IEnumerator PlacementDelay()
        {
            yield return null;
            FightHandled?.Invoke();
        }

        private IEnumerator TestBackPlacement()
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
            
            _activePlayerCharacter.GetComponent<Renderer>().sortingOrder = 0;
            _activeEnemyCharacter.GetComponent<Renderer>().sortingOrder = 0;
            
            _activePlayerCharacter.GetComponent<AnimationController>().GoIdle();
            _activeEnemyCharacter.GetComponent<AnimationController>().GoIdle();
            
            GameObject.FindWithTag("BattleUI").GetComponent<BattleHudController>().SetFightCurtain(false);
            
            ClearActiveCharacters();
            
            FightHandled?.Invoke();
            
        }

        private IEnumerator TestFight(IExitableState currentState)
        {
            while (_activePlayerCharacter.transform.position != _playerFightPosition.transform.position &&
                   _activeEnemyCharacter.transform.position != _enemyFightPosition.transform.position)
            {
                _activePlayerCharacter.transform.position = Vector3.Lerp(_activePlayerCharacter.transform.position, _playerFightPosition.transform.position, Time.deltaTime*10f);
                _activeEnemyCharacter.transform.position = Vector3.Lerp(_activeEnemyCharacter.transform.position, _enemyFightPosition.transform.position, Time.deltaTime*10f);
                yield return null;
            }
            
            var playerAnimation = _activePlayerCharacter.GetComponent<AnimationController>();
            var enemyAnimation = _activeEnemyCharacter.GetComponent<AnimationController>();

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
    }
}