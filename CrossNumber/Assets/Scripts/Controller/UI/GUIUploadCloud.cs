using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIUploadCloud : MonoBehaviour
{
    public void Upload() {
        FirebaseManager.Instance.WriteFireStore();
    }
}
