using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogActivator : MonoBehaviour
{
    [SerializeField, Header("âÔòbï∂èÕ"), Multiline(1)]
    private string[] lines;

    private bool canActivator;
    List<int> nums = new List<int>(100);


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0) && !GameManager.instance.dialogBox.activeInHierarchy)
        {
            GameManager.instance.ShowDialog(lines);
        }

       

    }



}
