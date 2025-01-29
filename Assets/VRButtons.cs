using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using RoslynCSharp.Example;
using Random = UnityEngine.Random;


public class VRButtons : MonoBehaviour
    {
        public InputActionReference rightButton, RightOtherButton;

        // Start is called before the first frame update
        void Start()
        {
            // rightButton.action.performed += RunCrawlerVR;
            RightOtherButton.action.performed += DebugTest; 
        }

        private void Update()
        {
            if (rightButton.action.IsPressed())
            {
                RunCrawlerVR(); 
            }
        }

        public void RunCrawlerVR()
        {
            Debug.Log("Trying to run the Code");
            FindObjectOfType<MazeCrawlerExample>().RunCrawler(); 
            Debug.Log("Ran the code?");
        }
        public void DebugTest(InputAction.CallbackContext obj)
        {
            Debug.Log("Changing the color");
            FindObjectOfType<MeshRenderer>().material.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
        }
        
        [ContextMenu("Run Crawler VR")]
        public void runCrawlerVRMenu()
        {
            Debug.Log("Trying to run the Code - menu");
            FindObjectOfType<MazeCrawlerExample>().RunCrawler(); 
            Debug.Log("Ran the code? - menu");
        }
    }

