using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class XRGrabPrimaryButton : XRGrabInteractable
{
    public UnityEvent primaryButtonPress;
    private bool lastButtonState = false;
    private List<InputDevice> handDevice = new List<InputDevice>();

    private string prevHandedness = "None";
    private string _handedness = "None";

    private string handedness
    {
        get
        {
            return _handedness;
        }
        set
        {
            _handedness = value;
            updateInputDevices();
        }
    }


    private void Start()
    {
        InputTracking.nodeAdded += InputTracking_nodeAdded;
    }

    // check for new input devices when new XRNode is added
    private void InputTracking_nodeAdded(XRNodeState obj)
    {
        updateInputDevices();
    }

    void Update()
    {
        if (selectingInteractor == null) return;

        if (selectingInteractor.name == "LeftBaseController")
        {
            if (prevHandedness != "Left")
            {
                handedness = "Left";
                prevHandedness = handedness;
            }
        }
        else if (selectingInteractor.name == "RightBaseController")
        {
            if (prevHandedness != "Right")
            {
                handedness = "Right";
                prevHandedness = handedness;
            }
        }
        bool tempState = false;
        bool invalidDeviceFound = false;
        foreach (var device in handDevice)
        {
            bool primaryButtonState = false;
            tempState = device.TryGetFeatureValue(CommonUsages.primaryButton, out primaryButtonState) // did get a value
                        && primaryButtonState // the value we got
                        || tempState; // cumulative result from other controllers
            if (!device.isValid) invalidDeviceFound = true;
        }

        if (tempState != lastButtonState) // Button state changed since last frame
        {
            if (tempState)
            {
                primaryButtonPress.Invoke();
            }
            lastButtonState = tempState;
        }

        if (invalidDeviceFound || handDevice.Count == 0)
        {
            updateInputDevices();
        }
    }

    // find any devices supporting the desired feature usage
    void updateInputDevices()
    {
        handDevice.Clear();
        List<InputDevice> devices = new List<InputDevice>();
        bool discardedValue;
        InputDeviceCharacteristics controllerCharacteristics = InputDeviceCharacteristics.Controller;
        if (handedness == "Left")
        {
            controllerCharacteristics |= InputDeviceCharacteristics.Left;
        }
        else if (handedness == "Right")
        {
            controllerCharacteristics |= InputDeviceCharacteristics.Right;
        }
        InputDevices.GetDevicesWithCharacteristics(controllerCharacteristics, devices);
        foreach (var device in devices)
        {
            if (device.TryGetFeatureValue(CommonUsages.primaryButton, out discardedValue))
            {
                handDevice.Add(device); // Add any devices that have a primary button.
            }
        }
    }
}
