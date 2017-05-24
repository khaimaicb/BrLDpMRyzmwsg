using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace GameTaco
{
	public class TacoDebugConsole : MonoBehaviour {
	    [Header("Outlets")]
	    public Text debugLogText;
	    public ScrollRect scrollRect;

	    private string debugLogString;
	    private Canvas canvas;

	    void Awake() {
	    	canvas = GetComponent<Canvas>();
	    	ClearLog();
	    }

		public void Log(string log) {
			// TODO: Check for max log length for log rotation (65k chars max)
			debugLogString += "\n" + Timestamp() + log;
			debugLogText.text = debugLogString;

			UpdateScroll();
		}

		private string Timestamp() {
			return System.DateTime.UtcNow.ToString("H:mm:ss.fff: ");
		}

		private void ClearLog() {
			debugLogString = "";
			debugLogText.text = "";
		}

		private void UpdateScroll() {
			Canvas.ForceUpdateCanvases();
			scrollRect.verticalNormalizedPosition = 0;
			Canvas.ForceUpdateCanvases();
		}
	}
}
