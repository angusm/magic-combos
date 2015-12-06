using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

public class KeyCodeHandler {

    private string KeyCode;
    private string JoystickNumberPattern = @"^Joystick(\d\d*)";

    /**
     * Constructor for the keyCodeHandler
     */
    public KeyCodeHandler(string keyCodeToHandle)
    {
        this.KeyCode = keyCodeToHandle;
    }

    public bool IsJoystickInput()
    {
        return Regex.IsMatch(this.KeyCode, this.JoystickNumberPattern);
    }

    public string GetJoystickNumber()
    {
        GroupCollection JoystickCode = Regex.Match(this.KeyCode, JoystickNumberPattern).Groups;
        return JoystickCode[1].Value;
    }

}
