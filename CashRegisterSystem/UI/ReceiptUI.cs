using CashRegisterSystem.Models;
using System.Text;

public static class ReceiptUI
{
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

        receiptBuilder.AppendLine("========================================");
        receiptBuilder.AppendLine("                FruktTorget");
        receiptBuilder.AppendLine("             Öppet 08:00 - 23:00");
        receiptBuilder.AppendLine("             Tel: 073-570 43 68");
        receiptBuilder.AppendLine("========================================");

        receiptBuilder.AppendLine(
            $"Säljare: {cashierName}".PadRight(19) +
            $"Kassa: {registerNumber}".PadRight(11) +
            $"Kvitto: {receiptNumber}"
        );

        receiptBuilder.AppendLine(
            $"Datum: {DateTime.Now:yyyy-MM-dd}".PadRight(30) +
            $"Tid: {DateTime.Now:HH:mm}"
        );

        receiptBuilder.AppendLine("----------------------------------------");

        // --- Items ---
        decimal receiptTotalPrice = 0;

        foreach (var cartItem in cartItems)
        {
            if (cartItem?.Product == null)
                continue;

            string unitLabel = cartItem.Product.PriceUnit == PriceUnit.PerKilo ? "kg" : "st";

            decimal lineTotalPrice = cartItem.TotalPrice;
            receiptTotalPrice += lineTotalPrice;

            string productNameColumn = cartItem.Product.Name.PadRight(20);
            string quantityColumn = $"{cartItem.Quantity}".PadLeft(3);

            decimal unitPriceValue = cartItem.UnitPrice;
            string unitPriceColumn = $"{unitPriceValue:0.00}".PadLeft(8);

            string lineTotalColumn = $"{lineTotalPrice:0.00}".PadLeft(10);

            // Varuraden
            receiptBuilder.AppendLine(
                $"{productNameColumn}{quantityColumn} x {unitPriceColumn}  {lineTotalColumn} kr"
            );

            // Rabatt-raden (tillfällig logik – ersätts av CampaignService)
            if (cartItem.Product.CampaignPrice > 0 &&
                cartItem.Product.CampaignPrice < cartItem.Product.Price)
            {
                decimal unitDiscountAmount =
                    cartItem.Product.Price - cartItem.Product.CampaignPrice;

                decimal totalDiscountAmount =
                    unitDiscountAmount * cartItem.Quantity;

                receiptBuilder.AppendLine(
                    $"   Rabatt: -{totalDiscountAmount:0.00} kr"
                );
            }
        }

        receiptBuilder.AppendLine("----------------------------------------");
        receiptBuilder.AppendLine($"TOTALT:{receiptTotalPrice,30:F2} kr");
        receiptBuilder.AppendLine();

        // --- VAT ---
        const decimal vatRate = 0.12m;
        decimal vatAmount = receiptTotalPrice * vatRate / (1 + vatRate);
        decimal priceExcludingVat = receiptTotalPrice - vatAmount;

        receiptBuilder.AppendLine("MOMSREDOVISNING");
        receiptBuilder.AppendLine("----------------------------------------");
        receiptBuilder.AppendLine($"12% moms:{vatAmount,28:F2} kr");
        receiptBuilder.AppendLine($"Exkl. moms:{priceExcludingVat,26:F2} kr");
        receiptBuilder.AppendLine("----------------------------------------");

        // --- Payment ---
        receiptBuilder.AppendLine();

        if (paymentMethod == "Kontant")
        {
            decimal changeAmount = amountReceived - receiptTotalPrice;
            receiptBuilder.AppendLine($"Mottaget kontant:{amountReceived,18:F2} kr");
            receiptBuilder.AppendLine($"Åter kontant:{changeAmount,22:F2} kr");
        }
        else if (paymentMethod == "Kort")
        {
            receiptBuilder.AppendLine("Betalt med kort");
        }

        receiptBuilder.AppendLine("========================================");

        // --- Footer ---
        receiptBuilder.AppendLine("     Tack för besöket, spara kvittot");
        receiptBuilder.AppendLine("            Välkommen åter!");
        receiptBuilder.AppendLine("          Org.nr: 987654-3210");
        receiptBuilder.AppendLine("========================================");

        return receiptBuilder.ToString();
    }
}