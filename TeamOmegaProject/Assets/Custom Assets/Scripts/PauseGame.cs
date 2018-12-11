using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGame : MonoBehaviour {

    [SerializeField] private bool m_LockCursor = false;

    public Transform canvas;

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (canvas.gameObject.activeInHierarchy == false)
            {
                canvas.gameObject.SetActive(true);
                Time.timeScale = 0;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            } else
            {
                canvas.gameObject.SetActive(false);
                Time.timeScale = 1;
//                Cursor.lockState = m_LockCursor ? CursorLockMode.Locked : CursorLockMode.None;
//                Cursor.visible = !m_LockCursor;
            }
        }
	}
}
