using CashRegisterSystem.Models;
using System.Text;

/// <summary>
/// Builds the printed receipt using a fixed-width layout (40 characters).
/// The class focuses only on text formatting: it assembles the header,
/// product lines, totals, VAT, and payment details into a complete receipt.
/// Each line is added with AppendLine, which writes the text and moves to
/// the next line, allowing the receipt to be built step by step.
/// </summary>
public static class ReceiptUI
{

    private const int ReceiptLineWidth = 40;
    private const int ProductNameColumnWidth = 14;
    private const int QuantityColumnWidth = 5;


    /// <summary>
    /// Creates the full receipt text in the same order it is printed.
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

        var receiptBuilder = new StringBuilder();

        BuildReceiptHeader(receiptBuilder, cashierName, registerNumber, receiptNumber);

        decimal totalReceiptAmount = 0;

        foreach (var cartItem in cartItems)
        {
            receiptBuilder.AppendLine(BuildProductLine(cartItem));
            totalReceiptAmount += cartItem.TotalPrice - cartItem.DiscountAmount;

            if (cartItem.DiscountAmount > 0)
                receiptBuilder.AppendLine(RightAlignText($"Rabatt: -{cartItem.DiscountAmount:0.00} kr"));
        }

        BuildTotalSection(receiptBuilder, totalReceiptAmount);
        BuildVatSection(receiptBuilder, cartItems);

        decimal totalDiscount = cartItems.Sum(i => i.DiscountAmount);

        receiptBuilder.AppendLine(
            AlignLabelAndValueRight("Erhållen rabatt:", $"{totalDiscount:0.00} kr")
        );
        receiptBuilder.AppendLine(new string('-', ReceiptLineWidth));
        receiptBuilder.AppendLine();

        BuildPaymentSection(receiptBuilder, paymentMethod, amountReceived, totalReceiptAmount);

        receiptBuilder.AppendLine(new string('=', ReceiptLineWidth));
        receiptBuilder.AppendLine(CenterText("SPARA KVITTOT"));
        receiptBuilder.AppendLine(CenterText("TACK FÖR BESÖKET!"));
        receiptBuilder.AppendLine(CenterText("VÄLKOMMEN ÅTER"));
        receiptBuilder.AppendLine(new string('=', ReceiptLineWidth));

        return receiptBuilder.ToString();
    }


    /// <summary>
    /// Writes store header, cashier info, date and time.
    /// </summary>
    private static void BuildReceiptHeader(
        StringBuilder receiptBuilder,
        string cashierName,
        int registerNumber,
        int receiptNumber)
    {
        receiptBuilder.AppendLine(new string('=', ReceiptLineWidth));
        receiptBuilder.AppendLine(CenterText("Frukt Marknaden"));
        receiptBuilder.AppendLine(CenterText("Öppet 08:00 - 23:00"));
        receiptBuilder.AppendLine(CenterText("Tel: 073-570 43 68"));
        receiptBuilder.AppendLine(new string('=', ReceiptLineWidth));

        receiptBuilder.AppendLine(
            $"Säljare: {cashierName}".PadRight(19) +
            $"Kassa: {registerNumber}".PadRight(11) +
            $"Kvitto: {receiptNumber}"
        );

        receiptBuilder.AppendLine(
            $"Datum: {DateTime.Now:yyyy-MM-dd}".PadRight(30) +
            $"Tid: {DateTime.Now:HH:mm}"
        );

        receiptBuilder.AppendLine(new string('-', ReceiptLineWidth));
    }


    /// <summary>
    /// Builds a single product line with quantity, unit and total.
    /// Ensures the line always fits within the fixed receipt width.
    /// </summary>
    private static string BuildProductLine(CartItem cartItem)
    {
        string productName = cartItem.Product.Name ?? "(saknas)";

        if (productName.Length > ProductNameColumnWidth)
            productName = productName.Substring(0, ProductNameColumnWidth - 3) + "...";

        string productNameColumn = productName.PadRight(ProductNameColumnWidth);

        string quantityColumn;
        string unitLabel;

        if (cartItem.Product.PriceUnit == PriceUnit.PerKilo)
        {
            quantityColumn = $"{cartItem.Weight:F2}".PadLeft(QuantityColumnWidth);
            unitLabel = "kg";
        }
        else
        {
            quantityColumn = $"{cartItem.Quantity}".PadLeft(QuantityColumnWidth);
            unitLabel = "st";
        }

        string leftSideText = $"{productNameColumn}{quantityColumn} {unitLabel}";
        string totalPriceText = $"{cartItem.TotalPrice:0.00} kr";

        int spacing = ReceiptLineWidth - leftSideText.Length - totalPriceText.Length;
        if (spacing < 1)
            spacing = 1;

        return leftSideText + new string(' ', spacing) + totalPriceText;
    }


    /// <summary>
    /// Writes the total amount section.
    /// </summary>
    private static void BuildTotalSection(StringBuilder receiptBuilder, decimal totalAmount)
    {
        receiptBuilder.AppendLine(new string('-', ReceiptLineWidth));
        receiptBuilder.AppendLine(AlignLabelAndValueRight("TOTALT:", $"{totalAmount:0.00} kr"));
        receiptBuilder.AppendLine();
    }

    /// <summary>
    /// Writes VAT breakdown (12%).
    /// </summary>
    private static void BuildVatSection(StringBuilder receiptBuilder, List<CartItem> cartItems)
    {
        decimal totalVat = 0;
        decimal totalExVat = 0;

        foreach (var item in cartItems)
        {
            decimal vatRate = item.Product.VatRate;
            decimal lineTotal = item.TotalPrice - item.DiscountAmount;

            decimal vatAmount = lineTotal * vatRate / (1 + vatRate);
            decimal exVat = lineTotal - vatAmount;

            totalVat += vatAmount;
            totalExVat += exVat;
        }

        receiptBuilder.AppendLine("MOMSREDOVISNING");
        receiptBuilder.AppendLine(new string('-', ReceiptLineWidth));
        receiptBuilder.AppendLine(AlignLabelAndValueRight("Moms:", $"{totalVat:0.00} kr"));
        receiptBuilder.AppendLine(AlignLabelAndValueRight("Exkl. moms:", $"{totalExVat:0.00} kr"));
        receiptBuilder.AppendLine(new string('-', ReceiptLineWidth));
        receiptBuilder.AppendLine();

    }


    /// <summary>
    /// Writes payment details depending on payment method.
    /// </summary>
    private static void BuildPaymentSection(
        StringBuilder receiptBuilder,
        string paymentMethod,
        decimal amountReceived,
        decimal totalAmount)
    {
        if (paymentMethod.Equals("Kontant", StringComparison.OrdinalIgnoreCase))
        {
            decimal safeAmountReceived = Math.Max(0, amountReceived);
            decimal changeAmount = safeAmountReceived - totalAmount;


            receiptBuilder.AppendLine(AlignLabelAndValueRight("Mottaget kontant:", $"{safeAmountReceived:0.00} kr"));
            receiptBuilder.AppendLine(AlignLabelAndValueRight("Åter kontant:", $"{changeAmount:0.00} kr"));
        }
        else
        {
            receiptBuilder.AppendLine("Betalt med kort");
            receiptBuilder.AppendLine(FormatCardPaymentDetails());
        }
    }

    // HELPER METHODS

    private static string AlignLabelAndValueRight(string leftLabel, string rightValue)
    {
        int spaces = ReceiptLineWidth - leftLabel.Length - rightValue.Length;
        if (spaces < 1)
            spaces = 1;

        return leftLabel + new string(' ', spaces) + rightValue;
    }

    private static string RightAlignText(string text)
    {
        return text.PadLeft(ReceiptLineWidth);
    }

    private static string CenterText(string text)
    {
        if (text.Length >= ReceiptLineWidth)
            return text;

        int padding = (ReceiptLineWidth - text.Length) / 2;
        return new string(' ', padding) + text;
    }

    private static string FormatCardPaymentDetails()
    {
        var cardDetailsBuilder = new StringBuilder();

        cardDetailsBuilder.AppendLine("Term: 1710025506");
        cardDetailsBuilder.AppendLine("DEBIT MASTERCARD ************5271");
        cardDetailsBuilder.AppendLine("Butik: 13403");
        cardDetailsBuilder.AppendLine($"Datum: {DateTime.Now:yyyy-MM-dd HH:mm}");
        cardDetailsBuilder.AppendLine("TVR: 0000008001");
        cardDetailsBuilder.AppendLine("Ref: 002550692064 130 Rsp:00 120440 K/1 7");
        cardDetailsBuilder.AppendLine("Personlig kod / Contactless");
        cardDetailsBuilder.AppendLine("Spara kvittot");

        return cardDetailsBuilder.ToString();
    }
}
