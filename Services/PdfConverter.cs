using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes;
using MigraDoc.DocumentObjectModel.Tables;
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

            // Header table (image left, text right)
            var headerTable = sectionBody.AddTable();
            headerTable.Borders.Visible = false;

            // Add columns
            headerTable.AddColumn(Unit.FromCentimeter(3)); // image column
            headerTable.AddColumn(Unit.FromCentimeter(14)); // text column

            var headerRow = headerTable.AddRow();
            headerRow.VerticalAlignment = VerticalAlignment.Center;

            // LEFT SIDE - placeholder image
            var imageParagraph = headerRow.Cells[0].AddParagraph();

            var ImagePath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                "images",
                "ritehlogo.jpg"
            );

            Image image = imageParagraph.AddImage(ImagePath);
            image.Width = Unit.FromCentimeter(2);


            // Add a paragraph to the section.
            var paragraphHeader = headerRow.Cells[1].AddParagraph();

            // Set font color.
            paragraphHeader.Format.Font.Color = Color.Parse("#0066CC");

            // Add some text to the paragraph.
            var part1 = paragraphHeader.AddFormattedText($"Obrazac {formSubmission.Form.Code}");
            paragraphHeader.AddLineBreak();
            part1.Font.Size = 11;
            var part2 = paragraphHeader.AddFormattedText("Sveučilište u Rijeci - Tehnički fakultet", TextFormat.Bold);
            paragraphHeader.AddLineBreak();
            part2.Font.Size = 13;
            var part3 = paragraphHeader.AddFormattedText("Vukovarska 58 • 51000 Rijeka • Hrvatska");
            part3.Font.Size = 10;
            paragraphHeader.Format.Alignment = ParagraphAlignment.Right;
            paragraphHeader.Format.SpaceAfter = Unit.FromCentimeter(1.5);

            

            

            // Add MigraDoc logo.
            //string imagePath = IOUtility.GetAssetsPath(@"migradoc/images/MigraDoc-128x128.png")!;
            //document.LastSection.AddImage(imagePath);

            // Add a section to the document.

            var paragraphTitle = sectionBody.AddParagraph();
            paragraphTitle.Format.Font.Color = Color.Parse("#0066CC");
            paragraphTitle.Format.LeftIndent = Unit.FromMillimeter(35);
            paragraphTitle.Format.RightIndent = Unit.FromMillimeter(35);

            //paragraphHeader.AddLineBreak();
            var title = paragraphTitle.AddFormattedText(formSubmission.Form.Title, TextFormat.Bold);
            title.Font.Size = 12;
            paragraphTitle.Format.Alignment = ParagraphAlignment.Center;
            paragraphTitle.Format.SpaceAfter = Unit.FromCentimeter(0.5);

            var paragraphAnswers = sectionBody.AddParagraph();
            paragraphAnswers.Format.Alignment = ParagraphAlignment.Left;
            paragraphAnswers.Format.LineSpacing = 1.5;
            paragraphAnswers.Format.LineSpacingRule = LineSpacingRule.Multiple;
            paragraphAnswers.Format.Font.Color = Color.Parse("#0066CC");

            //answers
            var answers = JsonSerializer.Deserialize<Dictionary<string, string>>(formSubmission.DataJson);
            var counter = 0;

            foreach(var field in formSubmission.Form.Fields.OrderBy(x => x.Order).ThenBy(x => x.Id))
            {
                if (field.Type == FormFieldType.Section)
                {
                    paragraphAnswers.AddLineBreak();
                    paragraphAnswers.AddFormattedText(field.Label, TextFormat.Bold);
                    paragraphAnswers.AddLineBreak();
                }
                else if (field.Type == FormFieldType.SubSection)
                {
                    paragraphAnswers.AddLineBreak();
                    paragraphAnswers.AddFormattedText(field.Label);
                    paragraphAnswers.AddLineBreak();
                }
                else
                {
                    paragraphAnswers.AddText("\u00A0\u00A0\u00A0");
                    paragraphAnswers.AddFormattedText(field.Label);
                    paragraphAnswers.AddText(": "); 
                    
                    if (answers != null && answers.TryGetValue(field.Name, out var answerValue))
                    {
                        var fieldValue = answerValue;
                        if (field.Type == FormFieldType.CheckBox)
                        {
                            fieldValue = fieldValue == "on" ? "DA" : "NE";
                        }
                        else if (field.Type == FormFieldType.Date && DateTime.TryParse(fieldValue, out var date))
                        {
                            fieldValue = date.ToString("dd.MM.yyyy.");
                        }

                        if (field.Type == FormFieldType.Select && field.SelectValues != null)
                        {
                            paragraphAnswers.AddLineBreak();
                            foreach (var selectVal in field.SelectValues)
                            {
                                var isSelected = selectVal.Value == answerValue;
                                var symbol = isSelected ? "X" : "O";
                                var value = paragraphAnswers.AddFormattedText($"\u00A0\u00A0\u00A0 {symbol} " + selectVal.Value);
                                paragraphAnswers.AddLineBreak();
                            }
                            counter--;
                        }
                        else
                        {
                            var value = paragraphAnswers.AddFormattedText("\u00A0\u00A0\u00A0" + fieldValue);
                            value.AddFormattedText("\u00A0\u00A0\u00A0\u00A0");
                            value.Font.Underline = Underline.Dotted;
                        }
                    }
                    else
                    {
                        var value = paragraphAnswers.AddFormattedText(new string('\u00A0', 10));
                        value.Font.Underline = Underline.Dotted;
                    }
                    
                    counter++;
                    if (counter % 2 == 0)
                    {
                        paragraphAnswers.AddLineBreak();
                    }
                }
            }

            if (answers != null)
            {
                var otherAnswerKeys = answers
                    .Keys
                    .Except(formSubmission.Form.Fields.Select(x => x.Name))
                    .ToList();

                if (otherAnswerKeys != null)
                {
                    foreach (var key in otherAnswerKeys)
                    {
                        paragraphAnswers.AddText("\u00A0\u00A0\u00A0");
                        paragraphAnswers.AddFormattedText(key);
                        paragraphAnswers.AddText(": ");

                        var fieldValue = answers[key];

                        var value = paragraphAnswers.AddFormattedText("\u00A0\u00A0\u00A0" + fieldValue);
                        value.AddFormattedText("\u00A0\u00A0\u00A0\u00A0");
                        value.Font.Underline = Underline.Dotted;
                        counter++;
                        if (counter % 2 == 0)
                        {
                            paragraphAnswers.AddLineBreak();
                        }

                    }
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
