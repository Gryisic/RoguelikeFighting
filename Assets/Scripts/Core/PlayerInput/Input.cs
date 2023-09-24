// GENERATED AUTOMATICALLY FROM 'Assets/Scripts/Core/PlayerInput/Input.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace Core.PlayerInput
{
    public class @Input : IInputActionCollection, IDisposable
    {
        public InputActionAsset asset { get; }
        public @Input()
        {
            asset = InputActionAsset.FromJson(@"{
    ""name"": ""Input"",
    ""maps"": [
        {
            ""name"": ""Gameplay"",
            ""id"": ""a5b46b2f-670e-4487-8d21-2d11260e2691"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""463a00af-cfa1-4949-b8d6-969cd6d17f6f"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Interact"",
                    ""type"": ""Button"",
                    ""id"": ""07e88862-fb53-49db-976b-a51ac5ee9311"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Attack"",
                    ""type"": ""Button"",
                    ""id"": ""1a5914dc-29a7-4311-8f11-fd0c24c4fb8b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Skill"",
                    ""type"": ""Button"",
                    ""id"": ""652c52f7-3eef-40a7-8bb0-cd4443745c6c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""799a2927-aac4-429d-9418-6a834ab0383c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Dash"",
                    ""type"": ""Button"",
                    ""id"": ""8ef006ab-a6aa-4d2c-88c3-f5ebf5087b10"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""FirstLegacySkill"",
                    ""type"": ""Button"",
                    ""id"": ""27958472-8dde-449d-86c3-c44fbab5a1e0"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""SecondLegacySkill"",
                    ""type"": ""Button"",
                    ""id"": ""0a06fc02-5015-488e-80ac-5f48a56f6381"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""Direction"",
                    ""id"": ""1999cdc7-4d92-4fef-9e72-7d6e4bd9e02f"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""083b9b41-ff79-47ce-878d-5460b057a486"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""c060d2f5-b84e-4c9d-9447-839b462987f4"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""ed4e09ce-d823-433a-8f81-79eba3ee83d2"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""293de7f8-2000-4d2a-a340-849e93cb7429"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""35fa8cef-8efa-49e3-8bbf-d2c5d89fd221"",
                    ""path"": ""<Keyboard>/f"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a7acb818-c4fe-4818-8f16-3c456b621a4a"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6b524025-88e6-401f-a8e0-bf05ef5556fa"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Skill"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f9e60d30-4d0c-4ac8-8c9e-1bb46ead8282"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""82fe78a1-1f56-4221-8264-e542abf57674"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Dash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""187aa843-9458-4e76-81c6-80fb22dba9d7"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""FirstLegacySkill"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""40e9b525-6de0-4daa-98d7-b7566849f31d"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SecondLegacySkill"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Menu"",
            ""id"": ""47a2cb44-f9c5-4501-85dc-b9eaa853a8a1"",
            ""actions"": [
                {
                    ""name"": ""Select"",
                    ""type"": ""Button"",
                    ""id"": ""d49e66eb-4291-4ef7-81f2-58cc3dbb0c32"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""ce56a31b-77d8-4b2b-8137-1b4889651a40"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Select"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Debug"",
            ""id"": ""07a6119a-61c2-4d0c-a2ed-c2f5f90e6502"",
            ""actions"": [
                {
                    ""name"": ""SetDebugModifier"",
                    ""type"": ""Button"",
                    ""id"": ""10b1fd06-2486-4159-85c2-d52cdefd2f8f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""DamageHero"",
                    ""type"": ""Button"",
                    ""id"": ""01add67c-a7ab-4fed-b902-3e5e2bf785f5"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""ef2ee727-a9b7-4e21-851a-73162f08f9b2"",
                    ""path"": ""<Keyboard>/1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SetDebugModifier"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""40e6fbfa-aee4-4b20-87c5-10ed4b53d3b8"",
                    ""path"": ""<Keyboard>/2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""DamageHero"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
            // Gameplay
            m_Gameplay = asset.FindActionMap("Gameplay", throwIfNotFound: true);
            m_Gameplay_Move = m_Gameplay.FindAction("Move", throwIfNotFound: true);
            m_Gameplay_Interact = m_Gameplay.FindAction("Interact", throwIfNotFound: true);
            m_Gameplay_Attack = m_Gameplay.FindAction("Attack", throwIfNotFound: true);
            m_Gameplay_Skill = m_Gameplay.FindAction("Skill", throwIfNotFound: true);
            m_Gameplay_Jump = m_Gameplay.FindAction("Jump", throwIfNotFound: true);
            m_Gameplay_Dash = m_Gameplay.FindAction("Dash", throwIfNotFound: true);
            m_Gameplay_FirstLegacySkill = m_Gameplay.FindAction("FirstLegacySkill", throwIfNotFound: true);
            m_Gameplay_SecondLegacySkill = m_Gameplay.FindAction("SecondLegacySkill", throwIfNotFound: true);
            // Menu
            m_Menu = asset.FindActionMap("Menu", throwIfNotFound: true);
            m_Menu_Select = m_Menu.FindAction("Select", throwIfNotFound: true);
            // Debug
            m_Debug = asset.FindActionMap("Debug", throwIfNotFound: true);
            m_Debug_SetDebugModifier = m_Debug.FindAction("SetDebugModifier", throwIfNotFound: true);
            m_Debug_DamageHero = m_Debug.FindAction("DamageHero", throwIfNotFound: true);
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

        // Gameplay
        private readonly InputActionMap m_Gameplay;
        private IGameplayActions m_GameplayActionsCallbackInterface;
        private readonly InputAction m_Gameplay_Move;
        private readonly InputAction m_Gameplay_Interact;
        private readonly InputAction m_Gameplay_Attack;
        private readonly InputAction m_Gameplay_Skill;
        private readonly InputAction m_Gameplay_Jump;
        private readonly InputAction m_Gameplay_Dash;
        private readonly InputAction m_Gameplay_FirstLegacySkill;
        private readonly InputAction m_Gameplay_SecondLegacySkill;
        public struct GameplayActions
        {
            private @Input m_Wrapper;
            public GameplayActions(@Input wrapper) { m_Wrapper = wrapper; }
            public InputAction @Move => m_Wrapper.m_Gameplay_Move;
            public InputAction @Interact => m_Wrapper.m_Gameplay_Interact;
            public InputAction @Attack => m_Wrapper.m_Gameplay_Attack;
            public InputAction @Skill => m_Wrapper.m_Gameplay_Skill;
            public InputAction @Jump => m_Wrapper.m_Gameplay_Jump;
            public InputAction @Dash => m_Wrapper.m_Gameplay_Dash;
            public InputAction @FirstLegacySkill => m_Wrapper.m_Gameplay_FirstLegacySkill;
            public InputAction @SecondLegacySkill => m_Wrapper.m_Gameplay_SecondLegacySkill;
            public InputActionMap Get() { return m_Wrapper.m_Gameplay; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(GameplayActions set) { return set.Get(); }
            public void SetCallbacks(IGameplayActions instance)
            {
                if (m_Wrapper.m_GameplayActionsCallbackInterface != null)
                {
                    @Move.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMove;
                    @Move.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMove;
                    @Move.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMove;
                    @Interact.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnInteract;
                    @Interact.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnInteract;
                    @Interact.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnInteract;
                    @Attack.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnAttack;
                    @Attack.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnAttack;
                    @Attack.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnAttack;
                    @Skill.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSkill;
                    @Skill.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSkill;
                    @Skill.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSkill;
                    @Jump.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnJump;
                    @Jump.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnJump;
                    @Jump.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnJump;
                    @Dash.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnDash;
                    @Dash.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnDash;
                    @Dash.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnDash;
                    @FirstLegacySkill.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnFirstLegacySkill;
                    @FirstLegacySkill.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnFirstLegacySkill;
                    @FirstLegacySkill.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnFirstLegacySkill;
                    @SecondLegacySkill.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSecondLegacySkill;
                    @SecondLegacySkill.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSecondLegacySkill;
                    @SecondLegacySkill.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSecondLegacySkill;
                }
                m_Wrapper.m_GameplayActionsCallbackInterface = instance;
                if (instance != null)
                {
                    @Move.started += instance.OnMove;
                    @Move.performed += instance.OnMove;
                    @Move.canceled += instance.OnMove;
                    @Interact.started += instance.OnInteract;
                    @Interact.performed += instance.OnInteract;
                    @Interact.canceled += instance.OnInteract;
                    @Attack.started += instance.OnAttack;
                    @Attack.performed += instance.OnAttack;
                    @Attack.canceled += instance.OnAttack;
                    @Skill.started += instance.OnSkill;
                    @Skill.performed += instance.OnSkill;
                    @Skill.canceled += instance.OnSkill;
                    @Jump.started += instance.OnJump;
                    @Jump.performed += instance.OnJump;
                    @Jump.canceled += instance.OnJump;
                    @Dash.started += instance.OnDash;
                    @Dash.performed += instance.OnDash;
                    @Dash.canceled += instance.OnDash;
                    @FirstLegacySkill.started += instance.OnFirstLegacySkill;
                    @FirstLegacySkill.performed += instance.OnFirstLegacySkill;
                    @FirstLegacySkill.canceled += instance.OnFirstLegacySkill;
                    @SecondLegacySkill.started += instance.OnSecondLegacySkill;
                    @SecondLegacySkill.performed += instance.OnSecondLegacySkill;
                    @SecondLegacySkill.canceled += instance.OnSecondLegacySkill;
                }
            }
        }
        public GameplayActions @Gameplay => new GameplayActions(this);

        // Menu
        private readonly InputActionMap m_Menu;
        private IMenuActions m_MenuActionsCallbackInterface;
        private readonly InputAction m_Menu_Select;
        public struct MenuActions
        {
            private @Input m_Wrapper;
            public MenuActions(@Input wrapper) { m_Wrapper = wrapper; }
            public InputAction @Select => m_Wrapper.m_Menu_Select;
            public InputActionMap Get() { return m_Wrapper.m_Menu; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(MenuActions set) { return set.Get(); }
            public void SetCallbacks(IMenuActions instance)
            {
                if (m_Wrapper.m_MenuActionsCallbackInterface != null)
                {
                    @Select.started -= m_Wrapper.m_MenuActionsCallbackInterface.OnSelect;
                    @Select.performed -= m_Wrapper.m_MenuActionsCallbackInterface.OnSelect;
                    @Select.canceled -= m_Wrapper.m_MenuActionsCallbackInterface.OnSelect;
                }
                m_Wrapper.m_MenuActionsCallbackInterface = instance;
                if (instance != null)
                {
                    @Select.started += instance.OnSelect;
                    @Select.performed += instance.OnSelect;
                    @Select.canceled += instance.OnSelect;
                }
            }
        }
        public MenuActions @Menu => new MenuActions(this);

        // Debug
        private readonly InputActionMap m_Debug;
        private IDebugActions m_DebugActionsCallbackInterface;
        private readonly InputAction m_Debug_SetDebugModifier;
        private readonly InputAction m_Debug_DamageHero;
        public struct DebugActions
        {
            private @Input m_Wrapper;
            public DebugActions(@Input wrapper) { m_Wrapper = wrapper; }
            public InputAction @SetDebugModifier => m_Wrapper.m_Debug_SetDebugModifier;
            public InputAction @DamageHero => m_Wrapper.m_Debug_DamageHero;
            public InputActionMap Get() { return m_Wrapper.m_Debug; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(DebugActions set) { return set.Get(); }
            public void SetCallbacks(IDebugActions instance)
            {
                if (m_Wrapper.m_DebugActionsCallbackInterface != null)
                {
                    @SetDebugModifier.started -= m_Wrapper.m_DebugActionsCallbackInterface.OnSetDebugModifier;
                    @SetDebugModifier.performed -= m_Wrapper.m_DebugActionsCallbackInterface.OnSetDebugModifier;
                    @SetDebugModifier.canceled -= m_Wrapper.m_DebugActionsCallbackInterface.OnSetDebugModifier;
                    @DamageHero.started -= m_Wrapper.m_DebugActionsCallbackInterface.OnDamageHero;
                    @DamageHero.performed -= m_Wrapper.m_DebugActionsCallbackInterface.OnDamageHero;
                    @DamageHero.canceled -= m_Wrapper.m_DebugActionsCallbackInterface.OnDamageHero;
                }
                m_Wrapper.m_DebugActionsCallbackInterface = instance;
                if (instance != null)
                {
                    @SetDebugModifier.started += instance.OnSetDebugModifier;
                    @SetDebugModifier.performed += instance.OnSetDebugModifier;
                    @SetDebugModifier.canceled += instance.OnSetDebugModifier;
                    @DamageHero.started += instance.OnDamageHero;
                    @DamageHero.performed += instance.OnDamageHero;
                    @DamageHero.canceled += instance.OnDamageHero;
                }
            }
        }
        public DebugActions @Debug => new DebugActions(this);
        public interface IGameplayActions
        {
            void OnMove(InputAction.CallbackContext context);
            void OnInteract(InputAction.CallbackContext context);
            void OnAttack(InputAction.CallbackContext context);
            void OnSkill(InputAction.CallbackContext context);
            void OnJump(InputAction.CallbackContext context);
            void OnDash(InputAction.CallbackContext context);
            void OnFirstLegacySkill(InputAction.CallbackContext context);
            void OnSecondLegacySkill(InputAction.CallbackContext context);
        }
        public interface IMenuActions
        {
            void OnSelect(InputAction.CallbackContext context);
        }
        public interface IDebugActions
        {
            void OnSetDebugModifier(InputAction.CallbackContext context);
            void OnDamageHero(InputAction.CallbackContext context);
        }
    }
}
