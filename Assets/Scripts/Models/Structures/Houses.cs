using UnityEngine;

namespace Models.Structures
{
    public class Houses : IBuildable
    {
        private GameObject _brokenHomes;
        private GameObject _repairedHomes;

        public Houses(GameObject brokenHomes, GameObject repairedHomes)
        {
            _brokenHomes = brokenHomes;
            _repairedHomes = repairedHomes;
        }
        
        public void Build()
        {
            _brokenHomes.SetActive(false);
            _repairedHomes.SetActive(true);
        }
    }
}