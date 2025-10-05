using UnityEngine;

namespace QbGameLib_Utils.Extension
{
    public static class CameraExtension
    {
        public static Bounds OrthographicBounds( Camera camera)
        {
            float cameraHeight = camera.orthographicSize * 2;
            Bounds bounds = new Bounds(
                camera.transform.position,
                new Vector3(cameraHeight * camera.aspect, cameraHeight, 0));
            return bounds;
        }
    }
}