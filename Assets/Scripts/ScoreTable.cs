using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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
        ListObj ListObj = JsonUtility.FromJson<ListObj>(jsonHS);

        for (int i = 0; i < ListObj.listRef.Count; i++) {
            for (int j = i+1; j < ListObj.listRef.Count; j++) {
                if (ListObj.listRef[j].score > ListObj.listRef[i].score) {
                    HighScore tmp = ListObj.listRef[i];
                    ListObj.listRef[i] = ListObj.listRef[j];
                    ListObj.listRef[j] = tmp;
                }
            }
        }

        foreach (HighScore data in ListObj.listRef)
        {
            CreateHighscore(data, container, transformList);
        }
    }
    private void CreateHighscore(HighScore data, Transform container, List<Transform> transformList)
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
    public void AddHighscore(int score,string name)
    {
        HighScore newScore = new HighScore { score = score, name = name };
        string jsonFW = PlayerPrefs.GetString("Highscore");
        ListObj listObj = JsonUtility.FromJson<ListObj>(jsonFW);
        listObj.listRef.Add(newScore);
        string jsonFR = JsonUtility.ToJson(listObj);
        PlayerPrefs.SetString("Highscore", jsonFR);
        PlayerPrefs.Save();
    }

    [System.Serializable]
    private class HighScore
    {
        public int score;
        public string name;
    }
    private class ListObj
    {
        public List<HighScore> listRef;
    }
}
