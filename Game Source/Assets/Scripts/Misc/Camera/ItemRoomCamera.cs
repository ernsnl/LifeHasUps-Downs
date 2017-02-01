using UnityEngine;

namespace Assets.Scripts.Misc.Camera
{
    public class ItemRoomCamera : MonoBehaviour
    {
        private GameObject _itemRoomTile;
        private GameObject _camera;
        private GameObject _returnWorldDoor;
        private GameObject _itemPedestal;
        private GameObject _player;

        public float ItemPedestalOffset;

        public void Awake()
        {
            _itemRoomTile = Resources.Load<GameObject>("Prefabs/Tiles/ItemRoom/ItemRoomTile");
            _camera = GameObject.FindGameObjectWithTag("MainCamera");
            _returnWorldDoor = Resources.Load<GameObject>("Prefabs/Misc/ReturnDoor");
            _itemPedestal = Resources.Load<GameObject>("Prefabs/Misc/ItemRoom_Pedestal");

            _player = GameObject.FindGameObjectWithTag("Player");

            var camera_bounds = OrthographicBounds(_camera.GetComponent<UnityEngine.Camera>());
            var item_bounds = _itemRoomTile.gameObject.GetComponent<Renderer>().bounds;
            var lowerLeftCorner = _camera.transform.position - (camera_bounds.size / 2);
            var upperRightCorner = _camera.transform.position + (camera_bounds.size / 2);

            float limitX = camera_bounds.size.x / item_bounds.size.x + 2;
            float limitY = camera_bounds.size.y / item_bounds.size.y + 2;

            for (int i = 0; i < limitX; i++)
            {
                var currentLowerXValue = lowerLeftCorner.x + item_bounds.size.x * i;
                var currentLowerYValue = lowerLeftCorner.y + item_bounds.size.y / 2;
                Instantiate(_itemRoomTile, new Vector3(currentLowerXValue, currentLowerYValue, 0), transform.rotation);
                var currentUpperXValue = upperRightCorner.x - item_bounds.size.x * i;
                var currentUpperYValue = upperRightCorner.y - item_bounds.size.y / 2;
                Instantiate(_itemRoomTile, new Vector3(currentUpperXValue, currentUpperYValue, 0), transform.rotation);

                // Item Padestal Spawn
                if (i == Mathf.FloorToInt(limitX/3))
                {
                    var pedestalbounds = _itemPedestal.gameObject.GetComponent<Renderer>().bounds;
                    Instantiate(_itemPedestal, new Vector3(currentLowerXValue, currentLowerYValue + pedestalbounds.size.x - ItemPedestalOffset, 0), transform.rotation);
                }

                if (i == Mathf.FloorToInt(limitX/2))
                {
                    _player.transform.position =  new Vector3(currentLowerXValue, currentLowerYValue + item_bounds.size.x );
                    _player.transform.rotation = _camera.transform.rotation;
                }

                if (i == Mathf.FloorToInt(2 * limitX / 3))
                {
                    var returnWorldBounds = _returnWorldDoor.gameObject.GetComponent<Renderer>().bounds;
                    Instantiate(_returnWorldDoor, new Vector3(currentLowerXValue, currentLowerYValue + returnWorldBounds.size.x, -1), transform.rotation);
                }

                for (int j = 1; j < 3; j++)
                {
                    Instantiate(_itemRoomTile, new Vector3(currentLowerXValue, currentLowerYValue - (item_bounds.size.y) * j, 0), transform.rotation);
                    Instantiate(_itemRoomTile, new Vector3(currentUpperXValue, currentUpperYValue + (item_bounds.size.y) * j, 0), transform.rotation);
                }

            }
            for (int i = 0; i < limitY; i++)
            {
                var currentLowerXValue = lowerLeftCorner.x - item_bounds.size.x / 2;
                var currentLowerYValue = lowerLeftCorner.y + item_bounds.size.y / 2 + item_bounds.size.y * i;
                Instantiate(_itemRoomTile, new Vector3(currentLowerXValue, currentLowerYValue, 0), transform.rotation);
                var currentUpperXValue = upperRightCorner.x + item_bounds.size.x /2;
                var currentUpperYValue = upperRightCorner.y - item_bounds.size.y / 2 - item_bounds.size.y * i;
                Instantiate(_itemRoomTile, new Vector3(currentUpperXValue, currentUpperYValue, 0),transform.rotation);
                for (int j = 1; j < 3; j++)
                {
                    Instantiate(_itemRoomTile, new Vector3(currentLowerXValue - (item_bounds.size.x) * j, currentLowerYValue, 0), transform.rotation);
                    Instantiate(_itemRoomTile, new Vector3(currentUpperXValue + (item_bounds.size.x) * j, currentUpperYValue, 0), transform.rotation);
                }
            }
        }

        public Bounds OrthographicBounds(UnityEngine.Camera camera)
        {
            float screenAspect = (float)Screen.width / (float)Screen.height;
            float cameraHeight = camera.orthographicSize * 2;
            Bounds bounds = new Bounds(
                camera.transform.position,
                new Vector3(cameraHeight * screenAspect, cameraHeight, 0));
            return bounds;
        }

    }
}