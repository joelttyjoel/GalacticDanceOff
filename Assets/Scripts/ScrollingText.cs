using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingText : MonoBehaviour {

    public float timeUntilShowSign = 15f;
    public float speedToGoal = 1f;
	public GameObject introSign;
    public GameObject endPoint;
	// Use this for initialization
	void Start () {
        StartCoroutine(TextScroller());
	}

	IEnumerator TextScroller()
	{
        float timer = timeUntilShowSign;
        bool hasStartedFading = false;

        while (Vector3.Distance(transform.position, endPoint.transform.position) >= 0.5f)
		{
            float step = speedToGoal * Time.deltaTime; // calculate distance to move
            transform.position = Vector3.MoveTowards(transform.position, endPoint.transform.position, step);

            timer -= Time.deltaTime;
            if (timer <= 0 && !hasStartedFading)
            {
                hasStartedFading = true;
                StartCoroutine(FadeInSign());
            }

            //Vector3 pos = this.gameObject.transform.position;
            //pos.z += Time.deltaTime * 4f;
            //pos.y += Time.deltaTime;
            //this.gameObject.transform.position = pos;
            //Quaternion target = Quaternion.Euler(0, 0, 0);
            //transform.rotation = Quaternion.Slerp(transform.rotation, target,  Time.deltaTime * 0.01f);
            yield return 0;
		}

        //scrolling done
	}

    private IEnumerator FadeInSign()
    {
        float opacity = 0f;
        GameObject theSign = Instantiate(introSign);
        theSign.GetComponent<SpriteRenderer>().color = new Vector4(1f, 1f, 1f, opacity);
        while(opacity <= 1f)
        {

            //set scal of text so it dissapeears
            GetComponent<RectTransform>().position += new Vector3(0f, 0f, opacity * 100);

            //set opacity to thing
            theSign.GetComponent<SpriteRenderer>().color = new Vector4(1f, 1f, 1f, opacity);
            //increase opacity
            opacity += 0.01f;
            //wait
            yield return new WaitForSeconds(0.01f);
        }
        opacity = 1f;
        theSign.GetComponent<SpriteRenderer>().color = new Vector4(1f, 1f, 1f, opacity);
        //wait for x
        yield return new WaitForSeconds(3f);
        //go to menu scene
        SceneSwitchereController.instance.LoadSceneByName("NewStartMenu", null);
    }
}
