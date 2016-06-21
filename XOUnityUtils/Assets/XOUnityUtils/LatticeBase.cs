using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace XOUnityUtils
{
    public enum LatticeShape
    {
        Square,
        HexPoint,
        HexFlat
    }

    public class LatticeTileBase
    {
        protected internal List<LatticeTileBase> m_Neighbors = new List<LatticeTileBase>();
        protected internal Vector3 m_Position;

        public Vector3 GetDirToNeighbor(LatticeTileBase other)
        {
            return m_Position - other.m_Position;
        }
    }

    public class LatticeBase<LatticeTile> : MonoBehaviour where LatticeTile : LatticeTileBase
    {
        public LatticeShape m_LatticeShape;
        public Sprite m_Sprite;
        public int m_Width;
        public int m_Height;

        void Awake()
        {
            switch (m_LatticeShape)
            {
                case LatticeShape.Square:
                    SetupSquareLattice();
                    break;
                default:
                    Debug.LogWarning("Lattice shape not implemented.");
                    break;
            }
        }

        void SetupSquareLattice()
        {
            var spriteBounds = m_Sprite.bounds;
            var halfWidth = m_Width / 2;
            var halfHeight = m_Height / 2;
            for (int x = -halfWidth; x < m_Width-halfWidth; ++x)
            {
                for (int y = -halfHeight; y < m_Height-halfHeight; ++y)
                {
                    var tileGameObject = new GameObject("Tile Sprite (" + x + "," + y + ")");
                    var tileTransform = tileGameObject.transform;
                    var tileSpriteRenderer = tileGameObject.AddComponent<SpriteRenderer>();

                    tileSpriteRenderer.sprite = m_Sprite;

                    tileTransform.SetParent(transform);
                    tileTransform.localPosition = new Vector3((x * spriteBounds.size.x), (y * spriteBounds.size.y), 0);
                }
            }
        }
    }

}