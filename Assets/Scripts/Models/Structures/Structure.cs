using UnityEngine;

namespace Models.Structures
{
    public class Structure : IBuildable
    {
        private GameObject _building;

        public Structure(GameObject building)
        {
            _building = building;
        }
        public void Build()
        {
            _building.SetActive(true);
        }
    }
}