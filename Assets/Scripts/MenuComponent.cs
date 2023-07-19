using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Towers;

namespace Menu
{
    public class MenuComponent : MonoBehaviour
    {
        [SerializeField]
        private GameObject _pauseMenu;
        [SerializeField]
        private GameObject _mainMenu;

        private string FileName = Directory.GetCurrentDirectory() + @"\Save.txt";



        public void StartGame()
        {
            GameManager.Instance._gameIsPaused = false;
            Time.timeScale = 1f;
            _mainMenu.SetActive(false);
        }

        public void PauseGame()
        {
            if (GameManager.Instance._gameIsPaused) return;
            Time.timeScale = 0f;
            GameManager.Instance._gameIsPaused = true;
            _pauseMenu.SetActive(true);
        }

        public void ResumeGame()
        {
            if (GameManager.Instance._gameIsPaused == false) return;
            Time.timeScale = 1f;
            GameManager.Instance._gameIsPaused = false;
            _pauseMenu.SetActive(false);
        }

        public void ReloadGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void QuitGame()
        {
            Application.Quit();
        }

        public void SaveGame()
        {
            if (File.Exists(FileName))
            {
                File.Delete(FileName);
            }     
            

            foreach (var cell in GameManager.Instance.GetRoundCellsList())
            {
                WriteReadFile("Tower = " + cell.GetTower().GetComponentInChildren<TowerComponent>().GetTowerPrefabPath() + " = " + cell.name);
            }

            foreach(var Param in GameManager.Instance.GetSaveParams())
            {
                WriteReadFile(Param.Key + " = " + Param.Value.ToString());
            }

     
        }

        public void LoadGame()
        {
            _mainMenu.SetActive(false);
            List<string> DeserealizedList = DeserializeString();
            Dictionary<string, int> IntParams = new Dictionary<string, int>();
            List<string> Towers = new List<string>();

            foreach (var Param in DeserealizedList)
            {
                string[] ParamParts = Param.Split(" = ");
                if (ParamParts.Length == 3)
                {
                    Towers.Add(Param);
                }
                else if (ParamParts.Length == 2)
                {
                    IntParams.Add(ParamParts[0], int.Parse(ParamParts[1]));
                }
            }

            GameManager.Instance.LoadGame(IntParams, Towers);

        }

        public async Task SerializeString(string SerializeString)
        {
            await using var fileStream = new FileStream(FileName, FileMode.Append, FileAccess.Write);
            await using var streamWriter = new StreamWriter(fileStream);

            await streamWriter.WriteLineAsync(SerializeString);

            streamWriter.Close();
            fileStream.Close();
        }

        private async void WriteReadFile(string Action)
        {
            await SerializeString(Action);
        }

        public List<string> DeserializeString()
        {
            if (!File.Exists(FileName))
            {
                return null;
            }

            using var fileStream = new FileStream(FileName, FileMode.Open);
            using var streamReader = new StreamReader(fileStream);

            List<string> DeserealizedList = new List<string>();

            while (!streamReader.EndOfStream)
            {
                DeserealizedList.Add(streamReader.ReadLine());
            }

            streamReader.Close();
            fileStream.Close();

            return DeserealizedList;
        }

    }
}

