using GameArea;
using GameArea.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace UI
{
    public class SimpleGUI : MonoBehaviour
    {
        [SerializeField] private GameAreaManager _GameAreaManager;
        [SerializeField] private GameAreaData _GameAreaCachedData;
        [SerializeField] private TMP_InputField _WidthInputField;
        [SerializeField] private TMP_InputField _HeightInputField;
        [SerializeField] private Button _CreateButton;

        private void Start()
        {
            _WidthInputField.text = _GameAreaCachedData.Width.ToString();
            _HeightInputField.text = _GameAreaCachedData.Height.ToString();
            _CreateButton.onClick.AddListener(OnCreateButtonClicked);
        }
        private void OnDestroy()
        {
            _CreateButton.onClick.RemoveAllListeners();
        }

        private void OnCreateButtonClicked()
        {
            _GameAreaManager.BuildArea(int.Parse(_WidthInputField.text), int.Parse(_HeightInputField.text));
        }
    }
}
