namespace QSL.Models
{
    public class Log
    {
        public string Call { get; set; } = string.Empty;
        public string Date { get; set; } = string.Empty;
        public string Time { get; set; } = string.Empty;
        public string Band { get; set; } = string.Empty;
        public string Freq { get; set; } = string.Empty;
        public string Send { get; set; } = string.Empty;
        public string Receive { get; set; } = string.Empty;
        public string Mode { get; set; } = string.Empty;
        public string Power { get; set; } = string.Empty;
    }
    public class QRZResult
    {
        public string RESULT { get; set; } = string.Empty;
        public string COUNT { get; set; } = string.Empty;
        public string LOGIDS { get; set; } = string.Empty;
        public string ADIF { get; set; } = string.Empty;
    }
}