using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public int test = 0;
    public static string [] Mmedals;
    public void PlayGame(){
        SceneManager.LoadSceneAsync(1);
    }
    public void GoMenu(){
        SceneManager.LoadSceneAsync(0);
    }
   
    void Update(){
        test = Game.cool;
        if(Input.GetKeyDown("space")){ //space
            string s = "";
            for(int i = 0; i < Mmedals.Length; i++){
                s += (Mmedals[i]) + " ";
            }
            Debug.Log(s);
        }

    }
    public static void setMedals(){
        Mmedals = Game.medals;
    }
}
