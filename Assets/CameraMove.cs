using UnityEngine;

public class CameraMove : MonoBehaviour {

	// Update is called once per frame
	private void Update () {
        if (Input.GetKey(KeyCode.W))
        {
            this.transform.Translate(Vector3.up * Camera.main.orthographicSize * 2 * Time.unscaledDeltaTime);
        }
        if (Input.GetKey(KeyCode.A))
        {
            this.transform.Translate(Vector3.left * Camera.main.orthographicSize * 2 * Time.unscaledDeltaTime);
        }
        if (Input.GetKey(KeyCode.S))
        {
            this.transform.Translate(Vector3.down * Camera.main.orthographicSize * 2 * Time.unscaledDeltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            this.transform.Translate(Vector3.right * Camera.main.orthographicSize * 2 * Time.unscaledDeltaTime);
        }

        float zoom = Camera.main.orthographicSize;
        zoom += Input.mouseScrollDelta.y * -100f * Time.unscaledDeltaTime;
        if (zoom < 5)
            zoom = 5;
        if (zoom > 50)
            zoom = 50;
        Camera.main.orthographicSize = zoom;
    }
}
