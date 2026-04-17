using System.Runtime.InteropServices;

namespace SlideDeck.Helpers
{
    /// <summary>
    /// Utility per il rilascio sicuro degli oggetti COM.
    /// Serve per Excel Interop e PowerPoint Interop.
    /// </summary>
    public static class ComReleaseHelper
    {
        /// <summary>
        /// Rilascia completamente un oggetto COM, se valido.
        /// </summary>
        public static void Release(object comObject)
        {
            if (comObject == null)
            {
                return;
            }

            try
            {
                if (Marshal.IsComObject(comObject))
                {
                    Marshal.FinalReleaseComObject(comObject);
                }
            }
            catch
            {
                // Ignorato volutamente:
                // non vogliamo che un problema di rilascio blocchi il flusso.
            }
        }
    }
}