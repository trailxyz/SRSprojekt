using iTextSharp.text;
using iTextSharp.text.pdf;
using SRSprojekt.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Hosting;

namespace SRSprojekt.Reports
{
    public class RezervacijeReport
    {
        public byte[] Podaci { get; private set; }

        private PdfPCell GenerirajCeliju(string sadrzaj, Font font, BaseColor boja, bool wrap)
        {
            PdfPCell c1 = new PdfPCell(new Phrase(sadrzaj, font));
            c1.BackgroundColor = boja;
            c1.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            c1.Padding = 5;
            c1.NoWrap = wrap;
            c1.Border = Rectangle.BOTTOM_BORDER;
            c1.BorderColor = BaseColor.LIGHT_GRAY;
            return c1;
        }

        public void ListaStolova(List<Stolovi> stolovi)
        {
     
            BaseFont bfontZaglavlje = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1250, false);
            BaseFont bfontTekst = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1250, false);
            BaseFont bfontPodnozje = BaseFont.CreateFont(BaseFont.TIMES_ITALIC, BaseFont.CP1250, false);

            Font fontZaglavlje = new Font(bfontZaglavlje, 12, Font.NORMAL, BaseColor.DARK_GRAY);
            Font fontZaglavljeBold = new Font(bfontZaglavlje, 12, Font.BOLD, BaseColor.DARK_GRAY);
            Font fontNaslov = new Font(bfontTekst, 14, Font.BOLDITALIC, BaseColor.DARK_GRAY);
            Font fontTablicaZaglavlje = new Font(bfontTekst, 10, Font.BOLD, BaseColor.WHITE);
            Font fontTekst = new Font(bfontTekst, 10, Font.NORMAL, BaseColor.BLACK);

            BaseColor tPozadinaZaglavlje = new BaseColor(11, 65, 121);
            BaseColor tPozadinaSadrzaj = BaseColor.WHITE;

            using (MemoryStream mstream = new MemoryStream())
            {
                using (Document pdfDokument = new Document(PageSize.A4, 50, 50, 20, 50))
                {
                    try
                    {
                        PdfWriter.GetInstance(pdfDokument, mstream).CloseStream = false;
                        pdfDokument.Open();

                
                        PdfPTable tZaglavlje = new PdfPTable(2);
                        tZaglavlje.HorizontalAlignment = Element.ALIGN_LEFT;
                        tZaglavlje.DefaultCell.Border = Rectangle.NO_BORDER;
                        tZaglavlje.WidthPercentage = 100f;
                        float[] sirinaKolonaZag = new float[] { 1f, 3f };
                        tZaglavlje.SetWidths(sirinaKolonaZag);

                        var logoPath = HostingEnvironment.MapPath("~/Slike/MEV.png");
                        if (File.Exists(logoPath))
                        {
                            var logo = iTextSharp.text.Image.GetInstance(logoPath);
                            logo.Alignment = Element.ALIGN_LEFT;
                            logo.ScaleAbsoluteWidth(50);
                            logo.ScaleAbsoluteHeight(50);

                            PdfPCell cLogo = new PdfPCell(logo);
                            cLogo.Border = Rectangle.NO_BORDER;
                            tZaglavlje.AddCell(cLogo);
                        }
                        else
                        {
                            throw new ApplicationException("Logo file not found: " + logoPath);
                        }

                        Paragraph info = new Paragraph();
                        info.Alignment = Element.ALIGN_RIGHT;
                        info.SetLeading(0, 1.2f);
                        info.Add(new Chunk("MEĐIMURSKO VELEUČILIŠTE U ČAKOVCU \n", fontZaglavljeBold));
                        info.Add(new Chunk("Bana Josipa Jelačića 22a \n Čakovec \n", fontZaglavlje));

                        PdfPCell cInfo = new PdfPCell();
                        cInfo.AddElement(info);
                        cInfo.HorizontalAlignment = Element.ALIGN_RIGHT;
                        cInfo.Border = Rectangle.NO_BORDER;
                        tZaglavlje.AddCell(cInfo);

                        pdfDokument.Add(tZaglavlje);

              
                        Paragraph pNaslov = new Paragraph("POPIS REZERVACIJA", fontNaslov);
                        pNaslov.Alignment = Element.ALIGN_CENTER;
                        pNaslov.SpacingBefore = 20;
                        pNaslov.SpacingAfter = 20;
                        pdfDokument.Add(pNaslov);

               
                        PdfPTable t = new PdfPTable(4);
                        t.WidthPercentage = 100;
                        t.SetWidths(new float[] { 1, 2, 1, 2 });

                        t.AddCell(GenerirajCeliju("R.br.", fontTablicaZaglavlje, tPozadinaZaglavlje, true));
                        t.AddCell(GenerirajCeliju("Broj stola", fontTablicaZaglavlje, tPozadinaZaglavlje, true));
                        t.AddCell(GenerirajCeliju("Zauzetost", fontTablicaZaglavlje, tPozadinaZaglavlje, true));
                        t.AddCell(GenerirajCeliju("Rezervirao", fontTablicaZaglavlje, tPozadinaZaglavlje, true));

                        int i = 1;
                        foreach (Stolovi s in stolovi)
                        {



                            string rezervirao = s.aktivniR != null ? s.aktivniR.ImePrezime : "N/A";

                            t.AddCell(GenerirajCeliju(i.ToString(), fontTekst, tPozadinaSadrzaj, false));
                            t.AddCell(GenerirajCeliju(s.broj_stola, fontTekst, tPozadinaSadrzaj, false));
                            t.AddCell(GenerirajCeliju(s.zauzetost ? "DA" : "NE", fontTekst, tPozadinaSadrzaj, false));
                            t.AddCell(GenerirajCeliju(rezervirao, fontTekst, tPozadinaSadrzaj, false));
                            i++;
                        }

                    
                        pdfDokument.Add(t);

                
                        Paragraph pMjesto = new Paragraph("Čakovec, " + DateTime.Now.ToString("dd.MM.yyyy"), fontTekst);
                        pMjesto.Alignment = Element.ALIGN_RIGHT;
                        pMjesto.SpacingBefore = 30;
                        pdfDokument.Add(pMjesto);

                        pdfDokument.Close();
                    }
                    catch (Exception ex)
                    {
                       
                        throw new ApplicationException("Error generating PDF: " + ex.Message);
                    }
                }

                Podaci = mstream.ToArray();

                using (var reader = new PdfReader(Podaci))
                {
                    using (var ms = new MemoryStream())
                    {
                        using (var stamper = new PdfStamper(reader, ms))
                        {
                            int PageCount = reader.NumberOfPages;
                            if (PageCount == 0)
                            {
                                throw new ApplicationException("The generated PDF document has no pages.");
                            }

                            for (int i = 1; i <= PageCount; i++)
                            {
                                Rectangle pageSize = reader.GetPageSize(i);
                                PdfContentByte canvas = stamper.GetOverContent(i);

                                canvas.BeginText();
                                canvas.SetFontAndSize(bfontPodnozje, 10);

                                canvas.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, $"Stranica {i} / {PageCount}", pageSize.Right - 50, 30, 0);
                                canvas.EndText();
                            }
                        }
                        Podaci = ms.ToArray();
                    }
                }
            }
        }



        public void Stol(Stolovi stol, string kreirao)
        {
            BaseFont bfontZaglavlje = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1250, false);
            BaseFont bfontText = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1250, BaseFont.EMBEDDED);
            BaseFont bfontPodnozje = BaseFont.CreateFont(BaseFont.TIMES_ITALIC, BaseFont.CP1250, false);

            Font fontZaglavlje = new Font(bfontZaglavlje, 12, Font.NORMAL, BaseColor.DARK_GRAY);
            Font fontZaglavljeBold = new Font(bfontZaglavlje, 12, Font.BOLD, BaseColor.DARK_GRAY);
            Font fontNaslov = new Font(bfontText, 14, Font.BOLDITALIC, BaseColor.DARK_GRAY);
            Font fontTekstBold = new Font(bfontText, 12, Font.BOLD, BaseColor.BLACK);
            Font fontTekst = new Font(bfontText, 12, Font.NORMAL, BaseColor.BLACK);

            BaseColor tPozadinaSadrzaj = BaseColor.WHITE;

            using (MemoryStream mstream = new MemoryStream())
            {
                using (Document pdfDokument = new Document(PageSize.A4, 50, 50, 20, 50))
                {
                    PdfWriter.GetInstance(pdfDokument, mstream).CloseStream = false;
                    pdfDokument.Open();

                    PdfPTable tZaglavlje = new PdfPTable(2);
                    tZaglavlje.HorizontalAlignment = Element.ALIGN_LEFT;
                    tZaglavlje.DefaultCell.Border = Rectangle.NO_BORDER;
                    tZaglavlje.WidthPercentage = 100f;
                    float[] sirinaKolonaZag = new float[] { 1f, 3f };
                    tZaglavlje.SetWidths(sirinaKolonaZag);

                    var logoPath = HostingEnvironment.MapPath("~/Slike/MEV.png");
                    if (File.Exists(logoPath))
                    {
                        var logo = iTextSharp.text.Image.GetInstance(logoPath);
                        logo.Alignment = Element.ALIGN_LEFT;
                        logo.ScaleAbsoluteWidth(50);
                        logo.ScaleAbsoluteHeight(50);

                        PdfPCell cLogo = new PdfPCell(logo);
                        cLogo.Border = Rectangle.NO_BORDER;
                        tZaglavlje.AddCell(cLogo);
                    }
                    else
                    {
                        throw new ApplicationException("Logo file not found: " + logoPath);
                    }

                    Paragraph info = new Paragraph();
                    info.Alignment = Element.ALIGN_RIGHT;
                    info.SetLeading(0, 1.2f);
                    info.Add(new Chunk("MEĐIMURSKO VELEUČILIŠTE U ČAKOVCU \n", fontZaglavljeBold));
                    info.Add(new Chunk("Bana Josipa Jelačića 22a \n Čakovec \n", fontZaglavlje));

                    PdfPCell cInfo = new PdfPCell();
                    cInfo.AddElement(info);
                    cInfo.HorizontalAlignment = Element.ALIGN_RIGHT;
                    cInfo.Border = Rectangle.NO_BORDER;
                    tZaglavlje.AddCell(cInfo);

                    pdfDokument.Add(tZaglavlje);

                    Paragraph pNaslov = new Paragraph("PODACI O STOLU", fontNaslov);
                    pNaslov.Alignment = Element.ALIGN_CENTER;
                    pNaslov.SpacingBefore = 20;
                    pNaslov.SpacingAfter = 20;
                    pdfDokument.Add(pNaslov);

                    PdfPTable t = new PdfPTable(2);
                    t.WidthPercentage = 100;
                    t.SetWidths(new float[] { 1, 2 });



                    t.AddCell(GenerirajCeliju("Broj stola:", fontTekstBold, tPozadinaSadrzaj, false));
                    t.AddCell(GenerirajCeliju(stol.broj_stola, fontTekst, tPozadinaSadrzaj, false));
                    t.AddCell(GenerirajCeliju("Zauzetost:", fontTekstBold, tPozadinaSadrzaj, false));
                    t.AddCell(GenerirajCeliju(stol.zauzetost ? "DA" : "NE", fontTekst, tPozadinaSadrzaj, false));
                    t.AddCell(GenerirajCeliju("Rezervirao:", fontTekstBold, tPozadinaSadrzaj, false));
                    t.AddCell(GenerirajCeliju(stol.aktivniR.ImePrezime, fontTekst, tPozadinaSadrzaj, false));
                    t.AddCell(GenerirajCeliju("Kreirao:", fontTekstBold, tPozadinaSadrzaj, false));
                    t.AddCell(GenerirajCeliju(kreirao, fontTekst, tPozadinaSadrzaj, false));

                    pdfDokument.Add(t);

                    Paragraph pMjesto = new Paragraph("Čakovec, " + DateTime.Now.ToString("dd.MM.yyyy"), fontTekst);
                    pMjesto.Alignment = Element.ALIGN_RIGHT;
                    pMjesto.SpacingBefore = 30;
                    pdfDokument.Add(pMjesto);

                    pdfDokument.Close();
                }

                Podaci = mstream.ToArray();

                using (var reader = new PdfReader(Podaci))
                {
                    using (var ms = new MemoryStream())
                    {
                        using (var stamper = new PdfStamper(reader, ms))
                        {
                            int PageCount = reader.NumberOfPages;
                            if (PageCount == 0)
                            {
                                throw new ApplicationException("The generated PDF document has no pages.");
                            }

                            for (int i = 1; i <= PageCount; i++)
                            {
                                Rectangle pageSize = reader.GetPageSize(i);
                                PdfContentByte canvas = stamper.GetOverContent(i);

                                canvas.BeginText();
                                canvas.SetFontAndSize(bfontPodnozje, 10);

                                canvas.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, $"Stranica {i} / {PageCount}", pageSize.Right - 50, 30, 0);
                                canvas.EndText();
                            }
                        }
                        Podaci = ms.ToArray();
                    }
                }
            }
        }
    }
}
