using SixLabors.Fonts;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp;
using QSL.Models;
using ADIFLib;

namespace QSL.Renderer
{
    public class Renderer
    {
        private const string CARD_FILE = "card.png";
        private const string FONT_FILE = "MAJOR_SHIFT.TTF";
        private Log log;

        public Renderer(string call, string date, string time, string band, string freq, string send, string receive, string mode, string power)
        {
            Log log = new Log()
            {
                Call = call,
                Date = date,
                Time = time,
                Band = band,
                Freq = freq,
                Send = send,
                Receive = receive,
                Mode = mode,
                Power = power
            };

            this.log = log;
        }

        public Renderer(Log log)
        {
            this.log = log;
        }

        public Renderer(ADIFQSO qso)
        {
            this.log = new Log()
            {
                Call = qso.Where(x => x.Name == "call").DefaultIfEmpty(new Token()).FirstOrDefault().Data,
                Date = qso.Where(x => x.Name == "qso_date").DefaultIfEmpty(new Token()).FirstOrDefault().Data,
                Time = qso.Where(x => x.Name == "time_off").DefaultIfEmpty(new Token()).FirstOrDefault().Data,
                Band = qso.Where(x => x.Name == "band").DefaultIfEmpty(new Token()).FirstOrDefault().Data,
                Freq = qso.Where(x => x.Name == "freq").DefaultIfEmpty(new Token()).FirstOrDefault().Data,
                Send = qso.Where(x => x.Name == "rst_sent").DefaultIfEmpty(new Token()).FirstOrDefault().Data,
                Receive = qso.Where(x => x.Name == "rst_send").DefaultIfEmpty(new Token()).FirstOrDefault().Data,
                Mode = qso.Where(x => x.Name == "mode").DefaultIfEmpty(new Token()).FirstOrDefault().Data,
                Power = qso.Where(x => x.Name == "tx_pwr").DefaultIfEmpty(new Token()).FirstOrDefault().Data

            };
        }


        public MemoryStream Generate()
        {
            MemoryStream memStream = new MemoryStream();
            string path = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "assets"));
            string downloadsPath = string.Empty;
            using (var img = SixLabors.ImageSharp.Image.Load(Path.Combine(path, CARD_FILE)))
            {

                FontCollection fontCollection = new FontCollection();
                FontFamily family = fontCollection.Add(Path.Combine(path, FONT_FILE));
                Font font = family.CreateFont(90, FontStyle.Regular);
                RichTextOptions options = new(font)
                {
                    Origin = new PointF(500, 500),
                    TabWidth = 8,
                    WrappingLength = 1000,
                    HorizontalAlignment = HorizontalAlignment.Left,
                };
                SolidBrush brush = Brushes.Solid(Color.Black);


                downloadsPath = Path.Combine(Path.GetFullPath(AppDomain.CurrentDomain.BaseDirectory), "newcard.png");
                img.Mutate(x => x.DrawText(options, log.Call, brush));
                img.Save(downloadsPath);
                img.SaveAsPng(memStream);
            }
            return memStream;
        }
    }
}