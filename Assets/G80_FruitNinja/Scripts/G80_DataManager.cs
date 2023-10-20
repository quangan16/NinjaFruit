using UnityEngine;

namespace FruitNinja.Scripts
{
    public class G80_DataManager : MonoBehaviour
    {
        public static G80_DataManager Instance;

        public void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                Instance = this;
            }
        }

        public void SaveBestScore(int score)
        {
            PlayerPrefs.SetInt("BestScore", score);
            
        }

        public int LoadBestScore()
        {
            return PlayerPrefs.GetInt("BestScore", 0);
        }

        public void SaveData()
        {
            PlayerPrefs.Save();
        }
    }
}