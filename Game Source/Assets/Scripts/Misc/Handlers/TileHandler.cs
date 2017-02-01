using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Misc.CustomFunctions;
using UnityEngine;

namespace Assets.Scripts.Misc.Handlers
{
    public class TileHandler : MonoBehaviour
    {
        private GameObject _tile;
        private TileGateModel _tileGateModel;
        private int _tileY;
        private int _tileX;


        public GameObject Tile { get { return _tile; } }
        public TileGateModel TileGateModel { get { return _tileGateModel; } }
        public int PositionX { get { return _tileX; } }
        public int PositionY { get { return _tileY; } }
        public int NumberOfPossibleNeighBors
        {
            get
            {
                return this.TileGateModel.eastGate +
                       this.TileGateModel.westGate +
                       this.TileGateModel.northGate +
                       this.TileGateModel.southGate;
            }
        }
        public bool IsPlaced;
        public bool Explored;

        private List<TileHandler> _tiles;

        public List<TileHandler> GetTiles()
        {
            if (_tiles == null)
            {

                string[] listOfTile =
                {
                    "Tile_2_EW", "Tile_2_NS",
                    "Tile_3_E", "Tile_3_N", "Tile_3_W", "Tile_3_S",
                    "Tile_4",
                    "Tile_L_NE", "Tile_L_NW", "Tile_L_SE", "Tile_L_SW",
                    "Tile_U_N", "Tile_U_E", "Tile_U_S", "Tile_U_W"
                };
                List<TileHandler> result = new List<TileHandler>();

                foreach (var tile in listOfTile)
                {
                    try
                    {

                        var gameobj = (GameObject)(Resources.Load("Prefabs/Tiles/" + tile));
                        var gateModel = gameobj.GetComponent<TileGateModel>();
                        gateModel.isCertainSouth = false;
                        gateModel.isCertainEast = false;
                        gateModel.isCertainNorth = false;
                        gateModel.isCertainWest = false;

                        TileHandler tileProp = new TileHandler(gateModel);

                        tileProp._tile = gameobj;
                        result.Add(tileProp);
                    }
                    catch (Exception)
                    {
                    }
                }
                _tiles = result;
            }

            return _tiles;
        }

        public void CloseWorld()
        {
            this.TileGateModel.northGate = this.TileGateModel.isCertainNorth ? 1 : 0;
            this.TileGateModel.eastGate = this.TileGateModel.isCertainEast ? 1 : 0;
            this.TileGateModel.westGate = this.TileGateModel.isCertainWest ? 1 : 0;
            this.TileGateModel.southGate = this.TileGateModel.isCertainSouth ? 1 : 0;

            var filter = GetTiles();
            filter = filter.FindAll(op => op.TileGateModel.northGate == TileGateModel.northGate);
            filter = filter.FindAll(op => op.TileGateModel.southGate == TileGateModel.southGate);
            filter = filter.FindAll(op => op.TileGateModel.westGate == TileGateModel.westGate);
            filter = filter.FindAll(op => op.TileGateModel.eastGate == TileGateModel.eastGate);

            filter.Shuffle();
            this._tile = filter[0]._tile;
            this.IsPlaced = true;
            this.Explored = false;
        }

        public TileHandler SelectTile()
        {
            var filteredResult = GetTiles();
            if (this.TileGateModel.isCertainSouth)
                filteredResult = filteredResult.FindAll(op => op.TileGateModel.southGate == 1);
            if (this.TileGateModel.isCertainNorth)
                filteredResult = filteredResult.FindAll(op => op.TileGateModel.northGate == 1);
            if (this.TileGateModel.isCertainWest)
                filteredResult = filteredResult.FindAll(op => op.TileGateModel.westGate == 1);
            if (this.TileGateModel.isCertainEast)
                filteredResult = filteredResult.FindAll(op => op.TileGateModel.eastGate == 1);

            filteredResult = filteredResult.FindAll(op => TileGateModel.eastGate >= op.TileGateModel.eastGate);
            filteredResult = filteredResult.FindAll(op => TileGateModel.westGate >= op.TileGateModel.westGate);
            filteredResult = filteredResult.FindAll(op => TileGateModel.northGate >= op.TileGateModel.northGate);
            filteredResult = filteredResult.FindAll(op => TileGateModel.southGate >= op.TileGateModel.southGate);

            filteredResult.Shuffle();
            return filteredResult[0];
        }

        public void PlaceTile(TileHandler tile)
        {

            if (_tile == null)
            {
                var temp = this.TileGateModel;
                this._tileGateModel = tile._tileGateModel;
                this._tileGateModel.isCertainSouth = temp.isCertainSouth;
                this._tileGateModel.isCertainEast = temp.isCertainEast;
                this._tileGateModel.isCertainNorth = temp.isCertainNorth;
                this._tileGateModel.isCertainWest = temp.isCertainWest;

                this._tile = tile._tile;
                IsPlaced = true;
                Explored = false;
            }

        }

        public void SetPosition(int x, int y)
        {
            this._tileX = x;
            this._tileY = y;
        }

        public TileHandler() : this(1, 1, 1, 1, true) { }

        public TileHandler(TileGateModel gate)
        {
            this._tileGateModel = gate;
            this.IsPlaced = false;
        }

        public TileHandler(int E, int W, int S, int N, bool initialSet)
        {
            TileGateModel _gate = new TileGateModel(N, S, E, W, initialSet);
            this._tileGateModel = _gate;
            if (!initialSet)
            {
                var possibleMatches = _tiles.ToList().FindAll(op => op._tileGateModel.northGate == _tileGateModel.northGate
                                               && op._tileGateModel.eastGate == _tileGateModel.eastGate
                                               && op._tileGateModel.southGate == _tileGateModel.southGate
                                               && op._tileGateModel.westGate == _tileGateModel.westGate).ToList();
                possibleMatches.Shuffle();

                this._tile = possibleMatches[0]._tile;
                IsPlaced = true;

            }
        }

    }
}