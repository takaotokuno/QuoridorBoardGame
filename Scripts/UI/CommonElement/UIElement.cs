using UnityEngine;
using System.Threading.Tasks;

namespace Quoridor.UI
{
    public class UIElement : MonoBehaviour
    {
        protected virtual void Awake() => SetActive(false); //初期は非表示
        public void SetActive(bool isActive) => gameObject.SetActive(isActive); //表示・非表示を切り替える
        public virtual void AppearanceElement() //画面に登場させる（モーション可）
        {
            if (gameObject.TryGetComponent<Easing>(out var easing))
            {
                easing.DOMoveFrom();
            }
            else gameObject.SetActive(true);
        }

        public async virtual void ExitElement() //画面から退場させる
        {
            if (gameObject.TryGetComponent<Easing>(out var easing))
            {
                await Task.Run(() => easing.DOMoveTo());
            }
            gameObject.SetActive(false);
        }

        public void TryAnimateIn()
        {
            if (!gameObject.activeSelf) Animate(true);
        }

        public void TryAnimateOut()
        {
            if (gameObject.activeSelf) Animate(false);
        }

        public virtual void Animate(bool isActive)
        {
            if (gameObject.TryGetComponent<Easing>(out var easing))
            {
                if (isActive) easing.DOMoveFrom();
                else easing.DOMoveTo();
            }
            SetActive(isActive);
        }
    }
}