using UnityEngine;
using Zenject;

namespace Installers
{
    public class LevelCreatorInstaller : MonoInstaller
    {
        [SerializeField] private LevelCreator _levelCreatorPrefab;

        public override void InstallBindings()
        {
            var levelCreatorPrefab = Container.InstantiatePrefab(_levelCreatorPrefab);

            var levelCreator = levelCreatorPrefab.GetComponentInChildren<LevelCreator>();
            levelCreator.CreateLevel();
        }
    }
}