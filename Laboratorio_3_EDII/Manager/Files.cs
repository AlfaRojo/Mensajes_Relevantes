namespace EDII_PROYECTO.Huffman
{
    public class Files
    {
        public string NombreArchivoOriginal { get; set; }
        public string RutaArchivoComprimido { get; set; }
        public double TamanoArchivoDescomprimido { get; set; }
        public double TamanoArchivoComprimido { get; set; }
        public double RazonCompresion { get; set; }
        public double FactorCompresion { get; set; }
        public string PorcentajeReduccion { get; set; }
        public string FormatoCompresion { get; internal set; }
    }
}
