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
        firstLayerCount =
            Mathf.Clamp(
                firstLayerCount,
                0,
                20
            );

        secondLayerCount =
            Mathf.Clamp(
                secondLayerCount,
                0,
                20
            );

        thirdLayerCount =
            Mathf.Clamp(
                thirdLayerCount,
                0,
                15
            );


        int totalCount =
            firstLayerCount +
            secondLayerCount +
            thirdLayerCount;


        const int maxAttempts = 100;


        // =====================================================
        // NORMAL GENERATION
        // =====================================================

        for (
            int attempt = 0;
            attempt < maxAttempts;
            attempt++)
        {
            List<MahjongTilePosition> result =
                TryGenerate(
                    firstLayerCount,
                    secondLayerCount,
                    thirdLayerCount
                );


            if (
                result != null &&
                result.Count == totalCount)
            {
                return result;
            }
        }


        Debug.LogWarning(
            $"Could not normally generate Mahjong layout: " +
            $"{firstLayerCount}/" +
            $"{secondLayerCount}/" +
            $"{thirdLayerCount}. " +
            $"Using greedy fallback."
        );


        // =====================================================
        // GREEDY FALLBACK
        // =====================================================

        List<MahjongTilePosition> fallback =
            GenerateFallback(
                totalCount
            );


        if (
            fallback.Count == totalCount)
        {
            return fallback;
        }


        // =====================================================
        // HARD FALLBACK
        // =====================================================

        Debug.LogWarning(
            $"Greedy Mahjong fallback failed: " +
            $"{fallback.Count}/{totalCount}. " +
            $"Using hard fallback."
        );


        return GenerateHardFallback(
            totalCount
        );
    }


    // =========================================================
    // SETTINGS
    // =========================================================

    private MahjongLayoutSettings GetSettings(
        int uniqueSpriteCount)
    {
        foreach (
            MahjongLayoutSettings settings
            in layouts)
        {
            if (
                settings.SpriteCount ==
                uniqueSpriteCount)
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


        Shuffle(
            firstCandidates
        );


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


        Shuffle(
            secondCandidates
        );


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


        Shuffle(
            thirdCandidates
        );


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
    // GREEDY FALLBACK
    // =========================================================

    private List<MahjongTilePosition> GenerateFallback(
        int totalCount)
    {
        List<MahjongTilePosition> positions =
            new List<MahjongTilePosition>();


        // =====================================================
        // ADD ONE TILE AT A TIME
        // =====================================================

        for (
            int i = 0;
            i < totalCount;
            i++)
        {
            bool added = false;


            // =================================================
            // TRY THIRD LAYER
            // =================================================

            List<Vector2Int> thirdCandidates =
                GetThirdLayerFallbackCandidates(
                    positions
                );


            Shuffle(
                thirdCandidates
            );


            if (thirdCandidates.Count > 0)
            {
                Vector2Int cell =
                    thirdCandidates[0];


                positions.Add(
                    new MahjongTilePosition(
                        2,
                        cell.x,
                        cell.y
                    )
                );


                added = true;
            }


            // =================================================
            // TRY SECOND LAYER
            // =================================================

            if (!added)
            {
                List<Vector2Int> secondCandidates =
                    GetSecondLayerFallbackCandidates(
                        positions
                    );


                Shuffle(
                    secondCandidates
                );


                if (secondCandidates.Count > 0)
                {
                    Vector2Int cell =
                        secondCandidates[0];


                    positions.Add(
                        new MahjongTilePosition(
                            1,
                            cell.x,
                            cell.y
                        )
                    );


                    added = true;
                }
            }


            // =================================================
            // TRY FIRST LAYER
            // =================================================

            if (!added)
            {
                List<Vector2Int> firstCandidates =
                    GetFirstLayerFallbackCandidates(
                        positions
                    );


                Shuffle(
                    firstCandidates
                );


                if (firstCandidates.Count > 0)
                {
                    Vector2Int cell =
                        firstCandidates[0];


                    positions.Add(
                        new MahjongTilePosition(
                            0,
                            cell.x,
                            cell.y
                        )
                    );


                    added = true;
                }
            }


            // =================================================
            // NOTHING AVAILABLE
            // =================================================

            if (!added)
            {
                break;
            }
        }


        return positions;
    }


    // =========================================================
    // GREEDY FALLBACK CANDIDATES
    // =========================================================

    private List<Vector2Int>
        GetThirdLayerFallbackCandidates(
            List<MahjongTilePosition> positions)
    {
        List<Vector2Int> candidates =
            new List<Vector2Int>();


        for (int y = 0; y < 5; y++)
        {
            for (int x = 0; x < 3; x++)
            {
                // Third layer requires
                // two supporting tiles on layer 1.

                if (
                    !HasSecondLayerSupport(
                        positions,
                        x,
                        y))
                {
                    continue;
                }


                // Position must be free.

                if (
                    HasPosition(
                        positions,
                        2,
                        x,
                        y))
                {
                    continue;
                }


                candidates.Add(
                    new Vector2Int(
                        x,
                        y
                    )
                );
            }
        }


        return candidates;
    }


    private List<Vector2Int>
        GetSecondLayerFallbackCandidates(
            List<MahjongTilePosition> positions)
    {
        List<Vector2Int> candidates =
            new List<Vector2Int>();


        for (int y = 0; y < 5; y++)
        {
            for (int x = 0; x < 4; x++)
            {
                // Second layer requires
                // a tile directly below it.

                if (
                    !HasFirstLayerSupport(
                        positions,
                        x,
                        y))
                {
                    continue;
                }


                // Position must be free.

                if (
                    HasPosition(
                        positions,
                        1,
                        x,
                        y))
                {
                    continue;
                }


                candidates.Add(
                    new Vector2Int(
                        x,
                        y
                    )
                );
            }
        }


        return candidates;
    }


    private List<Vector2Int>
        GetFirstLayerFallbackCandidates(
            List<MahjongTilePosition> positions)
    {
        List<Vector2Int> candidates =
            new List<Vector2Int>();


        for (int y = 0; y < 5; y++)
        {
            for (int x = 0; x < 4; x++)
            {
                // First layer can be placed anywhere.

                if (
                    HasPosition(
                        positions,
                        0,
                        x,
                        y))
                {
                    continue;
                }


                candidates.Add(
                    new Vector2Int(
                        x,
                        y
                    )
                );
            }
        }


        return candidates;
    }


    // =========================================================
    // HARD FALLBACK
    // =========================================================

    private List<MahjongTilePosition>
        GenerateHardFallback(
            int totalCount)
    {
        List<MahjongTilePosition> positions =
            new List<MahjongTilePosition>();


        List<MahjongTilePosition> allPositions =
            new List<MahjongTilePosition>();


        // =====================================================
        // LAYER 0
        // =====================================================

        for (int y = 0; y < 5; y++)
        {
            for (int x = 0; x < 4; x++)
            {
                allPositions.Add(
                    new MahjongTilePosition(
                        0,
                        x,
                        y
                    )
                );
            }
        }


        // =====================================================
        // LAYER 1
        // =====================================================

        for (int y = 0; y < 5; y++)
        {
            for (int x = 0; x < 4; x++)
            {
                allPositions.Add(
                    new MahjongTilePosition(
                        1,
                        x,
                        y
                    )
                );
            }
        }


        // =====================================================
        // LAYER 2
        // =====================================================

        for (int y = 0; y < 5; y++)
        {
            for (int x = 0; x < 3; x++)
            {
                allPositions.Add(
                    new MahjongTilePosition(
                        2,
                        x,
                        y
                    )
                );
            }
        }


        Shuffle(
            allPositions
        );


        int amount =
            Mathf.Min(
                totalCount,
                allPositions.Count
            );


        for (
            int i = 0;
            i < amount;
            i++)
        {
            positions.Add(
                allPositions[i]
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
    // RANDOM
    // =========================================================

    private List<Vector2Int> GetAllCells(
        int width,
        int height)
    {
        List<Vector2Int> cells =
            new List<Vector2Int>();


        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
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


            T temp =
                list[i];


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