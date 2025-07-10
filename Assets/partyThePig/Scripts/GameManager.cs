using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    /// <summary>
    /// タイトルシーンへの遷移
    /// </summary>
    public void LoadTitleScene()
    {
        SceneManager.LoadScene("00Title");
    }

    /// <summary>
    /// Homeシーンへの遷移
    /// </summary>
    public void LoadHomeScene()
    {
        SceneManager.LoadScene("01Home");
    }

    /// <summary>
    /// JumpThePigシーンへの遷移
    /// </summary>
    public void LoadJumpThePigScene()
    {
        SceneManager.LoadScene("02JumpThePig");
    }

    /// <summary>
    /// EscapeThePigシーンへの遷移
    /// </summary>
    public void LoadEscapeThePigScene()
    {
        SceneManager.LoadScene("02EscapeThePig");
    }

    /// <summary>
    /// MoneyThePigシーンへの遷移
    /// </summary>
    public void LoadMoneyThePigScene()
    {
        SceneManager.LoadScene("02MoneyThePig");
    }

    /// <summary>
    /// ShootThePigシーンへの遷移
    /// </summary>
    public void LoadShootThePigScene()
    {
        SceneManager.LoadScene("02ShootThePig");
    }

    /// <summary>
    /// SwingThePigシーンへの遷移
    /// </summary>
    public void LoadSwingThePigScene()
    {
        SceneManager.LoadScene("02SwingThePig");
    }

    /// <summary>
    /// TankThePigシーンへの遷移
    /// </summary>
    public void LoadTankThePigScene()
    {
        SceneManager.LoadScene("02TankThePig");
    }

    /// <summary>
    /// リザルトシーンへの遷移
    /// </summary>
    public void LoadResultScene()
    {
        SceneManager.LoadScene("03Result");
    }
    
    public void ReStartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
