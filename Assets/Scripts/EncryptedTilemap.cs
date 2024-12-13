using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EncryptedTilemap : MonoBehaviour
{
    public string color;
    private Tilemap myTilemap;
    private CompositeCollider2D myCompositeCollider;
    public TileBase nonSolidTile;
    public TileBase solidTile;

    private void Awake()
    {
        myTilemap = GetComponent<Tilemap>();
        myCompositeCollider = GetComponent<CompositeCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private bool ColorMatches(Player.EncState encState)
    {
        if (color.Equals("Purple"))
        {
            return encState == Player.EncState.PURPLE;
        }
        else if (color.Equals("Green"))
        {
            return encState == Player.EncState.GREEN;
        }
        Debug.LogError("Color does not match any state");
        return false;
    }

    public void toggleEncryption(Player.EncState encState)
    {
        if (ColorMatches(encState))
        {
            myCompositeCollider.isTrigger = !myCompositeCollider.isTrigger;
            for (int x = (int)myTilemap.localBounds.min.x; x <= myTilemap.localBounds.max.x; x++)
            {
                for (int y = (int)myTilemap.localBounds.min.y; y <= myTilemap.localBounds.max.y; y++)
                {
                    for (int z = (int)myTilemap.localBounds.min.z; z <= myTilemap.localBounds.max.z; z++)
                    {
                        TileBase tile = myTilemap.GetTile(new Vector3Int(x, y, z));
                        if (tile != null && tile.Equals(nonSolidTile))
                        {
                            myTilemap.SetTile(new Vector3Int(x, y, z), solidTile);
                        }
                        else if (tile != null && tile.Equals(solidTile))
                        {
                            myTilemap.SetTile(new Vector3Int(x, y, z), nonSolidTile);
                        }
                    }
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            player.setCanEncrypt(false);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            player.setCanEncrypt(true);
        }
    }
}
