using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    //TODO. ��i�W�����v�̎���
    //TODO. �GAI�̎���
    //TODO. �G�����������Ɏ~�܂�o�O�̏C��
    //TODO. Main�V�[���ōŏ��Ƀ_�C�A���O��\��









    public static GameManager instance;

    [SerializeField]
    private Slider WizardHpSlider;
    [SerializeField]
    private Slider BringerHpSlider;

    [SerializeField]
    private WizardController wizard;
    [SerializeField]
    private DeathBringerController bringer;

    // �\�����镶�́A�����s�ڂ��A�������蔻��
    public GameObject dialogBox;
    public Text dialogText;

    private string[] dialogLines;
    private int currentTextLine;
    private bool veryFirstClick; // 1�N���b�N�ڂ̓_�C�A���O�{�b�N�X�̕\���B�e�L�X�g�͖���

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

    //HPUI�̐ݒ���X�V
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
