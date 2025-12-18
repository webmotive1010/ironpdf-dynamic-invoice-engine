# IronPDF Dynamic HTML-to-PDF Invoice Engine

## 📄 Overview
This project demonstrates a **flexible, production-ready invoice PDF generation engine** built using **IronPDF** in C#. Instead of hard-coding invoice layouts, the system allows **multiple customers to define their own invoice designs using HTML templates**, which are dynamically populated with invoice data and converted into PDFs.

The core idea is to **separate data, layout, and rendering**, making the solution scalable, maintainable, and highly customizable.

👉 Learn more about IronPDF here: **[https://ironpdf.com/](https://ironpdf.com/)**

---

## 🚀 Key Features

- HTML-based invoice templates (customer-specific)
- Dynamic placeholders for invoice data
- Support for loops, conditions, and formatting
- Clean C# invoice models
- High-fidelity PDF rendering using IronPDF
- Chrome-based rendering for accurate CSS support

---

## 🏗 Architecture

```
Invoice Model (C#)
       ↓
HTML Template
       ↓
Custom Template Engine
       ↓
Final HTML
       ↓
IronPDF Renderer
       ↓
PDF Output
```

---

## 📦 Project Structure

```
IronPdf_Contest/
│
├── Templates/
│   ├── Invoice-1.html
│   ├── Invoice-2.html
│   └── Invoice-3.html
│
├── Models/
│   ├── Invoice.cs
│   └── InvoiceItem.cs
│
├── Services/
│   └── HtmlTemplateEngine.cs
│
├── Pdf/
│   └── Invoice-3.pdf
│
└── Program.cs
```

---

## 🧾 Invoice Model Example

```csharp
public class Invoice
{
    public string InvoiceNumber { get; set; }
    public DateTime InvoiceDate { get; set; }
    public string CustomerName { get; set; }
    public List<InvoiceItem> Items { get; set; }
    public decimal SubTotal { get; set; }
    public decimal Tax { get; set; }
    public decimal GrandTotal { get; set; }
}
```

---

## 🖨 PDF Generation Code

```csharp
var renderer = new ChromePdfRenderer();

renderer.RenderingOptions.PaperSize = PdfPaperSize.A4;
renderer.RenderingOptions.PrintHtmlBackgrounds = true;
renderer.RenderingOptions.MarginTop = 0;
renderer.RenderingOptions.MarginBottom = 0;

var pdf = renderer.RenderHtmlAsPdf(htmlContent);
pdf.SaveAs(savePath);
```

IronPDF ensures that the generated PDF matches the HTML exactly, including fonts, colors, tables, and layouts.

---

## 🧩 Custom HTML Placeholder Examples

### 1️⃣ Simple Value Placeholders

```html
<h1>Invoice {{InvoiceNumber}}</h1>
<p>Date: {{InvoiceDate}}</p>
<p>Customer: {{CustomerName}}</p>
```

---

### 2️⃣ Nested Object Placeholders

```html
<p>
  {{Company.Name}}<br />
  {{Company.Address}}<br />
  GSTIN: {{Company.GSTNumber}}
</p>
```

---

### 3️⃣ Loop / Repeating Sections (Invoice Items)

```html
<table>
  <tbody>
    {{#each Items}}
    <tr>
      <td>{{Description}}</td>
      <td>{{Quantity}}</td>
      <td>{{Rate}}</td>
      <td>{{Amount}}</td>
    </tr>
    {{/each}}
  </tbody>
</table>
```

---

### 4️⃣ Conditional Sections

```html
{{#if HasDiscount}}
<tr>
  <td colspan="3">Discount</td>
  <td>-{{DiscountAmount}}</td>
</tr>
{{/if}}
```

---

### 5️⃣ Conditional Tax Layouts

```html
{{#if IsGSTInvoice}}
<p>GST @ {{GSTPercent}}%</p>
{{/if}}

{{#if IsVATInvoice}}
<p>VAT @ {{VATPercent}}%</p>
{{/if}}
```

---

### 6️⃣ Formatting Dates & Currency

```html
<p>Date: {{InvoiceDate:dd-MMM-yyyy}}</p>
<p>Total: ₹ {{GrandTotal:N2}}</p>
```

---

## 🌍 Community Impact

This project demonstrates a real-world pattern applicable to:

- Accounting software
- ERP systems
- Billing & invoicing platforms
- SaaS reporting engines

By allowing invoice layouts to be fully HTML-driven, developers can reduce maintenance cost while empowering designers and business users.

---

## ✅ Why IronPDF

- Excellent HTML & CSS compatibility
- Chrome-based rendering engine
- Reliable, fast, and production-ready
- Ideal for business document generation

Learn more about IronPDF: **[https://ironpdf.com/](https://ironpdf.com/)**

---

## 🏁 Conclusion

This project shows how **IronPDF’s HTML-to-PDF capabilities** can be combined with custom template logic to build a **highly flexible invoice generation system**. It provides a clean separation of concerns, supports unlimited layouts, and delivers professional PDFs suitable for real business environments.

