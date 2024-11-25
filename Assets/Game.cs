using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Game : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject player;
    public GameObject medkitPrefab;
    private int round = 0;
    private int enemyCount = 1;
    private int currentEnemySpawned = 0;
    private int currentEnemyKilled = 0;
    // Start is called before the first frame update
    void Start()
    {
        StartRound();
    }

    void StartRound()
    {
        round++;
        if (round == 6)
        {
            FinishGame();
            PrintRound();
            Time.timeScale = 0;
        }
       
        StartCoroutine(PrintRound());
        enemyCount = round * 2;
        Player pl = player.GetComponent<Player>();
        pl.SetDamage(round);
        pl.Heal();
        currentEnemyKilled = 0;
        currentEnemySpawned = 0;
    }

    IEnumerator PrintRound()
    {
        GameObject textObj = GameObject.FindGameObjectWithTag("Text");
        TextMeshProUGUI text = textObj.GetComponent<TextMeshProUGUI>();
        text.text = "";
        String textString;
        if (round == 6)
        {
            textString = "You win!";
           
        }
        else
        {
            textString = "Round " + round;
        }
        for (int i = 0; i < textString.Length; i++)
        {
            text.text += textString[i];
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(1f);
        text.text = "";
    }

    public void FinishGame()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(currentEnemySpawned < enemyCount)
        {
            Vector3 pos = new Vector3();
            Instantiate(enemyPrefab, GetRandomCoords(),Quaternion.identity);
            currentEnemySpawned++;
        }

        if (currentEnemyKilled == enemyCount)
        {
            StartRound();
        }
        
    }

    public Vector3 GetRandomCoords()
    {

        float x = UnityEngine.Random.Range(-17,10);
        float z = UnityEngine.Random.Range(-35,32);
        return new Vector3(x,0, z);

    }

    public void IncreaseKills()
    {
        currentEnemyKilled++;
        int randNum = UnityEngine.Random.Range(0, 3);
        if (randNum ==2)
        {
            GameObject obj = GameObject.FindGameObjectWithTag("PropSpawn");
           
            Instantiate(medkitPrefab, obj.GetComponent<Renderer>().bounds.center + new Vector3(0,4f), new Quaternion());
        }

    }
}
