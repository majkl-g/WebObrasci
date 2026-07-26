using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using PdfSharp;
using PdfSharp.Fonts;
using PdfSharp.Pdf;
using PdfSharp.Snippets.Font;
using System.Text.Json;
using WebObrasci1.Models;

namespace WebObrasci1.Services
{
    public class PdfConverter : IPdfConverter
    {
        static PdfConverter()
        {
            if (OperatingSystem.IsWindows())
            {
                GlobalFontSettings.UseWindowsFontsUnderWindows = true;
            }
            else if(Capabilities.Build.IsCoreBuild)
            {
                GlobalFontSettings.FontResolver = new FailsafeFontResolver();
            }            
        }

        public Stream ConvertToPdf(FormSubmission formSubmission)
        {
            var document = new Document();

            // Add a section to the document.
            var sectionBody = document.AddSection();

            // Add a paragraph to the section.
            var paragraphHeader = sectionBody.AddParagraph();

            // Set font color.
            paragraphHeader.Format.Font.Color = Colors.DarkBlue;

            // Add some text to the paragraph.
            paragraphHeader.AddFormattedText("Obrazac <sifra>", TextFormat.Bold);
            paragraphHeader.AddFormattedText("Sveučilište u Rijeci - Tehnički fakultet", TextFormat.Bold);
            paragraphHeader.AddFormattedText("Vukovarska 58 • 51000 Rijeka • Hrvatska", TextFormat.Bold);
            paragraphHeader.Format.Alignment = ParagraphAlignment.Right;

            // Add MigraDoc logo.
            //string imagePath = IOUtility.GetAssetsPath(@"migradoc/images/MigraDoc-128x128.png")!;
            //document.LastSection.AddImage(imagePath);

            // Add a section to the document.
            var paragraphTitle = sectionBody.AddParagraph();
            paragraphTitle.AddFormattedText(formSubmission.Form.Title, TextFormat.Bold);
            paragraphTitle.Format.Alignment = ParagraphAlignment.Center;

            var paragraphAnswers = sectionBody.AddParagraph();
            paragraphAnswers.Format.Alignment = ParagraphAlignment.Left;

            //answers
            var answers = JsonSerializer.Deserialize<Dictionary<string, string>>(formSubmission.DataJson);
            if (answers != null)
            {
                foreach (var answer in answers)
                {
                    paragraphAnswers.AddFormattedText(answer.Key);
                    paragraphAnswers.AddFormattedText(answer.Value);
                }
            }

            var style = document.Styles[StyleNames.Normal]!;
            style.Font.Name = "Arial";

            // Create a renderer for the MigraDoc document.
            var pdfRenderer = new PdfDocumentRenderer
            {
                // Associate the MigraDoc document with a renderer.
                Document = document,
                PdfDocument =
                    {
                        // Change some settings before rendering the MigraDoc document.
                        PageLayout = PdfPageLayout.SinglePage,
                        ViewerPreferences =
                        {
                            FitWindow = true
                        }
                    }
            };

            // Layout and render document to PDF.
            pdfRenderer.RenderDocument();
            var ms = new MemoryStream();
            pdfRenderer.PdfDocument.Save(ms);
            return ms;
        }
    }
}
