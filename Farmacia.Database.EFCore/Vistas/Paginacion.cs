namespace WebApi.Controllers
{
    public class Paginacion
    {
        public int Pagina { get; set; }
        public int RegistrosPorPagina { get; set; }
        public string Filtro { get; set; }
    }
}