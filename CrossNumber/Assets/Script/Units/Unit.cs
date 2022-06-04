using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    protected GameManager gm;

    public string value = "1";

    bool calced = false;

    protected virtual void Awake()
    {
        gm = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
        gm.unCalced += 1;
        Place();
    }

    public virtual void Pick() {
        gameObject.layer = 2;
    }

    public void Hold(Vector3 pos) {

        Vector3 resultPos = CollideCheck(new Vector3(Mathf.Round(pos.x), Mathf.Round(pos.y), 0));
        transform.position = resultPos;

        if (calced) {
            gm.unCalced++;
            calced = false;
        }

    }

    public virtual Vector3 Place() {
        gameObject.layer = 0;
        return transform.position;
    }

    protected virtual Vector3 CollideCheck(Vector3 pos)
    {
        if (ObjectCheck(pos)) {
            pos = CollideCheck(pos - Vector3.up);
        }

        return pos;
    }

    protected RaycastHit2D ObjectCheck(Vector3 pos) {
        int layerMask = ~((1 << 2) | (1 << LayerMask.NameToLayer("Char")));
        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.down, 0.1f, layerMask);

        return hit;
    }

    protected RaycastHit2D ObjectCheck(Vector3 pos, int layerNum) {
        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.down, 0.1f, (1 << layerNum));

        return hit;
    }


    protected RaycastHit2D ObjectCheck(Vector3 pos, string layer) {
        int layerMask = ~(1 << LayerMask.NameToLayer(layer));
        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.down, 0.1f, layerMask);

        return hit;
    }

    public void Calced() {
        if (!calced) {
            gm.unCalced--;
            calced = true;
        }
    }

}
