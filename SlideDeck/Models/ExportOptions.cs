namespace SlideDeck.Models
{
    /// <summary>
    /// Opzioni usate durante l'esportazione da Excel a PowerPoint.
    /// </summary>
    public sealed class ExportOptions
    {
        /// <summary>
        /// Path completo del file Excel da elaborare.
        /// </summary>
        public string ExcelPath { get; set; }

        /// <summary>
        /// Se true, l'ultimo foglio viene escluso dall'export.
        /// </summary>
        public bool SkipLastWorksheet { get; set; } = true;

        /// <summary>
        /// Timeout massimo per il paste in PowerPoint, espresso in millisecondi.
        /// </summary>
        public int PasteTimeoutMs { get; set; } = 60000;
    }
}