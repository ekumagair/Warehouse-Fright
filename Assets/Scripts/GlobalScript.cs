using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlobalScript : MonoBehaviour
{
    public GameObject scoreText;

    public static int difficulty = 0;
    public static bool gameIsInfinite = false;
    public static int highScoreNormal = 0;
    public static int highScoreInfinite = 0;

    // Debug mode
    public static bool debug = false;

    public void AddPoints(int points, bool createText, Transform baseTransform)
    {
        Player.score += points;

        if(createText)
        {
            var st = Instantiate(scoreText, baseTransform.position, baseTransform.rotation);
            st.transform.SetParent(GameObject.Find("Canvas").transform, false);
            st.transform.position = baseTransform.position;
            st.GetComponent<Text>().text = points.ToString();
        }
    }

    public IEnumerator MajorDeath(GameObject caller, SpriteRenderer sr, Rigidbody2D rb, bool destroyAfterEnd)
    {
        rb.velocity = new Vector2(0, 0);
        rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;

        for (int i = 0; i < 30; i++)
        {
            yield return new WaitForSeconds(0.05f);
            sr.enabled = !sr.enabled;
        }

        sr.enabled = true;
        rb.constraints = RigidbodyConstraints2D.None;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        if(destroyAfterEnd)
        {
            Destroy(caller);
        }
    }
}
