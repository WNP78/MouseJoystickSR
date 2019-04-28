using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ModApi.Input;
using ModApi.Mocks;

public class MouseJoystickScript : MonoBehaviour
{
    GameInputMock myPitch;
    GameInputMock myRoll;
    IGameInput defaultPitch;
    IGameInput defaultRoll;
    private void Start()
    {
        ServiceProvider.Instance.Game.SceneManager.SceneLoaded += SceneLoaded;
        defaultPitch = ServiceProvider.Instance.Game.Inputs.Pitch;
        myPitch = new GameInputMock
        {
            Id = defaultPitch.Id,
            DescriptiveName = defaultPitch.DescriptiveName
        };
        defaultRoll = ServiceProvider.Instance.Game.Inputs.Roll;
        myRoll = new GameInputMock
        {
            Id = defaultRoll.Id,
            DescriptiveName = defaultRoll.Id
        };
        
    }

    private void SceneLoaded(object sender, ModApi.Scenes.Events.SceneEventArgs e)
    {
        enabled = e.Scene == ModApi.Scenes.SceneNames.Flight;
    }

    bool mouseJoystickEnabled = false;
    void OnEnable()
    {
        mouseJoystickEnabled = false;
    }
    void OnDisable()
    {
        mouseJoystickEnabled = false;
        SetOverride(false);
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(2)) //  middle click
        {
            mouseJoystickEnabled = !mouseJoystickEnabled;
            SetOverride(mouseJoystickEnabled);
            ServiceProvider.Instance.Game.FlightScene.FlightSceneUI.ShowMessage("Mouse Joystick " + (mouseJoystickEnabled ? "Enabled" : "Disabled") + ".");
        }
        if (mouseJoystickEnabled)
        {
            myPitch.AxisValue = ((Input.mousePosition.y / Screen.height) * 2) - 1;
            myRoll.AxisValue = ((Input.mousePosition.x / Screen.width) * 2) - 1;
        }
    }
    void SetOverride(bool value)
    {
        ServiceProvider.Instance.Game.Inputs.SetP("Pitch", value ? myPitch : defaultPitch);
        ServiceProvider.Instance.Game.Inputs.SetP("Roll", value ? myRoll : defaultRoll);
    }
}