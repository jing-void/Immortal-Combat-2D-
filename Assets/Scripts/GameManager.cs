using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    //TODO. 二段ジャンプの実装
    //TODO. 敵AIの実装
    //TODO. 敵を殴った時に止まるバグの修正
    //TODO. Mainシーンで最初にダイアログを表示









    public static GameManager instance;

    [SerializeField]
    private Slider WizardHpSlider;
    [SerializeField]
    private Slider BringerHpSlider;

    [SerializeField]
    private WizardController wizard;
    [SerializeField]
    private DeathBringerController bringer;

    // 表示する文章、今何行目か、文字送り判定
    public GameObject dialogBox;
    public Text dialogText;

    private string[] dialogLines;
    private int currentTextLine;
    private bool veryFirstClick; // 1クリック目はダイアログボックスの表示。テキストは無効

    public GameObject statusMenu;
    public Text continueText, restartText, exitText;
    bool isOpenMenu;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        WizardHpSlider.gameObject.SetActive(true);
        BringerHpSlider.gameObject.SetActive(true);
    }

    private void Update()
    {
        

        if (dialogBox.activeInHierarchy)
        {
            if (Input.GetMouseButton(0))
            {
                if (!veryFirstClick)
                {
                    currentTextLine++;
                    
                    if(currentTextLine >= dialogLines.Length)
                    {
                        dialogBox.SetActive(false);
                    }
                }
                else
                {
                    veryFirstClick = false;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.F) && !isOpenMenu && !statusMenu.activeInHierarchy)
        {
            isOpenMenu = true;
            statusMenu.SetActive(true);

            Time.timeScale = 0f;            
        }



    }

    //HPUIの設定を更新
    public void UpdateWizardHealthUI()
    {
        WizardHpSlider.maxValue = wizard.maxHp;
        WizardHpSlider.value = wizard.hp;
    }

    public void UpdateBringerHealthUI()
    {
        BringerHpSlider.maxValue = bringer.hp;

        var damage = GetComponent<WizardController>().at;
        bringer.hp -= damage;
        BringerHpSlider.value = bringer.hp;
    }

    public void ShowDialog(string[] lines)
    {
        dialogLines = lines;
        currentTextLine = 0;

        dialogText.text = dialogLines[currentTextLine];

        dialogBox.SetActive(true);

        veryFirstClick = true;
    }

    public void ShowDialogChange(bool x)
    {
        dialogBox.SetActive(x);
    }

   
    public void ContinueGame()
    {
        statusMenu.SetActive(false);
        Time.timeScale = 1;
    }

    public void RestartGame()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void ExitGame()
    {
        SceneManager.LoadScene("TitleScene");
    }
}
