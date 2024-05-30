using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEndFlagBehaviour : MonoBehaviour
{
    [SerializeField] GameObject fadeobj;
    private SpriteRenderer fade;

    // Start is called before the first frame update
    void Start()
    {
       fade = fadeobj.GetComponent<SpriteRenderer>();
        fade.color = new Color(0, 0, 0, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine("FadeBlack");

        }

    }

    IEnumerator FadeBlack()
    {
        while (fade.color.a < 1)
        {
        yield return new WaitForFixedUpdate();
        fade.color = new Color(0, 0, 0, fade.color.a + 0.04f);
        }
        SceneManager.LoadScene(3);

    }


}
