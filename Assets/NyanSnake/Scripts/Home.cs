using Gists;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace NyanSnake
{
    internal class Home : MonoBehaviour
    {
        #region Inspector

        [Header("References")]

        [SerializeField] private Image _homeImage;
        [SerializeField] private Button _homeButton;
        [SerializeField] private Image _cancelImage;
        [SerializeField] private Button _cancelButton;
        [SerializeField] private Image _confirmImage;
        [SerializeField] private Button _confirmButton;
        [SerializeField] private RectTransform _buttonsRectTransform;
        [SerializeField] private Game _game;

        [Header("Settings")]

        [SerializeField] private float _buttonsSlideTime = .5f;

        #endregion // Inspector

        private bool _areButtonsSliding;

        private void Start()
        {
            _areButtonsSliding = false;
            _homeButton.onClick.AddListener(OnHomeButtonClick);
            _cancelButton.onClick.AddListener(OnCancelButtonClick);
            _confirmButton.onClick.AddListener(OnConfirmButtonClick);
            ApplySkin();
            SkinDressing.Instance.OnSkinChanged += ApplySkin;
        }

        private void ApplySkin()
        {
            Skin skin = SkinDressing.Instance.GetSkin();
            _homeImage.color = _cancelImage.color = _confirmImage.color = skin.UIColor;
        }

        private void OnHomeButtonClick()
        {
            if (!_areButtonsSliding)
            {
                _game.Pause(true);
                StartCoroutine(SlideButtonsToShow());
            }
        }

        private void OnCancelButtonClick()
        {
            if (!_areButtonsSliding)
            {
                _game.Pause(false);
                StartCoroutine(SlideButtonsToHide());
            }
        }

        private void OnConfirmButtonClick()
        {
            if (!_areButtonsSliding)
            {
                _game.Quit();
            }
        }

        private IEnumerator SlideButtonsToShow()
        {
            _areButtonsSliding = true;
            float time = 0;
            EasingFunction.Function easingFunction = EasingFunction.GetEasingFunction(EasingFunction.Ease.EaseOutBack);
            while (time < _buttonsSlideTime)
            {
                float x = 1 - easingFunction(0, 1, time / _buttonsSlideTime);
                _buttonsRectTransform.pivot = new Vector3(x, .5f);
                yield return null;
                time += Time.unscaledDeltaTime;
            }
            _buttonsRectTransform.pivot = new Vector3(0, .5f);
            _areButtonsSliding = false;
        }

        private IEnumerator SlideButtonsToHide()
        {
            _areButtonsSliding = true;
            float time = 0;
            EasingFunction.Function easingFunction = EasingFunction.GetEasingFunction(EasingFunction.Ease.EaseOutQuart);
            while (time < _buttonsSlideTime)
            {
                float x = easingFunction(0, 1, time / _buttonsSlideTime);
                _buttonsRectTransform.pivot = new Vector3(x, .5f);
                yield return null;
                time += Time.unscaledDeltaTime;
            }
            _buttonsRectTransform.pivot = new Vector3(1, .5f);
            _areButtonsSliding = false;
        }

        public void SetButtonInteractable(bool interactable)
        {
            _homeButton.interactable = interactable;
        }
    }
}
