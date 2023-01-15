using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBuilder : MonoBehaviour
{
    #region Fields

    [SerializeField]
    GameObject prefabPaddle;

    [SerializeField]
    GameObject prefabStandardBlock;

    Vector3 paddleSpawnPos;

    float blockWidth;
    float blockHeight;

    int rowBlocksNo = 3;
    int colBlocksNo;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        // Spawning the paddle
        paddleSpawnPos.y = (float)((ScreenUtils.ScreenBottom) + (0.1 * ScreenUtils.ScreenRight));
        paddleSpawnPos.x = 0;

        Instantiate<GameObject>(prefabPaddle, paddleSpawnPos, Quaternion.identity);

        // Creating and destroying a temp block
        GameObject tempBlock = Instantiate(prefabStandardBlock);
        blockWidth = prefabStandardBlock.transform.localScale.x;
        blockHeight = prefabStandardBlock.transform.localScale.y;

        Destroy(tempBlock);

        // Calculating number of columns for the blocks
        colBlocksNo = (int)((ScreenUtils.ScreenRight * 2) / blockWidth);


        // Determining the padding
        float padding = (float)((ScreenUtils.ScreenRight * 2) - (colBlocksNo * blockWidth));
        padding *= 0.5f;


        // Calculating initial spawn of Block
        float blockX = padding + (ScreenUtils.ScreenLeft + (blockWidth / 2));

        float blockY = (float)(ScreenUtils.ScreenTop) - (blockHeight);

        //Vector2 blockSpawnPos = new Vector2(blockX, blockY);

        // For loop to generate rows of blocks
        for (int i = 0; i < rowBlocksNo; i++)
        {
            if (i == 0)
            {
                blockY = blockY + 0;
            } else
            {
                blockY -= blockHeight;
            }
            
            Vector2 blockSpawnPos = new Vector2(blockX, blockY);

            // For loop to generate columns of blocks
            for (int j = 0; j < colBlocksNo; j++)
            {
                if (j == 0)
                {
                    Instantiate(prefabStandardBlock, blockSpawnPos, Quaternion.identity);
                }
                else
                {
                    blockSpawnPos.x += blockWidth;
                    Instantiate(prefabStandardBlock, blockSpawnPos, Quaternion.identity);
                }
            }
        }


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
