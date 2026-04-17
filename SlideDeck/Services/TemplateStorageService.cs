using System;
using System.IO;

namespace SlideDeck.Services
{
    /// <summary>
    /// Gestisce il file Excel locale usato dall'applicazione.
    /// Se il file non esiste, viene creato dalla resource embedded DefaultTemplate.
    /// Inoltre permette di:
    /// - sostituire il template locale con un template custom
    /// - ripristinare il template di default
    /// - creare un backup del file locale prima delle sostituzioni
    /// </summary>
    public sealed class TemplateStorageService
    {
        private readonly TemplateBackupService _templateBackupService;

        /// <summary>
        /// Costruttore del servizio.
        /// </summary>
        public TemplateStorageService(TemplateBackupService templateBackupService)
        {
            _templateBackupService = templateBackupService
                ?? throw new ArgumentNullException(nameof(templateBackupService));
        }

        /// <summary>
        /// Garantisce che il file di lavoro locale esista.
        /// Se non esiste, lo crea copiandolo dalla resource DefaultTemplate.
        /// Restituisce sempre il path completo del file locale.
        /// </summary>
        public string EnsureWorkingTemplateExists()
        {
            string appDataFolder = AppPaths.AppDataFolder;
            string workingTemplatePath = AppPaths.WorkingTemplatePath;

            // Crea la cartella AppData dell'app, se non esiste.
            Directory.CreateDirectory(appDataFolder);

            // Se il file locale esiste già, non serve ricrearlo.
            if (File.Exists(workingTemplatePath))
            {
                return workingTemplatePath;
            }

            WriteDefaultTemplateToWorkingPath();

            return workingTemplatePath;
        }

        /// <summary>
        /// Sostituisce il file di lavoro locale con un template custom scelto dall'utente.
        /// Prima di sovrascrivere il file locale, ne crea un backup se presente.
        /// </summary>
        public string ReplaceWorkingTemplate(string sourceTemplatePath)
        {
            string fullSourcePath = Path.GetFullPath(sourceTemplatePath);
            string workingTemplatePath = AppPaths.WorkingTemplatePath;

            if (!File.Exists(fullSourcePath))
            {
                throw new FileNotFoundException(
                    "Il file template selezionato non esiste.",
                    fullSourcePath);
            }

            if (!IsSupportedTemplateFile(fullSourcePath))
            {
                throw new InvalidOperationException(
                    "Il file selezionato non è supportato. Per ora sono accettati solo file .xlsx.");
            }

            // Garantisce l'esistenza della cartella dati dell'app.
            Directory.CreateDirectory(AppPaths.AppDataFolder);

            // Se il file sorgente coincide già col file locale, non serve fare nulla.
            if (string.Equals(
                fullSourcePath,
                workingTemplatePath,
                StringComparison.OrdinalIgnoreCase))
            {
                return workingTemplatePath;
            }

            // Se esiste già un template locale, ne crea un backup prima della sostituzione.
            if (File.Exists(workingTemplatePath))
            {
                File.SetAttributes(workingTemplatePath, FileAttributes.Normal);
                _templateBackupService.CreateBackupOfWorkingTemplate();
            }

            // Copia il nuovo template sopra quello locale dell'app.
            File.Copy(fullSourcePath, workingTemplatePath, overwrite: true);

            return workingTemplatePath;
        }

        /// <summary>
        /// Ripristina il template di default embedded, sovrascrivendo il file locale.
        /// Prima di farlo, crea un backup del file corrente se presente.
        /// </summary>
        public string RestoreDefaultTemplate()
        {
            string workingTemplatePath = AppPaths.WorkingTemplatePath;

            Directory.CreateDirectory(AppPaths.AppDataFolder);

            // Se esiste un template locale corrente, lo salviamo prima come backup.
            if (File.Exists(workingTemplatePath))
            {
                File.SetAttributes(workingTemplatePath, FileAttributes.Normal);
                _templateBackupService.CreateBackupOfWorkingTemplate();
            }

            WriteDefaultTemplateToWorkingPath();

            return workingTemplatePath;
        }

        /// <summary>
        /// Verifica se il file è un template supportato dall'applicazione.
        /// Per ora, per restare coerenti con Template.xlsx, supportiamo solo .xlsx.
        /// </summary>
        public bool IsSupportedTemplateFile(string filePath)
        {
            string extension = Path.GetExtension(filePath);

            return string.Equals(
                extension,
                ".xlsx",
                StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Scrive il template di default embedded nel path locale di lavoro.
        /// </summary>
        private void WriteDefaultTemplateToWorkingPath()
        {
            string workingTemplatePath = AppPaths.WorkingTemplatePath;

            byte[] templateBytes = GetDefaultTemplateBytes();

            if (templateBytes == null || templateBytes.Length == 0)
            {
                throw new InvalidOperationException(
                    "La resource 'DefaultTemplate' non è presente o risulta vuota.");
            }

            File.WriteAllBytes(workingTemplatePath, templateBytes);
        }

        /// <summary>
        /// Restituisce il contenuto binario della resource DefaultTemplate.
        /// Questo metodo gestisce sia il caso in cui la resource venga esposta
        /// come byte[] sia il caso in cui venga esposta come stream.
        /// </summary>
        private byte[] GetDefaultTemplateBytes()
        {
            object resourceObject = Properties.Resources.DefaultTemplate;

            if (resourceObject is byte[] byteArray)
            {
                return byteArray;
            }

            if (resourceObject is UnmanagedMemoryStream memoryStream)
            {
                using (var ms = new MemoryStream())
                {
                    memoryStream.CopyTo(ms);
                    return ms.ToArray();
                }
            }

            throw new InvalidOperationException(
                "La resource 'DefaultTemplate' non ha un formato supportato.");
        }
    }
}