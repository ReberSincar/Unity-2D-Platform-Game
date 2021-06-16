using UnityEngine;
using UnityEngine.UI;

public class CoinController : MonoBehaviour
{
    public Text scoreValue;
    static int score = 0;
    void Start()
    {
        scoreValue.text = score.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0f, 100f*Time.deltaTime, 0f));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            score = score + 50;
            scoreValue.text = score.ToString();
            Destroy(gameObject);
        }
    }
}
