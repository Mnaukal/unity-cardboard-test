using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour {

    public Text GVRdemo, VRwalk, Instruction;
    public string CZ_GVRdemo, CZ_VRwalk, CZ_Instruction;

    void Start () {
        if(Application.systemLanguage == SystemLanguage.Czech)
        {
            GVRdemo.text = CZ_GVRdemo;
            VRwalk.text = CZ_VRwalk;
            Instruction.text = CZ_Instruction;
        }
	}
	
	public void OpenScene(string name)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(name);
    }
}
