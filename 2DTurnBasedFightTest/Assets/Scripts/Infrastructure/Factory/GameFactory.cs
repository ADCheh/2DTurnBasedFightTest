using System.Collections.Generic;
using Infrastructure.AssetManagement;
using Spine.Unity;
using UnityEngine;

namespace Infrastructure.Factory
{
    public class GameFactory : IGameFactory
    {
        private const string PlayerPositionTag = "PlayerPosition";
        private const string EnemyPositionTag = "EnemyPosition";

        private readonly IAssetProvider _assetProvider;

        public GameFactory(IAssetProvider assetProvider)
        {
            _assetProvider = assetProvider;
        }

        public void CreateHud()
        {
            _assetProvider.Instantiate(AssetPath.HudPath);
        }

        public List<GameObject> CreatePlayerCharacters()
        {
            return CreateCharacters(AssetPath.PlayerCharactersPath,GameObject.FindGameObjectsWithTag(PlayerPositionTag));
        }

        public List<GameObject> CreateEnemyCharacters()
        {
            var enemyList = CreateCharacters(AssetPath.EnemyCharactersPath,GameObject.FindGameObjectsWithTag(EnemyPositionTag), true);
            return enemyList;
        }

        private List<GameObject> CreateCharacters(string characterPath,GameObject[] positions, bool flipX = false)
        {
            List<GameObject> charactersList = new List<GameObject>();
            
            foreach (var position in positions)
            {
                GameObject character = _assetProvider.Instantiate(characterPath, position.transform, flipX);
                charactersList.Add(character);
            }

            return charactersList;
        }
    }
}