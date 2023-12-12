namespace PortaAviones.Models;

public class Aterrizaje
{

    public Aterrizaje() { }

    public Aterrizaje(int? id, int? despegueFk, DateTime? fechaRegistro, Despegue despegue, List<AeronaveAterrizaje>? aeronaves)
    {
        Id = id;
        DespegueFk = despegueFk;
        FechaRegistro = fechaRegistro;
        Despegue = despegue;
        Aeronaves = aeronaves;
    }

    public Aterrizaje(int? id, int? despegueFk, DateTime? fechaRegistro)
    {
        Id = id;
        DespegueFk = despegueFk;
        FechaRegistro = fechaRegistro;
    }

    public int? Id { get; set; }
    public int? DespegueFk { get; set; }
    public DateTime? FechaRegistro { get; set; }
    public Despegue? Despegue { get; set; }
    public List<AeronaveAterrizaje>? Aeronaves { get; set; }
}
