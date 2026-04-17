using System;
using System.IO;

namespace SlideDeck
{
    /// <summary>
    /// Contiene i path usati dall'applicazione.
    /// Tutti i file di lavoro vengono salvati in AppData.
    /// </summary>
    public static class AppPaths
    {
        /// <summary>
        /// Cartella dati dell'applicazione per l'utente corrente.
        /// Esempio:
        /// %AppData%\SlideDeck
        /// </summary>
        public static string AppDataFolder =>
            Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "SlideDeck");

        /// <summary>
        /// Cartella che contiene i backup del file Excel locale.
        /// </summary>
        public static string BackupFolder =>
            Path.Combine(AppDataFolder, "Backups");

        /// <summary>
        /// Path completo del file Excel locale usato dall'app.
        /// Questo è il file che l'utente apre, modifica e che viene
        /// poi usato per generare il PowerPoint.
        /// </summary>
        public static string WorkingTemplatePath =>
            Path.Combine(AppDataFolder, "Template.xlsx");
    }
}