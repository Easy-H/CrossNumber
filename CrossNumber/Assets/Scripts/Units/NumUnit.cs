using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumUnit : Unit {
    public void Overed() {
        gameObject.GetComponent<Collider2D>().enabled = false;
    }

    public void BreakOvered() {
        gameObject.GetComponent<Collider2D>().enabled = true;
    }
}
