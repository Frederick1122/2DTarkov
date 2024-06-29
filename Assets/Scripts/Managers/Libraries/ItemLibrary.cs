using ConfigScripts;

namespace Managers.Libraries
{
    public class ItemLibrary : BaseLibrary<ItemConfig>
    {
        private const string ITEMS_CONFIG_PATH_PREFIX = "Configs/Items/";
        private const string WEAPONS_CONFIG_PATH = "Weapons";
        private const string BULLETS_CONFIG_PATH = "Bullets";

        protected override void Awake()
        {
            _paths.Add(ITEMS_CONFIG_PATH_PREFIX + WEAPONS_CONFIG_PATH);
            _paths.Add(ITEMS_CONFIG_PATH_PREFIX + BULLETS_CONFIG_PATH);
            base.Awake();
        }
    }
}