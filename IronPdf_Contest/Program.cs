using IronPdf.Rendering;

namespace IronPdf_Contest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Resolve project root
            string projectRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, @"..\..\.."));
            string htmlPath = Path.Combine(projectRoot, "Templates", "Invoice-3.html");

            // Read template
            string template = File.ReadAllText(htmlPath);

            // Load invoice model
            var invoice = ReadInvoice();

            // Execute template
            HtmlTemplateEngine htmlTemplateEngine = new();
            var htmlContent = htmlTemplateEngine.Execute(template, invoice);

            // Output path (pdf name = html name)
            string pdfFileName = Path.GetFileNameWithoutExtension(htmlPath) + ".pdf";
            string outputPath = Path.Combine(projectRoot, "pdf", pdfFileName);

            // Generate the PDF
            GeneratePdf(htmlContent, outputPath);

            Console.WriteLine("PDF Generated successfully at: " + outputPath);
        }

        private static void GeneratePdf(string htmlContent, string outputPath)
        {
            IronPdf.License.LicenseKey = "IRONSUITE.SURINDER7310.GMAIL.COM.30466-BBA331D306-D424NDQ-2O24FRHJOSQ7-5ULHEBTEJ3SB-CP6ALV6RZZZ5-QYH35CS4WNIU-N5YHI4T3PUZ2-2BRXII27LLRV-UNMOTI-TRQDID2RHCGQUA-DEPLOYMENT.TRIAL-VOQL3O.TRIAL.EXPIRES.24.DEC.2025";

            var renderer = new ChromePdfRenderer();

            renderer.RenderingOptions.CssMediaType = PdfCssMediaType.Print;

            renderer.RenderingOptions.MarginTop = 0;
            renderer.RenderingOptions.MarginBottom = 0;
            renderer.RenderingOptions.MarginLeft = 0;
            renderer.RenderingOptions.MarginRight = 0;

            renderer.RenderingOptions.PrintHtmlBackgrounds = true;

            renderer.RenderingOptions.PaperSize = PdfPaperSize.A4;
            renderer.RenderingOptions.PaperOrientation = PdfPaperOrientation.Portrait;

            var pdf = renderer.RenderHtmlAsPdf(htmlContent);

            // Ensure folder exists
            Directory.CreateDirectory(Path.GetDirectoryName(outputPath));

            pdf.SaveAs(outputPath);
        }


        private static Invoice ReadInvoice()
        {
            var invoice = new Invoice
            {
                InvoiceNumber = "INV-1001",
                Date = new DateTime(2025, 1, 10),
                DueDate = new DateTime(2025, 1, 18),
                YourReference = "Ref-9087",
                ProjectName = "Name of project",
                Notes = "Thank you for your business.",

                From = new CompanyInfo
                {
                    Name = "Iron PDF",
                    AddressLine1 = "Dabi Bazar",
                    City = "",
                    CVR = "",
                    Phone = "",
                    Email = "",
                    Website = ""
                },

                To = new CompanyInfo
                {
                    Name = "CrossArtist ApS",
                    AddressLine1 = "N W Larsens Vej 10 Langesø",
                    AddressLine2 = "3080 Tikøb",
                    City = "Copenhegan",
                    CVR = "DK44953161",
                    Phone = "12 34 56 78",
                    Email = "hello@acconta.com",
                    Website = "www.acconta.com"
                },

                Items = new List<InvoiceItem>
                {
                    new InvoiceItem
                    {
                        Id = 1,
                        Description = "Website Development",
                        Quantity = 2,
                        Unit = "Hours",
                        UnitPrice = 500,
                        Discount = 0
                    },
                    new InvoiceItem
                    {
                        Id = 2,
                        Description = "Hosting Fee",
                        Quantity = 1,
                        Unit = "Unit",
                        UnitPrice = 200,
                        Discount = 20
                    }
                }
            };

            // Calculate totals
            invoice.SubTotal = invoice.Items.Sum(i => i.Price);
            invoice.Vat = invoice.SubTotal * 0.25m;   // 25% VAT example
            invoice.Discount = 0;
            invoice.Total = invoice.SubTotal + invoice.Vat - invoice.Discount;

            invoice.Terms = new PaymentTerms
            {
                TextLine1 = "Netto 8 dage - Forfaldsdato: 18/01/2025",
                BankInfo = "Bank / Reg.nr. 6695 / Kontonr. 45000978675",
                ExtraInfo = "Ved betaling efter forfald tilskrives renter på 0,89% pr. påbegyndt måned."
            };

            return invoice;
        }
    }
}
