using UnityEngine;

namespace PolySpatial.Template
{
    [RequireComponent(typeof(ObjectSpawner))]
    public class SpawnedObjectsManager : MonoBehaviour
    {
        [SerializeField]
        SpatialUIDropDown m_ObjectSelectorDropDown;

        [SerializeField]
        SpatialUIButton m_DestroyObjectsButton;

        ObjectSpawner m_Spawner;

        void OnEnable()
        {
            m_Spawner = GetComponent<ObjectSpawner>();
            m_Spawner.spawnAsChildren = true;
            OnObjectSelectorDropdownValueChanged(m_ObjectSelectorDropDown.CurrentAssetIndex);
            m_ObjectSelectorDropDown.DropDownValueChangedEvent.AddListener(OnObjectSelectorDropdownValueChanged);
            m_DestroyObjectsButton.PressEndEvent.AddListener(OnDestroyObjectsButtonClicked);
        }

        void OnDisable()
        {
            m_ObjectSelectorDropDown.DropDownValueChangedEvent.RemoveListener(OnObjectSelectorDropdownValueChanged);
            m_DestroyObjectsButton.PressEndEvent.RemoveListener(OnDestroyObjectsButtonClicked);
        }

        void OnObjectSelectorDropdownValueChanged(int value)
        {
            if (value == 0)
            {
                m_Spawner.RandomizeSpawnOption();
                return;
            }

            m_Spawner.spawnOptionIndex = value - 1;
        }

        void OnDestroyObjectsButtonClicked()
        {
            foreach (Transform child in m_Spawner.transform)
            {
                Destroy(child.gameObject);
            }
        }
    }
}
