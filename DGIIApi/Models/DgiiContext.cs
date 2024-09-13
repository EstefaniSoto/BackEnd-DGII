using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace DGIIApi.Models
{
    public partial class DgiiContext : DbContext
    {
        public DgiiContext(DbContextOptions<DgiiContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ComprobantesFiscale> ComprobantesFiscales { get; set; }

        public virtual DbSet<ListadoContribuyente> ListadoContribuyentes { get; set; }

        public async Task InsertComprobanteAsync(string rncCedula, decimal? monto, decimal? itbis18)
        {
            var rncCedulaParam = new SqlParameter("@RNC_Cedula", rncCedula);
            var montoParam = new SqlParameter("@Monto", monto ?? (object)DBNull.Value); // Usa DBNull.Value si el valor es nulo
            var itbis18Param = new SqlParameter("@ITBIS18", itbis18 ?? (object)DBNull.Value); 

            await Database.ExecuteSqlRawAsync("EXEC InsertComprobante @RNC_Cedula, @Monto, @ITBIS18", rncCedulaParam, montoParam, itbis18Param);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ComprobantesFiscale>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.Itbis18)
                    .HasColumnType("decimal(10, 2)")
                    .HasColumnName("ITBIS18");
                entity.Property(e => e.Monto).HasColumnType("decimal(10, 2)");
                entity.Property(e => e.Ncf)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("NCF");
                entity.Property(e => e.RncCedula)
                    .HasMaxLength(11)
                    .IsUnicode(false)
                    .HasColumnName("RNC_Cedula");
            });

            modelBuilder.Entity<ListadoContribuyente>(entity =>
            {
                entity.HasKey(e => e.RncCedula).HasName("PK__Listado___38AA49B32CBD3651");

                entity.ToTable("Listado_Contribuyentes");

                entity.Property(e => e.RncCedula)
                    .HasMaxLength(11)
                    .IsUnicode(false)
                    .HasColumnName("rncCedula");
                entity.Property(e => e.Estatus).HasColumnName("estatus");
                entity.Property(e => e.Nombre)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("nombre");
                entity.Property(e => e.Tipo).HasColumnName("tipo");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
