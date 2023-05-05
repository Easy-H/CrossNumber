using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public bool _pause = false;

    private void Oncreate() {
        _pause = false;

    }

}
