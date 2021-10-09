using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// 遊戲結束控制器：
/// 1. 擊殺所有怪物並觸發傳送門
/// 2. 玩家死亡
/// </summary>
public class GameOverController : MonoBehaviour
{
    [Header("結束畫面動畫元件")]
    public Animator aniFinal;
    [Header("結束標題")]
    public Text textFinalTitle;
    [Header("結束選項")]
    public Text textFinalChoose;
    [Header("遊戲勝利與失敗標題")]
    // 字串內的換行 \n
    [TextArea(1, 3)]
    public string stringWin = "你已經抵達試煉之門...\n要接受索斯的考驗嗎?";
    [TextArea(1, 3)]
    public string stringLose = "挑戰失敗...\n再挑戰一次吧...";
    [Header("遊戲勝利與失敗選項")]
    [TextArea(1, 2)]
    public string chooseWin;
    [TextArea(1, 2)]
    public string chooseLose;
    [Header("下一關場景名稱")]
    public string nextScene;
    [Header("重新場景名稱")]
    public string scene;
    [Header("回主選單")]
    public string menu;
    [Header("重新與離開按鈕")]
    public KeyCode kcNext = KeyCode.N;
    public KeyCode kcReplay = KeyCode.R;
    public KeyCode kcQuit = KeyCode.Q;

    /// <summary>
    /// 是否遊戲結束
    /// </summary>
    private bool isGameOver;

    private void Update()
    {
        Next();
        Replay();
        Quit();
    }

    private void Next()
    {
        if (isGameOver && Input.GetKeyDown(kcNext)) SceneManager.LoadScene(nextScene);
    }

    private void Replay()
    {
        if (isGameOver && Input.GetKeyDown(kcReplay)) SceneManager.LoadScene(scene);
    }

    private void Quit()
    {
        if (isGameOver && Input.GetKeyDown(kcQuit)) SceneManager.LoadScene(menu);
    }

    /// <summary>
    /// 顯示遊戲結束畫面
    /// 1. 設定為遊戲結束
    /// 2. 啟動動畫 - 淡入
    /// 3. 判斷勝利或失敗並更新標題
    /// </summary>
    /// <param name="win"></param>
    public void ShowGameOverView(bool win)
    {
        isGameOver = true;
        aniFinal.enabled = true;

        if (win)
        {
            textFinalTitle.text = stringWin;
            textFinalChoose.text = chooseWin;
        }
        else
        {
            textFinalTitle.text = stringLose;
            textFinalChoose.text = chooseLose;
        }
    }
}
