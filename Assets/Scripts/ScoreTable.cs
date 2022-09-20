using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreTable : MonoBehaviour
{
    private Transform container;
    private Transform template;
    private List<Transform> transformList;
    private void Awake()
    {
        container = transform.Find("Container");
        template = container.Find("Template");
        transformList = new List<Transform>();

        template.gameObject.SetActive(false);

        string jsonHS = PlayerPrefs.GetString("Highscore");
        ListObj listObj = JsonUtility.FromJson<ListObj>(jsonHS);

        for (int i = 0; i < listObj.listRef.Count; i++) {
            for (int j = i+1; j < listObj.listRef.Count; j++) {
                if (listObj.listRef[j].name == listObj.listRef[i].name)
                {
                    Debug.Log("Yas");

                    if (listObj.listRef[j].score > listObj.listRef[i].score)
                    {
                        listObj.listRef[i] = listObj.listRef[j];
                        listObj.listRef.Remove(listObj.listRef[j]);
                    }
                }
                if (listObj.listRef[j].score > listObj.listRef[i].score)
                {
                    HighScore tmp = listObj.listRef[i];
                    listObj.listRef[i] = listObj.listRef[j];
                    listObj.listRef[j] = tmp;
                }
                
            }
        }

        foreach (HighScore data in listObj.listRef)
        {
            CreateHighscore(data, container, transformList);
        }
    }
    public void CreateHighscore(HighScore data, Transform container, List<Transform> transformList)
    {
        float templateHeight = 18;
        Transform transform = Instantiate(template, container);
        RectTransform rectTransform = transform.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(0, -templateHeight * transformList.Count);
        transform.gameObject.SetActive(true);

        int rank = transformList.Count + 1;
        string rankS;
        switch (rank)
        {
            default: rankS = rank + "th"; break;
            case 1: rankS = "1st"; break;
            case 2: rankS = "2nd"; break;
            case 3: rankS = "3rd"; break;
        }
        transform.Find("Rank").GetComponent<TextMeshProUGUI>().text = rankS;
        int score = data.score;
        transform.Find("Score").GetComponent<TextMeshProUGUI>().text = score.ToString();
        string name = data.name;
        transform.Find("Name").GetComponent<TextMeshProUGUI>().text = name;
        transformList.Add(transform);
    }
    public static void AddHighscore(int score,string name)
    {
        HighScore newScore = new HighScore { score = score, name = name };
        string jsonFW = PlayerPrefs.GetString("Highscore");
        ListObj listObj = JsonUtility.FromJson<ListObj>(jsonFW);
        listObj.listRef.Add(newScore);
        string jsonFR = JsonUtility.ToJson(listObj);
        PlayerPrefs.SetString("Highscore", jsonFR);
        PlayerPrefs.Save();
    }
    public void EraseScore()
    {
        ListObj emptylistObj = new ListObj();
        PlayerPrefs.SetString("Highscore", new string (JsonUtility.ToJson(emptylistObj)));
        PlayerPrefs.Save();
        SceneManager.LoadScene("MainMenu");

    }

    [System.Serializable]
    public class HighScore
    {
        public int score;
        public string name;
    }
    private class ListObj
    {
        public List<HighScore> listRef;
    }
}
