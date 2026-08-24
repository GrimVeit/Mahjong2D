using System;
using System.Collections.Generic;
using UnityEngine;

public class MahjongBoardGenerator : MonoBehaviour
{
    [Header("Layouts")]

    [SerializeField]
    private List<MahjongLayoutSettings> layouts =
        new List<MahjongLayoutSettings>();


    // =========================================================
    // GENERATE FROM SPRITE COUNT
    // =========================================================

    public List<MahjongTilePosition> Generate(
        int uniqueSpriteCount)
    {
        MahjongLayoutSettings settings =
            GetSettings(
                uniqueSpriteCount * 2
            );


        if (settings == null)
            return new List<MahjongTilePosition>();


        return Generate(
            settings.FirstLayerCount,
            settings.SecondLayerCount,
            settings.ThirdLayerCount
        );
    }


    // =========================================================
    // GENERATION FOR MIX
    // =========================================================

    public List<MahjongTilePosition> Generate(
        int firstLayerCount,
        int secondLayerCount,
        int thirdLayerCount)
    {
        firstLayerCount = Mathf.Clamp(firstLayerCount, 0, 20);

        secondLayerCount = Mathf.Clamp(secondLayerCount, 0, 20);

        thirdLayerCount = Mathf.Clamp(thirdLayerCount, 0, 15);

        const int maxAttempts = 100;


        for (int attempt = 0; attempt < maxAttempts; attempt++)
        {
            List<MahjongTilePosition> result =
                TryGenerate(
                    firstLayerCount,
                    secondLayerCount,
                    thirdLayerCount
                );


            if (result != null)
                return result;
        }


        Debug.LogWarning(
            $"Could not generate Mahjong layout: " +
            $"{firstLayerCount}/{secondLayerCount}/{thirdLayerCount}"
        );


        return GenerateFallback(
            firstLayerCount,
            secondLayerCount,
            thirdLayerCount
        );
    }


    // =========================================================
    // SETTINGS
    // =========================================================

    private MahjongLayoutSettings GetSettings(int uniqueSpriteCount)
    {
        foreach (MahjongLayoutSettings settings in layouts)
        {
            if (settings.SpriteCount == uniqueSpriteCount)
            {
                return settings;
            }
        }

        return null;
    }


    // =========================================================
    // TRY GENERATE
    // =========================================================

    private List<MahjongTilePosition> TryGenerate(
        int firstLayerCount,
        int secondLayerCount,
        int thirdLayerCount)
    {
        List<MahjongTilePosition> positions =
            new List<MahjongTilePosition>();


        // =====================================================
        // FIRST LAYER
        // =====================================================

        List<Vector2Int> firstCandidates =
            GetAllCells(
                4,
                5
            );


        Shuffle(firstCandidates);


        for (
            int i = 0;
            i < firstLayerCount;
            i++)
        {
            Vector2Int cell =
                firstCandidates[i];


            positions.Add(
                new MahjongTilePosition(
                    0,
                    cell.x,
                    cell.y
                )
            );
        }


        // =====================================================
        // SECOND LAYER
        // =====================================================

        List<Vector2Int> secondCandidates =
            new List<Vector2Int>();


        for (int y = 0; y < 5; y++)
        {
            for (int x = 0; x < 4; x++)
            {
                if (
                    HasFirstLayerSupport(
                        positions,
                        x,
                        y))
                {
                    secondCandidates.Add(
                        new Vector2Int(
                            x,
                            y
                        )
                    );
                }
            }
        }


        if (
            secondCandidates.Count <
            secondLayerCount)
        {
            return null;
        }


        Shuffle(secondCandidates);


        for (
            int i = 0;
            i < secondLayerCount;
            i++)
        {
            Vector2Int cell =
                secondCandidates[i];


            positions.Add(
                new MahjongTilePosition(
                    1,
                    cell.x,
                    cell.y
                )
            );
        }


        // =====================================================
        // THIRD LAYER
        // =====================================================

        List<Vector2Int> thirdCandidates =
            new List<Vector2Int>();


        for (int y = 0; y < 5; y++)
        {
            for (int x = 0; x < 3; x++)
            {
                if (
                    HasSecondLayerSupport(
                        positions,
                        x,
                        y))
                {
                    thirdCandidates.Add(
                        new Vector2Int(
                            x,
                            y
                        )
                    );
                }
            }
        }


        if (
            thirdCandidates.Count <
            thirdLayerCount)
        {
            return null;
        }


        Shuffle(thirdCandidates);


        for (
            int i = 0;
            i < thirdLayerCount;
            i++)
        {
            Vector2Int cell =
                thirdCandidates[i];


            positions.Add(
                new MahjongTilePosition(
                    2,
                    cell.x,
                    cell.y
                )
            );
        }


        return positions;
    }


    // =========================================================
    // SUPPORT
    // =========================================================

    private bool HasFirstLayerSupport(
        List<MahjongTilePosition> positions,
        int x,
        int y)
    {
        foreach (
            MahjongTilePosition position
            in positions)
        {
            if (position.Layer != 0)
                continue;


            if (
                position.GridX == x &&
                position.GridY == y)
            {
                return true;
            }
        }


        return false;
    }


    private bool HasSecondLayerSupport(
        List<MahjongTilePosition> positions,
        int x,
        int y)
    {
        bool leftSupport =
            HasPosition(
                positions,
                1,
                x,
                y
            );


        bool rightSupport =
            HasPosition(
                positions,
                1,
                x + 1,
                y
            );


        return
            leftSupport &&
            rightSupport;
    }


    private bool HasPosition(
        List<MahjongTilePosition> positions,
        int layer,
        int x,
        int y)
    {
        foreach (
            MahjongTilePosition position
            in positions)
        {
            if (
                position.Layer == layer &&
                position.GridX == x &&
                position.GridY == y)
            {
                return true;
            }
        }


        return false;
    }


    // =========================================================
    // FALLBACK
    // =========================================================

    private List<MahjongTilePosition> GenerateFallback(
        int firstLayerCount,
        int secondLayerCount,
        int thirdLayerCount)
    {
        List<MahjongTilePosition> positions =
            new List<MahjongTilePosition>();


        // FIRST

        int firstAdded = 0;


        for (
            int y = 0;
            y < 5 &&
            firstAdded < firstLayerCount;
            y++)
        {
            for (
                int x = 0;
                x < 4 &&
                firstAdded < firstLayerCount;
                x++)
            {
                positions.Add(
                    new MahjongTilePosition(
                        0,
                        x,
                        y
                    )
                );


                firstAdded++;
            }
        }


        // SECOND

        int secondAdded = 0;


        for (
            int y = 0;
            y < 5 &&
            secondAdded < secondLayerCount;
            y++)
        {
            for (
                int x = 0;
                x < 4 &&
                secondAdded < secondLayerCount;
                x++)
            {
                if (
                    !HasFirstLayerSupport(
                        positions,
                        x,
                        y))
                {
                    continue;
                }


                positions.Add(
                    new MahjongTilePosition(
                        1,
                        x,
                        y
                    )
                );


                secondAdded++;
            }
        }


        // THIRD

        int thirdAdded = 0;


        for (
            int y = 0;
            y < 5 &&
            thirdAdded < thirdLayerCount;
            y++)
        {
            for (
                int x = 0;
                x < 3 &&
                thirdAdded < thirdLayerCount;
                x++)
            {
                if (
                    !HasSecondLayerSupport(
                        positions,
                        x,
                        y))
                {
                    continue;
                }


                positions.Add(
                    new MahjongTilePosition(
                        2,
                        x,
                        y
                    )
                );


                thirdAdded++;
            }
        }


        return positions;
    }


    // =========================================================
    // RANDOM
    // =========================================================

    private List<Vector2Int> GetAllCells(
        int width,
        int height)
    {
        List<Vector2Int> cells =
            new List<Vector2Int>();


        for (
            int y = 0;
            y < height;
            y++)
        {
            for (
                int x = 0;
                x < width;
                x++)
            {
                cells.Add(
                    new Vector2Int(
                        x,
                        y
                    )
                );
            }
        }


        return cells;
    }


    private void Shuffle<T>(
        List<T> list)
    {
        for (
            int i = list.Count - 1;
            i > 0;
            i--)
        {
            int randomIndex =
                UnityEngine.Random.Range(
                    0,
                    i + 1
                );


            T temp = list[i];


            list[i] =
                list[randomIndex];


            list[randomIndex] =
                temp;
        }
    }


    // =========================================================
    // SETTINGS
    // =========================================================

    [Serializable]
    private class MahjongLayoutSettings
    {
        [Header("Unique Sprites")]

        [Min(1)]
        [SerializeField]
        private int spriteCount = 4;


        [Header("Layer 1")]

        [Min(0)]
        [SerializeField]
        private int firstLayerCount;


        [Header("Layer 2")]

        [Min(0)]
        [SerializeField]
        private int secondLayerCount;


        [Header("Layer 3")]

        [Min(0)]
        [SerializeField]
        private int thirdLayerCount;


        public int SpriteCount =>
            spriteCount;


        public int FirstLayerCount =>
            firstLayerCount;


        public int SecondLayerCount =>
            secondLayerCount;


        public int ThirdLayerCount =>
            thirdLayerCount;
    }
}