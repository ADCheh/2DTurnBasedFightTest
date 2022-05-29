using System.Collections.Generic;
using Infrastructure.Services;
using UnityEngine;

namespace Infrastructure.Factory
{
    public interface IGameFactory : IService
    {
        void CreateHud();
        List<GameObject> CreatePlayerCharacters();
        List<GameObject> CreateEnemyCharacters();
    }
}