using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace SlideDeck.Services
{
    /// <summary>
    /// Gestisce la creazione e il ripristino dei backup versionati del file Excel locale.
    /// Ogni backup viene salvato nella cartella:
    /// %AppData%\SlideDeck\Backups
    /// </summary>
    public sealed class TemplateBackupService
    {
        /// <summary>
        /// Crea un backup del file di lavoro locale corrente.
        /// Restituisce il path completo del file di backup creato.
        /// </summary>
        public string CreateBackupOfWorkingTemplate()
        {
            return CreateBackupOfFile(AppPaths.WorkingTemplatePath);
        }

        /// <summary>
        /// Crea un backup versionato del file indicato.
        /// Il nome file viene generato nel formato:
        /// Template_yyyyMMdd_HHmmss.xlsx
        /// 
        /// Se esiste già un file con lo stesso nome, viene aggiunto
        /// un suffisso incrementale:
        /// Template_yyyyMMdd_HHmmss_1.xlsx
        /// Template_yyyyMMdd_HHmmss_2.xlsx
        /// </summary>
        public string CreateBackupOfFile(string sourceFilePath)
        {
            string fullSourcePath = Path.GetFullPath(sourceFilePath);

            if (!File.Exists(fullSourcePath))
            {
                throw new FileNotFoundException(
                    "Impossibile creare il backup: file sorgente non trovato.",
                    fullSourcePath);
            }

            // Garantisce l'esistenza della cartella backup.
            Directory.CreateDirectory(AppPaths.BackupFolder);

            string extension = Path.GetExtension(fullSourcePath);
            string backupFileName = BuildUniqueBackupFileName(extension);
            string backupFullPath = Path.Combine(AppPaths.BackupFolder, backupFileName);

            // Copia il file con qualche tentativo di retry.
            // Questo aiuta soprattutto quando Excel ha appena finito di salvare
            // e il file può risultare momentaneamente occupato.
            CopyFileWithRetry(fullSourcePath, backupFullPath);

            return backupFullPath;
        }

        /// <summary>
        /// Restituisce l'elenco dei backup disponibili, ordinati dal più recente al più vecchio.
        /// I file restituiti sono quelli presenti nella cartella backup dell'applicazione.
        /// </summary>
        public IReadOnlyList<FileInfo> GetAvailableBackups()
        {
            Directory.CreateDirectory(AppPaths.BackupFolder);

            var backupDirectory = new DirectoryInfo(AppPaths.BackupFolder);

            return backupDirectory
                .GetFiles("Template_*.*", SearchOption.TopDirectoryOnly)
                .Where(file => string.Equals(file.Extension, ".xlsx", StringComparison.OrdinalIgnoreCase))
                .OrderByDescending(file => file.LastWriteTimeUtc)
                .ToList();
        }

        /// <summary>
        /// Ripristina il file di lavoro locale a partire da un backup selezionato.
        /// 
        /// In questa versione NON viene creato un backup aggiuntivo del file corrente
        /// prima del ripristino, come richiesto.
        /// </summary>
        public string RestoreBackupAsWorkingTemplate(string backupFilePath)
        {
            string fullBackupPath = Path.GetFullPath(backupFilePath);
            string workingTemplatePath = AppPaths.WorkingTemplatePath;

            if (!File.Exists(fullBackupPath))
            {
                throw new FileNotFoundException(
                    "Il file di backup selezionato non esiste.",
                    fullBackupPath);
            }

            // Garantisce l'esistenza della cartella dati dell'app.
            Directory.CreateDirectory(AppPaths.AppDataFolder);

            // Se esiste già il file locale, normalizza gli attributi prima della copia.
            if (File.Exists(workingTemplatePath))
            {
                File.SetAttributes(workingTemplatePath, FileAttributes.Normal);
            }

            // Copia il backup selezionato sopra al file locale dell'app.
            File.Copy(fullBackupPath, workingTemplatePath, overwrite: true);

            return workingTemplatePath;
        }

        /// <summary>
        /// Costruisce un nome file di backup univoco.
        /// 
        /// Formato base:
        /// Template_yyyyMMdd_HHmmss.xlsx
        /// 
        /// Se il nome esiste già nella cartella backup, aggiunge un suffisso numerico:
        /// Template_yyyyMMdd_HHmmss_1.xlsx
        /// Template_yyyyMMdd_HHmmss_2.xlsx
        /// </summary>
        private string BuildUniqueBackupFileName(string extension)
        {
            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string baseFileName = $"Template_{timestamp}";
            string candidateFileName = $"{baseFileName}{extension}";
            string candidateFullPath = Path.Combine(AppPaths.BackupFolder, candidateFileName);

            // Se non esiste ancora, usiamo subito il nome base.
            if (!File.Exists(candidateFullPath))
            {
                return candidateFileName;
            }

            // Se esiste già, aggiungiamo un indice incrementale.
            int counter = 1;

            while (true)
            {
                candidateFileName = $"{baseFileName}_{counter}{extension}";
                candidateFullPath = Path.Combine(AppPaths.BackupFolder, candidateFileName);

                if (!File.Exists(candidateFullPath))
                {
                    return candidateFileName;
                }

                counter++;
            }
        }

        /// <summary>
        /// Copia il file sorgente nel file destinazione con alcuni tentativi di retry.
        /// Il file sorgente viene aperto in lettura condivisa, così da aumentare le
        /// probabilità di riuscire a leggerlo anche subito dopo un salvataggio di Excel.
        /// </summary>
        private void CopyFileWithRetry(string sourceFilePath, string destinationFilePath)
        {
            const int maxAttempts = 10;
            const int delayMs = 250;

            for (int attempt = 1; attempt <= maxAttempts; attempt++)
            {
                try
                {
                    using (var sourceStream = new FileStream(
                        sourceFilePath,
                        FileMode.Open,
                        FileAccess.Read,
                        FileShare.ReadWrite))
                    {
                        using (var destinationStream = new FileStream(
                            destinationFilePath,
                            FileMode.Create,
                            FileAccess.Write,
                            FileShare.None))
                        {
                            sourceStream.CopyTo(destinationStream);
                        }
                    }

                    return;
                }
                catch (IOException)
                {
                    if (attempt == maxAttempts)
                    {
                        throw;
                    }

                    Thread.Sleep(delayMs);
                }
                catch (UnauthorizedAccessException)
                {
                    if (attempt == maxAttempts)
                    {
                        throw;
                    }

                    Thread.Sleep(delayMs);
                }
            }
        }
    }
}