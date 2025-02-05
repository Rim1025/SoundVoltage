using System;
using UnityEngine.SceneManagement;

namespace Services
{
    public class SceneChanger
    {
        public void Change(string name)
        {
            try
            {
                SceneManager.LoadScene(name);
            }
            catch (Exception e)
            {
                Err.Err.ViewErr("シーン読み込み失敗\n" +e);
                throw;
            } 
        }
    }
}