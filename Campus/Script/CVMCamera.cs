using UnityEngine;
using System.Collections;

public class CVMCamera : JWMonoBehaviour {
    public bool edgeMove = true;
    bool clicked = false;
    float timer_ResetClick = 0.5f;
    bool doubleClicked = false;
    float timer_ResetDoublelick = 0.1f;

    // Use this for initialization
    void Start () {
        StartCoroutine(ResetClicked(timer_ResetClick));
        StartCoroutine(ResetDoubleClicked(timer_ResetDoublelick));
    }

    // Update is called once per frame
    void Update () {
        EdgeMove(50, 10);

    }

    void EdgeMove(float boundry, float speed)
    {
        if (edgeMove)
        {
            if (Input.mousePosition.x > Screen.width - boundry)
                transform.Translate(new Vector3(Time.deltaTime*speed, 0, 0));
            if (Input.mousePosition.x < boundry)
                transform.Translate(new Vector3(-Time.deltaTime * speed, 0, 0));
            if (Input.mousePosition.y > Screen.height - boundry)
                transform.Translate(new Vector3(0, Time.deltaTime * speed, 0));
            if (Input.mousePosition.y < boundry)
                transform.Translate(new Vector3(0, -Time.deltaTime * speed, 0));
        }
        if (Input.GetButtonDown("Fire1"))
        {
            if (!clicked)
                clicked = true;
            else
                doubleClicked = true;
        }
        if (doubleClicked)
        {
            float camerasize = GetComponent<Camera>().orthographicSize;
            if (camerasize == 12)
                GetComponent<Camera>().orthographicSize = 7.2f;
            else
                GetComponent<Camera>().orthographicSize = 12;

            doubleClicked = false;
        }

    }

    IEnumerator ResetClicked(float timer)
    {
        while (true)
        {
            yield return new WaitUntil(() => clicked);
            yield return new WaitForSeconds(timer);
            clicked = false;
        }
    }
    IEnumerator ResetDoubleClicked(float timer)
    {
        while (true)
        {
            yield return new WaitUntil(() => doubleClicked);
            yield return new WaitForSeconds(timer);
            doubleClicked = false;
        }
    }

    void Zoom(float size)
    {
        GetComponent<Camera>().orthographicSize = size;
    }
}
