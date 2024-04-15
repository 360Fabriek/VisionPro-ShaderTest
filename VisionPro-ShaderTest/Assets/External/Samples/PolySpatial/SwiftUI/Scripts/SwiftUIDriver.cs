using System.Collections.Generic;
using System.Runtime.InteropServices;
using AOT;
using PolySpatial.Samples;
using UnityEngine;

namespace Samples.PolySpatial.SwiftUI.Scripts
{
    // This is a driver MonoBehaviour that connects to SwiftUISamplePlugin.swift via
    // C# DllImport. See SwiftUISamplePlugin.swift for more information.
    public class SwiftUIDriver : MonoBehaviour
    {
        [SerializeField]
        SpatialUIButton m_Button;

        [SerializeField]
        List<GameObject> m_ObjectsToSpawn;

        [SerializeField]
        Transform m_SpawnPosition;

        bool m_SwiftUIWindowOpen = false;

        void OnEnable()
        {
            m_Button.WasPressed += WasPressed;
            SetNativeCallback(CallbackFromNative);
        }

        void OnDisable()
        {
            SetNativeCallback(null);
            CloseSwiftUIWindow("HelloWorld");
        }

        void WasPressed(string buttonText, MeshRenderer meshrenderer)
        {
            if (m_SwiftUIWindowOpen)
            {
                CloseSwiftUIWindow("HelloWorld");
                m_SwiftUIWindowOpen = false;
            }
            else
            {
                OpenSwiftUIWindow("HelloWorld");
                m_SwiftUIWindowOpen = true;
            }
        }

        delegate void CallbackDelegate(string command);

        // This attribute is required for methods that are going to be called from native code
        // via a function pointer.
        [MonoPInvokeCallback(typeof(CallbackDelegate))]
        static void CallbackFromNative(string command)
        {
            Debug.Log("Callback from native: " + command);

            // This could be stored in a static field or a singleton.
            // If you need to deal with multiple windows and need to distinguish between them,
            // you could add an ID to this callback and use that to distinguish windows.
            var self = Object.FindFirstObjectByType<SwiftUIDriver>();

            if (command == "closed") {
                self.m_SwiftUIWindowOpen = false;
                return;
            }

            if (command == "spawn red")
            {
                self.Spawn(Color.red);
            }
            else if (command == "spawn green")
            {
                self.Spawn(Color.green);
            }
            else if (command == "spawn blue")
            {
                self.Spawn(Color.blue);
            }
        }

        void Spawn(Color color)
        {
            var randomObject = Random.Range(0, m_ObjectsToSpawn.Count);
            var thing = Instantiate(m_ObjectsToSpawn[randomObject], m_SpawnPosition.position, Quaternion.identity);
            thing.GetComponent<MeshRenderer>().material.color = color;
        }

        #if UNITY_VISIONOS && !UNITY_EDITOR
        [DllImport("__Internal")]
        static extern void SetNativeCallback(CallbackDelegate callback);

        [DllImport("__Internal")]
        static extern void OpenSwiftUIWindow(string name);

        [DllImport("__Internal")]
        static extern void CloseSwiftUIWindow(string name);
        #else
        static void SetNativeCallback(CallbackDelegate callback) {}
        static void OpenSwiftUIWindow(string name) {}
        static void CloseSwiftUIWindow(string name) {}
        #endif

    }
}
