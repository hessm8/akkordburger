// GENERATED AUTOMATICALLY FROM 'Assets/InputMaster.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @InputMaster : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @InputMaster()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""InputMaster"",
    ""maps"": [
        {
            ""name"": ""NotePlay"",
            ""id"": ""7ed58c11-ee54-4769-9ecf-7b9cb6103403"",
            ""actions"": [
                {
                    ""name"": ""PressAny"",
                    ""type"": ""Button"",
                    ""id"": ""149b2649-488b-4934-982d-806ea417c8b2"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ReleaseAny"",
                    ""type"": ""Button"",
                    ""id"": ""2885b99c-53a5-44fc-851f-da2002717f7d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": []
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Computer"",
            ""bindingGroup"": ""Computer"",
            ""devices"": []
        }
    ]
}");
        // NotePlay
        m_NotePlay = asset.FindActionMap("NotePlay", throwIfNotFound: true);
        m_NotePlay_PressAny = m_NotePlay.FindAction("PressAny", throwIfNotFound: true);
        m_NotePlay_ReleaseAny = m_NotePlay.FindAction("ReleaseAny", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // NotePlay
    private readonly InputActionMap m_NotePlay;
    private INotePlayActions m_NotePlayActionsCallbackInterface;
    private readonly InputAction m_NotePlay_PressAny;
    private readonly InputAction m_NotePlay_ReleaseAny;
    public struct NotePlayActions
    {
        private @InputMaster m_Wrapper;
        public NotePlayActions(@InputMaster wrapper) { m_Wrapper = wrapper; }
        public InputAction @PressAny => m_Wrapper.m_NotePlay_PressAny;
        public InputAction @ReleaseAny => m_Wrapper.m_NotePlay_ReleaseAny;
        public InputActionMap Get() { return m_Wrapper.m_NotePlay; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(NotePlayActions set) { return set.Get(); }
        public void SetCallbacks(INotePlayActions instance)
        {
            if (m_Wrapper.m_NotePlayActionsCallbackInterface != null)
            {
                @PressAny.started -= m_Wrapper.m_NotePlayActionsCallbackInterface.OnPressAny;
                @PressAny.performed -= m_Wrapper.m_NotePlayActionsCallbackInterface.OnPressAny;
                @PressAny.canceled -= m_Wrapper.m_NotePlayActionsCallbackInterface.OnPressAny;
                @ReleaseAny.started -= m_Wrapper.m_NotePlayActionsCallbackInterface.OnReleaseAny;
                @ReleaseAny.performed -= m_Wrapper.m_NotePlayActionsCallbackInterface.OnReleaseAny;
                @ReleaseAny.canceled -= m_Wrapper.m_NotePlayActionsCallbackInterface.OnReleaseAny;
            }
            m_Wrapper.m_NotePlayActionsCallbackInterface = instance;
            if (instance != null)
            {
                @PressAny.started += instance.OnPressAny;
                @PressAny.performed += instance.OnPressAny;
                @PressAny.canceled += instance.OnPressAny;
                @ReleaseAny.started += instance.OnReleaseAny;
                @ReleaseAny.performed += instance.OnReleaseAny;
                @ReleaseAny.canceled += instance.OnReleaseAny;
            }
        }
    }
    public NotePlayActions @NotePlay => new NotePlayActions(this);
    private int m_ComputerSchemeIndex = -1;
    public InputControlScheme ComputerScheme
    {
        get
        {
            if (m_ComputerSchemeIndex == -1) m_ComputerSchemeIndex = asset.FindControlSchemeIndex("Computer");
            return asset.controlSchemes[m_ComputerSchemeIndex];
        }
    }
    public interface INotePlayActions
    {
        void OnPressAny(InputAction.CallbackContext context);
        void OnReleaseAny(InputAction.CallbackContext context);
    }
}
