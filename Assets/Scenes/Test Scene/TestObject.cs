using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestObject : MonoBehaviour
{
    public void ReadyAction(Action testFunction) {
        testFunction = TestFunction;
        TestFunction();
    }

    void TestFunction() {
        Debug.Log("TEST");
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
