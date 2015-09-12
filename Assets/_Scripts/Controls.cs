using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Controls : MonoBehaviour {
	public static Dictionary<string,List<string>> controls = new Dictionary<string, List<string>>();
	
	public const string keyboard01 = "Keyboard01";
	public const string keyboard02 = "Keyboard02";
	public const string joystick01 = "Joystick01";
	public const string joystick02 = "Joystick02";
	public const string joystick03 = "Joystick03";
	public const string joystick04 = "Joystick04";

	private List<string> _keyboard01Controls = new List<string>();
	private List<string> _keyboard02Controls = new List<string>();
	private List<string> _joystick01Controls = new List<string>();
	private List<string> _joystick02Controls = new List<string>();
	private List<string> _joystick03Controls = new List<string>();
	private List<string> _joystick04Controls = new List<string>();
	void Awake()
	{
		_keyboard01Controls.Add("HorizontalPlayer1"); //0
		_keyboard01Controls.Add("VerticalPlayer1"); //1
		_keyboard01Controls.Add("ActionKeyPlayer1"); //2

		_keyboard02Controls.Add("HorizontalPlayer2"); //0
		_keyboard02Controls.Add("VerticalPlayer2"); //1
		_keyboard02Controls.Add("ActionKeyPlayer2"); //2

		_joystick01Controls.Add("JoystickHorizontal1"); //0
		_joystick02Controls.Add("JoystickHorizontal2"); //0
		_joystick03Controls.Add("JoystickHorizontal3"); //0
		_joystick04Controls.Add("JoystickHorizontal4"); //0

		_joystick01Controls.Add("JoystickVertical1"); //1
		_joystick02Controls.Add("JoystickVertical2"); //1
		_joystick03Controls.Add("JoystickVertical3"); //1
		_joystick04Controls.Add("JoystickVertical4"); //1

		_joystick01Controls.Add("JoystickActionKey1"); //2
		_joystick02Controls.Add("JoystickActionKey2"); //2
		_joystick03Controls.Add("JoystickActionKey3"); //2
		_joystick04Controls.Add("JoystickActionKey4"); //2

		//joystick has extra jump button for easier playability
		_joystick01Controls.Add("JoystickJumpKey1"); //3
		_joystick02Controls.Add("JoystickJumpKey2"); //3
		_joystick03Controls.Add("JoystickJumpKey3"); //3
		_joystick04Controls.Add("JoystickJumpKey4"); //3

		controls.Add(keyboard01, _keyboard01Controls);
		controls.Add(keyboard02, _keyboard02Controls);

		controls.Add(joystick01, _joystick01Controls);
		controls.Add(joystick02, _joystick02Controls);
		controls.Add(joystick03, _joystick03Controls);
		controls.Add(joystick04, _joystick04Controls);
	}

	public static List<string> GetControls(string controller)
	{
		return controls[controller];
	}
}
