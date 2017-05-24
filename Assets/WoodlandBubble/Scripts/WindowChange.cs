 using UnityEngine;
 using UnityEngine.UI;
 
public class WindowChange : MonoBehaviour {
    public Dropdown resolutionDropdown;
    void Start() {
        resolutionDropdown.onValueChanged.AddListener(delegate {
            myDropdownValueChangedHandler(resolutionDropdown);
        });
        string resolutionCurrent = Screen.width + "x" + Screen.height;
        for (int i = 0; i < resolutionDropdown.options.Count; i++)
        {
            if(resolutionDropdown.options[i].text == resolutionCurrent) {
                SetDropdownIndex(i);
            }
        }
    }
    void Destroy() {
        resolutionDropdown.onValueChanged.RemoveAllListeners();
    }
 
    private void myDropdownValueChangedHandler(Dropdown target) {
        Debug.Log("selected: "+ target.captionText.text);
        if(!string.IsNullOrEmpty(target.captionText.text)) {
            string[] resolution  = target.captionText.text.Split('x');
            Screen.SetResolution(int.Parse(resolution[0]), int.Parse(resolution[1]), false);
        }
    }
    public void SetDropdownIndex(int index) {
        resolutionDropdown.value = index;
    }
}
