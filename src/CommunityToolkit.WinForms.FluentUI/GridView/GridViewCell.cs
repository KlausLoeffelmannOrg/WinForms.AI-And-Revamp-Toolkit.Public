using System.ComponentModel;

namespace CommunityToolkit.WinForms.FluentUI;

internal class GridViewColumn : DataGridViewColumn
{
    public GridViewColumn() : base(new GridViewCell())
    {
    }

    public override int GetPreferredWidth(DataGridViewAutoSizeColumnMode autoSizeColumnMode, bool fixedHeight)
        => base.GetPreferredWidth(autoSizeColumnMode, fixedHeight);

}
internal class GridViewCell : DataGridViewCell
{
    private static readonly Padding s_defaultPadding = new(5, 5, 5, 0);

    private Func<GridViewItemTemplate?>? _itemTemplateGetter;
    private bool _isMouseOver;

    public GridViewCell()
    {
    }

    protected override void OnMouseEnter(int rowIndex)
    {
        base.OnMouseEnter(rowIndex);

        _isMouseOver = true;
        DataGridView?.InvalidateCell(this);
    }

    override protected void OnMouseLeave(int rowIndex)
    {
        base.OnMouseLeave(rowIndex);

        _isMouseOver = false;
        DataGridView?.InvalidateCell(this);
    }

    internal Func<GridViewItemTemplate?>? ItemTemplateGetter
    {
        get => _itemTemplateGetter;

        set
        {
            if (_itemTemplateGetter == value)
            {
                return;
            }

            _itemTemplateGetter = value;
        }
    }

    public override object Clone()
    {
        GridViewCell clone = (GridViewCell)base.Clone();
        clone.ItemTemplateGetter = ItemTemplateGetter;
        return clone;
    }

    protected override object? GetFormattedValue(
        object? value,
        int rowIndex,
        ref DataGridViewCellStyle cellStyle,
        TypeConverter? valueTypeConverter,
        TypeConverter? formattedValueTypeConverter,
        DataGridViewDataErrorContexts context)
    {
        return $"{(value is null ? "- - -" : value)}";
    }

    protected override Size GetPreferredSize(
        Graphics graphics,
        DataGridViewCellStyle cellStyle,
        int rowIndex,
        Size constraintSize)
    {
        if (ItemTemplate is not GridViewItemTemplate itemTemplate)
        {
            return base.GetPreferredSize(graphics, cellStyle, rowIndex, constraintSize);
        }

        if (rowIndex < 0)
        {
            return itemTemplate.GetPreferredSize(constraintSize, null, rowIndex);
        }
        else
        {
            return itemTemplate.GetPreferredSize(constraintSize, GetValue(rowIndex), rowIndex);
        }
    }

    private GridViewItemTemplate? ItemTemplate => ItemTemplateGetter?.Invoke();

    protected override void Paint(
        Graphics graphics,
        Rectangle clipBounds,
        Rectangle cellBounds,
        int rowIndex,
        DataGridViewElementStates cellState,
        object? value,
        object? formattedValue,
        string? errorText,
        DataGridViewCellStyle cellStyle,
        DataGridViewAdvancedBorderStyle advancedBorderStyle,
        DataGridViewPaintParts paintParts)
    {
        if (ItemTemplate is null || value is null)
        {
            return;
        }

        Padding padding = ItemTemplate.Padding;

        Rectangle paddedBounds = cellBounds.Pad(padding);

        ItemTemplate.OnPaintContent(
            value,
            this,
            new PaintEventArgs(graphics, clipBounds),
            paddedBounds,
            _isMouseOver);
    }

    protected override void PaintErrorIcon(
        Graphics graphics,
        Rectangle clipBounds,
        Rectangle cellBounds,
        string? errorText)
    {

        if (ItemTemplate is null)
        {
            base.PaintErrorIcon(graphics, clipBounds, cellBounds, errorText);
            return;
        }

        Padding padding = ItemTemplate.Padding;

        // Set the padding for the cell:
        cellBounds.Inflate(-padding.Right, -padding.Bottom);
        cellBounds.Offset(padding.Left, padding.Top);

        ItemTemplate.PaintErrorIcon(graphics, clipBounds, cellBounds, errorText);
    }
}
