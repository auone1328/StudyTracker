using ClosedXML.Excel;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models.Entities;
using WebApplication1.Services;

public class ReportService : IReportService
{
    List<string> statuses = new List<string>() { "Не начато", "В работе", "Завершено" };
    public FileResult GenerateDocxReport(List<StudentAssignment> data, string fileName)
    {
        using (var mem = new MemoryStream())
        {
            using (var doc = WordprocessingDocument.Create(mem, WordprocessingDocumentType.Document))
            {
                var mainPart = doc.AddMainDocumentPart();
                mainPart.Document = new Document();
                var body = mainPart.Document.AppendChild(new Body());

                // Заголовок
                var title = body.AppendChild(new Paragraph());
                title.AppendChild(new Run(new Text("Отчет по заданиям"))).AppendChild(new Break());
                // Дата генерации
                body.AppendChild(new Paragraph(
                    new Run(
                        new Text($"Сформировано: {DateTime.Now:dd.MM.yyyy HH:mm}"))));
                // Таблица с данными
                var table = body.AppendChild(new Table());
                // Стили таблицы
                var tableProps = new TableProperties(
                    new TableBorders(
                        new TopBorder() { Val = BorderValues.Single, Size = 6 },
                        new BottomBorder() { Val = BorderValues.Single, Size = 6 },
                        new LeftBorder() { Val = BorderValues.Single, Size = 6 },
                        new RightBorder() { Val = BorderValues.Single, Size = 6 },
                        new InsideHorizontalBorder() { Val = BorderValues.Single, Size = 6 },
                        new InsideVerticalBorder() { Val = BorderValues.Single, Size = 6 }
                    ));

                table.AppendChild(tableProps);

                // Заголовки таблицы              
                var headerRow = table.AppendChild(new TableRow());
                headerRow.AppendChild(new TableCell(new Paragraph(new Run(new Text("Курс")))));
                headerRow.AppendChild(new TableCell(new Paragraph(new Run(new Text("Задание")))));
                headerRow.AppendChild(new TableCell(new Paragraph(new Run(new Text("Статус")))));
                headerRow.AppendChild(new TableCell(new Paragraph(new Run(new Text("Дедлайн")))));
               

                // Данные
                foreach (var item in data)
                {
                    var row = table.AppendChild(new TableRow());
                    row.AppendChild(new TableCell(new Paragraph(new Run(new Text(item.Assignment.Course.Title)))));
                    row.AppendChild(new TableCell(new Paragraph(new Run(new Text(item.Assignment.Title)))));
                    row.AppendChild(new TableCell(new Paragraph(new Run(new Text(statuses[(int)item.Status])))));
                    row.AppendChild(new TableCell(new Paragraph(new Run(new Text(item.Assignment.Deadline.ToString("dd.MM.yyyy"))))));
                }

                doc.Save();
            }

            return new FileContentResult(mem.ToArray(),
                "application/vnd.openxmlformats-officedocument.wordprocessingml.document")
            {
                FileDownloadName = fileName
            };
        }
    }

    public FileResult GenerateXlsxReport(List<StudentAssignment> data, string fileName)
    {
        using (var workbook = new XLWorkbook())
        {
            var worksheet = workbook.Worksheets.Add("Assignments");

            // Заголовки
            worksheet.Cell(1, 1).Value = "Курс";
            worksheet.Cell(1, 2).Value = "Задание";
            worksheet.Cell(1, 3).Value = "Статус";
            worksheet.Cell(1, 4).Value = "Дедлайн";        

            // Данные
            for (int i = 0; i < data.Count; i++)
            {
                var item = data[i];
                worksheet.Cell(i + 2, 1).Value = item.Assignment.Course?.Title;
                worksheet.Cell(i + 2, 2).Value = item.Assignment.Title;
                worksheet.Cell(i + 2, 3).Value = statuses[(int)item.Status];
                worksheet.Cell(i + 2, 4).Value = item.Assignment.Deadline;              
            }

            using (var mem = new MemoryStream())
            {
                workbook.SaveAs(mem);
                return new FileContentResult(mem.ToArray(),
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                {
                    FileDownloadName = fileName
                };
            }
        }
    }


    public FileResult GenerateOverdueDocxReport(List<StudentAssignment> data, string fileName)
    {
        using (var mem = new MemoryStream())
        {
            using (var doc = WordprocessingDocument.Create(mem, WordprocessingDocumentType.Document))
            {
                var mainPart = doc.AddMainDocumentPart();
                mainPart.Document = new Document();
                var body = mainPart.Document.AppendChild(new Body());

                // Заголовок отчета
                var title = body.AppendChild(new Paragraph(
                    new Run(
                        new Text("Отчет по просроченным заданиям"))));
                title.ParagraphProperties = new ParagraphProperties(
                    new Justification() { Val = JustificationValues.Center });

                // Дата генерации
                body.AppendChild(new Paragraph(
                    new Run(
                        new Text($"Сформировано: {DateTime.Now:dd.MM.yyyy HH:mm}"))));

                // Таблица с данными
                var table = body.AppendChild(new Table());

                // Стили таблицы
                var tableProps = new TableProperties(
                    new TableBorders(
                        new TopBorder() { Val = BorderValues.Single, Size = 6 },
                        new BottomBorder() { Val = BorderValues.Single, Size = 6 },
                        new LeftBorder() { Val = BorderValues.Single, Size = 6 },
                        new RightBorder() { Val = BorderValues.Single, Size = 6 },
                        new InsideHorizontalBorder() { Val = BorderValues.Single, Size = 6 },
                        new InsideVerticalBorder() { Val = BorderValues.Single, Size = 6 }
                    ));

                table.AppendChild(tableProps);

                // Заголовки таблицы
                var headerRow = table.AppendChild(new TableRow());
                AddTableCell(headerRow, "Студент", true);
                AddTableCell(headerRow, "Email", true);
                AddTableCell(headerRow, "Курс", true);
                AddTableCell(headerRow, "Задание", true);
                AddTableCell(headerRow, "Дедлайн", true);
                AddTableCell(headerRow, "Текущий статус", true);

                // Данные
                foreach (var item in data)
                {
                    var row = table.AppendChild(new TableRow());
                    AddTableCell(row, item.Student.FirstName + " " + item.Student.LastName);
                    AddTableCell(row, item.Student.Email);
                    AddTableCell(row, item.Assignment.Course.Title);
                    AddTableCell(row, item.Assignment.Title);
                    AddTableCell(row, item.Assignment.Deadline.ToString("dd.MM.yyyy"));
                    AddTableCell(row, statuses[(int)item.Status]);
                }

                doc.Save();
            }

            return new FileContentResult(mem.ToArray(),
                "application/vnd.openxmlformats-officedocument.wordprocessingml.document")
            {
                FileDownloadName = fileName
            };
        }
    }

    private void AddTableCell(TableRow row, string text, bool isHeader = false)
    {
        var cell = row.AppendChild(new TableCell());
        var paragraph = cell.AppendChild(new Paragraph());
        var run = paragraph.AppendChild(new Run());

        if (isHeader)
        {
            run.RunProperties = new RunProperties { Bold = new Bold() };
        }

        run.AppendChild(new Text(text));
    }

    public FileResult GenerateOverdueXlsxReport(List<StudentAssignment> data, string fileName)
    {
        using (var workbook = new XLWorkbook())
        {
            var worksheet = workbook.Worksheets.Add("Просроченные задания");

            // Заголовки
            worksheet.Cell(1, 1).Value = "Студент";
            worksheet.Cell(1, 2).Value = "Email";
            worksheet.Cell(1, 3).Value = "Курс";
            worksheet.Cell(1, 4).Value = "Задание";
            worksheet.Cell(1, 5).Value = "Дедлайн";
            worksheet.Cell(1, 6).Value = "Текущий статус";

            // Стиль заголовков
            var headerStyle = workbook.Style;
            headerStyle.Font.Bold = true;
            headerStyle.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            headerStyle.Fill.BackgroundColor = XLColor.LightGray;
            worksheet.Range("A1:G1").Style = headerStyle;

            // Данные
            for (int i = 0; i < data.Count; i++)
            {
                var item = data[i];
                worksheet.Cell(i + 2, 1).Value = item.Student.FirstName + " " + item.Student.LastName;
                worksheet.Cell(i + 2, 2).Value = item.Student.Email;
                worksheet.Cell(i + 2, 3).Value = item.Assignment.Course.Title;
                worksheet.Cell(i + 2, 4).Value = item.Assignment.Title;
                worksheet.Cell(i + 2, 5).Value = item.Assignment.Deadline;
                worksheet.Cell(i + 2, 6).Value = statuses[(int)item.Status];
            }

            // Авто-ширина колонок
            worksheet.Columns().AdjustToContents();

            using (var mem = new MemoryStream())
            {
                workbook.SaveAs(mem);
                return new FileContentResult(mem.ToArray(),
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                {
                    FileDownloadName = fileName
                };
            }
        }
    }
}