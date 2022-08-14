using System.Linq;
using UnityEngine;

namespace BombermanGame
{
    public class CameraController : MonoBehaviour
    {
        public Field field;

        private void Start()
        {
            var listOfStones = GameObject.FindGameObjectsWithTag("Stone").ToArray();
            field = new Field
            {
                MinX = listOfStones.Min(x => x.transform.position.x) - 0.5f,
                MinY = listOfStones.Min(x => x.transform.position.y) - 0.5f,
                MaxX = listOfStones.Max(x => x.transform.position.x) + 0.5f,
                MaxY = listOfStones.Max(x => x.transform.position.y) + 0.5f
            };
        }

        private void Update()
        {
            var bomberman = FindObjectOfType<Bomberman>();
            if (bomberman == null)
                return;
            
            var cameraHalfHeight = GetComponent<Camera>().orthographicSize;
            var cameraHalfWidth = cameraHalfHeight * ((float)Screen.width / Screen.height);

            var bombermanPosition = bomberman.transform.position;
            var x = bombermanPosition.x;
            var y = bombermanPosition.y;

            x = Mathf.Clamp(x, field.MinX + cameraHalfWidth, field.MaxX - cameraHalfWidth);
            y = Mathf.Clamp(y, field.MinY + cameraHalfHeight, field.MaxY - cameraHalfHeight);
            transform.position = new Vector3(x, y, transform.position.z);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(new Vector2(field.MinX, field.MinY), new Vector2(field.MaxX, field.MinY));
            Gizmos.DrawLine(new Vector2(field.MinX, field.MaxY), new Vector2(field.MaxX, field.MaxY));
            Gizmos.DrawLine(new Vector2(field.MinX, field.MinY), new Vector2(field.MinX, field.MaxY));
            Gizmos.DrawLine(new Vector2(field.MaxX, field.MinY), new Vector2(field.MaxX, field.MaxY));
        }

        public struct Field
        {
            public float MinX;
            public float MinY;
            public float MaxX;
            public float MaxY;
        }
    }
}