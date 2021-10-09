using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// �C����������G
/// 1. �����Ҧ��Ǫ���Ĳ�o�ǰe��
/// 2. ���a���`
/// </summary>
public class GameOverController : MonoBehaviour
{
    [Header("�����e���ʵe����")]
    public Animator aniFinal;
    [Header("�������D")]
    public Text textFinalTitle;
    [Header("�����ﶵ")]
    public Text textFinalChoose;
    [Header("�C���ӧQ�P���Ѽ��D")]
    // �r�ꤺ������ \n
    [TextArea(1, 3)]
    public string stringWin = "�A�w�g��F�շҤ���...\n�n���������������?";
    [TextArea(1, 3)]
    public string stringLose = "�D�ԥ���...\n�A�D�Ԥ@���a...";
    [Header("�C���ӧQ�P���ѿﶵ")]
    [TextArea(1, 2)]
    public string chooseWin;
    [TextArea(1, 2)]
    public string chooseLose;
    [Header("�U�@�������W��")]
    public string nextScene;
    [Header("���s�����W��")]
    public string scene;
    [Header("�^�D���")]
    public string menu;
    [Header("���s�P���}���s")]
    public KeyCode kcNext = KeyCode.N;
    public KeyCode kcReplay = KeyCode.R;
    public KeyCode kcQuit = KeyCode.Q;

    /// <summary>
    /// �O�_�C������
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
    /// ��ܹC�������e��
    /// 1. �]�w���C������
    /// 2. �Ұʰʵe - �H�J
    /// 3. �P�_�ӧQ�Υ��Ѩç�s���D
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
