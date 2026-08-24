using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MahjongTile : MonoBehaviour, IPointerClickHandler
{
    [Header("Visual")]
    [SerializeField] private Image tileImage;

    [Header("Colors")]
    [SerializeField] private Color activeColor = Color.white;
    [SerializeField] private Color inactiveColor = Color.gray;

    [Header("Alpha")]
    [SerializeField, Range(0f, 1f)]
    private float activeAlpha = 1f;

    [SerializeField, Range(0f, 1f)]
    private float inactiveAlpha = 0.7f;


    /*
     * Board, которому принадлежит этот тайл.
     */

    private MahjongBoard board;


    /*
     * Текущий статус тайла.
     */

    private bool isActive;


    // =========================================================
    // INITIALIZATION
    // =========================================================

    public void Initialize(MahjongBoard board)
    {
        this.board = board;
    }


    // =========================================================
    // POINTER CLICK
    // =========================================================

    public void OnPointerClick(
        PointerEventData eventData)
    {
        /*
         * Если тайл заблокирован —
         * ничего не делаем.
         */

        if (!isActive)
            return;


        /*
         * Передаём обработку Board.
         */

        if (board != null)
        {
            board.OnTileClicked(this);
        }
    }


    // =========================================================
    // ACTIVE STATE
    // =========================================================

    public void SetActiveVisual(bool active)
    {
        isActive = active;


        if (tileImage == null)
            return;


        Color color;


        if (active)
        {
            color = activeColor;
            color.a = activeAlpha;
        }
        else
        {
            color = inactiveColor;
            color.a = inactiveAlpha;
        }


        tileImage.color = color;
    }


    // =========================================================
    // GET STATE
    // =========================================================

    public bool IsActive()
    {
        return isActive;
    }
}
