using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace FadeInOut
{
    public class FadeInOutScene : MonoBehaviour
    {
        //�� �̵��� fadeinout ǥ��
        public GameObject getCanvas;
        public Image fadeImage; // ���� ȭ�� �̹��� (Canvas�� ����)
        public float fadeDuration = 1f;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject); // �� �Ѿ�� ����
        }

        public void FadeToScene(string sceneName)
        {
            getCanvas.gameObject.SetActive(true);
            StartCoroutine(FadeOutIn(sceneName));
        }

        private IEnumerator FadeOutIn(string sceneName)
        {
            yield return StartCoroutine(Fade(0f, 1f));

            Addressables.LoadSceneAsync(sceneName, LoadSceneMode.Single);

            yield return StartCoroutine(Fade(1f, 0f));
        }

        private IEnumerator Fade(float from, float to)
        {
            float time = 0f;
            Color color = fadeImage.color;

            while (time < fadeDuration)
            {
                float alpha = Mathf.Lerp(from, to, time / fadeDuration);
                color.a = alpha;
                fadeImage.color = color;
                time += Time.deltaTime;
                yield return null;
            }

            color.a = to;
            fadeImage.color = color;
        }
    }

}
