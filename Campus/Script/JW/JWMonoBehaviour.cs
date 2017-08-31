using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class JWMonoBehaviour : MonoBehaviour
{
    protected void Alert(string AlertPanelDir, string text)
    {
        GameObject AlertPanelTemplate = Resources.Load<GameObject>(AlertPanelDir);
        GameObject newPanel = JWInstantiate_UI(AlertPanelTemplate, FindObjectOfType<Canvas>().gameObject, false);
        newPanel.GetComponentInChildren<Text>().text = text;
    }

    protected GameObject JWInstantiate(GameObject target, GameObject parent, Vector3 position, Vector3 localscale, Vector3 rotation)
    {
        GameObject newObject = Instantiate(target);
        newObject.transform.SetParent(parent.transform);
        newObject.transform.localScale = localscale;
        newObject.transform.localPosition = position;
        newObject.transform.localEulerAngles = rotation;
        return newObject;
    }

    protected GameObject JWInstantiate(GameObject target, GameObject parent, Vector3 position)
    {
        return JWInstantiate(target, parent, position, Vector3.one, Vector3.zero);
    }

    protected GameObject JWInstantiate_UI(GameObject target, GameObject parent, string mode, object content, bool gradually, Vector3 localPosition, Vector3 localScale, Vector3 eulerAngles)
    {
        GameObject newGameObject = JWInstantiate(target, parent, localPosition, localScale, eulerAngles);
        float appearSpeed = 1;
        switch (mode)
        {
            case "Image":
                Image newImage = newGameObject.GetComponent<Image>();
                if (gradually)
                    StartCoroutine(JWUIFadeIn_UI(newImage, appearSpeed));
                newImage.sprite = (Sprite)content;
                break;
            case "Text":
                Text newText = newGameObject.GetComponent<Text>();
                if (gradually)
                    StartCoroutine(JWUIFadeIn_UI(newText, appearSpeed));
                newText.text = (string)content;
                break;
            case "InputField":
                InputField newInputField = newGameObject.GetComponent<InputField>();
                if (gradually)
                    StartCoroutine(JWUIFadeIn_UI(newInputField.GetComponent<Image>(), appearSpeed));
                newInputField.placeholder.GetComponent<Text>().text = (string)content;
                break;
            case "Button":
                Button newButton = newGameObject.GetComponent<Button>();
                if (gradually)
                {
                    StartCoroutine(JWUIFadeIn_UI(newButton.GetComponent<Image>(), appearSpeed));
                    StartCoroutine(JWUIFadeIn_UI(newButton.GetComponentInChildren<Text>(), appearSpeed));
                }
                newButton.GetComponentInChildren<Text>().text = (string)content;
                break;
            default:
                if (gradually)
                    foreach (Graphic graphic in newGameObject.GetComponentsInChildren<Graphic>())
                        StartCoroutine(JWUIFadeIn_UI(graphic, 1));
                break;

        }
        return newGameObject;
    }

    protected GameObject JWInstantiate_UI(GameObject target, GameObject parent, string mode, object content, bool gradually)
    {
        return JWInstantiate_UI(target, parent, mode, content, gradually, Vector3.zero, Vector3.one, Vector3.zero);
    }

    protected GameObject JWInstantiate_UI(GameObject target, GameObject parent, bool gradually)
    {
        GameObject newGameObject = JWInstantiate_UI(target, parent, "", "", gradually);
        return newGameObject;
    }

    protected string ChooseOneFrom(string[] strings)
    {
        int index = Random.Range(0, strings.Length);
        return strings[index];
    }

    protected bool isIn(int element, List<int> list)
    {
        foreach (int listelement in list)
            if (listelement == element)
                return true;
        return false;
    }

    protected IEnumerator Wait(float timer)
    {
        yield return new WaitForSeconds(timer);
    }

    protected string[] JWParseString(string originStr, char[] delimiterChars)
    {
        //string sampleText = "one\ttwo three:four,five six seven";
        string[] splitedStr = originStr.Split(delimiterChars);
        return splitedStr;
    }

    protected string[] JWParseString(string originStr)
    {
        char[] delimiterChars = { ' ', ',', '.', ':', '\t', '\n' };
        return JWParseString(originStr, delimiterChars);
    }

    IEnumerator JWUIFadeIn_UI(Graphic graphic, float speed)
    {
        graphic.color = new Color(graphic.color.r, graphic.color.g, graphic.color.b, 0);
        while (graphic && graphic.color.a < 1)
        {
            graphic.color = new Color(graphic.color.r, graphic.color.g, graphic.color.b, graphic.color.a + speed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
    }
}
