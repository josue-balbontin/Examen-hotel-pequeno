using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Backend.Modelos;

public partial class HotelDbContext : DbContext
{
    public HotelDbContext()
    {
    }

    public HotelDbContext(DbContextOptions<HotelDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Configuracione> Configuraciones { get; set; }

    public virtual DbSet<Estado> Estados { get; set; }

    public virtual DbSet<Habitacione> Habitaciones { get; set; }

    public virtual DbSet<Reserva> Reservas { get; set; }

    public virtual DbSet<Servicio> Servicios { get; set; }

    public virtual DbSet<TipoHabitacione> TipoHabitaciones { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    public virtual DbSet<UsuariosReserva> UsuariosReservas { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Database=hotel_pequeno;Username=postgres;Password=273153");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Configuracione>(entity =>
        {
            entity.HasKey(e => e.IdConfiguracion).HasName("configuraciones_pk");

            entity.ToTable("configuraciones");

            entity.Property(e => e.IdConfiguracion)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id_configuracion");
            entity.Property(e => e.NombreConfiguracion).HasColumnName("nombre_configuracion");
            entity.Property(e => e.TipoDato).HasColumnName("tipo_dato");
            entity.Property(e => e.ValorConfiguracion).HasColumnName("valor_configuracion");
        });

        modelBuilder.Entity<Estado>(entity =>
        {
            entity.HasKey(e => e.IdEstados).HasName("estados_pk");

            entity.ToTable("estados");

            entity.Property(e => e.IdEstados)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id_estados");
            entity.Property(e => e.Nombre).HasColumnName("nombre");
        });

        modelBuilder.Entity<Habitacione>(entity =>
        {
            entity.HasKey(e => e.IdHabitaciones).HasName("habitaciones_pk");

            entity.ToTable("habitaciones");

            entity.Property(e => e.IdHabitaciones)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id_habitaciones");
            entity.Property(e => e.IdTipoHabitacion).HasColumnName("id_tipo_habitacion");
            entity.Property(e => e.NumeroHabitacion).HasColumnName("numero_habitacion");

            entity.HasOne(d => d.IdTipoHabitacionNavigation).WithMany(p => p.Habitaciones)
                .HasForeignKey(d => d.IdTipoHabitacion)
                .HasConstraintName("id_tipo_habitaciones");
        });

        modelBuilder.Entity<Reserva>(entity =>
        {
            entity.HasKey(e => e.IdReservas).HasName("reservas_pk");

            entity.ToTable("reservas");

            entity.Property(e => e.IdReservas)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id_reservas");
            entity.Property(e => e.CantidadPersonas).HasColumnName("cantidad_personas");
            entity.Property(e => e.CargoCheckout).HasColumnName("cargo_checkout");
            entity.Property(e => e.FechaCheckin).HasColumnName("fecha_checkin");
            entity.Property(e => e.FechaCheckout).HasColumnName("fecha_checkout");
            entity.Property(e => e.FechaIngreso).HasColumnName("fecha_ingreso");
            entity.Property(e => e.FechaSalida).HasColumnName("fecha_salida");
            entity.Property(e => e.IdEstados).HasColumnName("id_estados");
            entity.Property(e => e.IdHabitaciones).HasColumnName("id_habitaciones");

            entity.HasOne(d => d.IdEstadosNavigation).WithMany(p => p.Reservas)
                .HasForeignKey(d => d.IdEstados)
                .HasConstraintName("id_estados");

            entity.HasOne(d => d.IdHabitacionesNavigation).WithMany(p => p.Reservas)
                .HasForeignKey(d => d.IdHabitaciones)
                .HasConstraintName("id_habitaciones");
        });

        modelBuilder.Entity<Servicio>(entity =>
        {
            entity.HasKey(e => e.IdServicios).HasName("Servicios_pk");

            entity.ToTable("servicios");

            entity.Property(e => e.IdServicios)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id_servicios");
            entity.Property(e => e.Encargado).HasColumnName("encargado");
            entity.Property(e => e.NombreServicio).HasColumnName("nombre_servicio");
            entity.Property(e => e.Telefono).HasColumnName("telefono");
        });

        modelBuilder.Entity<TipoHabitacione>(entity =>
        {
            entity.HasKey(e => e.IdTipoHabitaciones).HasName("tipo_habitaciones_pk");

            entity.ToTable("tipo_habitaciones");

            entity.Property(e => e.IdTipoHabitaciones)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id_tipo_habitaciones");
            entity.Property(e => e.Capacidad).HasColumnName("capacidad");
            entity.Property(e => e.Descripcion).HasColumnName("descripcion");
            entity.Property(e => e.Nombre).HasColumnName("nombre");
            entity.Property(e => e.PrecioReferencia).HasColumnName("precio_referencia");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuarios).HasName("usuarios_pk");

            entity.ToTable("usuarios");

            entity.HasIndex(e => e.DocumentoIdentidad, "documento_identidad_unico").IsUnique();

            entity.Property(e => e.IdUsuarios)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id_usuarios");
            entity.Property(e => e.Apellidos).HasColumnName("apellidos");
            entity.Property(e => e.DocumentoIdentidad).HasColumnName("documento_identidad");
            entity.Property(e => e.Edad).HasColumnName("edad");
            entity.Property(e => e.Email).HasColumnName("email");
            entity.Property(e => e.Nombres).HasColumnName("nombres");
            entity.Property(e => e.Telefono).HasColumnName("telefono");
        });

        modelBuilder.Entity<UsuariosReserva>(entity =>
        {
            entity.HasKey(e => e.IdUsuariosReservas).HasName("usuarios_reservas_pk");

            entity.ToTable("usuarios_reservas");

            entity.Property(e => e.IdUsuariosReservas)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id_usuarios_reservas");
            entity.Property(e => e.IdReservas).HasColumnName("id_reservas");
            entity.Property(e => e.IdUsuarios).HasColumnName("id_usuarios");

            entity.HasOne(d => d.IdReservasNavigation).WithMany(p => p.UsuariosReservas)
                .HasForeignKey(d => d.IdReservas)
                .HasConstraintName("id_reservas");

            entity.HasOne(d => d.IdUsuariosNavigation).WithMany(p => p.UsuariosReservas)
                .HasForeignKey(d => d.IdUsuarios)
                .HasConstraintName("id_usuarios");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
