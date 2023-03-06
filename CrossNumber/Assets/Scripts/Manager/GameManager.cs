using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    public bool _pause = false;

    private void Start() {
        _pause = false;

    }

}
