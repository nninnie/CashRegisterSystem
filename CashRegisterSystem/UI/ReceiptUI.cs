using CashRegisterSystem.Models;
using System.Text;

/// <summary>
/// Handles all receipt formatting and printing logic.
/// The class is responsible only for building the receipt text,
/// following a fixed-width layout (40 characters) to mimic real store receipts.
/// </summary>
public static class ReceiptUI
{
    // Fixed receipt width used for all alignment.
    private const int ReceiptWidth = 40;

    // Column widths for product line formatting.
    private const int ProductNameWidth = 14;
    private const int QuantityWidth = 5;
    private const int UnitPriceWidth = 6;

    /// <summary>
    /// Builds the full receipt as a formatted string.
    /// </summary>
    public static string FormatReceipt(
        int receiptNumber,
        List<CartItem> cartItems,
        string cashierName,
        int registerNumber,
        string paymentMethod,
        decimal amountReceived = 0)
    {
        if (cartItems == null)
            throw new ArgumentNullException(nameof(cartItems));

        var sb = new StringBuilder();

        BuildHeader(sb, cashierName, registerNumber, receiptNumber);

        decimal receiptTotal = 0;

        foreach (var item in cartItems)
        {
            sb.AppendLine(BuildProductLine(item));
            receiptTotal += item.TotalPrice - item.DiscountAmount;

            if (item.DiscountAmount > 0)
                sb.AppendLine(RightAlign($"Rabatt: -{item.DiscountAmount:0.00} kr"));
        }

        BuildTotals(sb, receiptTotal);
        BuildVatSection(sb, receiptTotal);
        BuildPaymentSection(sb, paymentMethod, amountReceived, receiptTotal);

        sb.AppendLine(new string('=', ReceiptWidth));
        sb.AppendLine(Center("SPARA KVITTOT"));
        sb.AppendLine(Center("TACK FÖR BESÖKET!"));
        sb.AppendLine(Center("VÄLKOMMEN ÅTER"));
        sb.AppendLine(new string('=', ReceiptWidth));

        return sb.ToString();
    }

    // ---------------------------------------------------------
    // HEADER SECTION
    // ---------------------------------------------------------

    /// <summary>
    /// Writes the store header, cashier info, date and time.
    /// </summary>
    private static void BuildHeader(StringBuilder sb, string cashier, int register, int receipt)
    {
        sb.AppendLine(new string('=', ReceiptWidth));
        sb.AppendLine(Center("FruktTorget"));
        sb.AppendLine(Center("Öppet 08:00 - 23:00"));
        sb.AppendLine(Center("Tel: 073-570 43 68"));
        sb.AppendLine(new string('=', ReceiptWidth));

        sb.AppendLine(
            $"Säljare: {cashier}".PadRight(19) +
            $"Kassa: {register}".PadRight(11) +
            $"Kvitto: {receipt}"
        );

        sb.AppendLine(
            $"Datum: {DateTime.Now:yyyy-MM-dd}".PadRight(30) +
            $"Tid: {DateTime.Now:HH:mm}"
        );

        sb.AppendLine(new string('-', ReceiptWidth));
    }

    // ---------------------------------------------------------
    // PRODUCT LINE SECTION
    // ---------------------------------------------------------

    /// <summary>
    /// Builds a single product line with quantity, unit, price and total.
    /// Ensures the line always fits within 40 characters.
    /// </summary>
    private static string BuildProductLine(CartItem item)
    {
        string name = item.Product.Name ?? "(saknas)";

        // Truncate long product names to fit the receipt width.
        if (name.Length > ProductNameWidth)
            name = name.Substring(0, ProductNameWidth - 3) + "...";

        string nameCol = name.PadRight(ProductNameWidth);

        string qtyCol;
        string unitLabel;
        string priceCol;

        // Weight-based product
        if (item.Product.PriceUnit == PriceUnit.PerKilo)
        {
            qtyCol = $"{item.Weight:F2}".PadLeft(QuantityWidth);
            unitLabel = "kg";
            priceCol = $"{item.UnitPrice:0.00} Kr/kg";
        }
        else // Piece-based product
        {
            qtyCol = $"{item.Quantity}".PadLeft(QuantityWidth);
            unitLabel = "st";
            priceCol = $"{item.UnitPrice:0.00}";
        }

        string left = $"{nameCol}{qtyCol} {unitLabel} * {priceCol}";
        string total = $"{item.TotalPrice:0.00} kr";

        // Calculate spacing so the total always aligns to the right.
        int spaces = ReceiptWidth - left.Length - total.Length;
        if (spaces < 1) spaces = 1;

        return left + new string(' ', spaces) + total;
    }

    // ---------------------------------------------------------
    // TOTALS SECTION
    // ---------------------------------------------------------

    /// <summary>
    /// Writes the total amount section.
    /// </summary>
    private static void BuildTotals(StringBuilder sb, decimal total)
    {
        sb.AppendLine(new string('-', ReceiptWidth));
        sb.AppendLine(AlignRight("TOTALT:", $"{total:0.00} kr"));
        sb.AppendLine();
    }

    // ---------------------------------------------------------
    // VAT SECTION
    // ---------------------------------------------------------

    /// <summary>
    /// Writes VAT breakdown (12%).
    /// </summary>
    private static void BuildVatSection(StringBuilder sb, decimal total)
    {
        const decimal vatRate = 0.12m;
        decimal vatAmount = total * vatRate / (1 + vatRate);
        decimal exVat = total - vatAmount;

        sb.AppendLine("MOMSREDOVISNING");
        sb.AppendLine(new string('-', ReceiptWidth));
        sb.AppendLine(AlignRight("12% moms:", $"{vatAmount:0.00} kr"));
        sb.AppendLine(AlignRight("Exkl. moms:", $"{exVat:0.00} kr"));
        sb.AppendLine(new string('-', ReceiptWidth));
        sb.AppendLine();
    }

    // ---------------------------------------------------------
    // PAYMENT SECTION
    // ---------------------------------------------------------

    /// <summary>
    /// Writes payment details depending on payment method.
    /// </summary>
    private static void BuildPaymentSection(StringBuilder sb, string method, decimal received, decimal total)
    {
        if (method.Equals("Kontant", StringComparison.OrdinalIgnoreCase))
        {
            decimal safeReceived = Math.Max(0, received);
            decimal change = safeReceived - total;

            sb.AppendLine(AlignRight("Mottaget kontant:", $"{safeReceived:0.00} kr"));
            sb.AppendLine(AlignRight("Åter kontant:", $"{change:0.00} kr"));
        }
        else
        {
            sb.AppendLine("Betalt med kort");
            sb.AppendLine(FormatCardPaymentDetails());
        }
    }

    // ---------------------------------------------------------
    // HELPER METHODS
    // ---------------------------------------------------------

    /// <summary>
    /// Aligns a label on the left and a value on the right.
    /// </summary>
    private static string AlignRight(string left, string right)
    {
        int spaces = ReceiptWidth - left.Length - right.Length;
        if (spaces < 1) spaces = 1;
        return left + new string(' ', spaces) + right;
    }

    /// <summary>
    /// Right-aligns a full string across the receipt width.
    /// </summary>
    private static string RightAlign(string text)
    {
        return text.PadLeft(ReceiptWidth);
    }

    /// <summary>
    /// Centers text within the fixed receipt width.
    /// </summary>
    private static string Center(string text)
    {
        if (text.Length >= ReceiptWidth)
            return text;

        int padding = (ReceiptWidth - text.Length) / 2;
        return new string(' ', padding) + text;
    }

    /// <summary>
    /// Builds the card payment detail block.
    /// </summary>
    private static string FormatCardPaymentDetails()
    {
        var sb = new StringBuilder();

        sb.AppendLine("Term: 1710025506");
        sb.AppendLine("DEBIT MASTERCARD ************5271");
        sb.AppendLine("Butik: 13403");
        sb.AppendLine($"Datum: {DateTime.Now:yyyy-MM-dd HH:mm}");
        sb.AppendLine("TVR: 0000008001");
        sb.AppendLine("Ref: 002550692064 130 Rsp:00 120440 K/1 7");
        sb.AppendLine("Personlig kod / Contactless");
        sb.AppendLine("Spara kvittot");

        return sb.ToString();
    }
}
